using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System.Linq;

[IncludeInSettings(true)]

public class Spawners 
{

    public static GameObject SimpleStimuli(GameObject stimuliObject, float depth)
    {
        GameObject stimuli = new GameObject();
        GameObject.Instantiate(stimuliObject, stimuli.transform, false);
        stimuli.transform.position = new Vector3(0f, Camera.main.transform.position.y, depth);
        return stimuli;
    }

    public static GameObject GridSpawner(bool targetPresent, float gridSep, int gridWidth, int gridHeight, int distractorCount, GameObject target, List<GameObject> distractors, int rndSeed)
    {
        var rnd = new System.Random(rndSeed);

        int targetPosition = rnd.Next(0, distractorCount);
        //int targetPosition = Random.Range(0, distractorCount);
        GameObject stimuli = new GameObject();

        // define display grid
        List<float[]> grid = new List<float[]>();
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                float x = (i * gridSep) - ((gridWidth * gridSep) / 2) + (gridSep / 2);
                float y = (j * gridSep) - ((gridHeight * gridSep) / 2) + (gridSep / 2);
                grid.Add(new float[2] { x, y });
            }
        }

        // select random object locations
        var objects = grid.OrderBy(x => rnd.Next()).Take(distractorCount).ToList();

        distractors = distractors.OrderBy(x => rnd.Next()).Take(distractors.Count).ToList();
        int distractorIndex = 0;
        for (int j = 0; j < distractorCount; j++)
        {
            GameObject instanceOfStimuli;

            // Check if this stimulus should be the target one.
            if ((targetPresent) && j == targetPosition)
            {
                // Instantiate the target and name it.
                instanceOfStimuli = GameObject.Instantiate(target, stimuli.transform, false);
                instanceOfStimuli.name = $"Target";
                //instanceOfTarget = instanceOfStimuli;
                // Output to console the target has been spawned.
                Debug.Log("Target instantiated in spawner.");
            }
            else
            {
                if (distractorIndex > (distractors.Count - 1))
                    distractorIndex = 0;

                // Instantiate the distractor and name it.
                instanceOfStimuli = GameObject.Instantiate(distractors[distractorIndex], stimuli.transform, false);
                instanceOfStimuli.name = $"Distractor{(j)}";

                distractorIndex++;
            }

            // Move the stimulus to its position.
            instanceOfStimuli.transform.localPosition = new Vector3(objects[j][0], objects[j][1], 0);

        }

        //stimuli.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        stimuli.transform.position = new Vector3(0f, Camera.main.transform.position.y, 1.5f);

        stimuli.SetActive(false);

        // DataEntry for spawned stimuli

        List<string> data = new List<string>(new string[] { targetPresent.ToString(), gridSep.ToString(), gridWidth.ToString(), gridHeight.ToString(), distractorCount.ToString(), target.ToString(), distractors.ToString(), rndSeed.ToString() });
        DataManager.NewData("GridSpawner", data);
        return stimuli;
    }

    public static GameObject CylinderSpawnerWithDepth(string name, float duration, string expectedAnswer, float gridSep, int gridWidth, int gridHeight, int distractorCount, GameObject target, List<GameObject> distractors, int rndSeed, List<float> depths, int targetDepthInd)
    {
        var rnd = new System.Random(rndSeed);

        //int targetPosition = Random.Range(0, distractorCount);
        GameObject stimuli = new GameObject();
        List<int[]> inds = new List<int[]>();
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                inds.Add(new int[2] { i, j });
            }
        }
        inds = inds.OrderBy(x => rnd.Next()).Take(distractorCount).ToList();

        // define display grid
        List<float[]> grid = new List<float[]>();
        int depthInd = 0;
        for (int k = 0; k < inds.Count; k++)
        {
            int i = inds[k][0];
            int j = inds[k][1];

            float depth = depths[depthInd];

            float x = depth * Mathf.Sin((i - (gridWidth - 1) / 2f) * gridSep);
            float y = (j - (gridHeight - 1) / 2f) * (1.5f * Mathf.Sin(gridSep));
            float z = depth * Mathf.Cos((i - (gridWidth - 1) / 2f) * gridSep);
            grid.Add(new float[3] { x, y, z });

            depthInd++;
            if (depthInd > depths.Count - 1)
                depthInd = 0;
        }

        int targetPosition = targetDepthInd;

        // select random object locations
        //var objects = grid.OrderBy(x => rnd.Next()).Take(distractorCount).ToList();
        var objects = grid;

        distractors = distractors.OrderBy(x => rnd.Next()).Take(distractors.Count).ToList();
        int distractorIndex = 0;
        for (int j = 0; j < distractorCount; j++)
        {
            GameObject instanceOfStimuli;

            // Check if this stimulus should be the target one.
            if ((expectedAnswer == "Present") && j == targetPosition)
            {
                // Instantiate the target and name it.
                instanceOfStimuli = GameObject.Instantiate(target, stimuli.transform, false);
                instanceOfStimuli.name = $"Target";
                //instanceOfTarget = instanceOfStimuli;
                // Output to console the target has been spawned.
                Debug.Log("Target instantiated in spawner.");


            }
            else
            {
                if (distractorIndex > (distractors.Count - 1))
                    distractorIndex = 0;

                // Instantiate the distractor and name it.
                instanceOfStimuli = GameObject.Instantiate(distractors[distractorIndex], stimuli.transform, false);
                //instanceOfStimuli = GameObject.Instantiate(distractors[rnd.Next(distractors.Count)], stimuli.transform, false);
                instanceOfStimuli.name = $"Distractor{(j)}";

                distractorIndex++;
            }

            // Move the stimulus to its position.
            instanceOfStimuli.transform.localPosition = new Vector3(objects[j][0], objects[j][1], objects[j][2]);
            //instanceOfStimuli.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
            instanceOfStimuli.transform.LookAt(Vector3.zero);
        }

        //stimuli.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        //stimuli.transform.position = new Vector3(0f, Camera.main.transform.position.y, 1.5f);
        return stimuli;
    }

}


