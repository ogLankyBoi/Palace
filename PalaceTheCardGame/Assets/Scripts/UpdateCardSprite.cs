using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCardSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (faceUp == false)
        {
            spriteRenderer.sprite = cardBack;
        }
        else if (faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
    }
}
