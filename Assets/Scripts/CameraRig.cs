using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour {

    public Transform target;
    public float followSpeed = 0.2f;
    public float returnSpeed = 0.5f;

    private bool activate = false;
    public bool Active
    {
        get { return activate; }
        set { activate = value; }
    }

    private Vector3 _velocity;

    [HideInInspector]
    private new Transform transform;


    private Vector3 moving_target_pos;
    private bool moving_animation = false;
    public bool MovingAnim
    {
        get { return moving_animation; }
    }

    public void MovingAnimation(float beginTime)
    {
        Invoke("TurnOnMovingAnim", beginTime);
    }

    void TurnOnMovingAnim()
    {
        moving_animation = true;
        moving_target_pos = transform.position - GameManager.Instance.PlayerVelocity * 4800f;
        _velocity = Vector3.zero;
        Debug.Log(returnSpeed);
        Invoke("TurnOffMovingAnim", returnSpeed);
    }

    void TurnOffMovingAnim()
    {
        GameManager.Instance.MovingAnim = false;
        moving_animation = false;
        activate = true;
        _velocity = Vector3.zero;
        GameManager.Instance.PlayerPosition = transform.position;
        //GameManager.Instance.PlayerPosition = transform.position;
    }

    // Use this for initialization
    void Start () {
        transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (moving_animation)
        {
            transform.position = Vector3.SmoothDamp(transform.position, moving_target_pos, ref _velocity, returnSpeed);
            GameManager.Instance.PlayerPosition = transform.position;
        }
        else if (activate)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, followSpeed);
        }
	}
}
