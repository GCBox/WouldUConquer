using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour {

    public Transform target;
    public float followSpeed = 0.2f;

    private Vector3 _velocity;

    private new Transform transform;

	// Use this for initialization
	void Start () {
        transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, followSpeed);
	}
}
