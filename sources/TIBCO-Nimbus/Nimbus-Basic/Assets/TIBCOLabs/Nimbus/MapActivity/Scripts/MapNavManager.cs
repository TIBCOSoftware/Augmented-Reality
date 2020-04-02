/*
* Copyright © 2020. TIBCO Software Inc.
* This file is subject to the license terms contained
* in the license file that is distributed with this file.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Text.RegularExpressions;

// TIBCO Cloud Nimbus - JSON Filereader, from Resource Folder

public class MapNavManager : MonoBehaviour
{
    private int record = 0;
    private NimbusMapData mapData;

    public TextMeshPro MapName;
    public TextMeshPro MapVersion;
    public TextMeshPro ActivityText;
    public TextMeshPro ActivityComment;
    public TextMeshPro ActivityRemarks;
    public TextMeshPro ActivityResource;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset jsonObj = Resources.Load<TextAsset>("NimbusMap");
        mapData = JsonUtility.FromJson<NimbusMapData>(jsonObj.text);

        setDetails();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void NextAction()
    {
        if (record == mapData.diagram.diagram.objects.Length) return;

        Debug.Log("** Next Action " + record);
        record++;

        for (int i = record; i < mapData.diagram.diagram.objects.Length; i++)
        {
           Debug.Log("i" + i +" "+ mapData.diagram.diagram.objects[i].objectType);
            if (mapData.diagram.diagram.objects[i].objectType == "otActivity")
            {
                record = i;
                setDetails();
                return;
            }
        }

        record--;
    }

    public void BackAction()
    {
        if (record == 0) return;

        Debug.Log("** Back Action " + record);

        for (int i = record-1; i > -1; i--)
        {
            Debug.Log("i" + i + " " + mapData.diagram.diagram.objects[i].objectType);
            if (mapData.diagram.diagram.objects[i].objectType == "otActivity")
            {
                record = i;
                setDetails();
                return;
            }
        } 
    }

    // Debug Object Details
    public void setDetails()
    {
        string DebugDetails = "Activity Details ["+record+"]:\n";
        DebugDetails += "Map  : " + (mapData.diagram.diagram.diagramTitle) + "\n";
        DebugDetails += "Type : " + (mapData.diagram.diagram.objects[record].objectType) + "\n";
        DebugDetails += "Title: " + (mapData.diagram.diagram.objects[record].properties.text) + "\n";
        DebugDetails += "Text : " + (StripHTML(mapData.diagram.diagram.objects[record].properties.hintText)) + "\n";
        //DebugDetails += "Note :" + (StripHTML(mapData.diagram.diagram.objects[record].bubbleText.properties.text)) + "\n";
        DebugDetails += "Reso :" + (mapData.diagram.diagram.objects[record].resources[0].properties.name) + "\n";
        DebugDetails += "Ownr :" + (mapData.details.owner) + "\n";
        DebugDetails += "Ver  :" + (mapData.details.version + "." + mapData.details.levelNumber) + "\n";

        Debug.Log(DebugDetails);

        MapName.text = mapData.diagram.diagram.diagramTitle;
        MapVersion.text = "Version " + mapData.details.version + "." + mapData.details.levelNumber;
        ActivityText.text = mapData.diagram.diagram.objects[record].properties.text;
        ActivityComment.text = StripHTML(mapData.diagram.diagram.objects[record].properties.hintText);
        if (mapData.diagram.diagram.objects[record].bubbleText.properties == null)
        {
            ActivityRemarks.text = "";
        } else
        {
            ActivityRemarks.text = StripHTML(mapData.diagram.diagram.objects[record].bubbleText.properties.text);
        }
        ActivityResource.text = mapData.diagram.diagram.objects[record].resources[0].properties.name;

    }

    // TIBCO Nimbus Data Structure JSON

    [System.Serializable]
    private class NimbusMapData {
        public diagramType diagram;
        public mapdetails details;
    }

    [System.Serializable]
    private class diagramType
    {
        public string type;
        public diagram diagram;
    }

    [System.Serializable]
    private class diagram
    {
        public string diagramTitle;
        public objects[] objects;
    }

    [System.Serializable]
    private class objects
    {
        //otActivity, otLine, otLineText
        public string objectType;               // type
        public Activityproperties properties;   // activity details
        public ActivityResources[] resources;   // resources
        public bubbleText bubbleText;
    }

    [System.Serializable]
    private class Activityproperties
    {
        public string text;             // title
        public string hintText;         // hint
        public string commentaryText;   // note
    }

    [System.Serializable]
    private class ActivityResources
    {
        public ResourceProperties properties;   // resource
    }
    [System.Serializable]
    private class ResourceProperties
    {
        public string name;             // title
        public string hintText;         // hint
    }

    [System.Serializable]
    private class bubbleText
    {
        public bubbleTextProperties properties;  // title
    }
    [System.Serializable]
    private class bubbleTextProperties
    {
        public string text;             // remark text
    }

    //general Mapdetails
    [System.Serializable]
    private class mapdetails
    {
        public string title;            // Map Title
        public string mapTitle;         // shown Map Title
        public string description;      // description
        public string owner;            // owner
        public string version;          // version
        public string levelNumber;      // minor version Number
        public string reviewDate;       // reviewDate
        public string date;             // creation date
    }

    // ** Helpers

    public static string StripHTML(string input)
    {
        input = Regex.Replace(input, @"\r", "\n");  //change line breaks
        input = Regex.Replace(input, @"�", " ");    //fix
        return Regex.Replace(input, "<.*?>", string.Empty); //remove HTML
    }
}
