using UnityEngine;
using System.Collections;

public class GameManager {


    private GameObject _player;
    private CharacterController _player_cc;
    private LookAtMouseMove _player_mover;
    private SpriteRenderer _player_render;
    private TimedTrailRenderer _player_trail_render;
    private pieceRig _piece_rigs;

    private Spawner[] _spawners;

    // game status
    private int _numConquerPlanet = 0;
    private int _numEarnStar = 0;
    private int _score = 0;

    private int _health = 300;

    private int _level = 1;

    public int Level
    {
        get { return _level; }
    }

    public enum GameState { Main=0, Play, GameOver, Pause }
    private GameState _state = GameState.Main;

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

        //GameBegin();
    }

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _player_cc = _player.GetComponent<CharacterController>();
        _player_mover = _player.GetComponent<LookAtMouseMove>();
        _player_render = _player.GetComponentInChildren<SpriteRenderer>();
        _player_trail_render = _player.GetComponentInChildren<TimedTrailRenderer>();
        _piece_rigs = GameObject.FindGameObjectWithTag("piece").GetComponent<pieceRig>();

        _spawners = GameObject.FindObjectsOfType<Spawner>();
        SetActivateSpawner(false);

        _player_render.enabled = false;
    }

    void SetActivateSpawner(bool enable = true)
    {
        foreach (Spawner spawner in _spawners)
        {
            spawner.SetActivate(enable);
        }
    }


    public void GameBegin()
    {
        SetState(GameState.Play);

        SetActivateSpawner(true);
        _player_cc.enabled = true;
        _player_render.enabled = true;
        _piece_rigs.Reset();

        _numConquerPlanet = 0;
        _numEarnStar = 0;
        _score = 0;

        _health = 300;
    }

    public void GameOver()
    {
        SetState(GameState.GameOver);

        _player_cc.enabled = false;
        _player_render.enabled = false;
        _piece_rigs.CreateExplosion();

        _player_trail_render.FadeOut();

        SetActivateSpawner(false);

        _player_mover.MovingAnimation();
    }

    public void Damage()
    {
        --_health;

        Debug.Log("Damage!" + _health);

        if (_health <= 0)
        {
            GameOver();
        }
    }

    // delete planet, asteroid, stars, etc.. (spawned objects)
    void DeleteAllSpawnedObjects()
    {
        
    }


    void SetState(GameState state)
    {
        _state = state;
    }


    // player transform status
    public Vector3 PlayerPosition
    {
        get { return _player.transform.position; }
    }

    public Vector3 PlayerVelocity
    {
        get { return _player_mover.currentVelocity; }
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
