using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCardSprite3P : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer sprite;
    private Image spriteRenderer;
    private Local3PHandler local3PHandler;
    public bool faceUp = false;

    void Start()
    {
        List<string> deck = Local3PHandler.GenerateDeck();
        local3PHandler = FindObjectOfType<Local3PHandler>();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = local3PHandler.cardFaces[i];
                break;
            }
            i++;
        }

        spriteRenderer = this.GetComponent<Image>();
        sprite = this.GetComponent<SpriteRenderer>();
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
