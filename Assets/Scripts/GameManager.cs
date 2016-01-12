using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


    private GameObject _player;

    public enum GameState { Main=0, Play, GameOver, Pause }
    private GameState _state;

    public GameState state
    {
        get { return _state; }
        set { _state = value; }
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}


    void SetState(GameState state)
    {
        _state = state;
    }



    // singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
}
