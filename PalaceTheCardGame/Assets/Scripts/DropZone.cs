using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Local2PHandler local2PHandler;
    public int round;


    void Start()
    {
        local2PHandler = FindObjectOfType<Local2PHandler>();
        round = local2PHandler.playerRound;
    }

    void Update()
    {
        round = local2PHandler.playerRound;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (round == 0 && gameObject.transform.childCount < 3)
        {
            eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
        }
        else if (round == 1 && gameObject.transform.childCount < 4)
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
        else if (round == 2 && gameObject.transform.childCount < 1)
        {
            eventData.pointerDrag.transform.SetParent(gameObject.transform, false);
        }
        
    }
}
