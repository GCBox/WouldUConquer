using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public Sprite sprite_conquer;
    public int score = 100;

    private SpriteRenderer spriteRenderer;

    private bool conquer = false;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool PickPlanet()
    {
        if (conquer) return false;
        
        spriteRenderer.sprite = sprite_conquer;
        conquer = true;

        GameManager.Instance.ConquerPlanet(score);
        

        return true;
    }
}
