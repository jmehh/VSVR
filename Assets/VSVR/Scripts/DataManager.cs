using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{

    //private string currentBlockName;
    //private string currentTrialName;

    [Serializable]
    struct BlockData
    {
        public string name;
        public List<TrialData> data;
    }

    [Serializable]
    struct TrialData
    {
        public string name;
        public List<Data> data;
    }

    [Serializable]
    struct Data
    {
        public string name;
        public float time;
        public List<string> data;
    }

    private static BlockData currentBlockData;
    private static TrialData currentTrialData;

    void Start()
    {
        
    }

    public static void NewBlock(string name)
    {
        currentBlockData = new BlockData();
        currentBlockData.name = name;
        currentBlockData.data = new List<TrialData>();
    }

    public static void NewTrial(string name)
    {
        currentTrialData = new TrialData();
        currentTrialData.name = name;
        currentTrialData.data = new List<Data>();
        currentBlockData.data.Add(currentTrialData);
    }

    public static void NewData(string name, List<string> data)
    {
        Data d = new Data();
        d.name = name;
        d.time = Time.time;
        d.data = data;
        currentTrialData.data.Add(d);
    }

    public static void WriteBlockData()
    {
        var jsonData = JsonUtility.ToJson(currentBlockData, true);

        long epoch = (DateTime.Now.Ticks - 621355968000000000) / 10000000;
#if UNITY_EDITOR
        System.IO.File.WriteAllText("./" + currentBlockData.name + "-" + epoch.ToString() + ".json", jsonData);
#else
        //string path = Application.persistentDataPath + "/" + blockData.name + "-" + sessionId.ToString() + "-" + epoch.ToString() + ".json";
        //System.IO.File.WriteAllText(path, jsonData);
#endif
    }

}
