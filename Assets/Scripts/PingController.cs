using UnityEngine;
using System.Collections;

public class PingController : MonoBehaviour
{

	public float expand_scale;
	public float expand_time;
	public GameObject hit_indicator;

	private float start_time;

	// Use this for initialization
	void Start ()
	{
		start_time = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float elapsed = Time.time - start_time;
		if (elapsed > expand_time) {
			Destroy (gameObject);
			return;
		}

		float current_time = elapsed / expand_time;
		float scale_lerp = Mathf.Lerp (1, expand_scale, current_time);
		transform.localScale = new Vector3 (scale_lerp, 1.0f, scale_lerp);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag != "Deposit") {
			return;
		}

		GameObject hit = Instantiate (hit_indicator, transform.position, Quaternion.identity) as GameObject;
		hit.transform.localScale = transform.localScale;
	}
}
