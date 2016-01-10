using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;

public class CCMoveByPad : MonoBehaviour {

    public float moveSpeed = 1f;
    public float turnSpeed = 360f;
    public float maxSpeed = 3f;

    //public TrailPolygonColliderGenerator polygonColliderGenerator;

    [HideInInspector]
    public new Transform transform;

    private new Rigidbody2D rigidbody2D;

    //private CharacterController _cc;

    private float _move_distance = 0f;

    // Use this for initialization
    void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        
        //_cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //float h = CrossPlatformInputManager.GetAxis("Horizontal");
        //float v = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 mpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mpoint.z = 0;
        //Debug.Log(mpoint);

        Vector3 diff = mpoint - transform.position;
        Vector3 movement = diff * moveSpeed * Time.deltaTime;
        float acc = diff.magnitude;

        Vector2 prev_vel = rigidbody2D.velocity;
        Vector2 forward = transform.up;
        float angle = Vector2.Dot(forward, diff.normalized);
        float rel_angle = Vector2.Dot(forward, prev_vel);
        float prev_vel_mag = prev_vel.magnitude;

        // accelerate velocity
        //if (rel_angle >= 0)
            rigidbody2D.AddForce(forward * angle * acc * Time.deltaTime * 250f);
        rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxSpeed);

        // rotate object and velocity

        transform.up = Vector3.Slerp(forward, diff, 3f * Time.deltaTime);

        //float ang_vel = Mathf.Acos(angle) * Time.deltaTime * 80f;
        //ang_vel *= ang_vel;
        //if (forward.x * diff.y - forward.y * diff.x < 0) ang_vel *= -1f;
        //rigidbody2D.angularVelocity += ang_vel;

        _move_distance += prev_vel.magnitude * Time.deltaTime;


        //Vector2 prev_vel = rigidbody2D.velocity;
        
        //rigidbody2D.AddForce(diff * moveSpeed * Time.deltaTime * 60f);

        ////rigidbody2D.velocity = Vector2.Lerp(prev_vel, rigidbody2D.velocity, 0.5f);

        //rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxSpeed);

        //_move_distance += rigidbody2D.velocity.magnitude * Time.deltaTime;

        //Vector3 temp_pos = transform.position;
        //temp_pos += movement;
        //transform.position = temp_pos;

        //Vector3 movement = new Vector3(h, v, 0f) * moveSpeed * Time.deltaTime;
        //_cc.Move(movement);

        //if (h != 0f || v != 0f)
        //    transform.up = _cc.velocity.normalized;

        //transform.Rotate(-transform.forward * h * turnSpeed * Time.deltaTime);

        //_move_distance += movement.magnitude;
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
