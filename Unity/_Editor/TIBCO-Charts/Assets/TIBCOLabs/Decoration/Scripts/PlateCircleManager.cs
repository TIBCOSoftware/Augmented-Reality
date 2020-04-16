/*
* Copyright © 2020. TIBCO Software Inc.
* This file is subject to the license terms contained
* in the license file that is distributed with this file.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCircleManager : MonoBehaviour
{
    // Just for Decoration, instantiates prefabs in a circle formation

    [Header("Plate Prefab")]
    public GameObject prefabPlate;
    public int numberOfObjects = 42;
    public float radius = 8f;
    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 0, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            var plateObj = Instantiate(prefabPlate, pos, rot);
            plateObj.transform.parent = this.transform;

            plateObj.transform.localScale = new Vector3(0.07f, 1.0f*radius/8, 1.0f*radius/8);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
