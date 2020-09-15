using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isDraggable;
    private bool isDragging;

    public GameObject canvas;
    public GameObject playerArea;

    public Local2PHandler local2PHandler;

    void Start()
    {
        local2PHandler = FindObjectOfType<Local2PHandler>();

        canvas = local2PHandler.canvas;
        playerArea = local2PHandler.playerArea;
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
    /*void Update()
    {
        if (isDragging && isDraggable)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && isDraggable)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }*/
}
