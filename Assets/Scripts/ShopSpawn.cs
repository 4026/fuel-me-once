using UnityEngine;
using System.Collections;

public class ShopSpawn : MonoBehaviour
{

	public GameObject ItemType;
	public float RefreshTime;
	public int Cost;
	public GameObject PriceIndicator;
	public Vector3 indicatorOffset;

	private GameObject lastSpawn;
	private TextMesh indicator;

	void Start ()
	{
		SpawnBox ();
		StartCoroutine (CheckItem ());

		GameObject new_indicator = Instantiate (PriceIndicator, transform.position + indicatorOffset, Quaternion.identity) as GameObject;
		indicator = new_indicator.GetComponent<TextMesh> ();
		indicator.text = "$ " + Cost.ToString ();
	}
	
	IEnumerator CheckItem ()
	{
		while (true) {
			if (lastSpawn == null) {
				yield return new WaitForSeconds (RefreshTime);
				SpawnBox ();
			}
			yield return null;
		}
	}

	void SpawnBox ()
	{
		lastSpawn = Instantiate (ItemType, transform.position, Quaternion.identity) as GameObject;
		lastSpawn.GetComponent<PickupBox> ().Cost = Cost;
	}
}
