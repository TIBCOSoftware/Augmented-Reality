using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TIBCO.LABS.LIVEAPPS
{
    public interface ICaseHandler
    {
        
        void GetAllCases(string applicationName, string stateName, string searchString, bool getArtifacts);
    }
}
