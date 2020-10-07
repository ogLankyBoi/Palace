using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayHandler : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public int stage = 1;
    public GameObject stage1Stuff;
    public GameObject stage2Stuff;
    public GameObject stage3Stuff;
    public GameObject previousButton;
    public GameObject nextButton;
    public GameObject title;

    void Start()
    {
        sceneChanger = GetComponent<SceneChanger>();

        stage1Stuff = GameObject.Find("Stage1");
        stage2Stuff = GameObject.Find("Stage2");
        stage3Stuff = GameObject.Find("Stage3");
        previousButton = GameObject.Find("PreviousButton");
        nextButton = GameObject.Find("NextButton");
        title = GameObject.Find("Title");

        stage2Stuff.SetActive(false);
        stage3Stuff.SetActive(false);
        previousButton.SetActive(false);
    }

    public void OnExitButton()
    {
        sceneChanger.SceneLoad("MainMenu");
    }

    public void OnNextButton()
    {
        if (stage == 1)
        {
            stage2Stuff.SetActive(true);
            stage1Stuff.SetActive(false);
            previousButton.SetActive(true);
            title.GetComponent<Text>().text = "How To Play: Stage 2";
            stage++;
        }
        else if (stage == 2)
        {
            stage3Stuff.SetActive(true);
            stage2Stuff.SetActive(false);
            nextButton.SetActive(false);
            title.GetComponent<Text>().text = "How To Play: Stage 3";
            stage++;
        }
    }

    public void OnPreviousButton()
    {
        if (stage == 2)
        {
            stage1Stuff.SetActive(true);
            stage2Stuff.SetActive(false);
            previousButton.SetActive(false);
            title.GetComponent<Text>().text = "How To Play: Stage 1";
            stage--;
        }
        else if (stage == 3)
        {
            stage2Stuff.SetActive(true);
            stage3Stuff.SetActive(false);
            nextButton.SetActive(true);
            title.GetComponent<Text>().text = "How To Play: Stage 2";
            stage--;
        }
    }
}
