using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text; // for Encoding when reading credential



namespace TIBCO.LABS.LIVEAPPS
{


    #region API data definition  
    [System.Serializable]
    public class LoginInfo
    {
        public string userName;
        public string firstName;
        public string lastName;
        public string userId;
        public long ts;
        public string orgName;
    }
    [System.Serializable]
    public class SandboxInfo
    {
        public string name;
        public string type;
        public string subscriptionId;
        public string id;
    }
    [System.Serializable]
    public class SandboxInfoWrapper
    {
        public SandboxInfo[] items;
    }
    [System.Serializable]
    public class StateInfo
    {
        public string id;
        public string label;
        public string value;
    }
    [System.Serializable]
    public class ActionInfo
    {
        public string id;
        public string name;
    }
    [System.Serializable]
    public class ApplicationInfo
    {
        public string applicationId;
        public string applicationName;
        public StateInfo[] states;
        public ActionInfo[] actions;
        public ActionInfo[] creators;
    }
    [System.Serializable]
    public class ApplicationInfoWrapper
    {
        public ApplicationInfo[] items;
        public string errorMsg;
    }
    [System.Serializable]
    public class CaseInfo
    {
        public string caseReference;
        public string casedata;
        public ArtifactInfo[] documents; // not in LiveApps API. Added data to group documents to caseinfo
        public List<UnityEngine.Texture2D> images; // not in LiveApps API. Added data to group all images to caseinfo

    }
    [System.Serializable]
    public class CaseInfoWrapper
    {
        public string applicationName; // not in LiveApps API. Added to help the onCaseData callback to handle the result of getCases
        public CaseInfo[] items;
    }
    [System.Serializable]
    public class ActionRequest
    {
        public string id;
        public string applicationId;
        public string sandboxId;
        public string caseReference;
        public string data;
    }
    [System.Serializable]
    public class CreateRequest
    {
        public string id;
        public string applicationId;
        public string sandboxId;
        public string data;
    }
    [System.Serializable]
    public class ActionResponse
    {
        public string applicationName; // not in LiveApps API. Added to help the onCaseCreated callback
        
        public string errorMsg;
        public string errorCode;
        public string id;
        public string applicationId;
        public string sandboxId;
        public string caseReference;
        public string data;


    }
    [System.Serializable]
    public class ArtifactInfo
    {
        public string id;
        public string name;
        public string artifactVersion;
        public string author;
        public string creationDate;
        public string lastModifiedDate;
        public string lastModifiedBy;

        public string artifactRef;
        public string size;
        public string mimeType;
        public string description;

    }
    [System.Serializable]
    public class ArtifactInfoWrapper
    {
        public ArtifactInfo[] items;
    }
    #endregion

    public class TIBCOLiveAppsConnector : MonoBehaviour
    {
        const string LIVEAPPS_PROPS_FILE = "TIBCO-credentials";
        [Header("Connection Details")]
        [Tooltip("TIBCO-credentials.txt resource file must contains <prefix>.user <prefix>.password <prefix>.region and <prefix>.clientID ")]
        [SerializeField]
        protected string ConnectionPropertyPrefix = "liveapps";

       //public delegate void CaseDataReceived(CaseInfoWrapper datalist);
       // public CaseDataReceived OnCaseData;
       // public delegate void CaseCreatedReceived(ActionResponse data);
       // public CaseCreatedReceived OnCaseCreated;
        public delegate void IsReady();
        public IsReady OnReady;

        // public enum EnumRegion  { US, EU, AU }
        public const string DOMAIN = "liveapps";
        private string clientID; // 
        private string userEmail;
        private string password;
        private string regionSelected;
        private string apiDomain;

        private string tsc; // tsc cookie
        private string domain; // domain cookie
        private string sandboxId;
        private Dictionary<string, ApplicationInfo> applicationsMap = new Dictionary<string, ApplicationInfo>();
        #region MonoBehavior methods
        // Start is called before the first frame update
        void Start()
        {


            TextAsset asset = Resources.Load<TextAsset>(LIVEAPPS_PROPS_FILE);
            if (null == asset)
            {
                throw new System.Exception(LIVEAPPS_PROPS_FILE + " file missing in Resources folder.");
            }

            Debug.Log("Credential " + asset.text);
            var props = new Dictionary<string, string>();
            foreach (var row in asset.text.Split('\n'))
            {

                if (!row.StartsWith("#"))
                {
                    string[] split = row.Split('=');
                    if (split.Length == 2)
                    {
                        string key = split[0].Trim();
                        string v = split[1].Trim();
                        props.Add(key, v);
                    }
                }

                //data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));

            }
            userEmail = props[ConnectionPropertyPrefix + ".user"];
            password = props[ConnectionPropertyPrefix + ".password"];
            clientID = props[ConnectionPropertyPrefix + ".clientID"];
            regionSelected = props[ConnectionPropertyPrefix + ".region"];
            switch (regionSelected.ToString().ToLower())
            {
                case "us":
                    apiDomain = DOMAIN;
                    break;
                default:
                    apiDomain = regionSelected.ToString().ToLower() + "." + DOMAIN;
                    break;
            }


            StartCoroutine(Login());
            Debug.Log("LiveApps login");

        }
        void Update()
        {
            // Debug.Log(".");
        }
        #endregion


        IEnumerator Login()
        {
            string LOGIN_URL = string.Format("https://{0}.cloud.tibco.com/idm/v3/login-oauth", apiDomain);

            WWWForm form = new WWWForm();
            form.AddField("ClientID", clientID);
            form.AddField("TenantId", "BPM");
            form.AddField("Email", userEmail);
            form.AddField("Password", password);
            UnityWebRequest uwr = UnityWebRequest.Post(LOGIN_URL, form);
            yield return uwr.SendWebRequest();
            Debug.Log("Post login response " + uwr.downloadHandler.text);
            string resultString = uwr.downloadHandler.text;
            var info = JsonUtility.FromJson<LoginInfo>(resultString);
            if (info.userId != null)
            {
                Debug.Log("login userId " + info.userId);
                extractCookie(uwr);
                yield return StartCoroutine(GetSandbox());
                Debug.Log("LiveApps connected");
                if (OnReady != null)
                {
                    OnReady();
                }

            }
            else
            {
                Debug.Log("Login to LiveApps failed.");
            }



        }
        protected virtual IEnumerator RunAfterLogin()
        {
            yield return null;
        }
        IEnumerator GetSandbox()
        {
            string SANDBOX_URL = string.Format("https://{0}.cloud.tibco.com/organisation/sandboxes?$filter=type eq Production", apiDomain);
            UnityWebRequest uwr = UnityWebRequest.Get(SANDBOX_URL);
            uwr.SetRequestHeader("Cookie", tsc);
            uwr.SetRequestHeader("Cookie", domain);
            yield return uwr.SendWebRequest();
            string resultString = uwr.downloadHandler.text;

            var sandbox = JsonUtility.FromJson<SandboxInfoWrapper>("{ \"items\" :" + resultString + "}");
            sandboxId = sandbox.items[0].id;
            Debug.Log("sandbox id : " + sandbox.items[0].id);
        }

        private void extractCookie(UnityWebRequest uwr)
        {
            string cookies = uwr.GetResponseHeader("Set-Cookie");
            tsc = cookies.Substring(cookies.LastIndexOf("tsc")).Split(';')[0];
            Debug.Log("tsc " + tsc);
            domain = cookies.Substring(cookies.LastIndexOf("domain")).Split(';')[0];
            Debug.Log("domain " + domain);
        }
        protected IEnumerator GetApplicationInfo(string applicationName)
        {
            ApplicationInfo application = null;
            // Get states, actions, creators
            string TYPES_URL = string.Format("https://{0}.cloud.tibco.com/case/types?$sandbox={1}&$select=b,ac,c,s&$filter=applicationName eq '{2}'", apiDomain, sandboxId, applicationName);
            UnityWebRequest uwr = UnityWebRequest.Get(TYPES_URL);
            uwr.SetRequestHeader("Cookie", tsc);
            uwr.SetRequestHeader("Cookie", domain);
            yield return uwr.SendWebRequest();
            string resultString = uwr.downloadHandler.text;

            var appList = JsonUtility.FromJson<ApplicationInfoWrapper>("{ \"items\" :" + resultString + "}");
            if (appList.errorMsg == null)
            {
                application = appList.items[0];
                

                yield return application;
            }
            else
            {
                Debug.Log(appList.errorMsg);
                yield return null;
            }





        }
        public void GetAllCases(string applicationName, string stateName, string searchString, bool getArtifacts = false, Action<CaseInfoWrapper> onComplete=null)
        {

            StartCoroutine(_GetAllCases(applicationName, stateName, searchString, getArtifacts, onComplete));
        }
        protected IEnumerator _GetAllCases(string applicationName, string stateName, string searchString, bool getArtifacts, Action<CaseInfoWrapper> onComplete)
        {
            var routine = GetApplicationInfo(applicationName);
            yield return StartCoroutine(routine);
            ApplicationInfo application = (ApplicationInfo)(routine.Current);
            try
            {
                applicationsMap.Add(applicationName, application);
            }
            catch (ArgumentException) { }
            

            routine = _GetApplicationCases(application, stateName, searchString);
            yield return StartCoroutine(routine);

            var cases = (CaseInfoWrapper)routine.Current;
            cases.applicationName = applicationName; // add the name to help the callback to handle the data
            if (getArtifacts == true)
            {
                foreach (CaseInfo item in cases.items)
                {
                    // Debug.Log("case data " + item.casedata );
                    //ElementData runner = JsonUtility.FromJson<ElementData>(item.casedata);

                    // Get All Runners
                    routine = GetCaseArtifacts(item.caseReference);
                    yield return StartCoroutine(routine);
                    ArtifactInfoWrapper docList = (ArtifactInfoWrapper)(routine.Current);

                    Debug.Log("docList for case  " + JsonUtility.ToJson(docList, true));
                    item.documents = docList.items;

                    foreach (ArtifactInfo doc in docList.items)
                    {
                        Debug.Log("doc case  " + doc.mimeType);
                        if (doc.mimeType.StartsWith("image"))
                        {
                            routine = GetArtifactAsTexture(item.caseReference, doc);
                            yield return StartCoroutine(routine);
                        }
                        item.images.Add((UnityEngine.Texture2D)(routine.Current));
                    }

                }
            }
            if (onComplete!=null)
            {
                onComplete(cases);
            }
           // OnCaseData(cases); // CaseInfoWrapper type

        }
        protected IEnumerator _GetApplicationCases(ApplicationInfo application, string stateName, string searchString)
        {
            Debug.Log("LiveApps GetCases");
            while (sandboxId == null)
            {
                yield return null;
            }
            // well retireve 100 first, not the All Cases !
            CaseInfoWrapper caseList = null;
            string stateCriteria = "";// no state criteria per default
            string searchCriteria = "";// no search criteria per default
            foreach (StateInfo s in application.states)
            {
                if (s.value == stateName)
                {
                    stateCriteria = string.Format("and stateId eq {0}", s.id); // create the filter criteria on the stateId
                    break;
                }
            }

            var applicationId = application.applicationId;
            
            if ((searchString != null) && (searchString != ""))
            {
                searchCriteria = string.Format("&$search={0}", searchCriteria); // create the serach criteria
            }

            string CASES_URL = string.Format("https://{0}.cloud.tibco.com/case/cases?$sandbox={1}&$select=c,cr&$filter=applicationId eq {2} and typeId eq 1 {3}{4}&$top=100", apiDomain, sandboxId, applicationId, stateCriteria, searchCriteria);
            Debug.Log("get cases : " + CASES_URL);
            UnityWebRequest uwr = UnityWebRequest.Get(CASES_URL);
            uwr.SetRequestHeader("Cookie", tsc);
            uwr.SetRequestHeader("Cookie", domain);
            yield return uwr.SendWebRequest();
            string resultString = uwr.downloadHandler.text;
            Debug.Log("cases response : " + resultString);
            caseList = JsonUtility.FromJson<CaseInfoWrapper>("{ \"items\" :" + resultString + "}");

            yield return caseList;


        }

        protected IEnumerator GetCaseArtifacts(string caseReference)
        {
            Debug.Log("LiveApps Get Folder Artifacts for a case reference " + caseReference);
            // well retireve 100 first, not the All Cases !
            ArtifactInfoWrapper artifactList = null;

            string CASES_URL = string.Format("https://{0}.cloud.tibco.com/webresource/caseFolders/{1}/artifacts?$sandbox={2}", apiDomain, caseReference, sandboxId);
            UnityWebRequest uwr = UnityWebRequest.Get(CASES_URL);
            uwr.SetRequestHeader("Cookie", tsc);
            uwr.SetRequestHeader("Cookie", domain);
            yield return uwr.SendWebRequest();
            string resultString = uwr.downloadHandler.text;
            Debug.Log("artifact response : " + resultString);
            if (!resultString.Contains("errorCode"))
            {

                try
                {
                    artifactList = JsonUtility.FromJson<ArtifactInfoWrapper>("{ \"items\" :" + resultString + "}");
                }
                catch (System.Exception err)
                {
                    Debug.Log(err);
                }
            }

            yield return artifactList;


        }
        protected IEnumerator GetArtifact(string caseReference, string artifactName)
        {
            Debug.Log("LiveApps GetArtifact " + caseReference + " " + artifactName);


            string TYPES_URL = string.Format("https://{0}.cloud.tibco.com/webresource/folders/{1}/{2}/{3}", apiDomain, caseReference, sandboxId, artifactName);
            UnityWebRequest uwr = UnityWebRequest.Get(TYPES_URL);
            uwr.SetRequestHeader("Cookie", tsc);
            uwr.SetRequestHeader("Cookie", domain);
            yield return uwr.SendWebRequest();
            Debug.Log("doc " + uwr.downloadHandler);
            string resultString = uwr.downloadHandler.text;

            // var appList = JsonUtility.FromJson<ApplicationInfoWrapper>("{ \"items\" :" + resultString + "}");

            // return as string -> should test the mimeType !
            yield return resultString;
        }
        protected IEnumerator GetArtifactAsTexture(string caseReference, ArtifactInfo artifact)
        {
            Debug.Log("LiveApps GetArtifact " + caseReference + " " + artifact.name);
            Texture myTexture = null;

            string TYPES_URL = string.Format("https://{0}.cloud.tibco.com/webresource/folders/{1}/{2}/{3}", apiDomain, caseReference, sandboxId, artifact.name);
            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(TYPES_URL);
            uwr.SetRequestHeader("Cookie", tsc);
            uwr.SetRequestHeader("Cookie", domain);
            yield return uwr.SendWebRequest();
            Debug.Log("doc " + uwr.downloadHandler);


            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                myTexture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
            }
            yield return myTexture;
        }
        public void CreateCase(string applicationName, System.Object data, string creatorName = null, Action<ActionResponse> onComplete = null)
        {
           
            StartCoroutine(AsyncCreateCase(applicationName, data, creatorName, onComplete));
          
        }
        protected IEnumerator AsyncCreateCase(string applicationName, System.Object data, string creatorName = null, Action<ActionResponse> onComplete = null)
        {
            var routine = _CreateCase(applicationName, data, creatorName);
            yield return StartCoroutine(routine);

            var caseref = (ActionResponse)routine.Current;
            if (caseref != null)
            {
                if (caseref.errorMsg != null)
                {
                    Debug.Log("Case created error " + caseref.errorMsg);
                }
                else
                {
                    Debug.Log("Case created " + caseref.caseReference);
                    caseref.applicationName = applicationName;
                    
                    if (onComplete != null)
                    {
                        onComplete(caseref);
                    }
                }
            }

        }

protected IEnumerator _CreateCase(string applicationName, System.Object data, string creatorName)
        {
            ApplicationInfo application ;
            if (applicationsMap.TryGetValue(applicationName, out application))
            {
                Debug.Log("Application already loaded " + applicationName);
            }
            else
            {
                var routine = GetApplicationInfo(applicationName);
                yield return StartCoroutine(routine);
                application = (ApplicationInfo)(routine.Current);
            }
            try
            {
                applicationsMap.Add(applicationName, application);
            } catch (ArgumentException) { }
            
           

            


            ActionInfo action= application.creators[0];
            if (creatorName != null)
            {
                action = System.Array.Find(application.creators,
                    a => a.name == creatorName);
            }
            if (action != null)
            {
                string ACTION_URL = string.Format("https://{0}.cloud.tibco.com/process/processes", apiDomain);
                CreateRequest actionRequest = new CreateRequest();
                actionRequest.id = action.id;
                actionRequest.applicationId = application.applicationId;
                actionRequest.sandboxId = sandboxId;
                actionRequest.data = JsonUtility.ToJson(data); 
                string body = JsonUtility.ToJson(actionRequest);
                Debug.Log("Post createCase : " + body);

                UnityWebRequest uwr = new UnityWebRequest(ACTION_URL);
                // we cannot use Post method because it escapes the body string (" got replaced by /") and the json is refused by server
                // so just do the post using the raw request :
                uwr.method = "POST";
                uwr.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
                uwr.downloadHandler = new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");
                uwr.SetRequestHeader("Cookie", tsc);
                uwr.SetRequestHeader("Cookie", domain);
                uwr.SetRequestHeader("Accept", "application/json");
                yield return uwr.SendWebRequest();

                string resultString = uwr.downloadHandler.text;
                Debug.Log("Create case response " + uwr.responseCode);
                Debug.Log("Create case response " + resultString);
                var response = JsonUtility.FromJson<ActionResponse>(resultString);

                yield return response;
            }
            else
            {
                yield return null;
            }



        }

        protected IEnumerator PostAction(ApplicationInfo application, CaseInfo casedata, string actionName)
        {
            ActionInfo action = System.Array.Find(application.actions,
                    a => a.name == actionName);
            if (action != null)
            {
                string ACTION_URL = string.Format("https://{0}.cloud.tibco.com/process/processes", apiDomain);
                ActionRequest actionRequest = new ActionRequest();
                actionRequest.id = action.id;
                actionRequest.applicationId = application.applicationId;
                actionRequest.sandboxId = sandboxId;
                actionRequest.caseReference = casedata.caseReference;
                string body = JsonUtility.ToJson(actionRequest);
                Debug.Log("Post Action : " + body);

                UnityWebRequest uwr = new UnityWebRequest(ACTION_URL);
                // we cannot use Post method because it escapes the body string (" got replaced by /") and the json is refused by server
                // so just do the post using the raw request :
                uwr.method = "POST";
                uwr.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
                uwr.downloadHandler = new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");
                uwr.SetRequestHeader("Cookie", tsc);
                uwr.SetRequestHeader("Cookie", domain);
                uwr.SetRequestHeader("Accept", "application/json");
                yield return uwr.SendWebRequest();

                string resultString = uwr.downloadHandler.text;
                Debug.Log("Post action response " + uwr.responseCode);
                Debug.Log("Post action response " + resultString);

                yield return true;
            }
            else
            {
                yield return null;
            }

            //yield return StartCoroutine(getAllCases(applicationName));


        }
        public void AttachDocument(string caseReference, string documentName, string description, byte[] data, string mimeType,Action onComplete=null)
        {
            
            StartCoroutine(AsyncAttachDocument( caseReference, documentName, description, data, mimeType, onComplete));
       
        }
        protected IEnumerator AsyncAttachDocument( string caseReference, string documentName, string description, byte[] data, string mimeType, Action onComplete = null)
        {
            string ACTION_URL = string.Format("https://{0}.cloud.tibco.com/webresource/v1/caseFolders/{1}/artifacts/{2}/upload?$sandbox={3}&description={4}", apiDomain,caseReference,documentName,sandboxId,description);
           
            // Create a Web Form
            WWWForm form = new WWWForm();
           
            form.AddBinaryData("artifactContents", data, documentName, mimeType);

            UnityWebRequest uwr = UnityWebRequest.Post(ACTION_URL, form);
            
           // uwr.method = "POST";
            uwr.SetRequestHeader("Cookie", tsc);
            uwr.SetRequestHeader("Cookie", domain);
            uwr.SetRequestHeader("Accept", "application/json");

            yield return uwr.SendWebRequest();
            string resultString = uwr.downloadHandler.text; 
            Debug.Log("Attach document response " + resultString);
           
            //var response = JsonUtility.FromJson<ActionResponse>(resultString);
            if (onComplete != null)
            {
                onComplete();
            }
           // yield return null ;

        }

    }
}
