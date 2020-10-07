using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop4P : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isDraggable;
    private bool isDragging;

    public GameObject canvas;
    public GameObject playerArea;

    public Local4PHandler local4PHandler;

    void Start()
    {
        local4PHandler = FindObjectOfType<Local4PHandler>();

        canvas = local4PHandler.canvas;
        playerArea = local4PHandler.playerArea;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            this.transform.SetParent(canvas.transform, true);
            isDragging = true;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && isDraggable)
        {
            this.transform.position = eventData.position;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            if (this.transform.parent.name != "PDZ")
            {
                this.transform.SetParent(playerArea.transform, true);
            }
            isDragging = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

    }
}
