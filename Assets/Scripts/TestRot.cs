using UnityEngine;
using System.Collections;

public class TestRot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
        transform.position += Vector3.forward * 4 * Time.deltaTime;
        transform.localScale += Vector3.one * Time.deltaTime;
	}
}
