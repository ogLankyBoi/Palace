using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{

    string Android_ID = "3840661";
    string IOS_ID = "3840660";
    void Start()
    {
        Advertisement.Initialize(IOS_ID, true);
    }

    public void PlayAd()
    {
        if (Advertisement.IsReady("video"))
        {
            Advertisement.Show("video");
        }
    }
}
