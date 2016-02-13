using UnityEngine;
using System.Collections;

public class Lazer : MonoBehaviour {
	
	GameObject lazerObject;
	GameObject lazerModelAndCol;
	GameObject lazerEnd;
	GameObject lazerMoveEnd;

	Vector3 lazerStartPoint;
	Vector3 lazerEndPoint;
	Vector3 moveDir;

	public float lazerSpeed;

	void Awake () {

		AssignLazerObjects();

		lazerStartPoint = transform.position;
		lazerEndPoint = lazerMoveEnd.transform.position;

		Vector3 lazerSize = lazerModelAndCol.transform.localScale;
		float lazerLenght = Vector3.Distance(lazerStartPoint, lazerEnd.transform.position);

		lazerModelAndCol.transform.localScale = new Vector3 (lazerSize.x, lazerSize.y, lazerLenght);
		lazerModelAndCol.transform.localPosition = new Vector3 (0, 0, lazerLenght / 2);
		lazerObject.transform.LookAt(lazerEnd.transform);
	}
	
	void Update () {
		MoveLazerTo();
	}

	void MoveLazerTo () {

		if (Vector3.Distance (lazerObject.transform.position, lazerEndPoint) < 0.1f) {
			moveDir = (lazerStartPoint - lazerObject.transform.position).normalized;
		} else if (Vector3.Distance(lazerObject.transform.position, lazerStartPoint) < 0.1f) {
			moveDir = (lazerEndPoint - lazerObject.transform.position).normalized;
		}

		lazerObject.transform.position += moveDir * lazerSpeed * Time.deltaTime;
	}

	void AssignLazerObjects () {

		lazerObject = transform.FindChild("LazerObject").gameObject;
		lazerModelAndCol = lazerObject.transform.FindChild("LazerModelAndCol").gameObject;
		lazerEnd = transform.FindChild("LazerEnd").gameObject;
		lazerMoveEnd = transform.FindChild("LazerMoveEnd").gameObject;
	}

	void OnTriggerEnter (Collider c) {
		if (c.tag == "Player") {
			Debug.Log ("Player passed through the lazer!");
		}
	}
}