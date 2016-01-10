using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public Material material_pick;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PickPlanet()
    {
        spriteRenderer.material = material_pick;
    }
}
