﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public void SceneLoad(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

}
