using UnityEngine;
using System.Collections;

public class DrillIntoGround : MonoBehaviour
{

	public float targetDepth;
	public float drillTime;
	public float numRotations;
	public AudioClip landingSound;
	public AudioClip drillSound;

	void OnCollisionEnter (Collision collision)
	{
		if (collision.collider.tag != "TheSurface") {
			return;
		}

		AudioSource.PlayClipAtPoint (landingSound, transform.position);
		
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().isKinematic = true;

		Hashtable rotation_options = new Hashtable ();
		rotation_options.Add ("y", numRotations);
		rotation_options.Add ("time", drillTime);
		rotation_options.Add ("space", Space.World);
		rotation_options.Add ("easetype", iTween.EaseType.easeInOutCubic);
		rotation_options.Add ("oncomplete", "sendDrillingComplete");
		iTween.RotateBy (gameObject, rotation_options);

		Hashtable move_options = new Hashtable ();
		move_options.Add ("y", targetDepth);
		move_options.Add ("time", drillTime);
		move_options.Add ("easetype", iTween.EaseType.easeInOutCubic);
		iTween.MoveTo (gameObject, move_options);

		AudioSource.PlayClipAtPoint (drillSound, transform.position);
	}

	void sendDrillingComplete ()
	{
		gameObject.SendMessage ("DrillingComplete");
	}
}
