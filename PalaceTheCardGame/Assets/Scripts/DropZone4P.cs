using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone4P : MonoBehaviour, IDropHandler
{
    public Local4PHandler local4PHandler;
    public int round;


    void Start()
    {
        local4PHandler = FindObjectOfType<Local4PHandler>();
        round = local4PHandler.playerRound;
    }

    void Update()
    {
        round = local4PHandler.playerRound;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (round == 0 && gameObject.transform.childCount < 3 && eventData.pointerDrag.GetComponent<DragDrop4P>().isDraggable == true)
        {
            eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
        }
        else if (round == 1 && gameObject.transform.childCount < 4 && eventData.pointerDrag.GetComponent<DragDrop4P>().isDraggable == true)
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
        else if (round == 2 && gameObject.transform.childCount < 1 && eventData.pointerDrag.GetComponent<DragDrop4P>().isDraggable == true)
        {
            eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
        }

    }
}
