using UnityEngine;
using System.Collections;

public class MoneyTracker : MonoBehaviour
{

	public GUIText BalanceIndicator;
	public GUIText OilPriceIndicator;

	public int OilStartPrice;
	public int OilMaxPrice;
	public int OilMinPrice;
	public int OilPriceVolatility;
	public int OilPriceIncrease;
	public int OilPriceDecreaseOnSell;
	public int StartingBalance;

	private int balance = 0;
	public int PlayerBalance {
		get { return balance; }
		set {
			balance = value;
			UpdateBalanceIndicator ();
		}
	}

	private int oilPrice;
	public int OilPrice {
		get { return oilPrice; }
		set {
			oilPrice = value;
			UpdatePriceIndicator ();
		}
	}

	void Start ()
	{
		balance = StartingBalance;
		UpdateBalanceIndicator (false);

		OilPrice = OilStartPrice;

		StartCoroutine (UpdateOilPrice ());
	}

	void UpdateBalanceIndicator (bool shake = true)
	{
		BalanceIndicator.text = "$ " + PlayerBalance.ToString ("#,##0");

		if (shake) {
			Hashtable shake_options = new Hashtable ();
			shake_options.Add ("x", 0.01f);
			shake_options.Add ("y", 0.01f);
			shake_options.Add ("time", 1.0f);
			iTween.ShakePosition (BalanceIndicator.gameObject, shake_options);
		}

	}

	void UpdatePriceIndicator ()
	{
		OilPriceIndicator.text = "$ " + OilPrice.ToString () + " / bbl";
	}

	IEnumerator UpdateOilPrice ()
	{
		while (true) {
			OilPrice += OilPriceIncrease + Random.Range (-OilPriceVolatility, OilPriceVolatility);
			OilPrice = Mathf.Clamp (OilPrice, OilMinPrice, OilMaxPrice);
			yield return new WaitForSeconds (5.0f);
		}
	}

}
