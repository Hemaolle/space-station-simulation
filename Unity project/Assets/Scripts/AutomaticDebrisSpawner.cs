using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns pieces of debris automatically.
/// </summary>
public class AutomaticDebrisSpawner : DebrisSpawner {

    // These fields are implemented as C# properties to enable adjusting them from UI Sliders. Backing fields are public so that
    // they can be seen in the inspector view.

	/// <summary>
	/// Backing field for the SpawnRate.
	/// </summary>
	public float spawnRate = 1;

    /// <summary>
    /// The debris spawn rate per second.
    /// </summary>
	public float SpawnRate {
		get { return spawnRate; }
		set { spawnRate = value; }
	}
    	

    /// <summary>
    /// Backing field for the MinDistanceFromStation.
    /// </summary>
	public float minDistanceFromStation = 40;

    /// <summary>
    /// The debris won't spawn closer to the spawner position than this.
    /// </summary>
    public float MinDistanceFromStation {
        get { return minDistanceFromStation; }
        set { minDistanceFromStation = value; }
    }


	/// <summary>
    /// Backing field for the MinDistanceFromStation.
	/// </summary>
	public float maxDistanceFromStation = 100;

    /// <summary>
    /// The debris won't spawn further from the spawner than this.
    /// </summary>
    public float MaxDistanceFromStation {
        get { return maxDistanceFromStation; }
        set { maxDistanceFromStation = value; }
    }
    	

    /// <summary>
    /// Backing field for the Speed.
    /// </summary>
	public float speed = 1;

    /// <summary>
    /// The spawned debris will be given a velocity of this magnitude.
    /// </summary>
    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

	/// <summary>
    /// Backing field for the MaxAngle.
    /// </summary>
	public float maxAngle = 90;

    /// <summary>
    /// Defines how much the direction of the velocity of spawned debris will deviate from a vector pointing from spawnPosition to
    /// the spawner in degrees.
    /// </summary>
	public float MaxAngle {
        get { return maxAngle; }
        set { maxAngle = value; }
	}	


    /// <summary>
    /// Indicates if the automatic spawning is on.
    /// </summary>
	private bool autoSpawningOn = true;

    /// <summary>
    /// The autospawn routine.
    /// </summary>
	private IEnumerator autospawnRoutine;

	// Use this for initialization
	void Start () {
		autospawnRoutine = SpawnDebrisContinuously();
        if (autoSpawningOn) {
            StartCoroutine(autospawnRoutine);
        }
	}

	/// <summary>
	/// Spawns the debris continuously. The debris will be spawned in an area defined by the minimum and maximum distances from
    /// the spawner position. It will have a velocity that deviates from a vector pointing from spawnPosition to the spawner by
    /// MaxAngle degrees.
	/// </summary>
	private IEnumerator SpawnDebrisContinuously() {
		while(true) {
			yield return new WaitForSeconds(1 / spawnRate);

			Vector3 spawnPosition;
			// Spawn inside a sphere with radius maxDistance - minDistance.
			if (maxDistanceFromStation - minDistanceFromStation == 0) {
				spawnPosition = Random.insideUnitSphere * 0.001f;
			} else {
				spawnPosition = Random.insideUnitSphere * (maxDistanceFromStation - minDistanceFromStation);
			}

			// Leave an empty sphere with radius minDistance around the spawner where no debris spawns.
			spawnPosition += spawnPosition.normalized * minDistanceFromStation;
			// Move to correct position relative to the spawner.
			spawnPosition += transform.position;

			Vector3 spawnVelocityDirection = RandomDirection((transform.position - spawnPosition).normalized, 
			                                        maxAngle);
			SpawnDebris(spawnPosition, spawnVelocityDirection * speed);
		}
	}

	/// <summary>
	/// Returns a random direction that differs from the default direction at most max angle degrees.
	/// </summary>
	/// <returns>The random direction.</returns>
	/// <param name="defaultDirection">The default direction.</param>
	/// <param name="maxAngle">Maximum deviation from the default direction.</param>
	public Vector3 RandomDirection(Vector3 defaultDirection, float maxAngle) {
        //
		Vector3 axis = Quaternion.AngleAxis(Random.Range(0,360), Vector3.forward) * Vector3.up;
		Vector3 deviatedFromForward = Quaternion.AngleAxis(Random.Range(0,maxAngle), axis) * Vector3.forward;
		return Quaternion.FromToRotation(Vector3.forward, defaultDirection.normalized) * deviatedFromForward;
	}

    /// <summary>
    /// Toggles autospawning on and off.
    /// </summary>
	public void Toggle() {
		if (autoSpawningOn) {
			StopCoroutine(autospawnRoutine);
		}
		else {
			StartCoroutine(autospawnRoutine);
		}
		autoSpawningOn = !autoSpawningOn;
	}
}
