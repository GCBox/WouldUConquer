using UnityEngine;
using System.Collections;

public class CCMove : MonoBehaviour {

    public float moveSpeed = 3f;
    public float turnSpeed = 360f;

    //public TrailPolygonColliderGenerator polygonColliderGenerator;

    [HideInInspector]
    public new Transform transform;

    //private CharacterController _cc;

    private float _move_distance = 0f;

    // Use this for initialization
    void Awake ()
    {
        transform = GetComponent<Transform>();
        //_cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 movement = transform.up * v * moveSpeed * Time.deltaTime;
        //Vector2 movement = new Vector2(h, v) * moveSpeed * Time.deltaTime;
        Vector2 pos = transform.position;
        pos += movement;
        transform.position = pos;
        //_cc.Move(movement);

        transform.Rotate(-transform.forward * h * turnSpeed * Time.deltaTime);

        _move_distance += moveSpeed * Time.deltaTime * v;
    }

    void LateUpdate()
    {
        //if (_move_distance > sampleDistance)
        //{
        //    polygonColliderGenerator.AddSegmentsPoints(transform.position);
        //    _move_distance = 0f;
        //}
    }
}
