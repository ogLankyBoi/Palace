using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCardSprite4P : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer sprite;
    private Image spriteRenderer;
    private Local4PHandler local4PHandler;
    public bool faceUp = false;

    void Start()
    {
        List<string> deck = Local4PHandler.GenerateDeck();
        local4PHandler = FindObjectOfType<Local4PHandler>();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = local4PHandler.cardFaces[i];
                break;
            }
            i++;
        }

        spriteRenderer = this.GetComponent<Image>();
        sprite = this.GetComponent<SpriteRenderer>();
    }

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
