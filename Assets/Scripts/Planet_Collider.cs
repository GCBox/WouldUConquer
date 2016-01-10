using UnityEngine;
using System.Collections;

public class Planet_Collider : MonoBehaviour {

    // Update is called once per frame
    //GameO;
    public GameObject pieces;
    protected pieceRig _piece;
    void Awake()
    {
        // pieces = GameObject.FindGameObjectWithTag("piece");
        _piece = GameObject.FindGameObjectWithTag("piece").GetComponent<pieceRig>();
    }
    void Update () {
	
	}
    
    void OnTriggerEnter(Collider other)
    {
        GameObject.FindGameObjectWithTag("Player").SetActive(false);

        //GameObject.FindGameObjectWithTag("picture").SetActive(false);
        // GameObject.FindGameObjectsWithTag("piece");
        //Destroy(other.gameObject);

        _piece.destroy = 1;
        
        pieces.SetActive(true);
        Debug.Log("Collided");
    }
    

}
