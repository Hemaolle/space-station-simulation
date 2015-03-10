using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Updates the text according to the slider value.
/// </summary>
[RequireComponent (typeof (Text))]
public class SliderLabel : MonoBehaviour {

    /// <summary>
    /// The slider to monitor.
    /// </summary>
	public Slider sliderToMonitor;

    /// <summary>
    /// The format string used in float to string conversion.
    /// </summary>
	public string formatString = "0";

    /// <summary>
    /// The Text component which text will be updated.
    /// </summary>
	private Text label;

    // Use this for initialization
	void Start() {
		label = GetComponent<Text>();
	}

    // Update is called once per frame
	void Update() {
		label.text = sliderToMonitor.value.ToString(formatString);
	}
}
