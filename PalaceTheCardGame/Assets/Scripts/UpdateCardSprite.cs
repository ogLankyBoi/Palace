using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCardSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private bool dragging;
    private SpriteRenderer sprite;
    private Image spriteRenderer;
    private Local2PHandler local2PHandler;
    public bool faceUp = false;

    void Start()
    {
        List<string> deck = Local2PHandler.GenerateDeck();
        local2PHandler = FindObjectOfType<Local2PHandler>();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = local2PHandler.cardFaces[i];
                break;
            }
            i++;
        }
        
        spriteRenderer = this.GetComponent<Image>();
        sprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //dragging = this.GetComponent<DragDrop>().isDragging;
        if (faceUp == false)
        {
            spriteRenderer.sprite = cardBack;
        }
        else if (faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        /*if (this.transform.parent.name == "PlayerArea" || this.transform.parent.name == "ComputerArea" || this.transform.parent.name == "PDZ" || dragging == true)
        {
            sprite.sprite = null;
        }
        else
        {
            sprite.sprite = cardBack;
        }*/
    }
}
