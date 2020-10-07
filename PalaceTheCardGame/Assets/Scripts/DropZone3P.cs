using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone3P : MonoBehaviour, IDropHandler
{
    public Local3PHandler local3PHandler;
    public int round;


    void Start()
    {
        local3PHandler = FindObjectOfType<Local3PHandler>();
        round = local3PHandler.playerRound;
    }

    void Update()
    {
        round = local3PHandler.playerRound;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (round == 0 && gameObject.transform.childCount < 3 && eventData.pointerDrag.GetComponent<DragDrop3P>().isDraggable == true)
        {
            eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
        }
        else if (round == 1 && gameObject.transform.childCount < 4 && eventData.pointerDrag.GetComponent<DragDrop3P>().isDraggable == true)
        {
            if (gameObject.transform.childCount == 0)
            {
                eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
            }
            else if (eventData.pointerDrag.name[0] == gameObject.transform.GetChild(0).name[0])
            {
                eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
            }
        }
        else if (round == 2 && gameObject.transform.childCount < 1 && eventData.pointerDrag.GetComponent<DragDrop3P>().isDraggable == true)
        {
            eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
        }

    }
}