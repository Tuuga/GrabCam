using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLookMove : MonoBehaviour {

    bool mouseLock;
    bool launched;
    public float maxGrappleDist;
    public float movSpeed;
    public float wasdSpeed;
    public float mouseSens;
    public float upDownRange;
    float verticalRotation;
    float horizontalRotation;
    Vector3 launchDir;
    Vector3 playerToObject;
    Vector3 wsDir;
    Vector3 adDir;
    public GameObject mainCam;
    public Slider mouseSlider;
    public Text mouseText;
    RaycastHit hitPoint;

    float radius = 1.0f;

    Transform attachedTo;
    Vector3 attachDir;
    Vector3 attachPos;
    Vector3 attachScale;
    Quaternion attachRot;

    void Start() {
        MouseLock();
    }

    void Update() {
        if (attachedTo != null) {
            StayAttached();
        }

        mainCam.transform.position = transform.position;
        mouseSens = mouseSlider.value;
        mouseText.text = ("Mouse Sensitivity\n" + mouseSens);
        if (mouseLock) {
            if (attachedTo != null) {
                ObjectDirCheck();
            }
            MouseLook();
            //WasdMove();
        }

        //Mouse Lock
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            MouseLock();
        }
        //Launch Player

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseLock && !launched) {
            StartLaunch();
        }
        if (launched) {
            LaunchMove();
        }
    }

    void MouseLook() {
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

        horizontalRotation += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        mainCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, horizontalRotation, transform.rotation.z);
    }

    void WasdMove() {

        if (Input.GetKey(KeyCode.W)) {
            transform.position += wsDir * wasdSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += -wsDir * wasdSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += -adDir * wasdSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += adDir * wasdSpeed * Time.deltaTime;
        }
    }

    void MouseLock() {
        mouseLock = !mouseLock;
        if (mouseLock) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void LaunchMove() {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, radius, launchDir, out hit, movSpeed * Time.deltaTime)) {
            launched = false;
            transform.position += launchDir * hit.distance;
            Attach(hit.collider.transform);
        } else {
		    transform.position += launchDir * movSpeed * Time.deltaTime;
        }
	}

	/*void OnCollisionEnter (Collision c) {
		launched = false;
		transform.rotation = c.transform.rotation;
        Attach(c.transform);
	}*/

    void Attach(Transform t) {
        attachedTo = t;
        attachPos = t.position;
        attachScale = t.localScale;
        attachRot = t.localRotation;
        Vector3 attachDifference = transform.position - t.position;
        attachDifference = attachDifference / attachDifference.magnitude * (attachDifference.magnitude - radius);
        attachDir = Quaternion.Inverse(attachRot)*new Vector3(attachDifference.x/attachScale.x, attachDifference.y / attachScale.y, attachDifference.z / attachScale.z);
    }

    void StayAttached() {
        if (attachedTo.rotation != attachRot)
            attachRot = attachedTo.rotation;

        if (attachedTo.localScale != attachScale)
            attachScale = attachedTo.localScale;

        if (attachedTo.position != attachPos)
            attachPos = attachedTo.position;

        Vector3 attachDifference = (attachRot * new Vector3(attachDir.x * attachScale.x, attachDir.y * attachScale.y, attachDir.z * attachScale.z));
        transform.position = attachPos + attachDifference/attachDifference.magnitude*(attachDifference.magnitude + radius);
    }

	void StartLaunch () {
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        
		//LayerMask mask = 1 << 8;
		//Debug.Log("GrappleDir");
		//if (Physics.Raycast(transform.position, mainCam.transform.forward, out hitPoint)) {
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPoint)) {
			Debug.DrawLine(transform.position, hitPoint.point, Color.red, 5f);
			if (Vector3.Distance(hitPoint.point, transform.position) > maxGrappleDist) {
				launchDir = (hitPoint.point - transform.position).normalized;
				launched = true;
                attachedTo = null;
            } else {
				launchDir = Vector3.zero;
				launched = false;
			}
		}
	}

	void ObjectDirCheck () {
		playerToObject = transform.position - attachedTo.transform.position;
		Vector3 objScale = attachedTo.transform.localScale;

		if (playerToObject.z > objScale.z / 2 || playerToObject.z < -objScale.z / 2) {
			Debug.Log("XY controls");
			wsDir = Vector3.up;
			adDir = Vector3.right;
		}

		if (playerToObject.x > objScale.x / 2 || playerToObject.x < -objScale.x / 2) {
			Debug.Log("YZ controls");
			wsDir = Vector3.up;
			adDir = Vector3.forward;
		}

		if (playerToObject.y > objScale.y / 2 || playerToObject.y < -objScale.y / 2) {
			Debug.Log("XZ controls");
			wsDir = Vector3.left;
			adDir = Vector3.forward;
		}
	}
}
