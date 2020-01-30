using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Sets the height of the ARSessionOrigin according to the value of a UI.Slider.
/// </summary>
[RequireComponent(typeof(ARSessionOrigin))]
public class HeightController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The slider used to control the height factor.")]
    Slider m_Slider;

    /// <summary>
    /// The slider used to control the scale factor.
    /// </summary>
    public Slider slider
    {
        get { return m_Slider; }
        set { m_Slider = value; }
    }

    [SerializeField]
    [Tooltip("The text used to display the current height factor on the screen.")]
    Text m_Text;

    /// <summary>
    /// The text used to display the current height factor on the screen.
    /// </summary>
    public Text text
    {
        get { return m_Text; }
        set { m_Text = value; }
    }

    [SerializeField]
    [Tooltip("Minimum height factor.")]
    public float m_Min = .1f;

    /// <summary>
    /// Minimum scale factor.
    /// </summary>
    public float min
    {
        get { return m_Min; }
        set { m_Min = value; }
    }

    [SerializeField]
    [Tooltip("Maximum height factor.")]
    public float m_Max = 10f;

    /// <summary>
    /// Maximum scale factor.
    /// </summary>
    public float max
    {
        get { return m_Max; }
        set { m_Max = value; }
    }

    /// <summary>
    /// Invoked whenever the slider's value changes
    /// </summary>
    public void OnSliderValueChanged()
    {
        if (slider != null)
            height = slider.value * (max - min) + min;
    }

    float height
    {
        get
        {
            Debug.Log("** get Value " + m_SessionOrigin.transform.position.y);
            return m_SessionOrigin.transform.position.y;
        }
        set
        {
            UpdateText();
            Debug.Log("** Value change " + value);
            m_SessionOrigin.transform.position = new Vector3(0, -value, 0);
            //    pos.y= value;
            Debug.Log("** Value change done " + value);


        }
    }

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    void OnEnable()
    {
        if (slider != null)
            slider.value = (height - min) / (max - min);
        UpdateText();
    }

    void UpdateText()
    {
        if (text != null)
        {
            Debug.Log("** Value change Height " + height);
            text.text = "Height: " + height;
        }
    }

    ARSessionOrigin m_SessionOrigin;
}
