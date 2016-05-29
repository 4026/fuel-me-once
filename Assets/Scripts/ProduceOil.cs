using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProduceOil : MonoBehaviour
{
	public float DrillingRange;
	public GameObject OilBarrel;
	public GameObject OilIndicator;
	public Vector3 IndicatorOffset;
	public float YieldEvery;

	private List<GameObject> deposits = new List<GameObject> ();
	private List<float> depositYieldProbabilities = new List<float> ();
	private float yieldProbabilityIncrement;
	private GameObject contentsIndicator;

	void Start ()
	{
		yieldProbabilityIncrement = 1.0f / YieldEvery;
	}


	void DrillingComplete ()
	{
		//Scan for deposits
		GameObject[] allDeposits = GameObject.FindGameObjectsWithTag ("Deposit");
		int nearbyOil = 0;

		foreach (GameObject deposit in allDeposits) {
			if (Vector3.Distance (transform.position, deposit.transform.position) <= DrillingRange) {
				deposits.Add (deposit);
				depositYieldProbabilities.Add (1.0f);
				nearbyOil += deposit.GetComponent<OilDeposit> ().Oil;
			}
		}

		//Set up Indicator
		Vector3 indicatorPosition = transform.position + IndicatorOffset;
		contentsIndicator = Instantiate (OilIndicator, indicatorPosition, Quaternion.identity) as GameObject;
		contentsIndicator.GetComponent<OilIndicator> ().Text = nearbyOil.ToString ();


		StartCoroutine (Produce ());
	}

	IEnumerator Produce ()
	{
		while (true) {
			int nearbyOil = 0;
			for (int i = 0; i < deposits.Count; ++i) {
				if (deposits [i] == null) {
					continue;
				}
				OilDeposit deposit = deposits [i].GetComponent<OilDeposit> ();
				float yieldProbability = depositYieldProbabilities [i];

				if (deposit.Oil > 0) {
					if (Random.value <= yieldProbability) {
						//Shoot out pickup
						Vector3 pickup_pos = transform.position;
						pickup_pos.y = 2.0f;
						GameObject pickup = Instantiate (OilBarrel, pickup_pos, Quaternion.identity) as GameObject;
						Vector3 launchVelocity = new Vector3 (3.0f, 4.0f, 0.0f);
						Quaternion launchAngle = Quaternion.AngleAxis (Random.Range (0, 360), Vector3.up);
						launchVelocity = launchAngle * launchVelocity;
						pickup.GetComponent<Rigidbody>().velocity = launchVelocity;

						deposit.Oil -= 1;
						depositYieldProbabilities [i] = 0.0f;
					} else {
						depositYieldProbabilities [i] = Mathf.Min (1.0f, yieldProbability + yieldProbabilityIncrement);
					}
				}

				nearbyOil += deposit.Oil;
			}

			contentsIndicator.GetComponent<OilIndicator> ().Text = nearbyOil.ToString ();

			yield return new WaitForSeconds (1.0f);
		}
	}

}
