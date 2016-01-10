using UnityEngine;
using System.Collections;

public class LookAtMouseMove : MonoBehaviour {

    public new Transform transform;
   
    private CharacterController _cc;
    float moveSpeed=2f;
    public float Dis_Per_Speed;
    public float Base_Speed;
    public float Rotation_Speed;
    public float acceleration;
    public Vector3 gravity;
  

    

    void Awake()
    {
        gravity = Vector3.zero;
        transform = GetComponent<Transform>();
        _cc = GetComponent<CharacterController>();
       
        
    }
	void Update () {

       

        Vector3 worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = worldpos - transform.position;
        dir = Vector3.Slerp(transform.right,dir,Time.deltaTime* Rotation_Speed);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
       
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        worldpos.z = 0;

        Vector3 framePos = Vector3.MoveTowards(transform.position, worldpos, moveSpeed * Time.deltaTime);
        Vector3 moveDir = framePos - transform.position;


        moveSpeed = Mathf.Pow(Vector3.Distance(framePos, worldpos) * Dis_Per_Speed, acceleration) + Base_Speed;
    
        framePos = Vector3.MoveTowards(transform.position, transform.position + transform.right+ gravity, moveSpeed * Time.deltaTime);
       // Debug.Log(gravity);
        moveDir = framePos - transform.position;

        if ( Vector3.Distance(framePos, worldpos) > 0.1f) _cc.Move(moveDir);
       

        //transform.Translate((worldpos - transform.position).normalized * Time.deltaTime * 2.0f);
    }
}
