using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLookMove : MonoBehaviour {

    bool mouseLock;
    bool launched;
    public float minGrappleDist;
    public float movSpeed;
    public float mouseSens;
    public float upDownRange;
	public float maxGrappleCount;
	float grappleCount;
    float verticalRotation;
    float horizontalRotation;
    Vector3 launchDir;
    Vector3 playerToObject;
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
            MouseLook();
        }

        //Mouse Lock
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            MouseLock();
        }

        //Launch Player
        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseLock && grappleCount < maxGrappleCount) {
			grappleCount++;
            StartLaunch();
        }

        if (launched) {
            LaunchMove();
        } else {
			grappleCount = 0;
		}
    }

    void MouseLook() {
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

        horizontalRotation += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        mainCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, horizontalRotation, transform.rotation.z);
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

	void OnCollisionEnter (Collision c) {
		launched = false;
	}

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
			if (Vector3.Distance(hitPoint.point, transform.position) > minGrappleDist) {
				launchDir = (hitPoint.point - transform.position).normalized;
				launched = true;
                attachedTo = null;
            } else {
				launchDir = Vector3.zero;
				launched = false;
			}
		}
	}
}
