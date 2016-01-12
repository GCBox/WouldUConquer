using UnityEngine;
using System.Collections;

public class GameManager {


    private GameObject _player;
    private pieceRig _piece_rigs;
    private GameObject _piece_child;


    // game status
    private int _numConquerPlanet;
    private int _numEarnStar;
    private int _score;

    private int _health;

    public enum GameState { Main=0, Play, GameOver, Pause }
    private GameState _state;

    public GameState state
    {
        get { return _state; }
        set { _state = value; }
    }



    // register methods

    public void RegisterPlayer(GameObject player)
    {
        _player = player;
    }

    public GameManager()
    {
        Awake();

        
        //Start();
    }

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _piece_child = GameObject.FindGameObjectWithTag("child_piece");
        _piece_rigs = GameObject.FindGameObjectWithTag("piece").GetComponent<pieceRig>();
        _piece_child.SetActive(false);
    }


    public void GameBegin()
    {
        _player.SetActive(true);
        _piece_rigs.destroy = 1;
        _piece_child.SetActive(true);

        _numConquerPlanet = 0;
        _numEarnStar = 0;
        _score = 0;

        _health = 3;

    }

    public void GameOver()
    {
        _player.SetActive(false);
        _piece_rigs.destroy = 1;
        _piece_child.SetActive(true);
        
    }

    public void Damage()
    {
        --_health;

        if (_health == 0)
        {
            GameOver();
        }
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
