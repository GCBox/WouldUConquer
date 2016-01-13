using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject[] prefabs;

    // 얼마나 자주 GameObject가 spawn될 것인가?
    // 초당 생성 횟수
    // 숫자가 클수록 자주 생성
    public float spawnRate = 0.5f;
    [Range(0.01f, 3f)]
    public float spawnDeviation = 0.1f;
    private float spawnInterval;
    [Range(0.01f, 3f)]
    public float spawnMinmumInterval = 0.1f;


    bool activate = false;

    public float Interval
    {
        get
        {
            float retval = spawnInterval + spawnInterval * Random.Range(-spawnDeviation, spawnDeviation);
            return retval > spawnMinmumInterval ? retval : spawnMinmumInterval;
        }
    }

    public float spawnRange_min = 10f;
    public float spawnRange_max = 15f;

    // 이 수치가 높을 수록 우주선 정면에서만 등장함. 0이면 완전 랜덤. 음수이면 뒷쪽에서 생성됨
    // 최대값 제한 없음.
    public float spawnDirection = 2f;

    // 난이도. 시간이 지날 수록 상승함.
    private int level;

    // 생성하려는 주변에 물체가 반경 이 수치 이내에 없을 경우만 spawn한다
    public float spawnColliderSize = 5f;

    private float _elapsedTime;

    void Awake()
    {
        spawnInterval = 1f / spawnRate;
    }

    float NextInterval()
    {
        return spawnInterval;
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        //_elapsedTime += Time.deltaTime;

        //if (spawnRate != 0f && _elapsedTime > spawnInterval)
        //{
        //    _elapsedTime -= spawnInterval;
        //    Spawn();
        //}

    }

    public void SetActivate(bool enable = true)
    {
        activate = enable;
        if (activate)
            NextSpawn();
        else
            CancelInvoke();
        
    }

    void NextSpawn()
    {
        if (activate)
            Invoke("Spawn", Interval);
    }

    void Spawn()
    {
        if (prefabs.Length == 0) return;

        int num_try = 5;
        bool done = false;
        while (num_try > 0 && !done) 
        {
            // random direction
            Vector3 dir = Random.onUnitSphere;
            dir += transform.right * spawnDirection;
            dir.z = 0;
            dir.Normalize();

            dir *= Random.Range(spawnRange_min, spawnRange_max);
            Vector3 target = transform.position + dir;

            Collider[] cols = Physics.OverlapSphere(target, spawnColliderSize);

            if (cols.Length < 2) // if only collide with remover
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
        NextSpawn();
    }

}
