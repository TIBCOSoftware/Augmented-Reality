/*
* Copyright © 2020. TIBCO Software Inc.
* This file is subject to the license terms contained
* in the license file that is distributed with this file.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public GameObject LabelPrefab;

    public float scale;
    public float size = 0.07f;
    public bool centered;
    public Color32 displayColor;

    private float currSize;

    // Start is called before the first frame update
    void Start()
    {
        currSize = 0.007f;
    }

    // Update is called once per frame
    void Update()
    {
        //Scale 
        currSize += 2f* scale * Time.deltaTime;

        //max Scale
        if (currSize * scale <= size*scale)
        {
            transform.localScale = new Vector3(0.08f/ transform.parent.localScale.x, currSize, scale);

            if (!centered)
            {
               // fix set from z=0 to 'transform.parent.position.z'
               transform.position = new Vector3(transform.position.x, transform.parent.position.y - transform.parent.localScale.y/2 + transform.parent.localScale.y * currSize /2, transform.parent.position.z);
            }
        }
    }

    private void OnMouseEnter()
    {
        //mouse over Bar
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_SpecColor", new Color32(100,100,100,100));
    }

    private void OnMouseExit()
    {
        //mouse left Bar
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_SpecColor", displayColor);
    }
}
