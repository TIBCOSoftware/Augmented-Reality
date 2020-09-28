//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

namespace TIBCO.InfoCard
{
    [System.Serializable]
    public class ElementData
    {
        public InfoCard info;

        public UnityEngine.Texture2D texture;

    }

   

    public class BoardLoader : MonoBehaviour
    {
        //Element are added to the transform of this script = object where this script is attached
        
        // Generic element prefab to instantiate at each position in the table
        public GameObject ElementPrefab;
   
        public float width = 1f;


        private void Start()
        {
          
        }



        public void InitializeBoard(List<ElementData> elements)
        {

            Transform container = transform;
            try
            {

              

                    var gutter = 0.02f; //
                    
                    
                    var x0 = 0f; // offest by 8 tiles on the left 
                    var y0 = 0f;
                    

                    
                        // Insantiate the element prefabs in their correct locations and with correct text
                       // this.prompt.text = "first init of elements\n";
                        foreach (ElementData element in elements)
                        {
                            GameObject newElement = Instantiate<GameObject>(ElementPrefab, container);
                           // this.prompt.text += "Instantiate " + element.Name + "\n";
                            Element e = newElement.GetComponentInChildren<Element>();
                            if (e != null)
                            {
                                e.SetFromElementData(element);
                            }
                            else
                            {
                               Debug.Log("No Element Script found\n");

                            }

                            
                            newElement.transform.localPosition = new Vector3(x0, y0, 0f);
                            newElement.transform.localRotation = Quaternion.identity;
                            x0 += width + gutter;

                        }
                    

                    
                

                Debug.Log("Board initialized");
            }
            catch (Exception e)
            {
                Debug.Log("\nError loading board " + e + " \n" + UnityEngine.StackTraceUtility.ExtractStackTrace());
            }
            

        }
    }
}