using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://tibcosoftware.github.io/Augmented-Reality/3DCharts/")]
public class PieChartManager : MonoBehaviour
{
    [Header("Pie Prefabs")]
    [Tooltip("this is how each Slice of the Pichart should look like.")]
    public GameObject PiePrefab;
    [Tooltip("store here a simple TextMesh to be used.")]
    public GameObject LabelPrefab;
    [Tooltip("general Chart Label, below the PieChart.")]
    public string ChartLabel;
    [Tooltip("number of elements of this PieChart.")]
    public int numberOfSlices = 100;
    [Tooltip("addantional Slice Size for each element.")]
    public float SlicesSize =0f;
    [Tooltip("displayed after each Scaling Variable.")]
    public string postFix;

    [Header("Pie Label")]
    [Tooltip("a Label Value shown at each Pie Section.")]
    public string[] PieLabel;

    [Header("Pie Scaling")]
    [Tooltip("the scale Value define size for each Section.")]
    public float[] PieSize;

    [Header("Pie Colors")]
    [Tooltip("Color to display for each Section.")]
    public Color[] PieColors;
    
    // Start is called before the first frame update
    void Start()
    {
        var scale = transform.localScale.x / (2.5f+ SlicesSize);
        var scale_y = transform.localScale.y;

        int slice = 0;
        for (int i = 0; i < numberOfSlices; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSlices;
            float x = Mathf.Cos(angle) * scale;
            //float z = Mathf.Tan(angle) * radius;
            float y = Mathf.Sin(angle) * scale;
            Vector3 pos = transform.position + new Vector3(x,y, 0);
            float angleDegrees = angle * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angleDegrees);
            var spliceObj = Instantiate(PiePrefab, pos, rotation);

            // first label
            if(i==0) renderLabel(x, y, slice, scale);

            // Pie chart
            if (100 * i / numberOfSlices < PieSize[slice] )
            {
                Renderer rend = spliceObj.GetComponent<Renderer>();
                rend.material.SetColor("_SpecColor", PieColors[slice]);
            } else
            {
                if (slice == PieColors.Length-1) {
                    Renderer rend = spliceObj.GetComponent<Renderer>();
                    rend.material.SetColor("_SpecColor", PieColors[slice]);
                } else
                {
                    Renderer rend = spliceObj.GetComponent<Renderer>();
                    rend.material.SetColor("_SpecColor", PieColors[slice]);

                    slice += 1;
                    // next label
                    renderLabel(x, y, slice, scale);
                }   
            }
            
            // move to main object
            spliceObj.transform.localScale = new Vector3((0.105f + SlicesSize) * scale, 0.05f * scale, 0.05f * scale);
            spliceObj.transform.parent = this.transform;
        }

        //Chart Label
        var LabelObj = Instantiate(LabelPrefab, new Vector3(transform.position.x, transform.position.y - scale_y / 2 - 0.1f * scale, transform.position.z - transform.localScale.z / 2), Quaternion.identity);
        LabelObj.transform.localScale = new Vector3(0.1f * scale, 0.1f * scale, 0.1f * scale);
        LabelObj.GetComponent<TextMesh>().text = ChartLabel;
        LabelObj.transform.parent = this.transform;

    }

    void renderLabel(float x, float y, int slice, float scale)
    {
        // Label Rendering
        Vector3 posLabel = transform.position + new Vector3(x + 0.2f * scale, y + 0.2f * scale, -0.5f * scale);
        var ValueObj = Instantiate(LabelPrefab, posLabel, Quaternion.identity);
        ValueObj.transform.localScale = new Vector3(0.2f * scale, 0.2f * scale, 0.2f * scale);
        ValueObj.GetComponent<TextMesh>().text = PieLabel[slice];
        ValueObj.transform.parent = this.transform;
    }
}
