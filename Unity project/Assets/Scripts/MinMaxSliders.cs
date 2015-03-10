using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Limits values of two sliders so that the value of the maxSlider can never be smaller than the value of the minSlider and vice
/// versa. Connect the On Value Changed events of the sliders to methods in this class.
/// </summary>
[ExecuteInEditMode]
public class MinMaxSliders : MonoBehaviour {

    /// <summary>
    /// The minimum slider.
    /// </summary>
    public Slider minSlider;

    /// <summary>
    /// The maximum slider.
    /// </summary>
    public Slider maxSlider;	

    /// <summary>
    /// Connect this method to the On Value Changed event of the maxSlider.
    /// </summary>
    public void MaxSliderUpdated() {
        if (minSlider != null && maxSlider != null) {
            if(maxSlider.value < minSlider.value) {
                maxSlider.value = minSlider.value;
            }
        }
    }

    /// <summary>
    /// Connect this method to the On Value Changed event of the minSlider.
    /// </summary>
    public void MinSliderUpdated() {
        if (minSlider != null && maxSlider != null) {
            if(minSlider.value > maxSlider.value) {
                minSlider.value = maxSlider.value;
            }
        }
    }
}
