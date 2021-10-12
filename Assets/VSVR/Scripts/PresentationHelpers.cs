using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationHelpers : MonoBehaviour
{

    public static void ShowStimuli(GameObject stimuli, bool record)
    {
        if (record)
        {
            DataManager.NewData("ShowStimuli", new List<string>(new string[] { stimuli.name }));
        }
        stimuli.SetActive(true);
    }

    public static void RemoveStimuli(GameObject stimuli, bool record)
    {
        if (record)
        {
            DataManager.NewData("RemoveStimuli", new List<string>(new string[] { stimuli.name }));
        }
        stimuli.SetActive(false);
        //GameObject.Destroy(stimuli);
    }


}
