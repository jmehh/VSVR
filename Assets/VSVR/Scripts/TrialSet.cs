using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ludiq;
using Bolt;

public class TrialSet : MonoBehaviour
{
    public string trialName = "default";
    public int nTrials;
    public Action trialFinished;

    private FlowMachine flowMachine;
    private bool trialActive = false;

    public void StartTrial()
    {
        Debug.Log("Starting trial " + gameObject.name);
        trialActive = true;
        flowMachine = GetComponent<FlowMachine>();
        CustomEvent.Trigger(flowMachine.gameObject, "StartTrial");

    }

    public void OnUserInput(object[] args)
    {
        if (trialActive)
        {
            Debug.Log("UserInput: " + args[0]);
            CustomEvent.Trigger(flowMachine.gameObject, (string)args[0], args);
        }
    }

    public void EndTrial()
    {
        trialActive = false;
        trialFinished.Invoke();
    }

}



