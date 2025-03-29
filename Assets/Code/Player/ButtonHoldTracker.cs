using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoldTracker : MonoBehaviour
{
    public static bool IsAnyButtonHeld { get; private set; }

    private EventSystem eventSystem;
    private StandaloneInputModule inputModule;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<StandaloneInputModule>();
        if (inputModule == null)
        {
            inputModule = eventSystem.gameObject.AddComponent<StandaloneInputModule>();
        }
    }

    private void Update()
    {
        IsAnyButtonHeld = false;

        // Check all pointer events
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                CheckPointerEvent(touch.fingerId);
            }
        }

        // Also check mouse
        if (Input.GetMouseButton(0))
        {
            CheckPointerEvent(PointerInputModule.kMouseLeftId);
        }
    }

    private void CheckPointerEvent(int pointerId)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.pointerId = pointerId;
        pointerData.position = GetPointerPosition(pointerId);

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            var button = result.gameObject.GetComponent<Button>();
            if (button != null && button.interactable)
            {
                IsAnyButtonHeld = true;
                return;
            }
        }
    }

    private Vector2 GetPointerPosition(int pointerId)
    {
        if (pointerId == PointerInputModule.kMouseLeftId)
        {
            return Input.mousePosition;
        }
        else if (pointerId >= 0 && pointerId < Input.touchCount)
        {
            return Input.GetTouch(pointerId).position;
        }
        return Vector2.zero;
    }
}