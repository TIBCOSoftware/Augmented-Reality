/*
* Copyright © 2020. TIBCO Software Inc.
* This file is subject to the license terms contained
* in the license file that is distributed with this file.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enables Objects just in Editor Play Mode
namespace TIBCOLabsToolkit
{
    public class EditorPlayModeEnabler : MonoBehaviour
    {
#if UNITY_EDITOR
        public GameObject background;

        void Start()
        {
            background.SetActive(true);
        }
#endif
    }
}
