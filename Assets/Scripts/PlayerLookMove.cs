using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLookMove : MonoBehaviour {

    
    bool launched;
    public float minGrappleDist;
    public float movSpeed;
    public float mouseSens;
    public float upDownRange;
	public float maxGrappleCount;
	float grappleCount;

    //Camera Vars
    float verticalRotation;
    float horizontalRotation;
    bool mouseLock;
    Quaternion baseRotation = Quaternion.identity;

    Vector3 launchDir;
    Vector3 playerToObject;
    public Camera mainCam;
    public Slider mouseSlider;
    public Text mouseText;
    RaycastHit hitPoint;
    public float minShrink;
    public float shrinkSpeed;

    float radius;
    float shrink = 1;
    
    //Attach Vars
    Transform attachedTo;
    Vector3 attachDir;
    Vector3 attachPos;
    Vector3 attachScale;
    Vector3 attachNormal;
    Quaternion attachRot;

    void Start() {
        MouseLock();
        radius = transform.localScale.x;
    }

    void Update() {
        //Launch Player
        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseLock && grappleCount < maxGrappleCount) {
            grappleCount++;
            StartLaunch();
        }
    }

    void LateUpdate() {
        if (attachedTo != null) {
            StayAttached();
        }

        //Mouse Lock
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            MouseLock();
        }

        if (launched) {
            LaunchMove();
        } else if (shrink < 1) {
            SetShrink(shrink + Time.deltaTime*shrinkSpeed);
        }

        if (mouseLock) {
            MouseLook();
        }

        mainCam.transform.position = transform.position;
        mouseSens = mouseSlider.value;
        mouseText.text = ("Mouse Sensitivity\n" + mouseSens);
    }

    void MouseLook() {
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

        horizontalRotation += Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;

        mainCam.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0) * baseRotation;
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

        if (Physics.SphereCast(transform.position, transform.localScale.x, launchDir, out hit, movSpeed * Time.deltaTime, Physics.DefaultRaycastLayers,QueryTriggerInteraction.Ignore)) {
            launched = false;
            grappleCount = 0;
            transform.position += launchDir * hit.distance;
            Attach(hit.collider.transform, hit);
        } else {
		    transform.position += launchDir * movSpeed * Time.deltaTime;
        }
	}

	/*void OnCollisionEnter (Collision c) {
		launched = false;
	}*/

    void Attach(Transform t, RaycastHit hit) {
        attachedTo = t;
        attachPos = t.position;
        attachScale = t.lossyScale;
        attachRot = t.rotation;
        Vector3 attachDifference = hit.point - t.position;
        attachDifference = Quaternion.Inverse(attachRot)*attachDifference;
        attachNormal = Quaternion.Inverse(attachRot) * hit.normal;
        attachDir = new Vector3(attachDifference.x / attachScale.x, attachDifference.y / attachScale.y, attachDifference.z / attachScale.z);
    }

    void StayAttached() {
        if (attachedTo.rotation != attachRot) {
            attachRot = attachedTo.rotation;
        }

        if (attachedTo.lossyScale != attachScale)
            attachScale = attachedTo.lossyScale;

        if (attachedTo.position != attachPos)
            attachPos = attachedTo.position;

        Vector3 attachDifference = attachRot * new Vector3(attachDir.x * attachScale.x, attachDir.y * attachScale.y, attachDir.z * attachScale.z);
        transform.position = attachPos + attachDifference + attachRot*attachNormal*radius*shrink;
    }

	void StartLaunch () {
        RaycastHit hit;
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.SphereCast(transform.position, radius*shrink, camRay.direction, out hit, movSpeed * Time.deltaTime, Physics.DefaultRaycastLayers,QueryTriggerInteraction.Ignore)) {
            SetShrink(minShrink);
            launchDir = camRay.direction;
            launched = true;
            attachedTo = null;
        } 

        //LayerMask mask = 1 << 8;
        //Debug.Log("GrappleDir");
        //if (Physics.Raycast(transform.position, mainCam.transform.forward, out hitPoint)) {
        /*if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPoint)) {
			Debug.DrawLine(transform.position, hitPoint.point, Color.red, 5f);
			if (Vector3.Distance(hitPoint.point, transform.position) > minGrappleDist) {
				launchDir = (hitPoint.point - transform.position).normalized;
				launched = true;
                attachedTo = null;
            } else {
				launchDir = Vector3.zero;
				launched = false;
			}
		}*/
	}

    void SetShrink(float shr) {
        shr = Mathf.Min(1, shr);
        Collider[] coll = Physics.OverlapSphere(transform.position + attachRot * attachNormal * Mathf.Max(0,shr - shrink), radius * shr, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        bool hit = false;

        foreach (Collider c in coll) {
            if (c.tag != "Player") {
                hit = true;
                print(c.name);
            }
        }

        if (!hit) {
            shrink = shr;
            transform.localScale = Vector3.one * radius * shrink;
        }
    }
}
