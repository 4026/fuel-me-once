using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour
{

	public GameObject cam;

	void Start ()
	{
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	void Update ()
	{
		transform.rotation = Quaternion.LookRotation (cam.transform.forward, cam.transform.up);
	}
}
