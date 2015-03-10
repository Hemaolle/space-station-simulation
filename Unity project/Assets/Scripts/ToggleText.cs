using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Toggles text of a Text component between the original text and the textToToggleTo.
/// </summary>
[RequireComponent (typeof (Text))]
public class ToggleText : MonoBehaviour {

    /// <summary>
    /// The text to toggle to.
    /// </summary>
	public string textToToggleTo;

    /// <summary>
    /// The original text.
    /// </summary>
	private string originalText;

    /// <summary>
    /// The Text component which text property will be toggled.
    /// </summary>
	private Text text;

    /// <summary>
    /// If the original text is on.
    /// </summary>
	private bool originalTextOn = true;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		originalText = text.text;
	}

    /// <summary>
    /// Toggle the text. If original text is visible change to textToToggleTo. Otherwise change to the original text.
    /// </summary>
	public void Toggle() {
		if (originalTextOn) {
			text.text = textToToggleTo;
		}
		else {
			text.text = originalText;
		}
		originalTextOn = !originalTextOn;
	}
}
