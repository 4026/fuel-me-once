using UnityEngine;
using System.Collections;

public class TargetController : MonoBehaviour
{
	public Rigidbody player;
	public float target_speed;
	public float max_range;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		float aim_x = Input.GetAxis ("Aim X");
		float aim_z = Input.GetAxis ("Aim Y");

		Vector3 aim_change = new Vector3 (aim_x, 0.0f, aim_z);

		Vector3 position_change = (player.velocity * Time.deltaTime) + (aim_change * target_speed);
		transform.position += position_change;

		//Tether target within range of player.
		Vector3 from_player = transform.position - player.position;
		if (from_player.magnitude > max_range) {
			transform.position = player.position + (from_player.normalized * max_range);
		}

		//Update Y
		Ray sky_ray = new Ray (new Vector3 (transform.position.x, 50.0f, transform.position.z), Vector3.down);
		RaycastHit hit_info;
		if (Physics.Raycast (sky_ray, out hit_info)) {
			transform.position = new Vector3 (transform.position.x, hit_info.point.y + 0.1f, transform.position.z);
		}

	}
}
