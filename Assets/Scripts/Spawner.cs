using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject[] prefabs;

    // 얼마나 자주 GameObject가 spawn될 것인가?
    // 초당 생성 횟수
    // 숫자가 클수록 자주 생성
    public float spawnRate = 0.5f;
    private float spawnInterval;
    public float spawnRange_min = 10f;
    public float spawnRange_max = 15f;

    // 난이도. 시간이 지날 수록 상승함.
    private int level;

    // 생성하려는 주변에 물체가 반경 이 수치 이내에 없을 경우만 spawn한다
    public float spawnColliderSize = 5f;

    private float _elapsedTime;

    void Awake()
    {
        spawnInterval = 1f / spawnRate;
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        _elapsedTime += Time.deltaTime;

        if (spawnRate != 0f && _elapsedTime > spawnInterval)
        {
            _elapsedTime -= spawnInterval;
            Spawn();
        }

    }

    void Spawn()
    {

        if (prefabs.Length == 0) return;

        int num_try = 5;
        bool done = false;
        while (num_try > 0 && !done) 
        {
            //Debug.Log("Spawn try... " + num_try);
            // random direction
            Vector3 dir = Random.onUnitSphere;
            dir += transform.right * 3f;
            dir.z = 0;
            dir.Normalize();

            dir *= Random.Range(spawnRange_min, spawnRange_max);
            Vector3 target = transform.position + dir;

            Collider[] cols = Physics.OverlapSphere(target, spawnColliderSize);
            if (cols.Length == 1) // if only collide with remover
            {
                // no overlapping nearby. spawn gogo
                Instantiate(prefabs[0], target, Quaternion.identity);

                done = true;
            }
            else
            {
                // spawn fail.. T.T
                --num_try;
                continue;
            }
        }
        
    }
}
