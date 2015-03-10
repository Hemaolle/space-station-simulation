using UnityEngine;
using System.Collections;

/// <summary>
/// The space station. Holds the safety zone radius variable and handles resizing a sphere visualizing the radius of the zone.
/// </summary>
public class SpaceStation : MonoBehaviour
{
    /// <summary>
    /// The safety zone radius.
    /// </summary>
    public float safetyZoneRadius = 30;

    /// <summary>
    /// A sphere visualizing the safety zone. Use the GameObject -> 3D Object -> Sphere to get a sphere of the correct size.
    /// </summary>
    public GameObject safetyZoneSphere;

    /// <summary>
    /// Gets or sets the safety zone radius. Setting updates also the scale of the sphere.
    /// </summary>
    /// <value>The safety zone radius.</value>
    public float SafetyZoneRadius
    {
        get { return safetyZoneRadius; }
        set
        { 
            safetyZoneRadius = value; 
            SetGlobalScale(safetyZoneSphere.transform, Vector3.one * safetyZoneRadius * 2);
        }
    }

    /// <summary>
    /// Sets the global scale of a transform.
    /// </summary>
    /// <param name="aTransform">A transform.</param>
    /// <param name="desiredGlobalScale">Desired global scale.</param>
    public void SetGlobalScale(Transform aTransform, Vector3 desiredGlobalScale)
    {
        float scaleX = desiredGlobalScale.x * (aTransform.localScale.x / aTransform.lossyScale.x);
        float scaleY = desiredGlobalScale.y * (aTransform.localScale.y / aTransform.lossyScale.y);
        float scaleZ = desiredGlobalScale.z * (aTransform.localScale.z / aTransform.lossyScale.z);
        aTransform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
}
