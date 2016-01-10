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

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) return;
        Transform parent = other.transform.parent;
        if (parent != null) Destroy(parent.gameObject);
        Destroy(other.gameObject);
    }
}
