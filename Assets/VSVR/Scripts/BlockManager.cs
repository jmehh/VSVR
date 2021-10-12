using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BlockManager : MonoBehaviour
{
    //public bool waitingForResponse;
    public string blockName;
    public bool randomizeTrials = true;

    public Action blockFinished;

    private int nTrials;
    private int currentTrial = -1;
    private List<TrialSet> trials = new List<TrialSet>();


    public TrialSet CurrentTrial { get => trials[currentTrial]; }

    void Start()
    {
        var trialsSets = GetComponentsInChildren<TrialSet>();
        foreach (var trialSet in trialsSets)
        {
            print("Adding some trials, " + trialSet.nTrials);
            nTrials = nTrials + trialSet.nTrials;
            trials.AddRange(Enumerable.Repeat(trialSet, trialSet.nTrials));

            trialSet.trialFinished += TrialFinished;
        }

        if (randomizeTrials)
        {
            var rnd = new System.Random();
            trials = trials.OrderBy(x => rnd.Next()).Take(trials.Count).ToList();
        }

    }

    public void StartBlock() 
    {
        //try
        //{
        //    blockStartCanvas = GameObject.Instantiate(blockStartCanvasPrefab, startCanvasPosition, false);
        //    //blockStartCanvas.transform.position = startCanvasPosition.position;
        //    blockStartCanvas.transform.rotation = lookAt.rotation;
        //} 
        //catch
        //{
        //    print("No canvas assigned");
        //}

        StartCoroutine(StartBlockInSeconds(1f));
    }

    public void NextTrial()
    {
        //UnityEngine.XR.InputTracking.Recenter();
        currentTrial++;
        if (currentTrial < trials.Count) { // go to next trial in block

            // update datamanager
            DataManager.NewTrial(trials[currentTrial].trialName);

            trials[currentTrial].StartTrial();
        }
        else // all trials in block complete
        {
            blockFinished.Invoke();
        }
    }

    public void TrialFinished()
    {
        print("EndTrial " + currentTrial);
        NextTrial();
    }

    IEnumerator StartBlockInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //Destroy(blockStartCanvas);
        NextTrial();
    }

}
