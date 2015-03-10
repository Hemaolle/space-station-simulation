using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns debris and notifies the radar about it.
/// </summary>
public abstract class DebrisSpawner : MonoBehaviour {

    /// <summary>
    /// The debris prefab.
    /// </summary>
    public GameObject debris;

    /// <summary>
    /// The debris will be placed as children of this transform so that the hierarchy doesn't get too cluttered.
    /// </summary>
    public Transform debrisContainer;

    /// <summary>
    /// The radar will be notified about any spawned debris.
    /// </summary>
    public Radar radar;

    /// <summary>
    /// Spawns a new piece of debris and notifies the radar.
    /// </summary>
    /// <param name="position">Position of the new piece of debris.</param>
    /// <param name="velocity">Velocity of the new piece of debris.</param>
    public void SpawnDebris(Vector3 position, Vector3 velocity) {
        GameObject newDebris = (GameObject)Instantiate(debris, position, Quaternion.identity);
        newDebris.transform.parent = debrisContainer;
        newDebris.rigidbody.velocity = velocity;
        newDebris.GetComponent<Debris>().origin = transform;
        radar.Notify(newDebris);
    }
}
