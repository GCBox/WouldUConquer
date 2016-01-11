using UnityEngine;
using System.Collections;

public class Planet_Gravity : MonoBehaviour {


    protected CircleCollider2D Gravity;
    public new Transform transform;
    protected LookAtMouseMove _player;
    protected GameObject gb;
   
    float gravityPower = 0.1f;
    public float gravity;
    bool touch;
    void Awake()
    {
        transform = GetComponent<Transform>();
        Gravity = GetComponent<CircleCollider2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<LookAtMouseMove>();
       
    }
    
	
	// Update is called once per frame
	void Update () {
        //touch = Gravity.IsTouching(_player.GetComponentInChildren<CircleCollider2D>());
        
      //  Debug.Log(touch);
	}
    void OnTriggerStay(Collider other)
    {
        Vector3 framePos = Vector3.MoveTowards(_player.transform.position,transform.position, gravityPower * Time.deltaTime);
        Vector3 moveDir = transform.position-framePos ;
        
        _player.gravity = moveDir * (gravity / (10*Vector3.Distance(framePos, transform.position)* Vector3.Distance(framePos, transform.position)));
            /*
        moveSpeed = Vector3.Distance(framePos, worldpos) * Dis_Per_Speed + Base_Speed;

        framePos = Vector3.MoveTowards(transform.position, transform.position + transform.right, moveSpeed * Time.deltaTime);
        moveDir = framePos - transform.position;
        */
        //Debug.Log("111");
    }
    void OnTriggerExit(Collider other)
    {
        Vector3 empty=Vector3.zero;
        _player.gravity = empty;
    }
}
