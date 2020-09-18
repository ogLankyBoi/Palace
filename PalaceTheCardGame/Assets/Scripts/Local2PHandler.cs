using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using Random = System.Random;

public class Local2PHandler : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject canvas;
    public GameObject playerArea;
    public GameObject computerArea;
    public GameObject playerDZ;

    public List<string> playerLastUpCards = new List<string>();
    public List<string> playerFinalCards = new List<string>();
    public List<string> computerLastUpCards = new List<string>();
    public List<string> computerFinalCards = new List<string>();
    public List<string> playedCards = new List<string>();

    public GameObject playButton;
    public GameObject clearButton;

    public bool playerTurn = true;
    public int playerRound = 0;
    public int computerRound = 0;

    public char lastPlayedCard;
    public int topDeckCard;

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
        playerDZ = GameObject.Find("PDZ");

        playButton = GameObject.Find("PlayButton");
        clearButton = GameObject.Find("ClearButton");
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
        float zOffset = 52;

        for (int i = 0; i < 52; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(200, 540, zOffset), Quaternion.identity);
            newCard.name = deck[i];


            zOffset = zOffset - 1;
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
            {
                GameObject.Find(deck[i]).transform.position = new Vector3(760 + xOffset1, 380, 30);
                playerLastUpCards.Add(deck[i]);
                xOffset1 = xOffset1 + 200;
            }
            else
            {
                GameObject.Find(deck[i]).transform.position = new Vector3(1160 - xOffset2, 700, 30);
                computerLastUpCards.Add(deck[i]);
                xOffset2 = xOffset2 + 200;
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
        topDeckCard = 18;
        SortPlayerCards();
        PickEndOfGameCards();
    }

    void SortPlayerCards()
    {
        int n = 0;
        List<int> playerCards = new List<int>();
        for (int i = 6; i < 18; i = i + 2)
        {
            playerCards.Add(cardValue[i]);
            n++;
        }
        playerCards.Sort((a, b) => a.CompareTo(b));

        List<char> cardValues = new List<char>();
        for (int i = 0; i < 6; i++)
        {
            switch (playerCards[i])
            {
                case 2:
                    cardValues.Add('2');
                    break;
                case 3:
                    cardValues.Add('3');
                    break;
                case 4:
                    cardValues.Add('4');
                    break;
                case 5:
                    cardValues.Add('5');
                    break;
                case 6:
                    cardValues.Add('6');
                    break;
                case 7:
                    cardValues.Add('7');
                    break;
                case 8:
                    cardValues.Add('8');
                    break;
                case 9:
                    cardValues.Add('9');
                    break;
                case 10:
                    cardValues.Add('T');
                    break;
                case 11:
                    cardValues.Add('J');
                    break;
                case 12:
                    cardValues.Add('Q');
                    break;
                case 13:
                    cardValues.Add('K');
                    break;
                default:
                    cardValues.Add('A');
                    break;
            }
        }

        List<string> sortedCards = new List<string>();
        bool x = true;
        while (cardValues.Count > 0)
        {
            for (int i = 6; i < 18; i = i + 2)
            {
                if (cardValues[0] == deck[i][0])
                {
                    if (sortedCards.Count == 0)
                    {
                        sortedCards.Add(deck[i]);
                        cardValues.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        foreach (var m in sortedCards)
                        {
                            
                                if (m == deck[i])
                                {
                                    x = false;
                                    break;
                                }

                        }
                        if (x == true)
                        {
                            sortedCards.Add(deck[i]);
                            cardValues.RemoveAt(0);
                            x = true;
                            break;
                        }
                        else
                        {
                            x = true;
                        }
                    }
                    
                }
            }
        }
        for (int t = 0; t < 6; t++)
        {
            Debug.Log(sortedCards[t]);
        }

        for(int z = 0; z < 6; z++)
        {
            GameObject.Find(sortedCards[z]).transform.SetParent(canvas.transform, true);
        }
        for (int y = 0; y < 6; y++)
        {
            GameObject.Find(sortedCards[y]).transform.SetParent(playerArea.transform, true);
        }
    }

    void PickEndOfGameCards()
    {
        for (int i = 6; i < 18; i = i + 2)
        {
            GameObject.Find(deck[i]).GetComponent<UpdateCardSprite>().faceUp = true;
            GameObject.Find(deck[i]).GetComponent<DragDrop>().isDraggable = true;
        }
    }

    public void OnClearButton()
    {
        while (playerDZ.transform.childCount != 0)
        {
            GameObject.Find(playerDZ.transform.GetChild(0).name).transform.SetParent(playerArea.transform, false);
        }
    }

    public void OnPlayButton()
    {
        if (playerTurn)
        {
            string tempCardName = "";
            int xOffset = 0;
            if (playerRound == 0)
            {
                if (playerDZ.transform.childCount == 3)
                {
                    while (playerDZ.transform.childCount != 0)
                    {
                        tempCardName = playerDZ.transform.GetChild(0).name;
                        GameObject.Find(tempCardName).transform.SetParent(canvas.transform, false);
                        playerFinalCards.Add(tempCardName);
                        GameObject.Find(tempCardName).transform.position = new Vector3(760 + xOffset, 380, 30);
                        GameObject.Find(tempCardName).GetComponent<DragDrop>().isDraggable = false;
                        xOffset += 200;
                    }
                    StartCoroutine(ComputerTurn());
                    playerRound++;
                }
            }
            else if (playerRound == 1)
            {
                if (playerDZ.transform.childCount == 0)
                {
                    if (topDeckCard <= 51)
                    {
                        FlipTopCard(1);
                        
                    }
                    else
                    {
                        lastPlayedCard = 'N';
                        while (playedCards.Count != 0)
                        {
                            GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, false);
                            GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = true;
                            playedCards.RemoveAt(0);
                        }
                        StartCoroutine(ComputerTurn());
                    }
                }
                else
                {
                    char cardV = playerDZ.transform.GetChild(0).name[0];
                    int cardsPlayed = playerDZ.transform.childCount;
                    while (playerDZ.transform.childCount != 0)
                    {
                        string tempName = playerDZ.transform.GetChild(0).name;
                        GameObject.Find(tempName).transform.SetParent(canvas.transform, true);
                        GameObject.Find(tempName).transform.position = new Vector3(420, 540, 0);
                        GameObject.Find(tempName).GetComponent<UpdateCardSprite>().faceUp = true;
                        GameObject.Find(tempName).GetComponent<DragDrop>().isDraggable = false;
                        playedCards.Add(tempName);
                    }
                    bool check = CheckCard(cardV);
                    if (cardV == 'T')
                    {
                        StartCoroutine(WaitOneSec());
                        lastPlayedCard = 'N';
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        if (playerArea.transform.childCount == 0 && playerLastUpCards.Count == 0)
                        {
                            PlayerWins();
                        }
                        else if (playerArea.transform.childCount == 0 && playerFinalCards.Count == 0)
                        {
                            if (playerLastUpCards.Count != 0)
                            {
                                for (int i = 0; i < playerLastUpCards.Count; i++)
                                {
                                    GameObject.Find(playerLastUpCards[i]).transform.SetParent(canvas.transform, true);
                                    GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop>().isDraggable = true;
                                }
                            }

                            playerRound = 2;
                        }
                        else if (playerArea.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (playerFinalCards.Count != 0)
                            {
                                GameObject.Find(playerFinalCards[0]).transform.SetParent(playerArea.transform, true);
                                GameObject.Find(playerFinalCards[0]).GetComponent<UpdateCardSprite>().faceUp = true;
                                GameObject.Find(playerFinalCards[0]).GetComponent<DragDrop>().isDraggable = true;
                                playerFinalCards.RemoveAt(0);
                            }
                        }
                        else if (playerArea.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsPlayed)
                            {
                                if (topDeckCard <= 51 && playerArea.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(playerArea.transform, true);
                                    GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite>().faceUp = true;
                                    GameObject.Find(deck[topDeckCard]).GetComponent<DragDrop>().isDraggable = true;
                                    topDeckCard++;
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }
                    else if (check == false)
                    {
                        lastPlayedCard = 'N';
                        while (playedCards.Count != 0)
                        {
                            GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, false);
                            GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = true;
                            playedCards.RemoveAt(0);
                        }
                        StartCoroutine(ComputerTurn());
                    }
                    else
                    {
                        lastPlayedCard = cardV;
                        if (playerArea.transform.childCount == 0 && playerLastUpCards.Count == 0)
                        {
                            PlayerWins();
                        }
                        else if (playerArea.transform.childCount == 0 && playerFinalCards.Count == 0)
                        {
                            if (playerLastUpCards.Count != 0)
                            {
                                for (int i = 0; i < playerLastUpCards.Count; i++)
                                {
                                    GameObject.Find(playerLastUpCards[i]).transform.SetParent(canvas.transform, true);
                                    GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop>().isDraggable = true;
                                }
                            }

                            playerRound = 2;
                            
                        }
                        else if (playerArea.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (playerFinalCards.Count != 0)
                            {
                                GameObject.Find(playerFinalCards[0]).transform.SetParent(playerArea.transform, true);
                                GameObject.Find(playerFinalCards[0]).GetComponent<UpdateCardSprite>().faceUp = true;
                                GameObject.Find(playerFinalCards[0]).GetComponent<DragDrop>().isDraggable = true;
                                playerFinalCards.RemoveAt(0);
                            }
                            
                        }
                        else if (playerArea.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsPlayed)
                            {
                                if (topDeckCard <= 51 && playerArea.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(playerArea.transform, true);
                                    GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite>().faceUp = true;
                                    GameObject.Find(deck[topDeckCard]).GetComponent<DragDrop>().isDraggable = true;
                                    topDeckCard++;
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            
                        }

                        if (!(playerArea.transform.childCount == 0 && playerLastUpCards.Count == 0))
                        {
                            StartCoroutine(ComputerTurn());
                        }
                    }
                    
                }
                
            }
            else if (playerRound == 2)
            {
                if (playerDZ.transform.childCount == 0)
                {
                    lastPlayedCard = 'N';
                    while (playedCards.Count != 0)
                    {
                        GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, false);
                        GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = true;
                        playedCards.RemoveAt(0);
                    }
                    if (playerLastUpCards.Count != 0)
                    {
                        for (int i = 0; i < playerLastUpCards.Count; i++)
                        {
                            GameObject.Find(playerLastUpCards[i]).transform.SetParent(canvas.transform.parent, true);
                            GameObject.Find(playerLastUpCards[i]).transform.position = new Vector3(760 + (200 * i), 380, 0);
                            GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop>().isDraggable = false;
                        }
                    }
                    playerRound = 1;
                    StartCoroutine(ComputerTurn());
                }
                else
                {
                    char cardV = playerDZ.transform.GetChild(0).name[0];

                    string tempName = playerDZ.transform.GetChild(0).name;
                    GameObject.Find(tempName).transform.SetParent(canvas.transform, true);
                    GameObject.Find(tempName).transform.position = new Vector3(420, 540, 0);
                    GameObject.Find(tempName).GetComponent<UpdateCardSprite>().faceUp = true;
                    GameObject.Find(tempName).GetComponent<DragDrop>().isDraggable = false;
                    playedCards.Add(tempName);
                    playerLastUpCards.Remove(tempName);

                    bool check = CheckCard(cardV);
                    if (check == false)
                    {
                        lastPlayedCard = 'N';
                        while (playedCards.Count != 0)
                        {
                            GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, false);
                            GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = true;
                            playedCards.RemoveAt(0);
                        }
                        if (playerLastUpCards.Count != 0)
                        {
                            
                            for (int i = 0; i < playerLastUpCards.Count; i++)
                            {

                                GameObject.Find(playerLastUpCards[i]).transform.SetParent(canvas.transform.parent, true);
                                GameObject.Find(playerLastUpCards[i]).transform.position = new Vector3(760 + (200 * i), 380, 0);
                                GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop>().isDraggable = false;
                            }
                        }
                        
                        playerRound = 1;
                        StartCoroutine(ComputerTurn());
                    }
                    else
                    {
                        if (tempName[0] == 'T')
                        {
                            if (playerLastUpCards.Count == 0)
                            {
                                PlayerWins();
                            }
                            StartCoroutine(WaitOneSec());
                            while (playedCards.Count != 0)
                            {
                                string tempN = playedCards[0];
                                Destroy(GameObject.Find(tempN));
                                playedCards.RemoveAt(0);
                            }
                            lastPlayedCard = 'N';
                        }
                        else
                        {
                            lastPlayedCard = cardV;

                            if (playerLastUpCards.Count == 0)
                            {
                                PlayerWins();
                            }
                            StartCoroutine(ComputerTurn());
                        }
                        
                    }
                    
                }
            }
        }
        
    }

    public void FlipTopCard(int who)
    {
        int num = topDeckCard;
        topDeckCard++;
        GameObject.Find(deck[num]).transform.SetParent(canvas.transform, true);
        GameObject.Find(deck[num]).transform.position = new Vector3(420, 540, 0);
        GameObject.Find(deck[num]).GetComponent<UpdateCardSprite>().faceUp = true;
        playedCards.Add(deck[num]);

        bool check = CheckCardFlip(num);
        if (check == false)
        {
            if (who == 1)
            {
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, false);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = true;
                    playedCards.RemoveAt(0);
                }
                lastPlayedCard = 'N';
                StartCoroutine(ComputerTurn()); 
            }
            else if (who == 2)
            {
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computerArea.transform, false);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite>().faceUp = false;
                    playedCards.RemoveAt(0);
                }
                lastPlayedCard = 'N';
            }
            
        }
        else
        {
            if (deck[num][0] != 'T')
            {
                lastPlayedCard = deck[num][0];
                if (who == 1)
                {
                    StartCoroutine(ComputerTurn());
                }
                
                
            }
            else
            {
                StartCoroutine(WaitOneSec());
                while (playedCards.Count != 0)
                {
                    string tempN = playedCards[0];
                    Destroy(GameObject.Find(tempN));
                    playedCards.RemoveAt(0);
                }
                lastPlayedCard = 'N';
                if (who == 2)
                {
                    StartCoroutine(ComputerTurn());
                }
            }

        }
        
    }

    public bool CheckCard(char cardValue)
    {
        bool checkGood;
        if (lastPlayedCard == '5')
        {
            if (cardValue == '2' || cardValue == '3' || cardValue == '4' || cardValue == '5' || cardValue == 'T')
            {
                checkGood = true;
            }
            else
            {
                checkGood = false;
            }
        }
        else if (lastPlayedCard == 'T' || lastPlayedCard == 'N' || lastPlayedCard == '2' || lastPlayedCard == '3')
        {
            checkGood = true;
        }
        else
        {
            switch (lastPlayedCard)
            {
                case '4':
                    if (cardValue == '2' || cardValue == '4' || cardValue == '5' || cardValue == '6' || cardValue == '7' || cardValue == '8' || cardValue == '9' || cardValue == 'T' || cardValue == 'J' || cardValue == 'Q' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '6':
                    if (cardValue == '2' || cardValue == '5' || cardValue == '6' || cardValue == '7' || cardValue == '8' || cardValue == '9' || cardValue == 'T' || cardValue == 'J' || cardValue == 'Q' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '7':
                    if (cardValue == '2' || cardValue == '5' || cardValue == '7' || cardValue == '8' || cardValue == '9' || cardValue == 'T' || cardValue == 'J' || cardValue == 'Q' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '8':
                    if (cardValue == '2' || cardValue == '5' || cardValue == '8' || cardValue == '9' || cardValue == 'T' || cardValue == 'J' || cardValue == 'Q' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '9':
                    if (cardValue == '2' || cardValue == '5' || cardValue == '9' || cardValue == 'T' || cardValue == 'J' || cardValue == 'Q' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case 'J':
                    if (cardValue == '2' || cardValue == '5' || cardValue == 'T' || cardValue == 'J' || cardValue == 'Q' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case 'Q':
                    if (cardValue == '2' || cardValue == '5' || cardValue == 'T' || cardValue == 'Q' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break; 
                case 'K':
                    if (cardValue == '2' || cardValue == '5' || cardValue == 'T' || cardValue == 'K' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                default:
                    if (cardValue == '2' || cardValue == '5' || cardValue == 'T' || cardValue == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
            }
        }
        return checkGood;
    }

    public bool CheckCardFlip(int newCard) 
    {
        bool checkGood;
        if (lastPlayedCard == '5')
        {
            if (deck[newCard][0] == '2' || deck[newCard][0] == '3' || deck[newCard][0] == '4' || deck[newCard][0] == '5' || deck[newCard][0] == 'T')
            {
                checkGood = true;
            }
            else
            {
                checkGood = false;
            }
        }
        else if (lastPlayedCard == 'T' || lastPlayedCard == 'N' || lastPlayedCard == '2' || lastPlayedCard == '3')
        {
            checkGood = true;
        }
        else
        {
            switch (lastPlayedCard)
            {
                case '4':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '4' || deck[newCard][0] == '5' || deck[newCard][0] == '6' || deck[newCard][0] == '7' || deck[newCard][0] == '8' || deck[newCard][0] == '9' || deck[newCard][0] == 'T' || deck[newCard][0] == 'J' || deck[newCard][0] == 'Q' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '6':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == '6' || deck[newCard][0] == '7' || deck[newCard][0] == '8' || deck[newCard][0] == '9' || deck[newCard][0] == 'T' || deck[newCard][0] == 'J' || deck[newCard][0] == 'Q' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '7':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == '7' || deck[newCard][0] == '8' || deck[newCard][0] == '9' || deck[newCard][0] == 'T' || deck[newCard][0] == 'J' || deck[newCard][0] == 'Q' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '8':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == '8' || deck[newCard][0] == '9' || deck[newCard][0] == 'T' || deck[newCard][0] == 'J' || deck[newCard][0] == 'Q' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case '9':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == '9' || deck[newCard][0] == 'T' || deck[newCard][0] == 'J' || deck[newCard][0] == 'Q' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case 'J':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == 'T' || deck[newCard][0] == 'J' || deck[newCard][0] == 'Q' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case 'Q':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == 'T' || deck[newCard][0] == 'Q' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                case 'K':
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == 'T' || deck[newCard][0] == 'K' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
                default:
                    if (deck[newCard][0] == '2' || deck[newCard][0] == '5' || deck[newCard][0] == 'T' || deck[newCard][0] == 'A')
                    {
                        checkGood = true;
                    }
                    else
                    {
                        checkGood = false;
                    }
                    break;
            }
        }
        return checkGood;
    }
        

    public IEnumerator ComputerTurn()
    {
        playerTurn = false;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop>().isDraggable = false;
        }
        yield return new WaitForSeconds(0.8f);
        if (computerRound == 0)
        {
            FindBestLastCards();
            computerRound++;
            GameObject.Find(deck[topDeckCard]).transform.SetParent(canvas.transform, true);
            GameObject.Find(deck[topDeckCard]).transform.position = new Vector3(420, 540, 0);
            GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite>().faceUp = true;
            lastPlayedCard = deck[topDeckCard][0];
            playedCards.Add(deck[topDeckCard]);
            if (lastPlayedCard == 'T')
            {
                StartCoroutine(WaitOneSec());
                Destroy(GameObject.Find(deck[topDeckCard]));
                playedCards.RemoveAt(0);
                lastPlayedCard = 'N';
            }
            topDeckCard++;
        }
        else if (computerRound == 1)
        {
            bool pos = CanComputerPlayHand();
            if (pos == true)
            {
                string bestCard = BestComputerCard();
                GameObject.Find(bestCard).transform.SetParent(canvas.transform, true);
                GameObject.Find(bestCard).transform.position = new Vector3(420, 540, 0);
                GameObject.Find(bestCard).GetComponent<UpdateCardSprite>().faceUp = true;
                lastPlayedCard = bestCard[0];
                playedCards.Add(bestCard);
                if (bestCard[0] == 'T')
                {
                    StartCoroutine(WaitOneSec());
                    while (playedCards.Count != 0)
                    {
                        string tempN = playedCards[0];
                        Destroy(GameObject.Find(tempN));
                        playedCards.RemoveAt(0);
                    }
                    lastPlayedCard = 'N';
                    if (computerArea.transform.childCount == 0 && computerLastUpCards.Count == 0)
                    {
                        ComputerWins();
                    }
                    else if (computerArea.transform.childCount == 0 && computerFinalCards.Count == 0)
                    {
                        playerRound = 2;
                        StartCoroutine(ComputerTurn());
                    }
                    else if (computerArea.transform.childCount == 0 && topDeckCard == 52)
                    {
                        while (computerFinalCards.Count != 0)
                        {
                            GameObject.Find(computerFinalCards[0]).transform.SetParent(computerArea.transform, true);
                            GameObject.Find(computerFinalCards[0]).GetComponent<UpdateCardSprite>().faceUp = false;
                            GameObject.Find(computerFinalCards[0]).GetComponent<DragDrop>().isDraggable = false;
                            computerFinalCards.RemoveAt(0);
                        }
                        StartCoroutine(ComputerTurn());
                    }
                    else if (computerArea.transform.childCount < 3)
                    {
                        if (topDeckCard <= 51)
                        {
                            GameObject.Find(deck[topDeckCard]).transform.SetParent(computerArea.transform, true);
                            topDeckCard++;
                        }
                        StartCoroutine(ComputerTurn());
                    }
                }
                else
                {
                    if (computerArea.transform.childCount == 0 && computerLastUpCards.Count == 0)
                    {
                        ComputerWins();
                    }
                    else if (computerArea.transform.childCount == 0 && computerFinalCards.Count == 0)
                    {
                        playerRound = 2;
                    }
                    else if (computerArea.transform.childCount == 0 && topDeckCard == 52)
                    {
                        while (computerFinalCards.Count != 0)
                        {
                            GameObject.Find(computerFinalCards[0]).transform.SetParent(computerArea.transform, true);
                            GameObject.Find(computerFinalCards[0]).GetComponent<UpdateCardSprite>().faceUp = false;
                            GameObject.Find(computerFinalCards[0]).GetComponent<DragDrop>().isDraggable = false;
                            computerFinalCards.RemoveAt(0);
                        }
                    }
                    else if (computerArea.transform.childCount < 3)
                    {
                        if (topDeckCard <= 51)
                        {
                            GameObject.Find(deck[topDeckCard]).transform.SetParent(computerArea.transform, true);
                            topDeckCard++;
                        }
                    }
                }
                
                
            }
            else if (pos == false)
            {
                if (topDeckCard <= 51)
                {
                    FlipTopCard(2);
                }
                else
                {
                    lastPlayedCard = 'N';
                    while (playedCards.Count != 0)
                    {
                        GameObject.Find(playedCards[0]).transform.SetParent(computerArea.transform, false);
                        GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = false;
                        GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite>().faceUp = false;
                        playedCards.RemoveAt(0);
                    }
                }
            }
        }
        else if (computerRound == 2)
        {
            Random rand = new System.Random();
            int num = rand.Next(computerLastUpCards.Count - 1);
            string cardN = computerLastUpCards[num];
            computerLastUpCards.RemoveAt(num);
            GameObject.Find(cardN).transform.SetParent(canvas.transform, true);
            GameObject.Find(cardN).transform.position = new Vector3(420, 540, 0);
            GameObject.Find(cardN).GetComponent<UpdateCardSprite>().faceUp = true;
            playedCards.Add(cardN);

            bool check = CheckCard(cardN[0]);
            if (check == false)
            {
                lastPlayedCard = 'N';
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computerArea.transform, false);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite>().faceUp = false;
                    playedCards.RemoveAt(0);
                }

                computerRound = 1;
            }
            else
            {
                if (cardN[0] == 'T')
                {
                    StartCoroutine(WaitOneSec());
                    while (playedCards.Count != 0)
                    {
                        string tempN = playedCards[0];
                        Destroy(GameObject.Find(tempN));
                        playedCards.RemoveAt(0);
                    }
                    lastPlayedCard = 'N';
                    if (computerLastUpCards.Count != 0)
                    {
                        StartCoroutine(ComputerTurn());
                    }
                }
                else
                {
                    lastPlayedCard = cardN[0];
                }
                

                if (computerLastUpCards.Count == 0)
                {
                    ComputerWins();
                }

            }
        }
        playerTurn = true;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop>().isDraggable = true;
        }

    }

    public string BestComputerCard()
    {
        string bestCard = "";
        List<string> playable = new List<string>();

        for (int i = 0; i < computerArea.transform.childCount; i++)
        {
            char cardV = computerArea.transform.GetChild(i).name[0];
            switch (lastPlayedCard)
            {
                case '4':
                    if (cardV == '2' || cardV == '4' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    break;
                case '6':
                    if (cardV == '2' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    break;
                case '7':
                    if (cardV == '2' || cardV == '5' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    break;
                case '8':
                    if (cardV == '2' || cardV == '5' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    break;
                case '9':
                    if (cardV == '2' || cardV == '5' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    
                    break;
                case 'J':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    
                    break;
                case 'Q':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    
                    break;
                case 'K':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    
                    break;
                case 'A':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'A')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    
                    break;
                case '5':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == '4' || cardV == '3')
                    {
                        playable.Add(computerArea.transform.GetChild(i).name);
                    }
                    
                    break;
                default:
                    playable.Add(computerArea.transform.GetChild(i).name);
                    break;
            }

            
        }

        if (playable.Count == 1)
        {
            bestCard = playable[0];
        }
        else
        {
            Random rand = new System.Random();
            int num = rand.Next(playable.Count - 1);
            bestCard = playable[num];
        }

        return bestCard;
    }

    public bool CanComputerPlayHand()
    {
        bool possible = false;

        for (int i = 0; i < computerArea.transform.childCount; i++)
        {
            char cardV = computerArea.transform.GetChild(i).name[0];
            switch (lastPlayedCard)
            {
                case '4':
                    if (cardV == '2' || cardV == '4' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case '6':
                    if (cardV == '2' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case '7':
                    if (cardV == '2' || cardV == '5' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case '8':
                    if (cardV == '2' || cardV == '5' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case '9':
                    if (cardV == '2' || cardV == '5' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case 'J':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case 'Q':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case 'K':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'K' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case 'A':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'A')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                case '5':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == '4' || cardV == '3')
                    {
                        possible = true;
                        break;
                    }
                    else
                    {
                        possible = false;
                    }
                    break;
                default:
                    possible = true;
                    break;
            }
            if (possible == true)
            {
                break;
            }
        }

        return possible;
    }

    public void FindBestLastCards()
    {
        char[] rankedValues = new char[] { 'T', '5', '2', 'A', 'K', 'Q', 'J', '9', '8', '7', '6', '4', '3'};
        List<string> bestCards = new List<string>();
        string tempName = "";

        foreach (char v in rankedValues)
        {
            for (int i = 0; i < 6; i++)
            {
                tempName = computerArea.transform.GetChild(i).name;
                if (v == tempName[0])
                {
                    bestCards.Add(computerArea.transform.GetChild(i).name);
                }
                if (bestCards.Count == 3)
                {
                    break;
                }
            }
            if (bestCards.Count == 3)
            {
                break;
            }
        }

        int xOffset = 0;
        for (int j = 0; j < 3; j++)
        {
            GameObject.Find(bestCards[j]).transform.SetParent(canvas.transform, false);
            GameObject.Find(bestCards[j]).transform.position = new Vector3(1160 - xOffset, 700, 30);
            GameObject.Find(bestCards[j]).GetComponent<UpdateCardSprite>().faceUp = true;
            computerFinalCards.Add(bestCards[j]);
            xOffset = xOffset + 200;
        }
        

    }

    void PlayerWins()
    {
        Debug.Log("Player Won");
    }

    void ComputerWins()
    {
        Debug.Log("Computer Won");
    }

    IEnumerator WaitOneSec()
    {
        playerTurn = false;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop>().isDraggable = false;
        }
        yield return new WaitForSeconds(0.5f);
        playerTurn = true;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop>().isDraggable = true;
        }
    }
}
