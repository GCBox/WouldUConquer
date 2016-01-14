using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public Sprite sprite_conquer;
    public int score = 100;
    public bool giftPlanet = false;

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

        if (giftPlanet)
        {
            RandomEffect();
        }
        
        GameManager.Instance.ConquerPlanet(score);
        

        return true;
    }

    public void RandomEffect()
    {
        int index = Random.Range(0, 4);

        switch (index)
        {
            case 0:
                // rocket shield
                break;

            case 1:
                // star explosion

                break;

            case 2:
                // steroid explosion
                break;

            case 3:
                // slow debuff
                break;

            default:
                break;
        }
    }
}
