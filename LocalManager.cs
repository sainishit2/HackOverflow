using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LocalManager : MonoBehaviour
{ 
    public GameObject theDisplay;
    public int hour;
    public int minutes;
    public int seconds;
    public static LocalManager instance;
    public class AnalysedQuery
    {
        public TopScoringIntentData topScoringIntent;
        public EntityData[] entities;
        public string query;
    }
    public class TopScoringIntentData
    {
        public string intent;
        public float score;
    }
    public class EntityData
    {
        public string entity;
        public string type;
        public int startIndex;
        public int endIndex;
        public float score;
    }

    string localEndpoint = "https://sailuissample-authoring.cognitiveservices.azure.com/";
        private void Awake()
    {
        instance = this;
    }
    public IEnumerator SubmitRequestToLocal(string text, Action done)
    {
        string queryString = string.Concat(Uri.EscapeDataString(text));

        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(localEndpoint + queryString))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.Log(unityWebRequest.error);
            }
            else
            {
                try
                {
                    AnalysedQuery analysedQuery = JsonUtility.FromJson<AnalysedQuery>(unityWebRequest.downloadHandler.text);

                    //analyse the elements of the response 
                    AnalyseResponseElements(analysedQuery);
                }
                catch (Exception exception)
                {
                    Debug.Log("Luis Request Exception Message: " + exception.Message);
                }
            }

            done();
            yield return null;
        }
    }
    private void AnalyseResponseElements(AnalysedQuery aQuery)
    {
        string topIntent = aQuery.topScoringIntent.intent;

        Dictionary<string, string> entityDic = new Dictionary<string, string>();

        foreach (EntityData ed in aQuery.entities)
        {
            entityDic.Add(ed.type, ed.entity);
        }
        //We Can Add the intent and the entities over here
        switch (aQuery.topScoringIntent.intent)
        {
            case "Time":
                string actiion = null;
                string goal = null;

                foreach (var pair in entityDic)
                {
                    if (pair.Key == "action" && pair.Key == "goal")
                    {
                        Update();
                    }
                }
                break;
        }
    }
    public void Update()
    {
        hour = System.DateTime.Now.Hour;
        minutes = System.DateTime.Now.Minute;
        seconds = System.DateTime.Now.Second;
        theDisplay.GetComponent<Text>().text = "" + hour + ":" + minutes + ":" + seconds;
    }
}
