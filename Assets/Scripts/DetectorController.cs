using UnityEngine;
using System.Collections;

public class DetectorController : MonoBehaviour
{

	public float ping_delay;
	public GameObject ping;

	public float tumble;

	private float next_ping = 0;
	private bool isDeployed = false;
	
	void Start ()
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}


	void Update ()
	{
		if (isDeployed && Input.GetButton ("PingDetector") && Time.time > next_ping) {
			next_ping = Time.time + ping_delay;

			Vector3 start_pos = transform.position;
			start_pos.y = 0.0f;
			Instantiate (ping, start_pos, Quaternion.identity);

			GetComponent<AudioSource>().Play ();
		}
	}

	void DrillingComplete ()
	{
		isDeployed = true;
	}
}
