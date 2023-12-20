using System;
using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerSurvivors.PlayerScripts
{
    /// <summary>
    /// MonoBehaviour that controls the player's input and Movement.
    /// </summary>
    public class PlayerInputController : MonoBehaviour
    {
        private static readonly LayerMask _structureLayer = 1 << 7;
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
        private int _selectedItemIndex = 0;
        [SerializeField]
        private GameObject _selectedItemGO;
        [SerializeField]
        private Structure _structureSelected;
        [SerializeField]
        private Structure _hoveredStructure;

        private Camera _cam;
        [SerializeField]
        private Vector3 mousePosition;
        [SerializeField]
        private float _mouseHoverRange = 2;

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
                _selectedItemIndex = (_selectedItemIndex - scrollDirection + 5) % 5;
                Player.Inventory.SelectItem(_selectedItemIndex);
            }

            // Handle number key input
            for (int i = 1; i <= 5; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    _selectedItemIndex = i - 1;
                    Player.Inventory.SelectItem(_selectedItemIndex);
                }
            }


            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    SecondaryAction();
            } 
            else if (Input.GetKeyDown(KeyCode.E))
            {
                PlaceStructure();
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    PlaceStructure();
            }
        }

        private void PlaceStructure()
        {
            //Return if the player isn't holding a structure
            if (_structureSelected == null)
                return;
            
            Debug.Log(_hoveredStructure);
            //If the player is hovering it over another structure, check if it can be upgraded
            if (_hoveredStructure != null)
            {
                //If it can't be upgraded, return.
                if (!_hoveredStructure.Upgrade(_structureSelected))
                {
                    //TODO: Play can't place sound
                    return;
                }
                //If upgraded succesfully
                //TODO: Play structure upgrade sound
                _hoveredStructure.OutLine(false);
                AssetsHolder.Instance.HUD.HideUpBox();
                Destroy(_selectedItemGO);
            }
            //Return if the structure is not on a valid place
            else if(!_structureSelected.CheckIfPlaceable())
            {
                //TODO: Play can't place sound
                return;
            }
            //If there is no hovered structure and the selected structure can be placed
            else
            {
                StructureManager.Instance.PlaceStructure(_structureSelected);
                //TODO: Play structure placed sound
            }

            //Remove the item from the inventory
            Player.Inventory.UseItem();
            _selectedItemGO = null;
            _structureSelected = null;
            Player.Instance.ApplyBuffs();
        }

        /// <summary>
        /// Checks the mouse postion, if a structure item is selected and can be placed shows it
        /// </summary>
        private void CheckMouse()
        {
            mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
            CheckMouseHover();
            CheckForUpgrades();
            //If the player has no structure selected
            if (Player.Inventory.selectedItem == null)
            {
                //If there's an item selected, remove it from the scene
                if(_selectedItemGO != null)
                {
                    _selectedItemGO.SetActive(false);
                    _selectedItemGO = null;
                    _structureSelected = null;
                }
                return;
            } 

            if (_selectedItemGO != null && _selectedItemGO != Player.Inventory.selectedItem.itemInstance)
            {
                _selectedItemGO.SetActive(false);
                _selectedItemGO = null;
            }

            //If an item was selected, enable it, if more structures can be placed
            if (_selectedItemGO == null)
            {
                _selectedItemGO = Player.Inventory.selectedItem.itemInstance;
                _structureSelected = _selectedItemGO.GetComponent<Structure>();
                _structureSelected.EnableStructure(false);

                if (!StructureManager.Instance.CanPlace())
                {
                    _selectedItemGO.SetActive(false);
                    return;
                }

                _selectedItemGO.SetActive(true);

                _structureSelected.OutLine(true);
            }

            //Put the structure towards where the mouse is
            float distance = Vector2.Distance(mousePosition, transform.position);
            if (distance > _placingRange)
            {
                Vector3 fromOriginToObject = mousePosition - transform.position; //~GreenPosition~ - *BlackCenter*
                fromOriginToObject *= _placingRange / distance; //Multiply by radius //Divide by Distance
                _selectedItemGO.transform.position = transform.position + fromOriginToObject; //*BlackCenter* + all that Math
                _selectedItemGO.transform.position = new Vector3(_selectedItemGO.transform.position.x, _selectedItemGO.transform.position.y, _selectedItemGO.transform.position.y);
            }
            else
            {
                _selectedItemGO.transform.position = new Vector3(mousePosition.x, mousePosition.y, mousePosition.y);
            }

            _structureSelected.CheckIfPlaceable();
        }

        private void CheckMouseHover()
        {
            Collider2D hit = Physics2D.OverlapCircle(mousePosition, _mouseHoverRange, _structureLayer);

            //If the mouse is over another structure
            if (hit != null)
            {
                //Check if the structure is within range
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                if (distance > _placingRange)
                    return;
                //Assign the hovered structure
                _hoveredStructure = hit.GetComponent<Structure>();
                _hoveredStructure.OutLine(true);
            }
            //If the mouse is not over a structure
            else
            {
                //If last update the mouse was over a structure, deselect it now
                if (_hoveredStructure != null)
                {
                    _hoveredStructure.OutLine(false);
                    _hoveredStructure = null;
                }
            }
        }

        private void CheckForUpgrades()
        {
            //If a structure is hovered
            if (_hoveredStructure)
            {
                _hoveredStructure.ShowLevelUpStats(_structureSelected);
            }
            //If the mouse is not over a structure
            else
            {
                AssetsHolder.Instance.HUD.HideUpBox();
            }

            if(_structureSelected)
                _structureSelected.OutLine(!_hoveredStructure);
        }

        private void SecondaryAction()
        {
            if(_structureSelected && !StructureManager.Instance.CanPlace())
            {
                //TODO: Play can't place sound
                return;
            }

            if (_structureSelected)
                ChangeStructureOrientation();
            else
                PickUpStructure();
        }

        private void PickUpStructure()
        {
            //If there's no hovered structure return because what would you even pick up cmon man
            if (!_hoveredStructure)
                return;

            Player.Inventory.PickUpStructure(_hoveredStructure);
        }

        private void ChangeStructureOrientation()
        {
            _structureSelected.ChangeOrientation();
        }

        private void FixedUpdate()
        {
            Move();
            CheckMouse();

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

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(mousePosition, _mouseHoverRange);
        }
    }
}
