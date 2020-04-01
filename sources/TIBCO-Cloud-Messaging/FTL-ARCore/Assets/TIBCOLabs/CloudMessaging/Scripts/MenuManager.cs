/*
* Copyright © 2020. TIBCO Software Inc.
* This file is subject to the license terms contained
* in the license file that is distributed with this file.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Options Menu Manager, for touch with 2 fingers
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A Options Menu Canvas")]
    Transform m_Canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0 || m_Canvas == null)
            return;

        var touch = Input.GetTouch(0);

        if (Input.touchCount == 2)
        {
            m_Canvas.gameObject.SetActive(true);
        }
    }
}
