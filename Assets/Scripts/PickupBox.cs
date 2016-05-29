using UnityEngine;
using System.Collections;

public class PickupBox : MonoBehaviour
{

	public float HoverHeight;
	public float RotateSpeed;
	public float BounceAmount;
	public float BounceSpeed;
	public AudioClip PickupNoise;
	public AudioClip FailNoise;

	public InventoryController.InventoryItemType contents;
	public int quantity;

	private float bounceCenter;
	private float bounceStart;
	private int cost = 0;
	public int Cost {
		set { cost = value; }
	}

	void Start ()
	{
		GetComponent<Rigidbody>().angularVelocity = new Vector3 (0.0f, RotateSpeed, 0.0f);
	}

	void FixedUpdate ()
	{
		if (GetComponent<Rigidbody>().useGravity) {
			Ray sky_ray = new Ray (new Vector3 (transform.position.x, 50.0f, transform.position.z), Vector3.down);
			RaycastHit hit_info;
			if (Physics.Raycast (sky_ray, out hit_info)) {
				if (transform.position.y < hit_info.point.y + HoverHeight) {
					bounceCenter = hit_info.point.y + HoverHeight;
					bounceStart = Time.time;
					transform.position = new Vector3 (transform.position.x, hit_info.point.y + HoverHeight, transform.position.z);
					
					GetComponent<Rigidbody>().useGravity = false;
					GetComponent<Rigidbody>().velocity = Vector3.zero;
				}
			}
		} else {
			transform.position = new Vector3 (transform.position.x, bounceCenter + -BounceAmount * Mathf.Sin (BounceSpeed * (Time.time - bounceStart)), transform.position.z);
		}

	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag != "Player") {
			return;
		}

		if (cost > 0) {
			MoneyTracker money = GameObject.FindGameObjectWithTag ("GameController").GetComponent<MoneyTracker> ();
			if (money.PlayerBalance < cost) {
				AudioSource.PlayClipAtPoint (FailNoise, transform.position);
				return;
			}
			money.PlayerBalance -= cost;
		}

		InventoryController inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<InventoryController> ();
		int new_quantity = inventory.AddItem (contents, quantity);

		if (quantity != new_quantity) {
			AudioSource.PlayClipAtPoint (PickupNoise, transform.position);
			quantity = new_quantity;
		} else {
			AudioSource.PlayClipAtPoint (FailNoise, transform.position);
		}

		if (quantity == 0) {
			Destroy (gameObject);
		}
	}
}
