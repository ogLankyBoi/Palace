using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class Local2PHandler : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject canvas;
    public GameObject playerArea;
    public GameObject computerArea;


    public List<string> deck;
    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
    public List<int> cardValue;
    public List<string> restOfDeck;

    void Start()
    {
        AssignGameObjects();
        deck = GenerateDeck();
        ShuffleDeck(deck);
        cardValue = AssignCardValues(deck);
        StartCoroutine(Deal());
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(v + s);
            }
        }

        return newDeck;
    }

    void AssignGameObjects()
    {
        canvas = GameObject.Find("Canvas");
        playerArea = GameObject.Find("PlayerArea");
        computerArea = GameObject.Find("ComputerArea");
    }

    void ShuffleDeck<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public static List<int> AssignCardValues(List<string> thisDeck)
    {
        List<int> cardValues = new List<int>();

        for (int i = 0; i < 52; i++)
        {
            string cardName = thisDeck[i];
            char value = cardName[0];
            switch (value)
            {
                case '2':
                    cardValues.Add(2);
                    break;
                case '3':
                    cardValues.Add(3);
                    break;
                case '4':
                    cardValues.Add(4); ;
                    break;
                case '5':
                    cardValues.Add(5);
                    break;
                case '6':
                    cardValues.Add(6);
                    break;
                case '7':
                    cardValues.Add(7);
                    break;
                case '8':
                    cardValues.Add(8);
                    break;
                case '9':
                    cardValues.Add(9);
                    break;
                case 'T':
                    cardValues.Add(10);
                    break;
                case 'J':
                    cardValues.Add(11);
                    break;
                case 'Q':
                    cardValues.Add(12);
                    break;
                case 'K':
                    cardValues.Add(13);
                    break;
                default:
                    cardValues.Add(14);
                    break;
            }
        }

        return cardValues;
    }

    IEnumerator Deal()
    {
        float xOffset1 = 0;
        float xOffset2 = 0;
        float zOffset = 0;

        for (int i = 0; i < 52; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(880, 540, zOffset), Quaternion.identity);
            newCard.name = deck[i];
            GameObject.Find(deck[i]).transform.SetParent(canvas.transform, true);


            zOffset = zOffset + 1;
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
            {
                GameObject.Find(deck[i]).transform.position = new Vector3(810 + xOffset1, 330, 0);

                xOffset1 = xOffset1 + 150;
            }
            else
            {
                GameObject.Find(deck[i]).transform.position = new Vector3(1110 - xOffset2, 750, 0);

                xOffset2 = xOffset2 + 150;
            }
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 6; i < 18; i++)
        {

            if (i % 2 == 0)
            {
                GameObject.Find(deck[i]).transform.SetParent(playerArea.transform, true);
            }
            else
            {
                GameObject.Find(deck[i]).transform.SetParent(computerArea.transform, true);
            }

            yield return new WaitForSeconds(0.2f);
        }


        PickEndOfGameCards();
    }

    void PickEndOfGameCards()
    {
        for (int i = 6; i < 18; i++)
        {
            GameObject.Find(deck[i]).GetComponent<UpdateCardSprite>().faceUp = true;
            GameObject.Find(deck[i]).GetComponent<DragDrop>().isDraggable = true;
        }
    }

    void AfterPickingEndCards()
    {

    }
}
