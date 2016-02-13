using UnityEngine;
using System.Collections;

public class Lazer : MonoBehaviour {

	public GameObject lazerEnd;
	public GameObject lazerObject;
	public GameObject visibleLazer;

	Vector3 lazerStartPoint;
	Vector3 lazerEndPoint;
	Vector3 moveDir;
	public Transform lazerMoveEndTransform;

	BoxCollider lazerCol;

	public float lazerSpeed;

	void Awake () {
		lazerStartPoint = transform.position;
		lazerEndPoint = lazerMoveEndTransform.position;
		lazerCol = lazerObject.GetComponent<BoxCollider>();
		Vector3 lazerSize = lazerCol.size;
		float lazerZ = Vector3.Distance(transform.position, lazerEnd.transform.position);

		lazerCol.size = new Vector3(lazerSize.x, lazerSize.y, lazerZ);
		lazerCol.center = new Vector3(0, 0, lazerZ / 2);
		visibleLazer.transform.localScale = new Vector3(lazerSize.x, lazerSize.y, lazerZ);
		visibleLazer.transform.localPosition = new Vector3(0, 0, lazerZ / 2);
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
}