using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class DragCard : Singleton<DragCard>
{
    private VisualElement card;
    //private Vector2 startCardPos;

    private void Start()
    {
        InputManager.Instance.OnUIMousePosition_performed += MousePosition_performed;
    }

    private void MousePosition_performed(InputAction.CallbackContext obj)
    {
        if (card != null)
        {
            var pos = obj.ReadValue<Vector2>();
            pos.y = Screen.height - pos.y;
            pos = card.WorldToLocal(pos);
            Debug.Log(pos);
            card.style.left = pos.x;
            card.style.top = pos.y;
        }
    }

    public void StartDragging(VisualElement card)
    {
        Debug.Log("Start dragging card");
        this.card = card;
    }

    public void StopDragging(VisualElement card)
    {
        Debug.Log("Stop dragging card");
        card.style.top = StyleKeyword.Auto;
        card.style.left = StyleKeyword.Auto;
        this.card = null;
    }
}
