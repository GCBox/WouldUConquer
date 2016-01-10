using UnityEngine;
using System.Collections;

public class Remover : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Remover Enter! - " + other);

    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Remover detect! - " + other);
    }
}
