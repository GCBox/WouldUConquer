using UnityEngine;
using System.Collections;

public class GameManager {


    private GameObject _player;
    private CharacterController _player_cc;
    private LookAtMouseMove _player_mover;
    private SpriteRenderer _player_render;
    private TimedTrailRenderer _player_trail_render;
    private pieceRig _piece_rig;
    private CameraRig _camera_rig;

    private Spawner[] _spawners;

    // game status
    private int _numConquerPlanet = 0;
    private int _numEarnStar = 0;
    private int _score = 0;

    private int _health = 300;

    private int _level = 1;

    private bool _moving_anim = false;
    public bool MovingAnim
    {
        get { return _moving_anim; }
        set { _moving_anim = value; }
    }
    
    public void ConquerPlanet(int score)
    {
        _score += score;
        ++_numConquerPlanet;

        ++_numEarnStar;

        Debug.Log("Num of Planet : " + _numConquerPlanet);
        Debug.Log("Score " + _score);
    }

    public int Level
    {
        get { return _level; }
    }





    public enum GameState { Main=0, Tutorial, Play, GameOver, Pause }
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
        _piece_rig = GameObject.FindGameObjectWithTag("piece").GetComponent<pieceRig>();
        _camera_rig = GameObject.FindObjectOfType<CameraRig>();

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
        if (_camera_rig.MovingAnim) return;
        if (state == GameState.Play) return;
        if (MovingAnim) return;

        SetState(GameState.Play);
        Time.timeScale = 1f;

        PlayerPosition = _camera_rig.transform.position;
        

        SetActivateSpawner(true);
        _player_cc.enabled = true;
        _player_render.enabled = true;
        _player_trail_render.enabled = true;
        _player_mover.gravity = Vector3.zero;
        _piece_rig.Reset();

        _piece_rig.Active = true;
        _camera_rig.Active = true;

        _numConquerPlanet = 0;
        _numEarnStar = 0;
        _score = 0;

        _health = 3;
    }

    public void GameOver()
    {
        SetState(GameState.GameOver);

        _player_cc.enabled = false;
        _player_render.enabled = false;
        _piece_rig.CreateExplosion();

        _player_trail_render.FadeOut();
        _player_trail_render.enabled = false;

        _piece_rig.Active = false;
        _camera_rig.Active = false;

        SetActivateSpawner(false);

        MovingAnimation();

        //DeleteAllSpawnedObjects();
    }

    public void GamePause()
    {
        if (state == GameState.Play)
        {
            Debug.Log("Pause");
            Time.timeScale = 0f;
            SetState(GameState.Pause);
        }
        else if (state == GameState.Pause)
        {
            Debug.Log("Resume");
            Time.timeScale = 1f;
            SetState(GameState.Play);
        }
    }

    void MovingAnimation()
    {
        MovingAnim = true;
        _camera_rig.MovingAnimation(2f);
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




    /////// random gift ///////
    public void RocketShield()
    {

    }

    public void StarExplosion()
    {

    }

    public void AsteroidExplosion()
    {

    }

    public void SlowDebuff()
    {

    }



    // delete planet, asteroid, stars, etc.. (spawned objects)
    void DeleteAllSpawnedObjects()
    {

        // planet
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach (GameObject obj in planets) GameObject.Destroy(obj);

        // asteroid
        Asteroid[] asteroids = GameObject.FindObjectsOfType<Asteroid>();
        foreach (Asteroid asteroid in asteroids) GameObject.Destroy(asteroid.gameObject);

        // star

    }


    void SetState(GameState state)
    {
        _state = state;
    }


    // player transform status
    public Vector3 PlayerPosition
    {
        get { return _player.transform.position; }
        set
        {
            _player.transform.position = value;
            _camera_rig.transform.position = value;
            //_piece_rig.transform.position = value;
        }
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
