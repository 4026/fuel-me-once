using UnityEngine;
using System.Collections;

public class DepositSpawner : MonoBehaviour
{

	public int NumDeposits;
	public int DepositContentsMax;
	public int DepositContentsMin;
	public Rect SpawnBoundary;
	public GameObject Deposit;

	void Start ()
	{
		for (int i = 0; i < NumDeposits; ++i) {
			float x = Random.Range (SpawnBoundary.xMin, SpawnBoundary.xMax);
			float y = -1.0f;
			float z = Random.Range (SpawnBoundary.yMin, SpawnBoundary.yMax);

			Vector3 spawnPos = new Vector3 (x, y, z);
			GameObject newDeposit = Instantiate (Deposit, spawnPos, Quaternion.identity) as GameObject;
			newDeposit.GetComponent<OilDeposit> ().Oil = Random.Range (DepositContentsMin, DepositContentsMax);
		}
	}

}
