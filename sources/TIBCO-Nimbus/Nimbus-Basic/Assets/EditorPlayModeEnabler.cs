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
