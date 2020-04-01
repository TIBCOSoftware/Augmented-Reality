/*
* Copyright © 2020. TIBCO Software Inc.
* This file is subject to the license terms contained
* in the license file that is distributed with this file.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Options Menu Manager, options Menu Box
public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A Options Menu Canvas")]
    Transform m_Canvas;

    private Color32 displayColor;

    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        displayColor = rend.material.GetColor("_SpecColor");
    }

    private void OnMouseDown()
    {
        //mouse over Bar
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_SpecColor", new Color32(100, 100, 130, 100));
        m_Canvas.gameObject.SetActive(true);
    }
    private void OnMouseUp()
    {
        //mouse left Bar
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_SpecColor", displayColor);
    }

}
