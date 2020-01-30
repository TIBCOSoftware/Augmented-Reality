using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNextAction : MonoBehaviour
{
    [SerializeField]
    public MapNavManager MapNavManager;

    private Color32 displayColor;

    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        displayColor = rend.material.GetColor("_SpecColor");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        //mouse over Bar
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_SpecColor", new Color32(100, 100, 130, 100));
        MapNavManager.NextAction();
    }
    private void OnMouseUp()
    {
        //mouse left Bar
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_SpecColor", displayColor);
    }

}
