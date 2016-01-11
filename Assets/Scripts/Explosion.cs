using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    protected pieceRig _piece;
    protected new Transform transform;
    float angle;
  
    // Use this for initialization
    void Awake () {
        transform = GetComponent<Transform>();
        _piece = GameObject.FindGameObjectWithTag("piece").GetComponent<pieceRig>();
        angle = 0;
    }
	
	// Update is called once per frame
	void Update () {
	    if(_piece.destroy==1)
        {
            angle+=20f;
            if (angle == 361) angle = 0;
           transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
           

        }
	}
}
