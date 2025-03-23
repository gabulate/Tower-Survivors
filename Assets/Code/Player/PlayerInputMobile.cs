using System.Collections;
using System.Collections.Generic;
using TowerSurvivors.Game;
using TowerSurvivors.Structures;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerSurvivors.PlayerScripts
{
    public class PlayerInputMobile : PlayerInputController
    {
        public Joystick joystick;
        public Touch mainTouch;
        public bool isTouching = false;

        public float placeTime = 0.2f;

        protected override void Start()
        {
            base.Start();
            GameObject.FindGameObjectWithTag("PauseButton").GetComponent<Button>().onClick.AddListener(PauseGame);
            joystick = GameObject.FindGameObjectWithTag("joystick").GetComponent<Joystick>();
            mousePosition = Vector2.zero;
        }

        private void PauseGame()
        {
            GameManager.Instance.ShowPauseMenu(!GameManager.isPaused);
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.ShowPauseMenu(!GameManager.isPaused);
            }

            if (GameManager.isPaused)
                return;

            _input = joystick.Direction;

           if (isTouching & mainTouch.phase == TouchPhase.Ended)
           {
                if (_structureSelected & mainTouch.deltaTime < placeTime)
                    PlaceStructure();
                else if (_hoveredStructure & mainTouch.deltaTime < placeTime)
                    PickUpStructure();
                isTouching = false;
           }
        } 

        protected override void CheckMouse()
        {
            isTouching = false;
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (!EventSystem.current.IsPointerOverGameObject(i) & i != joystick.pointerId)
                {
                    mainTouch = Input.GetTouch(i);
                    isTouching = true;
                    break;
                }
            }

            if(isTouching)
                mousePosition = _cam.ScreenToWorldPoint(mainTouch.position);

            CheckMouseHover();
            CheckForUpgrades();

            //If the player has no structure selected
            if (Player.Inventory.selectedItem == null | !isTouching)
            {
                //If there's an item selected, remove it from the scene
                if (_selectedItemGO != null)
                {
                    _selectedItemGO.SetActive(false);
                    _selectedItemGO = null;
                    _structureSelected = null;
                }
                return;
            }

            if (_selectedItemGO != null & _selectedItemGO != Player.Inventory.selectedItem.itemInstance)
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
    }
}
