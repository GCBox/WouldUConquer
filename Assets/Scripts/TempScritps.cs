using UnityEngine;
using System.Collections;

public class TempScritps : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetButton("Fire1"))
        {
            GameManager.Instance.GameBegin();
        }

        if (Input.GetButtonUp("Jump"))
        {
            GameManager.Instance.GamePause();
        }
	}
}
