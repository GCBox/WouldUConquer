using UnityEngine;
using System.Collections;

public class TempScritps : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetButton("Jump"))
        {
            GameManager.Instance.GameBegin();
        }
	}
}
