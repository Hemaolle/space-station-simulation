using UnityEngine;
using System.Collections;

/// <summary>
/// Clears the screen from debris and torpedos.
/// </summary>
public class Cleaner : MonoBehaviour {

    /// <summary>
    /// Clears the screen from debris and torpedos.
    /// </summary>
	public void Clear() {
		foreach(GameObject pieceOfDebris in GameObject.FindGameObjectsWithTag("Debris")) {
			Destroy(pieceOfDebris);
		}
        foreach(GameObject pieceOfDebris in GameObject.FindGameObjectsWithTag("Torpedo")) {
            Destroy(pieceOfDebris);
        }
	}
}
