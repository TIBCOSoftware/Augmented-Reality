using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text; // for Encoding when reading credential

namespace TIBCO.LABS
{
    public class Utils 
        
    {
        const string CREDENTIALS_FILE = "TIBCO-credentials";
        public static Dictionary<string, string> ReadCredentialsFile()
        {
            return ReadPropertyFile(CREDENTIALS_FILE);
        }

        public static Dictionary<string, string> ReadPropertyFile(string FileName)
        {
            var props = new Dictionary<string, string>();
            TextAsset asset = Resources.Load<TextAsset>(FileName);
            if (null == asset)
            {
                throw new System.Exception(FileName + " file missing in Resources folder.");
            }


            
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
            return props;

        }
    }
}
