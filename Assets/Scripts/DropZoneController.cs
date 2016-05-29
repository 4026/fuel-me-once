using UnityEngine;
using System.Collections;

public class DropZoneController : MonoBehaviour
{
	public InventoryController inventory;
	public MoneyTracker moneyTracker;


	void OnTriggerEnter (Collider other)
	{
		if (other.tag != "Player") {
			return;
		}

		int oilSold = inventory.DeductItem (InventoryController.InventoryItemType.Oil, 10);

		if (oilSold == 0) {
			return;
		}

		GetComponent<AudioSource>().Play ();
		moneyTracker.PlayerBalance += (oilSold * moneyTracker.OilPrice);
		moneyTracker.OilPrice -= moneyTracker.OilPriceDecreaseOnSell;
	}
}
