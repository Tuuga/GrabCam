using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLookMove : MonoBehaviour {

	bool mouseLock = true;
	bool launched;
	public float movSpeed;
	public float wasdSpeed;
	public float mouseSens;
	public float upDownRange;
	float verticalRotation;
	float horizontalRotation;
	Vector3 launchDir;
	public GameObject mainCam;
	//public Rigidbody rb;
	public Slider mouseSlider;
	public Text mouseText;

	void Start () {
		Cursor.visible = false;
	}
	
	void Update () {
		mainCam.transform.position = transform.position;
		mouseSens = mouseSlider.value;
		mouseText.text = ("Mouse Sensitivity\n" + mouseSens);

		if (mouseLock) {
			MouseLook();
			WasdMove();
		}


		//Mouse Lock
		if (Input.GetKeyDown(KeyCode.LeftAlt)) {
			MouseLock();
		}
		//Launch Player
		if (Input.GetKeyDown(KeyCode.Mouse0) && mouseLock && !launched) {
			launchDir = mainCam.transform.forward;
			launched = true;
		}
		if (launched) {
			LaunchPlayer();
		}
	}

	void MouseLook () {
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

		horizontalRotation += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
		mainCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
		//transform.rotation = Quaternion.Euler(transform.rotation.x, horizontalRotation, transform.rotation.z);
	}

	void WasdMove () {
		if (Input.GetKey(KeyCode.W)) {
			transform.position += transform.forward * wasdSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S)) {
			transform.position += -transform.forward * wasdSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A)) {
			transform.position += -transform.right * wasdSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D)) {
			transform.position += transform.right * wasdSpeed * Time.deltaTime;
		}
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
		transform.position += launchDir * movSpeed * Time.deltaTime;
	}

	void OnCollisionEnter (Collision c) {
		Debug.Log("hit");
		launched = false;
		transform.rotation = c.transform.rotation;
	}
}
