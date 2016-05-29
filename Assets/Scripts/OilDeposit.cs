using UnityEngine;
using System.Collections;

public class OilDeposit : MonoBehaviour
{
	private int oil;
	public int Oil {
		get { return oil;} 
		set {
			oil = value;
			if (oil <= 0) {
				Destroy (gameObject);
			}
		}
	}

}
