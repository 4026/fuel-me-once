using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public WheelCollider[] front_wheels;
	public WheelCollider[] rear_wheels;
	public float max_speed;
	public float acceleration;
	public float max_turn_angle;
	public float dead_zone;
	public float brake_power;
	public float SRiMecActivationDelay;
	public Vector3 CenterOfMass;

	void Start ()
	{
		GetComponent<Rigidbody>().centerOfMass = CenterOfMass;

		StartCoroutine (SRiMec ());
	}

	void FixedUpdate ()
	{
		float move_x = Input.GetAxis ("Horizontal");
		float move_z = Input.GetAxis ("Vertical");
		float steer_relative = Input.GetAxis ("RelativeHorizontal");


		Vector3 intention = new Vector3 (move_x, 0.0f, move_z);
		Vector3 current = transform.forward;
		current.y = 0.0f;

		float angle = Vector3.Angle (intention, current);
		Vector3 cross = Vector3.Cross (intention, current);

		float steer_angle = (cross.y < 0) ? angle : -angle;
		steer_angle = Mathf.Clamp (steer_angle, -max_turn_angle, max_turn_angle);

		foreach (WheelCollider wheel in front_wheels) {
			if (intention.magnitude > dead_zone) { //Steering - Absolute Controls
				wheel.steerAngle = steer_angle;
			} else if (Mathf.Abs (steer_relative) > dead_zone) { //Steering - Relative Controls
				wheel.steerAngle = max_turn_angle * steer_relative;
			} else {
				wheel.steerAngle = 0;
			}
		}

		//Throttle
		//  - Absolute Controls
		float throttle = intention.magnitude;

		//  - Relative Controls
		float relative_throttle = Input.GetAxis ("RelativeVertical");
		if (relative_throttle > dead_zone) {  	
			throttle = relative_throttle;
		}

		foreach (WheelCollider wheel in front_wheels) {
			if (GetComponent<Rigidbody>().velocity.magnitude < max_speed) {
				wheel.motorTorque = throttle * acceleration;
			} else {
				wheel.motorTorque = 0;
			}
		}

		foreach (WheelCollider wheel in rear_wheels) {
			if (GetComponent<Rigidbody>().velocity.magnitude < max_speed) {
				wheel.motorTorque = throttle * acceleration;
			} else {
				wheel.motorTorque = 0;
			}
		}

		//Brakes
		float brakes = Input.GetAxis ("Left Trigger");
		if (relative_throttle < -dead_zone) {
			brakes = -relative_throttle;
		}

		if (brakes > dead_zone) {
			foreach (WheelCollider wheel in rear_wheels) {
				if (Vector3.Dot (GetComponent<Rigidbody>().velocity, transform.forward) > dead_zone) {
					wheel.brakeTorque = brakes * brake_power;
				} else {
					wheel.motorTorque = brakes * acceleration * -0.5f;
				}
			}

		} else {
			foreach (WheelCollider wheel in rear_wheels) {
				wheel.brakeTorque = 0.0f;
			}
		}

	}

	IEnumerator SRiMec ()
	{
		while (true) {
			float totalWaitTime = 0.0f;

			while (Vector3.Dot (transform.up, Vector3.down) > -0.8f && GetComponent<Rigidbody>().velocity.magnitude < 0.5f) {
				if (totalWaitTime >= SRiMecActivationDelay) {
					transform.position = new Vector3 (transform.position.x, transform.position.y + 2.0f, transform.position.z);
					transform.rotation = Quaternion.identity;
					break;
				}
				totalWaitTime += 0.5f;

				yield return new WaitForSeconds (0.5f);
			}

			yield return new WaitForSeconds (0.5f);
		}

	}

}
