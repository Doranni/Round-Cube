using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private float yPositionOffset;
    private int index_currentNodePos;
    private Vector3 targetPos;
    private Coroutine movingCoroutine;

    public event Action OnMoveFinished;

    void Start()
    {
        //InputManager.Instance.OnMouseClick_canceled += MouseClicked;

        DiceRoller.Instance.OnDiceResChanged += DiceResChanged;

        yPositionOffset = GetComponent<Collider>().bounds.extents.y;
        MoveToStart();
    }

    private void DiceResChanged(int value)
    {
        if (value > 0)
        {
            Move();
        }
    }

    private void MouseClicked(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Mouse Clicked");
    }

    private void MoveToStart()
    {
        index_currentNodePos = Map.Instance.Index_start;
        transform.position = Map.Instance.MapNodes[index_currentNodePos].transform.position 
            + Vector3.up * yPositionOffset;
    }

    private void Move()
    {
        var links = Map.Instance.AvailableLinks(index_currentNodePos);
        if (links.Count == 1)
        {
            index_currentNodePos = links[0].Index;
            targetPos = links[0].transform.position + Vector3.up * yPositionOffset;
            movingCoroutine = StartCoroutine(Moving());
        }
    }

    private IEnumerator Moving()
    {
        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return 0;
        }
        OnMoveFinished?.Invoke();
    }

    private void OnDestroy()
    {
        //InputManager.Instance.OnMouseClick_performed -= MouseClicked;
    }
}
