using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using TIBCO.LABS.LIVEAPPS;

namespace TIBCO.InfoCard
{
    
    [System.Serializable]
    public class InfoCard
    {
        public string Category;
        public string Name;
        public string Description;

    }
    [System.Serializable]
    public class InfoCardWrapper
    {
        public InfoCard InfoCard;

    }
    public class BoardViewController : TIBCOLiveAppsHandler
    {
        public const string TEST_CARD = "Ant";
        public const string TEST_CARD_DESC = "a sample card created from Unity application using the LiveApps connector.";
        public const string TEST_IMAGE_NAME = "ant";

        public string LiveAppsAppName = "Info Card";
        private BoardLoader loader;
        #region IDataHandler methods
        public override void ConnectionReady()
        {
           // Debug.Log("path " + Application.dataPath);
           // CreateInfoCard("Animals", BoardViewController.TEST_CARD, BoardViewController.TEST_CARD_DESC);

            connector.GetAllCases(LiveAppsAppName, "Active", "", true, OnCaseData); // true to get artifacts ie documents and images

        }
        private void CreateInfoCard(string category, string name, string description)
        {
            InfoCardWrapper card = new InfoCardWrapper();
            
            card.InfoCard = new InfoCard();
            card.InfoCard.Category = category;
            card.InfoCard.Name = name;
            card.InfoCard.Description = description;
            connector.CreateCase(LiveAppsAppName, card, null,  OnCaseCreated);
            
        }
        public void OnCaseCreated(ActionResponse data)
        {
          
            Debug.Log("received case created ref " + data.caseReference);
            Debug.Log("path "+Application.dataPath);
            

            //var texture = Resources.Load<Texture2D>(TEST_IMAGE_NAME);
            //var img = texture.EncodeToPNG();
            byte[] img;
            img =  File.ReadAllBytes(Application.dataPath+ "/TIBCOLabs/LiveApps/Resources/"+TEST_IMAGE_NAME+".jpg");
            Debug.Log("img length " + img.Length);
            connector.AttachDocument(data.caseReference, TEST_IMAGE_NAME+".jpg", "picture", img, "image/jpeg", () =>
            {
                connector.GetAllCases(LiveAppsAppName, "Active", "", true, OnCaseData);
            });
            // get all cards now that we have created a test card !
            
        }
        private void OnCaseData(CaseInfoWrapper data)
        {
            // create a list of Elements and init the board UI
            // verify that the test card is present else create it.

            List<ElementData> elements = new List<ElementData>();
            bool found = false;
            try
            {
                Debug.Log("Received case data " + JsonUtility.ToJson(data, true));
                foreach (CaseInfo item in data.items)
                {
                    // Debug.Log("case data " + item.casedata );
                    InfoCard info = JsonUtility.FromJson<InfoCard>(item.casedata);
                    if (info.Name.Equals(BoardViewController.TEST_CARD))
                    {
                        found = true;
                    }
                    ElementData card = new ElementData();
                    card.info = info;
                    if (item.images.Count > 0)
                    {
                        card.texture = item.images[0];
                    }
                    elements.Add(card);
                }
                if (found == true)
                {
                    loader.InitializeBoard(elements);
                } else
                {
                    CreateInfoCard("Animals", BoardViewController.TEST_CARD, BoardViewController.TEST_CARD_DESC);
                }

            }
            catch (Exception e)
            {
                Debug.Log("Error in OnCaseData " + e.Message);
            }
        }
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            
            loader = transform.GetComponentInChildren<BoardLoader>();

        }
        void Update()
        {

        }
    }
}