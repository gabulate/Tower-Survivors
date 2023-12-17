using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Structures;
using UnityEngine;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// MonoBehaviour that controls the player's input and Movement.
    /// </summary>
    public class PlayerInputController : MonoBehaviour
    {
        public float initialSpeed = 2;
        public float Speed = 2;

        [SerializeField]
        private bool _canMove = true;
        [SerializeField]
        private Vector2 _input;
        [SerializeField]
        private Rigidbody2D _rb;
        [SerializeField]
        private float _offset = 1f;
        [SerializeField]
        private int selectedItemIndex = 0;
        [SerializeField]
        private GameObject selectedItemGO;
        [SerializeField]
        private Structure structureSelected;

        void Update()
        {
            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

            // Handle scroll wheel input
            if (scrollWheelInput != 0f)
            {
                int scrollDirection = Mathf.RoundToInt(Mathf.Sign(scrollWheelInput));
                selectedItemIndex = (selectedItemIndex + scrollDirection + 5) % 5;
                Player.Inventory.SelectItem(selectedItemIndex);
            }

            // Handle number key input
            for (int i = 1; i <= 5; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    selectedItemIndex = i - 1;
                    Player.Inventory.SelectItem(selectedItemIndex);
                }
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlaceStructure();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void PlaceStructure()
        {
            //Return if the player isn't holding a structure
            if (structureSelected == null)
                return;

            //Return if the structure is not on a valid place
            if (!structureSelected.CheckIfPlaceable())
                return;

            StructureManager.Instance.PlaceStructure(structureSelected);

            Player.Inventory.UseItem();
            selectedItemGO = null;
            structureSelected = null;
            Player.Instance.ApplyBuffs();
        }

        private void ShowStructureOutline()
        {
            //If the player has no structure selected
            if (Player.Inventory.selectedItem == null)
            {
                //If there's an item selected, remove it from the scene
                if(selectedItemGO != null)
                {
                    Destroy(selectedItemGO);
                }
                return;
            }

            //If an item was selected, spawn it
            if(selectedItemGO == null)
            {
                selectedItemGO = Instantiate(Player.Inventory.selectedItem.item.prefab, transform);
                structureSelected = selectedItemGO.GetComponent<Structure>();
                structureSelected.range += Player.Instance.rangeIncrease;
                structureSelected.EnableStructure(false);
            }

            structureSelected.CheckIfPlaceable();
            //Flip accordingly
            int flipityflop = Player.Sprite.flipX ? -1 : 1;
            selectedItemGO.transform.localPosition = new Vector3(_offset * flipityflop, 0, 0);

            if(!structureSelected.uniqueOrientation)
            {
                ChangeStructureOrientation();
            }
        }

        private void ChangeStructureOrientation()
        {
            float absX = Mathf.Abs(_input.x);
            float absY = Mathf.Abs(_input.y);

            // Check which component has a higher absolute value to determine the dominant direction
            if (absX > absY)
            {
                // Check if the movement is towards the right or left
                if (_input.x > 0)
                {
                    structureSelected.ChangeOrientation(Orientation.RIGHT);
                }
                else if (_input.x < 0)
                {
                    structureSelected.ChangeOrientation(Orientation.LEFT);
                }
            }
            else if (absY > absX)
            {
                // Check if the movement is towards up or down
                if (_input.y > 0)
                {
                    structureSelected.ChangeOrientation(Orientation.UP);
                }
                else if (_input.y < 0)
                {
                    structureSelected.ChangeOrientation(Orientation.DOWN);
                }
            }
        }

        private void FixedUpdate()
        {
            Move();
            ShowStructureOutline();
        }

        private void Move()
        {
            _rb.velocity = Vector2.zero;
            if (!_canMove)
            {
                Player.PlayerAnimator.SetFloat("speed", 0);
                return;
            }

            //Normalize the input vector to ensure consistent speed in all directions.
            Vector2 normalizedInput = _input.normalized;

            //Calculate the target position based on the normalized input.
            Vector3 targetPosition = transform.position + new Vector3(normalizedInput.x * (Speed * Time.fixedDeltaTime),
                normalizedInput.y * (Speed * Time.fixedDeltaTime), 0);

            //Move towards the target position using Lerp.
            transform.position = targetPosition;

            //Animator and sprite orientation////////////////////////////////////
            Player.PlayerAnimator.SetFloat("speed", Mathf.Abs(_input.magnitude));
            if (_input.x > 0)
            {
                Player.Sprite.flipX = false;
            }
            else if (_input.x < 0)
            {
                Player.Sprite.flipX = true;
            }
        }

        public void EnableMovement(bool enabled)
        {
            _canMove = enabled;
        }
    }
}
