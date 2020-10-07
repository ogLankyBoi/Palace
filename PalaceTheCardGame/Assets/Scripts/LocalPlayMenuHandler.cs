using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayMenuHandler : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public Slider slider;

    void Start()
    {
        sceneChanger = GetComponent<SceneChanger>();
    }

    public void OnPlayButton()
    {
        if (slider.value == 1)
        {
            sceneChanger.SceneLoad("Local2P");
        }
        else if (slider.value == 2)
        {
            sceneChanger.SceneLoad("Local3P");
        }
        else
        {
            sceneChanger.SceneLoad("Local4P");
        }
    }

    public void OnExitButton()
    {
        sceneChanger.SceneLoad("MainMenu");
    }
}
