using UnityEngine;
using System.Collections;

public class PlayerLookMove : MonoBehaviour {

	bool mouseLock = true;
	bool moveing;
	public float movSpeed;
	public float mouseSens;
	public float upDownRange;
	float verticalRotation;
	float horizontalRotation;
	public GameObject mainCam;
	public Rigidbody rb;

	void Start () {
		Cursor.visible = false;
	}
	
	void Update () {
		if (mouseLock)
		MouseLook();

		//Mouse Lock
		if (Input.GetKeyDown(KeyCode.L)) {
			MouseLock();
		}
		//Launch Player
		if (Input.GetKeyDown(KeyCode.Mouse0) && mouseLock && !moveing) {
			LaunchPlayer();
			moveing = true;
		}
	}

	void MouseLook () {
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

		horizontalRotation += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;

		//mainCam.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime, 0));
		mainCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
	}

	void MouseLock () {
		mouseLock = !mouseLock;
		if (mouseLock) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	void LaunchPlayer () {
		rb.velocity = mainCam.transform.forward * movSpeed;
	}

	void OnCollisionEnter (Collision c) {
		moveing = false;
		rb.velocity = Vector3.zero;
	}
}
