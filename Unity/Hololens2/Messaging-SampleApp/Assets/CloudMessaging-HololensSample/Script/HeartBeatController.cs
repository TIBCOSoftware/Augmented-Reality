using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TIBCO.UX
{
    public class HeartBeatController : MonoBehaviour
    {
        public Text heartBeatText;
        public GameObject heart1;
        public GameObject heart2;
        public GameObject heart3;
        private int heartRate;
        // Start is called before the first frame update
        void Start()
        {
            SetHeartRate(0);
        }

        // Update is called once per frame
        void Update()
        {
            heartBeatText.text = this.heartRate.ToString("0") ;
            heart3.SetActive((heartRate > 120));
            heart2.SetActive((heartRate > 80));
            heart1.SetActive((heartRate > 1));

        }
        public void SetHeartRate(int value)
        {
            heartRate = value;
            
               
           
            
        }
    }
}