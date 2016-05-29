using UnityEngine;
using System.Collections;

public class Speedometer : MonoBehaviour
{

	public GUIText speedometer;

	void Start ()
	{
		speedometer.text = "Speed: ";
	}
	
	void FixedUpdate ()
	{
		speedometer.text = "Speed: " + (GetComponent<Rigidbody>().velocity.magnitude * 10).ToString ("0");
	}
}
