using UnityEngine;
using System.Collections;

/// <summary>
/// <para>Gets notifications from detected pieces of debris. Changes the color of the debris like this:</para>
/// 
/// <para>No change: The trajectory of the piece of debris will not enter the safety zone of the station.</para>
/// <para>Green: The piece of debris can be destroyed before it enters the safety zone.</para>
/// <para>Yellow: The piece of debris can be destroyed inside the safety zone.</para>
/// <para>Red: The trajectory of the piece of debris will enter the safety zone and it can't be destroyed.</para>
/// </summary>
public class Radar : MonoBehaviour {

    /// <summary>
    /// The torpedo launcher.
    /// </summary>
    public TorpedoLauncher launcher;

    /// <summary>
    /// The space station.
    /// </summary>
    public SpaceStation station;

    /// <summary>
    /// Asks the launcher if the trajectory will enter the safety zone and if it will, asks the launcher to intercept the piece
    /// of debris with a torpedo. Updates the color of the piece of debris if it will enter the safety zone (see class
    /// description).
    /// </summary>
    /// <param name="debris">The detected piece of debris</param>
	public void Notify(GameObject debris) {
        if (launcher.DoesTrajectoryEnterSafetyZone(debris.transform.position, debris.rigidbody.velocity))
        {
            Vector3 torpedoCollisionLocation;
            if (launcher.TryInterceptDebris(debris, out torpedoCollisionLocation)) {
                if ((torpedoCollisionLocation - station.transform.position).magnitude < station.safetyZoneRadius) {
                    // Can be shot down inside the safety zone
                    debris.renderer.material.color = Color.yellow;
                }
                else {
                    // Can be shot down before entering the safety zone
                    debris.renderer.material.color = Color.green;
                }
            }
            else {
                // Trajectory enters the safety zone but can't be shot down at all
                debris.renderer.material.color = Color.red;
            }
        }
    }
}
