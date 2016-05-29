using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{

	public GameObject Projectile;
	public InventoryController Inventory;
	public InventoryController.InventoryItemType Ammo;
	public Transform Target;
	public float MuzzleVelocity;
	public float FireDelay;
	public string FireButton;

	private float next_shot = 0;
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButton (FireButton) && Time.time > next_shot && Inventory.GetQuantityOf (Ammo) > 0) {

			Inventory.DeductItem (Ammo, 1);
			next_shot = Time.time + FireDelay;

			GetComponent<AudioSource>().Play ();

			GameObject shot = Instantiate (Projectile, transform.position, Quaternion.identity) as GameObject;

			float target_altitude = Target.position.y - transform.position.y;

			Vector3 target_pos_xz = new Vector3 (Target.position.x, 0.0f, Target.position.z);
			Vector3 launcher_pos_xz = new Vector3 (transform.position.x, 0.0f, transform.position.z);
			Vector3 target_direction_xz = target_pos_xz - launcher_pos_xz;
			float target_range = Vector3.Distance (target_pos_xz, launcher_pos_xz);

			float g = Physics.gravity.magnitude;

			float discriminant = Mathf.Pow (MuzzleVelocity, 4) - (g * (g * Mathf.Pow (target_range, 2) + 2 * target_altitude * Mathf.Pow (MuzzleVelocity, 2)));

			float launchAngle;
			if (discriminant < 0) {
				//Target out of range. Launch at 45 degrees and hope for the best.
				launchAngle = Mathf.Deg2Rad * 45.0f;
			} else {
				float sqrt_discriminant = Mathf.Sqrt (discriminant);
				float launchAngle_1 = Mathf.Atan ((Mathf.Pow (MuzzleVelocity, 2) + sqrt_discriminant) / (g * target_range));
				float launchAngle_2 = Mathf.Atan ((Mathf.Pow (MuzzleVelocity, 2) - sqrt_discriminant) / (g * target_range));
				launchAngle = Mathf.Max (launchAngle_1, launchAngle_2); //Choose the more theatrical launch trajectory.
			}

			Vector3 launchDirection = Vector3.RotateTowards (target_direction_xz, Vector3.up, launchAngle, 0.0f);

			shot.GetComponent<Rigidbody>().velocity = launchDirection.normalized * MuzzleVelocity;
		}
	}
}
