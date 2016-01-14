using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    [HideInInspector]
    public new Transform transform;
    private SpriteRenderer _sprite_render;

    Vector3 velocity;
    float angle;
    float angleSpeed;

    bool destroy = false;
  
    // Use this for initialization
    void Awake () {
        transform = GetComponent<Transform>();
        _sprite_render = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Reset();
    }

    public void CreateExplosion()
    {
        destroy = true;
        _sprite_render.enabled = true;

        velocity = Random.insideUnitSphere * 4f;
        velocity += (GameManager.Instance.PlayerVelocity * Random.Range(1.2f, 6f));
        velocity.z = 0;

        angleSpeed = Random.Range(-15f, 15f);
        angle = 0;
    }

    public void Reset()
    {
        destroy = false;
        _sprite_render.enabled = false;

        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
	
	// Update is called once per frame
	void Update () {
	    if (destroy)
        {
            angle += angleSpeed;
            if (angle > 360f) angle -= 360f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Vector3 pos = transform.position;
            pos += velocity * Time.deltaTime;
            transform.position = pos;

            // damping
            velocity *= 0.995f;
            angle *= 0.995f;
        }
	}
}
