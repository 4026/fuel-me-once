using UnityEngine;
using System.Collections;

public class OilIndicator : MonoBehaviour
{

	private TextMesh textComponent;
	public string Text {
		get {
			return gameObject.GetComponentInChildren<TextMesh> ().text;
		}
		set {
			gameObject.GetComponentInChildren<TextMesh> ().text = value;
		}
	}

}
