//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TIBCO.InfoCard
{
    public class Element : MonoBehaviour
    {
        public static Element ActiveElement;

        public Text Name;
        public Text Description;

        public SpriteRenderer spriteRenderer;

        [HideInInspector]
        public ElementData data;
  
        /**
         * Set the display data for this element based on the given parsed JSON data
         */
        public void SetFromElementData(ElementData data)
        {
            this.data = data;
            Debug.Log("set from element " + data.info.Name);
            Name.text = data.info.Name;
            Description.text = data.info.Description;
         
            if (data.texture != null )
            {
                Debug.Log("Texture width "+ data.texture.width);
                
                
                //Destroy(picture.GetComponent<Renderer>().material.mainTexture);
                // picture.GetComponent<Renderer>().material.mainTexture = data.texture;
                //Destroy(data.texture);
                spriteRenderer.sprite = Sprite.Create(data.texture, new Rect(0.0f, 0.0f, data.texture.width, data.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                spriteRenderer.transform.localScale = Vector3.one * (float)(800.0f/data.texture.width)*0.1f;
                Debug.Log("Scale width " + spriteRenderer.transform.localScale);

            }
            

            // Set our name so the container can alphabetize
             transform.parent.name = data.info.Name;
        }
    }
}
