using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Used to spawn a piece of debris manually. The user can first determine the coordinates and velocity of the new piece
/// of debris and launch the piece of debris when satisfied with the settings. The location and velocity of the piece of
/// debris will be visualized with the debrisPlacementPrefab and a line pointing in direction of the velocity.
/// </summary>
public class ManualDebrisSpawner : DebrisSpawner {

	/// <summary>
	/// Sliders for adjusting the position of the new piece of debris.
	/// </summary>
	public Slider [] positionSliders;

	/// <summary>
	/// Sliders for adjusting the velocity of the new piece of debris.
	/// </summary>
	public Slider [] velocitySliders;

	/// <summary>
	/// The velocity will be multiplied with this scalar.
	/// </summary>
	public Slider velocityMultiplierSlider;

	/// <summary>
	/// Aid for visualizing where the piece of debris will be spawned.
	/// </summary>
	public GameObject debrisPlacement;

    /// <summary>
    /// A line renderer for visualizing the velocity.
    /// </summary>
    public LineRenderer velocityLine;

	/// <summary>
	/// The position of the new piece of debris.
	/// </summary>
	private Vector3 debrisPosition;

	/// <summary>
	/// The velocity of the new piece of debris.
	/// </summary>
	private Vector3 debrisVelocity;	
	
	// Update is called once per frame
	void Update () {
		UpdateDebrisPosition();
		UpdateVelocityLine();
	}

	/// <summary>
	/// Updates the debris position and the velocity line.
	/// </summary>
	private void UpdateDebrisPosition ()
	{
		debrisPosition = new Vector3(positionSliders[0].value, positionSliders[1].value, positionSliders[2].value);
		debrisPlacement.transform.position = debrisPosition;
		velocityLine.SetPosition(0, debrisPosition);
		velocityLine.SetPosition(1, debrisPosition + debrisVelocity);
	}

	/// <summary>
	/// Updates the velocity line end point.
	/// </summary>
	void UpdateVelocityLine ()
	{
		debrisVelocity = new Vector3(velocitySliders[0].value, velocitySliders[1].value, velocitySliders[2].value) 
			* velocityMultiplierSlider.value;
		velocityLine.SetPosition(1, debrisPosition + debrisVelocity);
	}

    /// <summary>
    /// Launch a new piece of debris in the stored position and with the stored velocity.
    /// </summary>
    public void Launch() {
        SpawnDebris(debrisPosition, debrisVelocity);
    }
}
