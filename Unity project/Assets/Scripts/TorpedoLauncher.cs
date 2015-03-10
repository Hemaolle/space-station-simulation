using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Can calculate if the trajectory of a piece of debris will enter the safety zone and try to shoot torpedoes at debris.
/// </summary>
[RequireComponent(typeof(SpaceStation))]
public class TorpedoLauncher : MonoBehaviour {

	/// <summary>
	/// The GameObject that the launcher will shoot instances of.
	/// </summary>
	public GameObject torpedo;
		

    /// <summary>
    /// Backing field for the TorpedoSpeed.
    /// </summary>
	public float torpedoSpeed = 5f;

    /// <summary>
    /// The speed of the torpedoes.
    /// </summary>
    public float TorpedoSpeed {
        get { return torpedoSpeed; }
        set { torpedoSpeed = value; }
    }

    /// <summary>
    /// The torpedos will be placed as children of this transform so that the hierarchy doesn't get too cluttered.
    /// </summary>
    public Transform torpedoContainer;

    /// <summary>
    /// The space station.
    /// </summary>
    public SpaceStation spaceStation;

    /// <summary>
    /// This type of equation will be thrown if there is no solution to a quadratic equation.
    /// </summary>
	[Serializable]
	public class QuadraticEquationException : System.Exception
	{
		public QuadraticEquationException() {}
		public QuadraticEquationException(string message) : base(message) {}
		public QuadraticEquationException(string message, System.Exception inner) : base(message, inner) {}
		
		// Constructor needed for serialization 
		// when exception propagates from a remoting server to the client.
		protected QuadraticEquationException(System.Runtime.Serialization.SerializationInfo info,
		                                     System.Runtime.Serialization.StreamingContext context) 
										: base(info, context){}
	}

    /// <summary>
    /// Shoots a torpedo at a piece of debris if it's possible.
    /// </summary>
    /// <returns><c>true</c>, if interception was possible, <c>false</c> otherwise.</returns>
    /// <param name="debris">The piece of debris to intercept.</param>
    /// <param name="collisionLocation">Predicted collision location.</param>
	public bool TryInterceptDebris(GameObject debris, out Vector3 collisionLocation) {		
		try {
            float timeToCollision;
			Vector3 torpedoVelocity = CalculateTorpedoVelocity(debris.transform.position, 
			                                                   debris.rigidbody.velocity, out timeToCollision);
			GameObject newTorpedo = (GameObject)Instantiate(torpedo, transform.position, Quaternion.identity);
            newTorpedo.transform.parent = torpedoContainer;
			newTorpedo.rigidbody.velocity = torpedoVelocity;
            newTorpedo.GetComponent<Torpedo>().target = debris;

            collisionLocation = timeToCollision * torpedoVelocity + transform.position;

		} catch (QuadraticEquationException ex) {
            collisionLocation = Vector3.zero;
			return false;
		}        
        return true;
	}

    /// <summary>
    /// <para>Calculates the torpedo velocity so that it will intercept the trajectory of the target.</para>
    /// <para>Based on <a href="http://www.gamedev.net/page/resources/_/technical/math-and-physics/shooting-at-stuff-r3884"> 
    /// an article on gamedev.net</a>, expanded to 3D</para>
    /// </summary>
    /// <returns>The torpedo velocity.</returns>
    /// <param name="targetPosition">Target position.</param>
    /// <param name="targetVelocity">Target velocity.</param>
    /// <param name="timeToCollision">When this method returns, contains the predicted time to collision if the torpedo would be
    ///                                 shot at the target with the returned velocity.</param>
    /// <exception cref="QuadraticEquationException">If there is no solution.</exception>
	Vector3 CalculateTorpedoVelocity(Vector3 targetPosition, Vector3 targetVelocity, out float timeToCollision) {
		Vector3 r = targetPosition - transform.position;
		float a = targetVelocity.x * targetVelocity.x + targetVelocity.y * targetVelocity.y
			+ targetVelocity.z * targetVelocity.z - torpedoSpeed * torpedoSpeed;
		float b = 2 * (r.x * targetVelocity.x + r.y * targetVelocity.y + r.z * targetVelocity.z);
		float c = r.x * r.x + r.y * r.y + r.z * r.z;
		float bulletFlightTime = 0;

		if(r.sqrMagnitude < 2 * double.MinValue) {
			throw new QuadraticEquationException("Torpedo launcher and target are already colliding");
		}

		// If the squared velocity of the target and the bullet are the same, the equation
		// collapses to tBullet*b = -c.  If they are REALLY close to each other,
		// you could get some weirdness here.  Do some "is it close" checking?
		if(Mathf.Abs(a) < 2 * 0.01)
		{
			if(Mathf.Abs(b) < 2 * 0.01)
			{
				throw new QuadraticEquationException("No solution exists and the bullet and the target have same velocity");
			}
			bulletFlightTime = -c/b;
		}
		else {
			// Calculate the discriminant to figure out how many solutions there are.
			float discriminant = b * b - 4 * a * c;

			if (discriminant < 0) {
				throw new QuadraticEquationException("No real solution exists, negative discriminant");
			}
			else if (discriminant == 0) {
				bulletFlightTime = -b / (2 * a);
			}
			else {
				float time1, time2;
				float sqrt = Mathf.Sqrt(discriminant);
				time1 = (-b - sqrt) / (2 * a);
				time2 = (-b + sqrt) / (2 * a);

				if(0 < time1 && ((time2 < 0) || (time1 < time2))) {
                    // time1 is positive and it is either smaller than time2 or time2 is negative
					bulletFlightTime = time1;
				}
				else {
					bulletFlightTime = time2;
				}
			}
		}
		if (bulletFlightTime < 0)
			throw new QuadraticEquationException("No solution exists in the future");
		Vector3 collisionPosition = targetPosition + targetVelocity * bulletFlightTime;
		Vector3 torpedoVelocity = (collisionPosition - transform.position).normalized * torpedoSpeed;
        timeToCollision = bulletFlightTime;
		return torpedoVelocity;
	}

    /// <summary>
    /// Calculates if the object's trajectory will enter the safety zone. Based on line-sphere intersection, see
    /// <a href="http://en.wikipedia.org/wiki/Line%E2%80%93sphere_intersection"> Wikipedia</a>.
    /// </summary>
    /// <returns><c>true</c>, if trajectory enters the safety zone, <c>false</c> otherwise.</returns>
    /// <param name="objectPosition">Object position.</param>
    /// <param name="objectVelocity">Object velocity.</param>
	public bool DoesTrajectoryEnterSafetyZone(Vector3 objectPosition, Vector3 objectVelocity) {         
        Vector3 objectDirection = objectVelocity.normalized;
        float a = Vector3.Dot(objectDirection, (objectPosition - spaceStation.transform.position));
        float b = (objectPosition - spaceStation.transform.position).magnitude;
        float underSqrt = a * a - b * b + spaceStation.safetyZoneRadius * spaceStation.safetyZoneRadius;

        if (underSqrt < 0) {
            // No interception points.
            return false;
        }
        float distance1 = -a + Mathf.Sqrt(underSqrt);
        float distance2 = -a - Mathf.Sqrt(underSqrt);

        if (0 < distance1 || 0 < distance2) {
            // One of the interception points in forward of the object.
            return true;
        }
        else {
            return false;
        }
	}
}
