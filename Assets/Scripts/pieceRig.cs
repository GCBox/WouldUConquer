using UnityEngine;
using System.Collections;

public class pieceRig : MonoBehaviour {
    public Transform target;
    public float followSpeed = 0.2f;

    private Vector3 _velocity;

    private new Transform transform;
    public int destroy;
    // Use this for initialization
    void Start()
    {
        transform = GetComponent<Transform>();
        destroy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, followSpeed);
    }
}
