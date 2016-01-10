using UnityEngine;
using System.Collections;

public class Planet_Collider : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        //Destroy(other.gameObject);
        Debug.Log("Collided");
    }
    

}
