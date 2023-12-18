using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private float _placingRange = 1f;
        [SerializeField]
        private int selectedItemIndex = 0;
        [SerializeField]
        private GameObject selectedItemGO;
        [SerializeField]
        private Structure structureSelected;

        private Camera _cam;

        private void Start()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.TogglePause();
            }

            if (GameManager.isPaused)
                return;

            _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

            // Handle scroll wheel input
            if (scrollWheelInput != 0f)
            {
                int scrollDirection = Mathf.RoundToInt(Mathf.Sign(scrollWheelInput));
                selectedItemIndex = (selectedItemIndex - scrollDirection + 5) % 5;
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                PlaceStructure();
            } 
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    PlaceStructure();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeStructureOrientation();
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    ChangeStructureOrientation();
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
                structureSelected.stats.range += Player.Instance.stats.rangeIncrease;
                structureSelected.EnableStructure(false);
            }

            structureSelected.CheckIfPlaceable();

            //Put the structure towards where the mouse is
            Vector3 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(mousePosition, transform.position);
            if (distance > _placingRange)
            {
                Vector3 fromOriginToObject = mousePosition - transform.position; //~GreenPosition~ - *BlackCenter*
                fromOriginToObject *= _placingRange / distance; //Multiply by radius //Divide by Distance
                selectedItemGO.transform.position = transform.position + fromOriginToObject; //*BlackCenter* + all that Math
            }
            else
            {
                selectedItemGO.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
        }

        private void ChangeStructureOrientation()
        {
            if (structureSelected)
                structureSelected.ChangeOrientation();
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
