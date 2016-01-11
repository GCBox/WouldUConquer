using UnityEngine;
using System.Collections;

public class Explosion_parent : MonoBehaviour
{
    protected pieceRig _piece;
    protected new Transform transform;
    protected Transform _pieceRig;
    int rot;
    public float explosion_speed;
    // Use this for initialization
    void Awake()
    {
        transform = GetComponent<Transform>();
        _piece = GameObject.FindGameObjectWithTag("piece").GetComponent<pieceRig>();
        _pieceRig = GameObject.FindGameObjectWithTag("piece").transform;
        rot = 1;
        explosion_speed += Random.Range(0f, 23f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_piece.destroy == 1)
        {
            if (rot == 1)
            {
                transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
                rot = 0;
            }
            
            transform.Translate(transform.up * Time.deltaTime * explosion_speed/(Vector3.Distance(transform.position, _pieceRig.position)*9f+1));
            Debug.Log(Vector3.Distance(transform.position, _pieceRig.position));

        }
    }
}
