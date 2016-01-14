using UnityEngine;
using System.Collections;

public class pieceRig : MonoBehaviour {
    public Transform target;
    public float followSpeed = 0.2f;

    private bool activate = false;
    public bool Active
    {
        get { return activate; }
        set { activate = value; }
    }

    private Vector3 _velocity;

    [HideInInspector]
    private new Transform transform;

    private Explosion[] pieces;

    void Awake()
    {
        transform = GetComponent<Transform>();
        pieces = GetComponentsInChildren<Explosion>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            //transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, followSpeed);
            transform.position = target.position;
        }
    }

    public void CreateExplosion()
    {
        foreach (Explosion exp in pieces)
        {
            exp.CreateExplosion();
        }
    }

    public void Reset()
    {
        foreach (Explosion exp in pieces)
        {
            exp.Reset();
        }
    }
}
