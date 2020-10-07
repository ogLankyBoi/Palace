using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using Random = System.Random;

public class Local4PHandler : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject canvas;
    public GameObject playerArea;
    public GameObject computer1Area;
    public GameObject computer2Area;
    public GameObject computer3Area;
    public GameObject playerDZ;
    public GameObject endGameScreen;
    public GameObject endGameText;

    public List<string> playerLastUpCards = new List<string>();
    public List<string> playerFinalCards = new List<string>();
    public List<string> computer1LastUpCards = new List<string>();
    public List<string> computer2LastUpCards = new List<string>();
    public List<string> computer1FinalCards = new List<string>();
    public List<string> computer3LastUpCards = new List<string>();
    public List<string> computer3FinalCards = new List<string>();
    public List<string> computer2FinalCards = new List<string>();
    public List<string> playedCards = new List<string>();

    public GameObject playButton;
    public GameObject playButtonText;
    public GameObject clearButton;

    public bool playerTurn = true;
    public int playerRound = 0;
    public int computer1Round = 0;
    public int computer2Round = 0;
    public int computer3Round = 0;

    public char lastPlayedCard;
    public int topDeckCard;

    public List<string> deck;
    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
    public List<int> cardValue;
    public List<string> restOfDeck;

    public SceneChanger sceneChanger;
    public AdManager adManager;

    void Start()
    {
        AssignGameObjects();
        deck = GenerateDeck();
        ShuffleDeck(deck);
        cardValue = AssignCardValues(deck);
        StartCoroutine(Deal());
    }

    void Update()
    {
        if (playerRound > 0 && playerDZ.transform.childCount == 0)
        {
            playButtonText.GetComponent<Text>().text = "Flip";
        }
        else
        {
            playButtonText.GetComponent<Text>().text = "Play";
        }
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
        computer1Area = GameObject.Find("Computer1Area");
        computer2Area = GameObject.Find("Computer2Area");
        computer3Area = GameObject.Find("Computer3Area");
        playerDZ = GameObject.Find("PDZ");

        playButton = GameObject.Find("PlayButton");
        playButtonText = GameObject.Find("PlayButtonText");
        clearButton = GameObject.Find("ClearButton");

        endGameScreen = GameObject.Find("EndGameScreen");
        endGameText = GameObject.Find("EndGameText");
        endGameScreen.SetActive(false);

        sceneChanger = GetComponent<SceneChanger>();
        adManager = GetComponent<AdManager>();
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
        float xOffset3 = 0;
        float xOffset4 = 0;
        float zOffset = 52;

        for (int i = 0; i < 52; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(Screen.width/2 - 100, Screen.height/2 + 30, zOffset), Quaternion.identity);
            newCard.transform.localScale = new Vector3(Screen.width / 75, Screen.width / 75, Screen.width / 75);
            newCard.transform.SetParent(canvas.transform, true);
            newCard.name = deck[i];


            zOffset = zOffset - 1;
        }
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < 12; i++)
        {
            if (i == 0 || i == 4 || i == 8)
            {
                GameObject.Find(deck[i]).transform.position = new Vector3(Screen.width/2 - 200 + xOffset1, Screen.height/2 - 200, 30);
                playerLastUpCards.Add(deck[i]);
                xOffset1 = xOffset1 + 200;
            }
            else if (i == 1 || i == 5 || i == 9)
            {
                GameObject.Find(deck[i]).transform.position = new Vector3((Screen.width*4)/5 - xOffset2, Screen.height/2 + 150 + xOffset2, 30);
                computer1LastUpCards.Add(deck[i]);
                xOffset2 = xOffset2 + 75;
            }
            else if (i == 2 || i == 6 || i == 10)
            {
                GameObject.Find(deck[i]).transform.position = new Vector3(Screen.width/2 + 75 - xOffset3, (Screen.height*4)/5, 30);
                computer2LastUpCards.Add(deck[i]);
                xOffset3 = xOffset3 + 75;
            }
            else
            {
                GameObject.Find(deck[i]).transform.position = new Vector3(Screen.width/5 + 150 - xOffset4, Screen.height/2 + 300 - xOffset4, 30);
                computer3LastUpCards.Add(deck[i]);
                xOffset4 = xOffset4 + 75;
            }
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 12; i < 36; i++)
        {

            if (i == 12 || i == 16 || i == 20 || i == 24 || i == 28 || i == 32)
            {
                GameObject.Find(deck[i]).transform.SetParent(playerArea.transform, true);
            }
            else if (i == 13 || i == 17 || i == 21 || i == 25 || i == 29 || i == 33)
            {
                GameObject.Find(deck[i]).transform.SetParent(computer1Area.transform, true);
            }
            else if (i == 14 || i == 18 || i == 22 || i == 26 || i == 30 || i == 34)
            {
                GameObject.Find(deck[i]).transform.SetParent(computer2Area.transform, true);
            }
            else
            {
                GameObject.Find(deck[i]).transform.SetParent(computer3Area.transform, true);
            }

            yield return new WaitForSeconds(0.1f);
        }
        topDeckCard = 36;
        PickEndOfGameCards();
    }

    public void OnSortButton()
    {
        List<int> cardValues = new List<int>();

        for (int i = 0; i < playerArea.transform.childCount; i++)
        {
            string cardName = playerArea.transform.GetChild(i).name;
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

        cardValues.Sort((a, b) => a.CompareTo(b));
        List<char> charV = new List<char>();
        for (int i = 0; i < playerArea.transform.childCount; i++)
        {
            switch (cardValues[i])
            {
                case 2:
                    charV.Add('2');
                    break;
                case 3:
                    charV.Add('3');
                    break;
                case 4:
                    charV.Add('4');
                    break;
                case 5:
                    charV.Add('5');
                    break;
                case 6:
                    charV.Add('6');
                    break;
                case 7:
                    charV.Add('7');
                    break;
                case 8:
                    charV.Add('8');
                    break;
                case 9:
                    charV.Add('9');
                    break;
                case 10:
                    charV.Add('T');
                    break;
                case 11:
                    charV.Add('J');
                    break;
                case 12:
                    charV.Add('Q');
                    break;
                case 13:
                    charV.Add('K');
                    break;
                default:
                    charV.Add('A');
                    break;
            }
        }

        List<string> sortedCards = new List<string>();
        bool x = true;
        while (charV.Count > 0)
        {
            for (int i = 0; i < playerArea.transform.childCount; i++)
            {
                if (charV[0] == playerArea.transform.GetChild(i).name[0])
                {
                    if (sortedCards.Count == 0)
                    {
                        sortedCards.Add(playerArea.transform.GetChild(i).name);
                        charV.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        foreach (var m in sortedCards)
                        {

                            if (m == playerArea.transform.GetChild(i).name)
                            {
                                x = false;
                                break;
                            }

                        }
                        if (x == true)
                        {
                            sortedCards.Add(playerArea.transform.GetChild(i).name);
                            charV.RemoveAt(0);
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

        for (int z = 0; z < sortedCards.Count; z++)
        {
            GameObject.Find(sortedCards[z]).transform.SetParent(canvas.transform, true);
        }
        for (int y = 0; y < sortedCards.Count; y++)
        {
            GameObject.Find(sortedCards[y]).transform.SetParent(playerArea.transform, true);
        }
    }

    void PickEndOfGameCards()
    {
        for (int i = 0; i < 6; i++)
        {
            playerArea.transform.GetChild(i).GetComponent<UpdateCardSprite4P>().faceUp = true;
            playerArea.transform.GetChild(i).GetComponent<DragDrop4P>().isDraggable = true;
        }
    }

    public void OnClearButton()
    {
        while (playerDZ.transform.childCount != 0)
        {
            GameObject.Find(playerDZ.transform.GetChild(0).name).transform.SetParent(playerArea.transform, true);
        }
    }

    public void OnActualPlayButton()
    {
        StartCoroutine(OnPlayButton());
    }

    public IEnumerator OnPlayButton()
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
                        GameObject.Find(tempCardName).transform.SetParent(canvas.transform, true);
                        playerFinalCards.Add(tempCardName);
                        GameObject.Find(tempCardName).transform.position = new Vector3(Screen.width/2 - 200 + xOffset, Screen.height/2 - 200, 30);
                        GameObject.Find(tempCardName).GetComponent<DragDrop4P>().isDraggable = false;
                        xOffset += 200;
                    }
                    StartCoroutine(Computer1Turn());
                    playerRound++;
                    
                }
            }
            else if (playerRound == 1)
            {
                if (playerDZ.transform.childCount == 0)
                {
                    if (topDeckCard <= 51)
                    {
                        StartCoroutine(FlipTopCard(1));

                    }
                    else
                    {
                        lastPlayedCard = 'N';
                        while (playedCards.Count != 0)
                        {
                            GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, true);
                            GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
                            playedCards.RemoveAt(0);
                        }
                        StartCoroutine(Computer1Turn());
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
                        GameObject.Find(tempName).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
                        GameObject.Find(tempName).GetComponent<UpdateCardSprite4P>().faceUp = true;
                        GameObject.Find(tempName).GetComponent<DragDrop4P>().isDraggable = false;
                        playedCards.Add(tempName);
                    }
                    bool check = CheckCard(cardV);
                    
                    if (playedCards.Count >= 4)
                    {
                        if (cardV == 'T' || cardsPlayed == 4 || (cardsPlayed == 3 && playedCards[playedCards.Count - 4][0] == cardV) || (cardsPlayed == 2 && playedCards[playedCards.Count - 3][0] == cardV && playedCards[playedCards.Count - 4][0] == cardV) || (cardsPlayed == 1 && playedCards[playedCards.Count - 2][0] == cardV && playedCards[playedCards.Count - 3][0] == cardV && playedCards[playedCards.Count - 4][0] == cardV))
                        {
                            for (int i = 1; i < 5; i++)
                            {
                                Debug.Log(playedCards[playedCards.Count - i]);
                            }
                            yield return new WaitForSeconds(0.5f);
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
                                        GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = true;
                                    }
                                }

                                playerRound = 2;
                            }
                            else if (playerArea.transform.childCount == 0 && topDeckCard == 52)
                            {
                                while (playerFinalCards.Count != 0)
                                {
                                    GameObject.Find(playerFinalCards[0]).transform.SetParent(playerArea.transform, true);
                                    GameObject.Find(playerFinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                    GameObject.Find(playerFinalCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
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
                                        GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                        GameObject.Find(deck[topDeckCard]).GetComponent<DragDrop4P>().isDraggable = true;
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
                            yield return new WaitForSeconds(0.3f);
                            lastPlayedCard = 'N';
                            while (playedCards.Count != 0)
                            {
                                GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, true);
                                GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
                                playedCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer1Turn());
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
                                        GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = true;
                                    }
                                }

                                playerRound = 2;

                            }
                            else if (playerArea.transform.childCount == 0 && topDeckCard == 52)
                            {
                                while (playerFinalCards.Count != 0)
                                {
                                    GameObject.Find(playerFinalCards[0]).transform.SetParent(playerArea.transform, true);
                                    GameObject.Find(playerFinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                    GameObject.Find(playerFinalCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
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
                                        GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                        GameObject.Find(deck[topDeckCard]).GetComponent<DragDrop4P>().isDraggable = true;
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
                                StartCoroutine(Computer1Turn());
                            }
                        }
                    }
                    else
                    {
                        if (cardV == 'T' || cardsPlayed == 4)
                        {
                            yield return new WaitForSeconds(0.5f);
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
                                        GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = true;
                                    }
                                }

                                playerRound = 2;
                            }
                            else if (playerArea.transform.childCount == 0 && topDeckCard == 52)
                            {
                                while (playerFinalCards.Count != 0)
                                {
                                    GameObject.Find(playerFinalCards[0]).transform.SetParent(playerArea.transform, true);
                                    GameObject.Find(playerFinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                    GameObject.Find(playerFinalCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
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
                                        GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                        GameObject.Find(deck[topDeckCard]).GetComponent<DragDrop4P>().isDraggable = true;
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
                            yield return new WaitForSeconds(0.3f);
                            lastPlayedCard = 'N';
                            while (playedCards.Count != 0)
                            {
                                GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, true);
                                GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
                                playedCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer1Turn());
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
                                        GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = true;
                                    }
                                }

                                playerRound = 2;

                            }
                            else if (playerArea.transform.childCount == 0 && topDeckCard == 52)
                            {
                                while (playerFinalCards.Count != 0)
                                {
                                    GameObject.Find(playerFinalCards[0]).transform.SetParent(playerArea.transform, true);
                                    GameObject.Find(playerFinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                    GameObject.Find(playerFinalCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
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
                                        GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                                        GameObject.Find(deck[topDeckCard]).GetComponent<DragDrop4P>().isDraggable = true;
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
                                StartCoroutine(Computer1Turn());
                            }
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
                        GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, true);
                        GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
                        playedCards.RemoveAt(0);
                    }
                    if (playerLastUpCards.Count != 0)
                    {
                        for (int i = 0; i < playerLastUpCards.Count; i++)
                        {
                            GameObject.Find(playerLastUpCards[i]).transform.SetParent(canvas.transform, true);
                            GameObject.Find(playerLastUpCards[i]).transform.position = new Vector3(Screen.width/2 - 200 + (200 * i), Screen.height/2 - 200, 0);
                            GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = false;
                        }
                    }
                    playerRound = 1;
                    StartCoroutine(Computer1Turn());
                }
                else
                {
                    char cardV = playerDZ.transform.GetChild(0).name[0];

                    string tempName = playerDZ.transform.GetChild(0).name;
                    GameObject.Find(tempName).transform.SetParent(canvas.transform, true);
                    GameObject.Find(tempName).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
                    GameObject.Find(tempName).GetComponent<UpdateCardSprite4P>().faceUp = true;
                    GameObject.Find(tempName).GetComponent<DragDrop4P>().isDraggable = false;
                    playedCards.Add(tempName);
                    playerLastUpCards.Remove(tempName);

                    bool check = CheckCard(cardV);
                    if (check == false)
                    {
                        yield return new WaitForSeconds(0.3f);
                        lastPlayedCard = 'N';
                        while (playedCards.Count != 0)
                        {
                            GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, true);
                            GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
                            playedCards.RemoveAt(0);
                        }
                        if (playerLastUpCards.Count != 0)
                        {

                            for (int i = 0; i < playerLastUpCards.Count; i++)
                            {

                                GameObject.Find(playerLastUpCards[i]).transform.SetParent(canvas.transform, true);
                                GameObject.Find(playerLastUpCards[i]).transform.position = new Vector3(Screen.width/2 - 200 + (200 * i), Screen.height/2 - 200, 0);
                                GameObject.Find(playerLastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = false;
                            }
                        }

                        playerRound = 1;
                        StartCoroutine(Computer1Turn());
                    }
                    else
                    {
                        if (playedCards.Count >= 4)
                        {
                            if (tempName[0] == 'T' || (playedCards[playedCards.Count - 2][0] == tempName[0] && playedCards[playedCards.Count - 3][0] == tempName[0] && playedCards[playedCards.Count - 4][0] == tempName[0]))
                            {
                                for (int i = 1; i < 5; i++)
                                {
                                    Debug.Log(playedCards[playedCards.Count - i]);
                                }
                                yield return new WaitForSeconds(0.5f);
                                if (playerLastUpCards.Count == 0)
                                {
                                    PlayerWins();
                                }

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
                                else
                                {
                                    StartCoroutine(Computer1Turn());
                                }
                            }
                        }
                        else
                        {
                            if (tempName[0] == 'T')
                            {
                                yield return new WaitForSeconds(0.5f);
                                if (playerLastUpCards.Count == 0)
                                {
                                    PlayerWins();
                                }


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
                                else
                                {
                                    StartCoroutine(Computer1Turn());
                                }

                            }
                        }

                    }

                }
            }
        }

    }

    public IEnumerator FlipTopCard(int who)
    {
        int num = topDeckCard;
        topDeckCard++;
        GameObject.Find(deck[num]).transform.SetParent(canvas.transform.parent, true);
        GameObject.Find(deck[num]).transform.SetParent(canvas.transform, true);
        GameObject.Find(deck[num]).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
        GameObject.Find(deck[num]).GetComponent<UpdateCardSprite4P>().faceUp = true;
        playedCards.Add(deck[num]);

        bool check = CheckCardFlip(num);
        if (check == false)
        {
            yield return new WaitForSeconds(0.5f);
            if (who == 1)
            {
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(playerArea.transform, true);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = true;
                    playedCards.RemoveAt(0);
                }
                lastPlayedCard = 'N';
                StartCoroutine(Computer1Turn());
            }
            else if (who == 2)
            {
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computer1Area.transform, true);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                    playedCards.RemoveAt(0);
                }
                lastPlayedCard = 'N';
                StartCoroutine(Computer2Turn());
            }
            else if (who == 3)
            {
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computer2Area.transform, true);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                    playedCards.RemoveAt(0);
                }
                lastPlayedCard = 'N';
            }
            else if (who == 4)
            {
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computer3Area.transform, true);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                    playedCards.RemoveAt(0);
                }
                lastPlayedCard = 'N';
            }
        }
        else
        {
            if (playedCards.Count >= 4)
            {
                if (deck[num][0] == 'T' || (playedCards[playedCards.Count - 2][0] == deck[num][0] && playedCards[playedCards.Count - 3][0] == deck[num][0] && playedCards[playedCards.Count - 4][0] == deck[num][0]))
                {
                    for (int i = 1; i < 5; i++)
                    {
                        Debug.Log(playedCards[playedCards.Count - i]);
                    }
                    yield return new WaitForSeconds(0.5f);
                    while (playedCards.Count != 0)
                    {
                        string tempN = playedCards[0];
                        Destroy(GameObject.Find(tempN));
                        playedCards.RemoveAt(0);
                    }
                    lastPlayedCard = 'N';
                    if (who == 2)
                    {
                        StartCoroutine(Computer1Turn());
                    }
                    else if (who == 3)
                    {
                        StartCoroutine(Computer2Turn());
                    }
                    else if (who == 4)
                    {
                        StartCoroutine(Computer3Turn());
                    }

                }
                else
                {

                    lastPlayedCard = deck[num][0];
                    if (who == 1)
                    {
                        StartCoroutine(Computer1Turn());
                    }
                    else if (who == 2)
                    {
                        StartCoroutine(Computer2Turn());
                    }
                    else if (who == 3)
                    {
                        StartCoroutine(Computer3Turn());
                    }
                }
            }
            else
            {
                if (deck[num][0] == 'T')
                {
                    yield return new WaitForSeconds(0.5f);
                    while (playedCards.Count != 0)
                    {
                        string tempN = playedCards[0];
                        Destroy(GameObject.Find(tempN));
                        playedCards.RemoveAt(0);
                    }
                    lastPlayedCard = 'N';
                    if (who == 2)
                    {
                        StartCoroutine(Computer1Turn());
                    }
                    else if (who == 3)
                    {
                        StartCoroutine(Computer2Turn());
                    }
                    else if (who == 4)
                    {
                        StartCoroutine(Computer3Turn());
                    }


                }
                else
                {
                    lastPlayedCard = deck[num][0];
                    if (who == 1)
                    {
                        StartCoroutine(Computer1Turn());
                    }
                    else if (who == 2)
                    {
                        StartCoroutine(Computer2Turn());
                    }
                    else if (who == 3)
                    {
                        StartCoroutine(Computer3Turn());
                    }
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


    public IEnumerator Computer1Turn()
    {
        playerTurn = false;
        playButton.GetComponent<Button>().interactable = false;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop4P>().isDraggable = false;
        }
        yield return new WaitForSeconds(0.5f);
        if (computer1Round == 0)
        {
            FindBestLastCards1();
            computer1Round++;
            StartCoroutine(Computer2Turn());

        }
        else if (computer1Round == 1)
        {
            bool pos = CanComputer1PlayHand();
            if (pos == true)
            {
                List<string> sameCards = new List<string>();
                string bestCard = BestComputer1Card();

                for (int i = 0; i < computer1Area.transform.childCount; i++)
                {
                    if (bestCard[0] == computer1Area.transform.GetChild(i).name[0])
                    {
                        sameCards.Add(computer1Area.transform.GetChild(i).name);
                    }
                }

                lastPlayedCard = bestCard[0];
                int cardsP = 0;

                while (sameCards.Count != 0)
                {
                    GameObject.Find(sameCards[0]).transform.SetParent(canvas.transform, true);
                    GameObject.Find(sameCards[0]).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
                    GameObject.Find(sameCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                    playedCards.Add(sameCards[0]);
                    sameCards.RemoveAt(0);
                    cardsP++;
                }
                if (playedCards.Count >= 4)
                {
                    if (bestCard[0] == 'T' || cardsP == 4 || (cardsP == 3 && playedCards[playedCards.Count - 4][0] == bestCard[0]) || (cardsP == 2 && playedCards[playedCards.Count - 3][0] == bestCard[0] && playedCards[playedCards.Count - 4][0] == bestCard[0]) || (cardsP == 1 && playedCards[playedCards.Count - 2][0] == bestCard[0] && playedCards[playedCards.Count - 3][0] == bestCard[0] && playedCards[playedCards.Count - 4][0] == bestCard[0]))
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Debug.Log(playedCards[playedCards.Count - i]);
                        }
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer1Area.transform.childCount == 0 && computer1LastUpCards.Count == 0)
                        {
                            Computer1Wins();
                        }
                        else if (computer1Area.transform.childCount == 0 && computer1FinalCards.Count == 0)
                        {
                            computer1Round = 2;
                            StartCoroutine(Computer1Turn());
                        }
                        else if (computer1Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer1FinalCards.Count != 0)
                            {
                                GameObject.Find(computer1FinalCards[0]).transform.SetParent(computer1Area.transform, true);
                                GameObject.Find(computer1FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer1FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer1FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer1Turn());
                        }
                        else if (computer1Area.transform.childCount < 3)
                        {
                            if (topDeckCard <= 51)
                            {
                                GameObject.Find(deck[topDeckCard]).transform.SetParent(computer1Area.transform, true);
                                topDeckCard++;
                            }
                            StartCoroutine(Computer1Turn());
                        }
                    }
                    else
                    {
                        if (computer1Area.transform.childCount == 0 && computer1LastUpCards.Count == 0)
                        {
                            Computer1Wins();
                        }
                        else if (computer1Area.transform.childCount == 0 && computer1FinalCards.Count == 0)
                        {
                            computer1Round = 2;
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer1Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer1FinalCards.Count != 0)
                            {
                                GameObject.Find(computer1FinalCards[0]).transform.SetParent(computer1Area.transform, true);
                                GameObject.Find(computer1FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer1FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer1FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer1Area.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsP)
                            {
                                if (topDeckCard <= 51 && computer1Area.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(computer1Area.transform, true);
                                    topDeckCard++;
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            StartCoroutine(Computer2Turn());
                        }
                        else
                        {
                            StartCoroutine(Computer2Turn());
                        }
                    }
                }
                else
                {
                    if (bestCard[0] == 'T' || cardsP == 4)
                    {
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer1Area.transform.childCount == 0 && computer1LastUpCards.Count == 0)
                        {
                            Computer1Wins();
                        }
                        else if (computer1Area.transform.childCount == 0 && computer1FinalCards.Count == 0)
                        {
                            computer1Round = 2;
                            StartCoroutine(Computer1Turn());
                        }
                        else if (computer1Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer1FinalCards.Count != 0)
                            {
                                GameObject.Find(computer1FinalCards[0]).transform.SetParent(computer1Area.transform, true);
                                GameObject.Find(computer1FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer1FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer1FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer1Turn());
                        }
                        else if (computer1Area.transform.childCount < 3)
                        {
                            if (topDeckCard <= 51)
                            {
                                GameObject.Find(deck[topDeckCard]).transform.SetParent(computer1Area.transform, true);
                                topDeckCard++;
                            }
                            StartCoroutine(Computer1Turn());
                        }
                    }
                    else
                    {
                        if (computer1Area.transform.childCount == 0 && computer1LastUpCards.Count == 0)
                        {
                            Computer1Wins();
                        }
                        else if (computer1Area.transform.childCount == 0 && computer1FinalCards.Count == 0)
                        {
                            computer1Round = 2;
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer1Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer1FinalCards.Count != 0)
                            {
                                GameObject.Find(computer1FinalCards[0]).transform.SetParent(computer1Area.transform, true);
                                GameObject.Find(computer1FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer1FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer1FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer1Area.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsP)
                            {
                                if (topDeckCard <= 51 && computer1Area.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(computer1Area.transform, true);
                                    topDeckCard++;
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            StartCoroutine(Computer2Turn());
                        }
                        else
                        {
                            StartCoroutine(Computer2Turn());
                        }
                    }
                }



            }
            else if (pos == false)
            {

                if (topDeckCard <= 51)
                {
                    StartCoroutine(FlipTopCard(2));
                }
                else
                {

                    lastPlayedCard = 'N';
                    while (playedCards.Count != 0)
                    {
                        GameObject.Find(playedCards[0]).transform.SetParent(computer1Area.transform, true);
                        GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                        GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                        playedCards.RemoveAt(0);
                    }
                }
                StartCoroutine(Computer2Turn());
            }
        }
        else if (computer1Round == 2)
        {
            if (computer1LastUpCards.Count != 0)
            {

                for (int i = 0; i < computer1LastUpCards.Count; i++)
                {

                    GameObject.Find(computer1LastUpCards[i]).transform.SetParent(canvas.transform, true);
                    GameObject.Find(computer1LastUpCards[i]).transform.position = new Vector3((Screen.width*4)/5 - (i * 75), Screen.height/2 + 150 + (i * 75), 0);

                }
            }
            Random rand = new System.Random();

            int num = rand.Next(computer1LastUpCards.Count - 1);

            string cardN = computer1LastUpCards[num];
            computer1LastUpCards.RemoveAt(num);
            GameObject.Find(cardN).transform.SetParent(canvas.transform.parent, true);
            GameObject.Find(cardN).transform.SetParent(canvas.transform, true);
            GameObject.Find(cardN).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
            GameObject.Find(cardN).GetComponent<UpdateCardSprite4P>().faceUp = true;
            playedCards.Add(cardN);

            bool check = CheckCard(cardN[0]);
            if (check == false)
            {
                yield return new WaitForSeconds(0.5f);
                lastPlayedCard = 'N';
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computer1Area.transform, true);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                    playedCards.RemoveAt(0);
                }
                if (computer1LastUpCards.Count != 0)
                {

                    for (int i = 0; i < computer1LastUpCards.Count; i++)
                    {

                        GameObject.Find(computer1LastUpCards[i]).transform.SetParent(canvas.transform, true);
                        GameObject.Find(computer1LastUpCards[i]).transform.position = new Vector3((Screen.width*4)/5 - (i * 75), Screen.height/2 + 150 + (i * 75), 0);
                        GameObject.Find(computer1LastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = false;
                    }
                }

                computer1Round = 1;
                StartCoroutine(Computer2Turn());
            }
            else
            {
                if (playedCards.Count >= 4)
                {
                    if (cardN[0] == 'T' || (playedCards[playedCards.Count - 2][0] == cardN[0] && playedCards[playedCards.Count - 3][0] == cardN[0] && playedCards[playedCards.Count - 4][0] == cardN[0]))
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Debug.Log(playedCards[playedCards.Count - i]);
                        }
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer1LastUpCards.Count != 0)
                        {
                            StartCoroutine(Computer1Turn());
                        }
                        else
                        {
                            Computer1Wins();
                        }
                    }
                    else
                    {
                        lastPlayedCard = cardN[0];
                        StartCoroutine(Computer2Turn());
                    }
                }
                else
                {
                    if (cardN[0] == 'T')
                    {
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer1LastUpCards.Count != 0)
                        {
                            StartCoroutine(Computer1Turn());
                        }
                        else
                        {
                            Computer1Wins();
                        }
                    }
                    else
                    {
                        lastPlayedCard = cardN[0];
                        StartCoroutine(Computer2Turn());
                    }
                }



                if (computer1LastUpCards.Count == 0)
                {
                    Computer1Wins();
                }
                else
                {
                    StartCoroutine(Computer2Turn());
                }

            }
        }
        playerTurn = true;
        playButton.GetComponent<Button>().interactable = true;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop4P>().isDraggable = true;
        }

    }

    public IEnumerator Computer2Turn()
    {
        playerTurn = false;
        playButton.GetComponent<Button>().interactable = false;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop4P>().isDraggable = false;
        }
        yield return new WaitForSeconds(0.5f);
        if (computer2Round == 0)
        {
            FindBestLastCards2();
            computer2Round++;
            StartCoroutine(Computer3Turn());

        }
        else if (computer2Round == 1)
        {
            bool pos = CanComputer2PlayHand();
            if (pos == true)
            {
                List<string> sameCards = new List<string>();
                string bestCard = BestComputer2Card();

                for (int i = 0; i < computer2Area.transform.childCount; i++)
                {
                    if (bestCard[0] == computer2Area.transform.GetChild(i).name[0])
                    {
                        sameCards.Add(computer2Area.transform.GetChild(i).name);
                    }
                }

                lastPlayedCard = bestCard[0];
                int cardsP = 0;

                while (sameCards.Count != 0)
                {
                    GameObject.Find(sameCards[0]).transform.SetParent(canvas.transform, true);
                    GameObject.Find(sameCards[0]).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
                    GameObject.Find(sameCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                    playedCards.Add(sameCards[0]);
                    sameCards.RemoveAt(0);
                    cardsP++;
                }
                if (playedCards.Count >= 4)
                {
                    if (bestCard[0] == 'T' || cardsP == 4 || (cardsP == 3 && playedCards[playedCards.Count - 4][0] == bestCard[0]) || (cardsP == 2 && playedCards[playedCards.Count - 3][0] == bestCard[0] && playedCards[playedCards.Count - 4][0] == bestCard[0]) || (cardsP == 1 && playedCards[playedCards.Count - 2][0] == bestCard[0] && playedCards[playedCards.Count - 3][0] == bestCard[0] && playedCards[playedCards.Count - 4][0] == bestCard[0]))
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Debug.Log(playedCards[playedCards.Count - i]);
                        }
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer2Area.transform.childCount == 0 && computer2LastUpCards.Count == 0)
                        {
                            Computer1Wins();
                        }
                        else if (computer2Area.transform.childCount == 0 && computer2FinalCards.Count == 0)
                        {
                            computer1Round = 2;
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer2Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer2FinalCards.Count != 0)
                            {
                                GameObject.Find(computer2FinalCards[0]).transform.SetParent(computer2Area.transform, true);
                                GameObject.Find(computer2FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer2FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer2FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer2Area.transform.childCount < 3)
                        {
                            if (topDeckCard <= 51)
                            {
                                GameObject.Find(deck[topDeckCard]).transform.SetParent(computer2Area.transform, true);
                                topDeckCard++;
                            }
                            StartCoroutine(Computer2Turn());
                        }
                    }
                    else
                    {
                        if (computer2Area.transform.childCount == 0 && computer2LastUpCards.Count == 0)
                        {
                            Computer2Wins();
                        }
                        else if (computer2Area.transform.childCount == 0 && computer2FinalCards.Count == 0)
                        {
                            computer2Round = 2;
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer2Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer2FinalCards.Count != 0)
                            {
                                GameObject.Find(computer2FinalCards[0]).transform.SetParent(computer2Area.transform, true);
                                GameObject.Find(computer2FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer2FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer2FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer2Area.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsP)
                            {
                                if (topDeckCard <= 51 && computer2Area.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(computer2Area.transform, true);
                                    topDeckCard++;
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            StartCoroutine(Computer3Turn());
                        }
                        else
                        {
                            StartCoroutine(Computer3Turn());
                        }
                    }
                }
                else
                {
                    if (bestCard[0] == 'T' || cardsP == 4)
                    {
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer2Area.transform.childCount == 0 && computer2LastUpCards.Count == 0)
                        {
                            Computer2Wins();
                        }
                        else if (computer2Area.transform.childCount == 0 && computer2FinalCards.Count == 0)
                        {
                            computer2Round = 2;
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer2Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer2FinalCards.Count != 0)
                            {
                                GameObject.Find(computer2FinalCards[0]).transform.SetParent(computer2Area.transform, true);
                                GameObject.Find(computer2FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer2FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer2FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer2Turn());
                        }
                        else if (computer2Area.transform.childCount < 3)
                        {
                            if (topDeckCard <= 51)
                            {
                                GameObject.Find(deck[topDeckCard]).transform.SetParent(computer2Area.transform, true);
                                topDeckCard++;
                            }
                            StartCoroutine(Computer2Turn());
                        }
                    }
                    else
                    {
                        if (computer2Area.transform.childCount == 0 && computer2LastUpCards.Count == 0)
                        {
                            Computer2Wins();
                        }
                        else if (computer2Area.transform.childCount == 0 && computer2FinalCards.Count == 0)
                        {
                            computer2Round = 2;
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer2Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer2FinalCards.Count != 0)
                            {
                                GameObject.Find(computer2FinalCards[0]).transform.SetParent(computer2Area.transform, true);
                                GameObject.Find(computer2FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer2FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer2FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer2Area.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsP)
                            {
                                if (topDeckCard <= 51 && computer2Area.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(computer2Area.transform, true);
                                    topDeckCard++;
                                    j++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            StartCoroutine(Computer3Turn());
                        }
                        else
                        {
                            StartCoroutine(Computer3Turn());
                        }
                    }
                }



            }
            else if (pos == false)
            {

                if (topDeckCard <= 51)
                {
                    StartCoroutine(FlipTopCard(3));
                }
                else
                {

                    lastPlayedCard = 'N';
                    while (playedCards.Count != 0)
                    {
                        GameObject.Find(playedCards[0]).transform.SetParent(computer2Area.transform, true);
                        GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                        GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                        playedCards.RemoveAt(0);
                    }
                }
                StartCoroutine(Computer3Turn());
            }
        }
        else if (computer2Round == 2)
        {
            if (computer2LastUpCards.Count != 0)
            {

                for (int i = 0; i < computer2LastUpCards.Count; i++)
                {

                    GameObject.Find(computer2LastUpCards[i]).transform.SetParent(canvas.transform, true);
                    GameObject.Find(computer2LastUpCards[i]).transform.position = new Vector3(Screen.width/2 + 75 - (i * 75), (Screen.height*4)/5, 0);

                }
            }
            Random rand = new System.Random();

            int num = rand.Next(computer2LastUpCards.Count - 1);

            string cardN = computer2LastUpCards[num];
            computer2LastUpCards.RemoveAt(num);
            GameObject.Find(cardN).transform.SetParent(canvas.transform.parent, true);
            GameObject.Find(cardN).transform.SetParent(canvas.transform, true);
            GameObject.Find(cardN).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
            GameObject.Find(cardN).GetComponent<UpdateCardSprite4P>().faceUp = true;
            playedCards.Add(cardN);

            bool check = CheckCard(cardN[0]);
            if (check == false)
            {
                yield return new WaitForSeconds(0.5f);
                lastPlayedCard = 'N';
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computer2Area.transform, true);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                    playedCards.RemoveAt(0);
                }
                if (computer2LastUpCards.Count != 0)
                {

                    for (int i = 0; i < computer2LastUpCards.Count; i++)
                    {

                        GameObject.Find(computer2LastUpCards[i]).transform.SetParent(canvas.transform, true);
                        GameObject.Find(computer2LastUpCards[i]).transform.position = new Vector3(Screen.width/2 + 75 - (i * 75), (Screen.height*4)/5, 0);
                        GameObject.Find(computer2LastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = false;
                    }
                }

                computer2Round = 1;
                StartCoroutine(Computer3Turn());
            }
            else
            {
                if (playedCards.Count >= 4)
                {
                    if (cardN[0] == 'T' || (playedCards[playedCards.Count - 2][0] == cardN[0] && playedCards[playedCards.Count - 3][0] == cardN[0] && playedCards[playedCards.Count - 4][0] == cardN[0]))
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Debug.Log(playedCards[playedCards.Count - i]);
                        }
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer2LastUpCards.Count != 0)
                        {
                            StartCoroutine(Computer2Turn());
                        }
                        else
                        {
                            Computer2Wins();
                        }
                    }
                    else
                    {
                        lastPlayedCard = cardN[0];
                        StartCoroutine(Computer3Turn());
                    }
                }
                else
                {
                    if (cardN[0] == 'T')
                    {
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer2LastUpCards.Count != 0)
                        {
                            StartCoroutine(Computer2Turn());
                        }
                        else
                        {
                            Computer2Wins();
                        }
                    }
                    else
                    {
                        lastPlayedCard = cardN[0];
                        StartCoroutine(Computer3Turn());
                    }
                }



                if (computer2LastUpCards.Count == 0)
                {
                    Computer2Wins();
                }
                else
                {
                    StartCoroutine(Computer3Turn());
                }

            }
        }
        playerTurn = true;
        playButton.GetComponent<Button>().interactable = true;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop4P>().isDraggable = true;
        }

    }

    public IEnumerator Computer3Turn()
    {
        playerTurn = false;
        playButton.GetComponent<Button>().interactable = false;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop4P>().isDraggable = false;
        }
        yield return new WaitForSeconds(0.5f);
        if (computer3Round == 0)
        {
            FindBestLastCards3();
            computer3Round++;
            GameObject.Find(deck[topDeckCard]).transform.SetParent(canvas.transform, true);
            GameObject.Find(deck[topDeckCard]).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
            GameObject.Find(deck[topDeckCard]).GetComponent<UpdateCardSprite4P>().faceUp = true;
            lastPlayedCard = deck[topDeckCard][0];
            playedCards.Add(deck[topDeckCard]);
            if (lastPlayedCard == 'T')
            {
                yield return new WaitForSeconds(0.5f);
                Destroy(GameObject.Find(deck[topDeckCard]));
                playedCards.RemoveAt(0);
                lastPlayedCard = 'N';
            }
            topDeckCard++;
        }
        else if (computer3Round == 1)
        {
            bool pos = CanComputer3PlayHand();
            if (pos == true)
            {
                List<string> sameCards = new List<string>();
                string bestCard = BestComputer3Card();

                for (int i = 0; i < computer3Area.transform.childCount; i++)
                {
                    if (bestCard[0] == computer3Area.transform.GetChild(i).name[0])
                    {
                        sameCards.Add(computer3Area.transform.GetChild(i).name);
                    }
                }

                lastPlayedCard = bestCard[0];
                int cardsP = 0;

                while (sameCards.Count != 0)
                {
                    GameObject.Find(sameCards[0]).transform.SetParent(canvas.transform, true);
                    GameObject.Find(sameCards[0]).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
                    GameObject.Find(sameCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = true;
                    playedCards.Add(sameCards[0]);
                    sameCards.RemoveAt(0);
                    cardsP++;
                }
                if (playedCards.Count >= 4)
                {
                    if (bestCard[0] == 'T' || cardsP == 4 || (cardsP == 3 && playedCards[playedCards.Count - 4][0] == bestCard[0]) || (cardsP == 2 && playedCards[playedCards.Count - 3][0] == bestCard[0] && playedCards[playedCards.Count - 4][0] == bestCard[0]) || (cardsP == 1 && playedCards[playedCards.Count - 2][0] == bestCard[0] && playedCards[playedCards.Count - 3][0] == bestCard[0] && playedCards[playedCards.Count - 4][0] == bestCard[0]))
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Debug.Log(playedCards[playedCards.Count - i]);
                        }
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer3Area.transform.childCount == 0 && computer3LastUpCards.Count == 0)
                        {
                            Computer3Wins();
                        }
                        else if (computer3Area.transform.childCount == 0 && computer3FinalCards.Count == 0)
                        {
                            computer3Round = 2;
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer3Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer3FinalCards.Count != 0)
                            {
                                GameObject.Find(computer3FinalCards[0]).transform.SetParent(computer3Area.transform, true);
                                GameObject.Find(computer3FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer3FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer3FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer3Area.transform.childCount < 3)
                        {
                            if (topDeckCard <= 51)
                            {
                                GameObject.Find(deck[topDeckCard]).transform.SetParent(computer3Area.transform, true);
                                topDeckCard++;
                            }
                            StartCoroutine(Computer3Turn());
                        }
                    }
                    else
                    {
                        if (computer3Area.transform.childCount == 0 && computer3LastUpCards.Count == 0)
                        {
                            Computer3Wins();
                        }
                        else if (computer3Area.transform.childCount == 0 && computer3FinalCards.Count == 0)
                        {
                            computer3Round = 2;
                        }
                        else if (computer3Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer3FinalCards.Count != 0)
                            {
                                GameObject.Find(computer3FinalCards[0]).transform.SetParent(computer3Area.transform, true);
                                GameObject.Find(computer3FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer3FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer3FinalCards.RemoveAt(0);
                            }
                        }
                        else if (computer3Area.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsP)
                            {
                                if (topDeckCard <= 51 && computer3Area.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(computer3Area.transform, true);
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
                }
                else
                {
                    if (bestCard[0] == 'T' || cardsP == 4)
                    {
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer3Area.transform.childCount == 0 && computer3LastUpCards.Count == 0)
                        {
                            Computer3Wins();
                        }
                        else if (computer3Area.transform.childCount == 0 && computer3FinalCards.Count == 0)
                        {
                            computer3Round = 2;
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer3Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer3FinalCards.Count != 0)
                            {
                                GameObject.Find(computer3FinalCards[0]).transform.SetParent(computer3Area.transform, true);
                                GameObject.Find(computer3FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer3FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer3FinalCards.RemoveAt(0);
                            }
                            StartCoroutine(Computer3Turn());
                        }
                        else if (computer3Area.transform.childCount < 3)
                        {
                            if (topDeckCard <= 51)
                            {
                                GameObject.Find(deck[topDeckCard]).transform.SetParent(computer3Area.transform, true);
                                topDeckCard++;
                            }
                            StartCoroutine(Computer3Turn());
                        }
                    }
                    else
                    {
                        if (computer3Area.transform.childCount == 0 && computer3LastUpCards.Count == 0)
                        {
                            Computer3Wins();
                        }
                        else if (computer3Area.transform.childCount == 0 && computer3FinalCards.Count == 0)
                        {
                            computer3Round = 2;
                        }
                        else if (computer3Area.transform.childCount == 0 && topDeckCard == 52)
                        {
                            while (computer3FinalCards.Count != 0)
                            {
                                GameObject.Find(computer3FinalCards[0]).transform.SetParent(computer3Area.transform, true);
                                GameObject.Find(computer3FinalCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                                GameObject.Find(computer3FinalCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                                computer3FinalCards.RemoveAt(0);
                            }
                        }
                        else if (computer3Area.transform.childCount < 3)
                        {
                            int j = 0;
                            while (j < cardsP)
                            {
                                if (topDeckCard <= 51 && computer3Area.transform.childCount < 3)
                                {
                                    GameObject.Find(deck[topDeckCard]).transform.SetParent(computer3Area.transform, true);
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
                }



            }
            else if (pos == false)
            {

                if (topDeckCard <= 51)
                {
                    StartCoroutine(FlipTopCard(3));
                }
                else
                {

                    lastPlayedCard = 'N';
                    while (playedCards.Count != 0)
                    {
                        GameObject.Find(playedCards[0]).transform.SetParent(computer3Area.transform, true);
                        GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                        GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                        playedCards.RemoveAt(0);
                    }
                }
            }
        }
        else if (computer3Round == 2)
        {
            if (computer3LastUpCards.Count != 0)
            {

                for (int i = 0; i < computer3LastUpCards.Count; i++)
                {

                    GameObject.Find(computer3LastUpCards[i]).transform.SetParent(canvas.transform, true);
                    GameObject.Find(computer3LastUpCards[i]).transform.position = new Vector3(Screen.width/5 + 150 - (75 * i), Screen.height/2 + 300 - (75 * i), 0);

                }
            }
            Random rand = new System.Random();

            int num = rand.Next(computer3LastUpCards.Count - 1);

            string cardN = computer3LastUpCards[num];
            computer3LastUpCards.RemoveAt(num);
            GameObject.Find(cardN).transform.SetParent(canvas.transform.parent, true);
            GameObject.Find(cardN).transform.SetParent(canvas.transform, true);
            GameObject.Find(cardN).transform.position = new Vector3(Screen.width/2 +100, Screen.height/2 + 30, 0);
            GameObject.Find(cardN).GetComponent<UpdateCardSprite4P>().faceUp = true;
            playedCards.Add(cardN);

            bool check = CheckCard(cardN[0]);
            if (check == false)
            {
                yield return new WaitForSeconds(0.5f);
                lastPlayedCard = 'N';
                while (playedCards.Count != 0)
                {
                    GameObject.Find(playedCards[0]).transform.SetParent(computer3Area.transform, true);
                    GameObject.Find(playedCards[0]).GetComponent<DragDrop4P>().isDraggable = false;
                    GameObject.Find(playedCards[0]).GetComponent<UpdateCardSprite4P>().faceUp = false;
                    playedCards.RemoveAt(0);
                }
                if (computer3LastUpCards.Count != 0)
                {

                    for (int i = 0; i < computer3LastUpCards.Count; i++)
                    {

                        GameObject.Find(computer3LastUpCards[i]).transform.SetParent(canvas.transform, true);
                        GameObject.Find(computer3LastUpCards[i]).transform.position = new Vector3(Screen.width/5 + 150 - (75 * i), Screen.height/2 + 300 - (i * 75), 0);
                        GameObject.Find(computer3LastUpCards[i]).GetComponent<DragDrop4P>().isDraggable = false;
                    }
                }

                computer3Round = 1;
            }
            else
            {
                if (playedCards.Count >= 4)
                {
                    if (cardN[0] == 'T' || (playedCards[playedCards.Count - 2][0] == cardN[0] && playedCards[playedCards.Count - 3][0] == cardN[0] && playedCards[playedCards.Count - 4][0] == cardN[0]))
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            Debug.Log(playedCards[playedCards.Count - i]);
                        }
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer3LastUpCards.Count != 0)
                        {
                            StartCoroutine(Computer3Turn());
                        }
                        else
                        {
                            Computer3Wins();
                        }
                    }
                    else
                    {
                        lastPlayedCard = cardN[0];
                    }
                }
                else
                {
                    if (cardN[0] == 'T')
                    {
                        yield return new WaitForSeconds(0.5f);
                        while (playedCards.Count != 0)
                        {
                            string tempN = playedCards[0];
                            Destroy(GameObject.Find(tempN));
                            playedCards.RemoveAt(0);
                        }
                        lastPlayedCard = 'N';
                        if (computer3LastUpCards.Count != 0)
                        {
                            StartCoroutine(Computer3Turn());
                        }
                        else
                        {
                            Computer3Wins();
                        }
                    }
                    else
                    {
                        lastPlayedCard = cardN[0];
                    }
                }



                if (computer3LastUpCards.Count == 0)
                {
                    Computer3Wins();
                }

            }
        }
        playerTurn = true;
        playButton.GetComponent<Button>().interactable = true;
        for (int i = 0; i < playerDZ.transform.childCount; i++)
        {
            playerDZ.transform.GetChild(i).GetComponent<DragDrop4P>().isDraggable = true;
        }

    }

    public string BestComputer1Card()
    {
        string bestCard = "";
        List<string> playable = new List<string>();

        for (int i = 0; i < computer1Area.transform.childCount; i++)
        {
            char cardV = computer1Area.transform.GetChild(i).name[0];
            switch (lastPlayedCard)
            {
                case '4':
                    if (cardV == '2' || cardV == '4' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }
                    break;
                case '6':
                    if (cardV == '2' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }
                    break;
                case '7':
                    if (cardV == '2' || cardV == '5' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }
                    break;
                case '8':
                    if (cardV == '2' || cardV == '5' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }
                    break;
                case '9':
                    if (cardV == '2' || cardV == '5' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }

                    break;
                case 'J':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }

                    break;
                case 'Q':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }

                    break;
                case 'K':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }

                    break;
                case 'A':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'A')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }

                    break;
                case '5':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == '4' || cardV == '3')
                    {
                        playable.Add(computer1Area.transform.GetChild(i).name);
                    }

                    break;
                default:
                    playable.Add(computer1Area.transform.GetChild(i).name);
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

    public string BestComputer2Card()
    {
        string bestCard = "";
        List<string> playable = new List<string>();

        for (int i = 0; i < computer2Area.transform.childCount; i++)
        {
            char cardV = computer2Area.transform.GetChild(i).name[0];
            switch (lastPlayedCard)
            {
                case '4':
                    if (cardV == '2' || cardV == '4' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }
                    break;
                case '6':
                    if (cardV == '2' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }
                    break;
                case '7':
                    if (cardV == '2' || cardV == '5' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }
                    break;
                case '8':
                    if (cardV == '2' || cardV == '5' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }
                    break;
                case '9':
                    if (cardV == '2' || cardV == '5' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }

                    break;
                case 'J':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }

                    break;
                case 'Q':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }

                    break;
                case 'K':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }

                    break;
                case 'A':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'A')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }

                    break;
                case '5':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == '4' || cardV == '3')
                    {
                        playable.Add(computer2Area.transform.GetChild(i).name);
                    }

                    break;
                default:
                    playable.Add(computer2Area.transform.GetChild(i).name);
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

    public string BestComputer3Card()
    {
        string bestCard = "";
        List<string> playable = new List<string>();

        for (int i = 0; i < computer3Area.transform.childCount; i++)
        {
            char cardV = computer3Area.transform.GetChild(i).name[0];
            switch (lastPlayedCard)
            {
                case '4':
                    if (cardV == '2' || cardV == '4' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }
                    break;
                case '6':
                    if (cardV == '2' || cardV == '5' || cardV == '6' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }
                    break;
                case '7':
                    if (cardV == '2' || cardV == '5' || cardV == '7' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }
                    break;
                case '8':
                    if (cardV == '2' || cardV == '5' || cardV == '8' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }
                    break;
                case '9':
                    if (cardV == '2' || cardV == '5' || cardV == '9' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }

                    break;
                case 'J':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'J' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }

                    break;
                case 'Q':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'Q' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }

                    break;
                case 'K':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'K' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }

                    break;
                case 'A':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == 'A')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }

                    break;
                case '5':
                    if (cardV == '2' || cardV == '5' || cardV == 'T' || cardV == '4' || cardV == '3')
                    {
                        playable.Add(computer3Area.transform.GetChild(i).name);
                    }

                    break;
                default:
                    playable.Add(computer3Area.transform.GetChild(i).name);
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

    public bool CanComputer1PlayHand()
    {
        bool possible = false;

        for (int i = 0; i < computer1Area.transform.childCount; i++)
        {
            char cardV = computer1Area.transform.GetChild(i).name[0];
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

    public bool CanComputer2PlayHand()
    {
        bool possible = false;

        for (int i = 0; i < computer2Area.transform.childCount; i++)
        {
            char cardV = computer2Area.transform.GetChild(i).name[0];
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

    public bool CanComputer3PlayHand()
    {
        bool possible = false;

        for (int i = 0; i < computer3Area.transform.childCount; i++)
        {
            char cardV = computer3Area.transform.GetChild(i).name[0];
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

    public void FindBestLastCards1()
    {
        char[] rankedValues = new char[] { 'T', '5', '2', 'A', 'K', 'Q', 'J', '9', '8', '7', '6', '4', '3' };
        List<string> bestCards = new List<string>();
        string tempName = "";

        foreach (char v in rankedValues)
        {
            for (int i = 0; i < 6; i++)
            {
                tempName = computer1Area.transform.GetChild(i).name;
                if (v == tempName[0])
                {
                    bestCards.Add(computer1Area.transform.GetChild(i).name);
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
            GameObject.Find(bestCards[j]).transform.SetParent(canvas.transform, true);
            GameObject.Find(bestCards[j]).transform.position = new Vector3((Screen.width*4)/5 - xOffset, Screen.height/2 + 150 + xOffset, 0);
            GameObject.Find(bestCards[j]).GetComponent<UpdateCardSprite4P>().faceUp = true;
            computer1FinalCards.Add(bestCards[j]);
            xOffset = xOffset + 75;
        }


    }
    public void FindBestLastCards2()
    {
        char[] rankedValues = new char[] { 'T', '5', '2', 'A', 'K', 'Q', 'J', '9', '8', '7', '6', '4', '3' };
        List<string> bestCards = new List<string>();
        string tempName = "";

        foreach (char v in rankedValues)
        {
            for (int i = 0; i < 6; i++)
            {
                tempName = computer2Area.transform.GetChild(i).name;
                if (v == tempName[0])
                {
                    bestCards.Add(computer2Area.transform.GetChild(i).name);
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
            GameObject.Find(bestCards[j]).transform.SetParent(canvas.transform, true);
            GameObject.Find(bestCards[j]).transform.position = new Vector3(Screen.width/2 + 75 - xOffset, (Screen.height*4)/5, 30);
            GameObject.Find(bestCards[j]).GetComponent<UpdateCardSprite4P>().faceUp = true;
            computer2FinalCards.Add(bestCards[j]);
            xOffset = xOffset + 75;
        }


    }

    public void FindBestLastCards3()
    {
        char[] rankedValues = new char[] { 'T', '5', '2', 'A', 'K', 'Q', 'J', '9', '8', '7', '6', '4', '3' };
        List<string> bestCards = new List<string>();
        string tempName = "";

        foreach (char v in rankedValues)
        {
            for (int i = 0; i < 6; i++)
            {
                tempName = computer3Area.transform.GetChild(i).name;
                if (v == tempName[0])
                {
                    bestCards.Add(computer3Area.transform.GetChild(i).name);
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
            GameObject.Find(bestCards[j]).transform.SetParent(canvas.transform, true);
            GameObject.Find(bestCards[j]).transform.position = new Vector3(Screen.width/5 + 150 - xOffset, Screen.height/2 + 300 - xOffset, 0);
            GameObject.Find(bestCards[j]).GetComponent<UpdateCardSprite4P>().faceUp = true;
            computer3FinalCards.Add(bestCards[j]);
            xOffset = xOffset + 75;
        }


    }

    void PlayerWins()
    {
        adManager.PlayAd();
        for (int i = 0; i < 52; i++)
        {
            if (GameObject.Find(deck[i]) != null)
            {
                Destroy(GameObject.Find(deck[i]));
            }
        }
        endGameScreen.SetActive(true);
        endGameText.GetComponent<Text>().text = "Player Wins";
    }

    void Computer1Wins()
    {
        adManager.PlayAd();
        for (int i = 0; i < 52; i++)
        {
            if (GameObject.Find(deck[i]) != null)
            {
                Destroy(GameObject.Find(deck[i]));
            }
        }
        endGameScreen.SetActive(true);
        endGameText.GetComponent<Text>().text = "Computer 1 Wins";
    }

    void Computer2Wins()
    {
        adManager.PlayAd();
        for (int i = 0; i < 52; i++)
        {
            if (GameObject.Find(deck[i]) != null)
            {
                Destroy(GameObject.Find(deck[i]));
            }
        }
        endGameScreen.SetActive(true);
        endGameText.GetComponent<Text>().text = "Computer 2 Wins";
    }

    void Computer3Wins()
    {
        adManager.PlayAd();
        for (int i = 0; i < 52; i++)
        {
            if (GameObject.Find(deck[i]) != null)
            {
                Destroy(GameObject.Find(deck[i]));
            }
        }
        endGameScreen.SetActive(true);
        endGameText.GetComponent<Text>().text = "Computer 3 Wins";
    }

    public void OnPlayAgainButton()
    {
        sceneChanger.SceneLoad("Local4P");
    }

    public void OnExitButton()
    {
        sceneChanger.SceneLoad("MainMenu");
    }
}
