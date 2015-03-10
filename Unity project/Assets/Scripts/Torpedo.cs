using UnityEngine;
using System.Collections;

/// <summary>
/// When collides with the target, destroys both itself and the target and instantiates an explosion that will be destroyed
/// after given timespan.
/// </summary>
public class Torpedo : MonoBehaviour {

    /// <summary>
    /// The explosion.
    /// </summary>
	public GameObject explosion;

    /// <summary>
    /// The explosion lifetime.
    /// </summary>
    public float explosionLifetime = 5f;

    /// <summary>
    /// The target.
    /// </summary>
    public GameObject target;

    /// <summary>
    /// The spawn position.
    /// </summary>
	private Vector3 spawnPosition;

    // Use this for initialization
	void Start () {
		spawnPosition = transform.position;
	}

    /// <summary>
    /// Draws the trajectory as a blue line in editor window.
    /// </summary>
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;			 
		Gizmos.DrawRay(spawnPosition, rigidbody.velocity.normalized * 150);
	}

    /// <summary>
    /// Checks if the collider is the target and if it is, both the torpedo and the target will be destroyed and an explosion
    /// will be instantiated.
    /// </summary>
    /// <param name="collider">Collider.</param>
	void OnTriggerEnter(Collider collider) {
		if(collider.gameObject == target)
		{
			GameObject newExplosion = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.transform.parent = transform.parent;
            Destroy(newExplosion, explosionLifetime);
			Destroy(collider.gameObject);
			Destroy(gameObject);
		}
	}


}
