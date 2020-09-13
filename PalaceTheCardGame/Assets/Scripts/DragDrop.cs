using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isDraggable;
    public static GameObject DraggedInstance;

    Vector3 _startPosition;
    Vector3 _offsetToMouse;
    float _zDistanceToCamera;

    #region Interface Implementations

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            DraggedInstance = gameObject;
            _startPosition = transform.position;
            _zDistanceToCamera = Mathf.Abs(_startPosition.z - Camera.main.transform.position.z);

            _offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
            );
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            if (Input.touchCount > 1)
                return;

            transform.position = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
                ) + _offsetToMouse;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            DraggedInstance = null;
            _offsetToMouse = Vector3.zero;
        }
    }

    #endregion
}
