using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    private new Rigidbody rigidbody;

    private Vector3 _velocity;

	// Use this for initialization
	void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();

        Vector3 player_pos = GameManager.Instance.PlayerPosition;
        
        // initial velocity
        Vector3 dir = player_pos - transform.position;
        dir.z = 0;

        //Debug.Log(dir);
        //dir.Normalize();
        //Debug.Log(dir);
        float vel = 0.5f;// GameManager.Instance.Level;
        _velocity = dir;
        Vector3 random_vel = Random.insideUnitCircle;
        _velocity += random_vel;
        _velocity *= vel;
        //rigidbody.velocity = player_pos - transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += _velocity * Time.deltaTime;
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player") return;

        GameManager.Instance.Damage();
        //rigidbody.velocity = -(rigidbody.velocity + col.relativeVelocity);

        Destroy(gameObject);

    }

}
