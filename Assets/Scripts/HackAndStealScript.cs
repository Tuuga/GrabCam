using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HackAndStealScript : MonoBehaviour 
{

	public bool Hack;

	public float NeededToHack;
	public float HackTime;
	public GameObject PressText;
	public GameObject HackBarObject;
	public Slider HackBar;

	public GameObject HackedObject;

	void Start ()
	{
		HackBar.maxValue = NeededToHack;
		PressText.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKey(KeyCode.E) && Hack == true) 
		{
			HackBarObject.SetActive (true);
			HackTime += Time.deltaTime;
			HackBar.value = HackTime;
			if (HackTime >= NeededToHack) {
				PressText.SetActive (false);
				Hack = false;
				HackBarObject.SetActive (false);
				Instantiate (HackedObject, gameObject.transform.position, gameObject.transform.rotation);
				Destroy (gameObject);
			}
		} 
		if (Input.GetKeyUp (KeyCode.E)) 
		{
			HackTime = 0;
			HackBarObject.SetActive (false);
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Player") {
			Hack = true;
			PressText.SetActive (true);
		}
	}

	void OnTriggerExit (Collider other)
	{
		Hack = false;
		PressText.SetActive (false);
	}
}
