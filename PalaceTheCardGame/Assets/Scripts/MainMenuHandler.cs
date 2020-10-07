using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public SceneChanger sceneChanger;

    void Start()
    {
        sceneChanger = GetComponent<SceneChanger>();
    }

    public void OnLocalPlayButton()
    {
        sceneChanger.SceneLoad("LocalPlayMenu");
    }

    public void OnHowToPlayButton()
    {
        sceneChanger.SceneLoad("HowToPlay");
    }

    public void OnOnlinePlayButton()
    {
        //sceneChanger.SceneLoad("OnlinePlayMenu");
    }
}