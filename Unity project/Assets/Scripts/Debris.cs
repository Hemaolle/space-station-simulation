using UnityEngine;
using System.Collections;

/// <summary>
/// A piece of debris. Will destroy itself when far enough from the origin. Draws its trajectory on the editor view.
/// </summary>
public class Debris : MonoBehaviour {

	/// <summary>
	/// The debris is destroyed when further from the origin than this distance.
	/// </summary>
	public float destroyDistance = 120;

	/// <summary>
	/// The point from which the destruction distance is measured.
	/// </summary>
	public Transform origin = null;

    /// <summary>
    /// The start position.
    /// </summary>
	private Vector3 spawnPosition;	

    void Start() {
        spawnPosition = transform.position;
    }

	/// <summary>
    /// Destroys the gameobject if it's far enough from the origin.
    /// </summary>
	void Update () {
		if ((origin.position - transform.position).magnitude > destroyDistance)
			Destroy(gameObject);
	}

    /// <summary>
    /// Draws the trajectory of the gameobject on the editor view.
    /// </summary>
	void OnDrawGizmos() {
		Gizmos.color = Color.red;			 
		Gizmos.DrawRay(spawnPosition, rigidbody.velocity.normalized * 150);
	}
}
