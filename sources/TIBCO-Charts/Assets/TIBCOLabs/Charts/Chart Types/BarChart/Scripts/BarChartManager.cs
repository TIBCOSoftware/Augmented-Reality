using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://tibcosoftware.github.io/Augmented-Reality/3DCharts/")]
public class BarChartManager : MonoBehaviour
{
    [Header("Bar Prefab")]
    [Tooltip("store here your Ground Plane Prefab to be used.")]
    public GameObject GroundPrefab;
    [Tooltip("this is how each Bar of the Barchart should look like.")]
    public GameObject BarPrefab;
    [Tooltip("store here a simple TextMesh to be used.")]
    public GameObject LabelPrefab;
    [Tooltip("general Chart Label, below the BarChart.")]
    public string ChartLabel;
    [Tooltip("define if all bars should be rendered in centered mode.")]
    public bool centered;
    [Tooltip("displayed after each Scaling Variable.")]
    public string postFix;

    [Header("Bar Label")]
    [Tooltip("a Label Value shown at each Bar.")]
    public string[] BarLabel;

    [Header("Bar Scaling")]
    [Tooltip("the scale Value define size for each Bar.")]
    public float[] BarSize;

    [Header("Bar Color")]
    [Tooltip("Color to display for each Bar.")]
    public Color[] BarColor;

    // Start is called before the first frame update
    void Start()
    {
        var scale = transform.localScale.x;
        var scale_y = transform.localScale.y;
        var scale_z = transform.localScale.z;

        this.transform.localScale = new Vector3(scale * BarSize.Length/2, scale_y, scale_z);
        this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        var GroundObj = Instantiate(GroundPrefab, new Vector3(transform.position.x, transform.position.y-scale_y/2, transform.position.z), Quaternion.identity);

        Renderer rend = GroundObj.GetComponent<Renderer>();
        rend.material.SetColor("_SpecColor", new Color32(222,222,222,80));

        GroundObj.transform.localScale = new Vector3(scale * BarSize.Length/2, 0.01f*scale, scale_z/2);
        GroundObj.GetComponent<Rigidbody>().mass = 1;
        GroundObj.transform.parent = this.transform;

        //Chart Bars
        for (int i = 0; i < BarSize.Length; i++)
        {
            // Bar Rendering
            var BarObj = Instantiate(BarPrefab, new Vector3(transform.position.x - transform.localScale.x / 2 + transform.localScale.x/BarSize.Length*i + 0.04f, transform.position.y, transform.position.z), Quaternion.identity);
            BarObj.transform.parent = this.transform;

            rend = BarObj.GetComponent<Renderer>();
            rend.material.SetColor("_SpecColor", BarColor[i]);

            var scaledSize = (1f/ 100) * BarSize[i];
            BarObj.GetComponent<Bar>().size = scaledSize;

            BarObj.GetComponent<Bar>().scale = scale;
            BarObj.GetComponent<Bar>().centered = centered;
            BarObj.GetComponent<Bar>().displayColor = BarColor[i];
            BarObj.GetComponent<Rigidbody>().mass = 1;

            // Value Rendering
            var ValueObj = Instantiate(LabelPrefab, new Vector3(transform.position.x - transform.localScale.x / 2 + transform.localScale.x / BarSize.Length * i + 0.04f, transform.position.y + scaledSize*scale/2, transform.position.z - transform.localScale.z/4), Quaternion.identity);
            ValueObj.transform.localScale = new Vector3(0.09f*scale, 0.09f * scale, 0.09f * scale);
            ValueObj.GetComponent<TextMesh>().text = BarSize[i].ToString() + postFix;
            ValueObj.transform.parent = this.transform;

            // Label Rendering
            var BarLabelObj = Instantiate(LabelPrefab, new Vector3(transform.position.x - transform.localScale.x / 2 + transform.localScale.x / BarSize.Length * i + 0.04f, transform.position.y - scale_y / 2 + 0.1f * scale, transform.position.z - transform.localScale.z/4), Quaternion.identity);
            BarLabelObj.transform.localScale = new Vector3(0.09f * scale, 0.09f * scale, 0.09f * scale);
            BarLabelObj.GetComponent<TextMesh>().text = BarLabel[i];
            BarLabelObj.transform.parent = this.transform;

        }

        //Chart Label
        var LabelObj = Instantiate(LabelPrefab, new Vector3(transform.position.x, transform.position.y - scale_y / 2 - 0.1f*scale, transform.position.z - transform.localScale.z / 2), Quaternion.identity);
        LabelObj.transform.localScale = new Vector3(0.1f * scale, 0.1f * scale, 0.1f * scale);
        LabelObj.GetComponent<TextMesh>().text = ChartLabel;
        LabelObj.transform.parent = this.transform;
    }
}
