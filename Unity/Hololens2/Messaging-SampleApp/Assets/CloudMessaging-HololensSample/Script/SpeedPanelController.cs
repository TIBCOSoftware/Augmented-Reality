using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

namespace TIBCO.UX
{
    public class SpeedPanelController : MonoBehaviour
    {
        private string unit = "metric";
        public Text description;
        public Text speedText;
        private float speed = 0f;
        // Start is called before the first frame update
        public void ToggleUnitSystem(Interactable button)
        {
            // don't know to get the button state 
            // so assuming we start with metric 
            if ("metric" == unit)
            {
                unit = "imperial";
                description.text = "On a racing bicycle, a reasonably fit rider can ride at 25 mph.";
            }
            else
            {
                unit = "metric";
                description.text = "On a racing bicycle, a reasonably fit rider can ride at 40 km/h.";
            }
            Debug.Log("Switch unit system " + unit);

        }
        public void SetSpeedInKmh(float value)
        {
            Debug.Log("Set speed " + value);
            // protect against dummy values
            if (value < 80f)
            {
                this.speed = value;
            }
        }

        void Update()
        {
            if ("metric" == unit)
            {
                this.speedText.text = speed.ToString("0") + " Km/h";
            }
            else
            {
                var mph = speed / 1.609f;
                this.speedText.text = mph.ToString("0") + " mph";
            }
        }
    }

}