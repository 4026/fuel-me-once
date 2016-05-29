using UnityEngine;
using System.Collections;

public class RisingTemperature : MonoBehaviour
{

	public float StartTemperature;
	public float GameOverTemperature;
	public float GameLength;

	public GUIText TemperatureIndicator;
	public GameObject GameOverText;
	public GUIText GameOverScore;
	public MoneyTracker money;

	public GameObject Sun;
	public Color FinalSunColor;
	public Vector3 FinalSunAngle;

	private float temperature;
	public float CurrentTemperature {
		get { return temperature; }
		set {
			temperature = value;
			UpdateIndicator ();
		}
	}

	private float startTime;

	void Start ()
	{
		startTime = Time.time;
		CurrentTemperature = StartTemperature;
		StartCoroutine (IncreaseTemperature ());

		iTween.ColorTo (Sun, FinalSunColor, GameLength);
		iTween.RotateTo (Sun, FinalSunAngle, GameLength);
	}

	IEnumerator IncreaseTemperature ()
	{
		while (Time.time < startTime + GameLength) {
			CurrentTemperature = Mathf.Lerp (StartTemperature, GameOverTemperature, (Time.time - startTime) / GameLength);
			yield return new WaitForSeconds (0.5f);
		}

		GameOver ();
	}
	
	void UpdateIndicator ()
	{
		TemperatureIndicator.text = "Global Temperature Anomaly: + " + CurrentTemperature.ToString ("0.00") + " °C";
	}

	void GameOver ()
	{
		CurrentTemperature = GameOverTemperature;
		
		GameObject.FindGameObjectWithTag ("Player").SetActive (false);
		
		GameOverScore.text = "$ " + money.PlayerBalance.ToString ("#,##0");
		GameOverText.SetActive (true);
	}

	void Update ()
	{
		if (Input.GetButton ("Restart")) {
			Application.LoadLevel (Application.loadedLevel);
		}

		if (Input.GetButton ("Quit")) {
			Application.Quit ();
		}
	}
}
