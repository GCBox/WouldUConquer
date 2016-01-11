using UnityEngine;
using System.Collections;

public class BackgroundMoving : MonoBehaviour
{

    public float min_range = 10f;
    public float max_range = 20f;
    public int numStars = 100;

    public GameObject[] star_prefabs;

    private Vector3 prev_pos;

    Transform[] objs;

    // Use this for initialization
    void Start()
    {
        prev_pos = transform.position;

        if (star_prefabs.Length == 0) return;

        objs = new Transform[numStars];

        for (int i = 0; i < numStars; ++i)
        {
            Vector3 dir = Random.onUnitSphere;
            float length = Random.Range(min_range, max_range);
            dir *= length;

            int index = Random.Range(0, star_prefabs.Length);

            GameObject obj = Instantiate<GameObject>(star_prefabs[index]);
            obj.transform.parent = transform;
            obj.transform.localPosition = dir;

            objs[i] = obj.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = transform.position - prev_pos;
        prev_pos = transform.position;

        Vector3 axis = new Vector3(dir.y, -dir.x, 0f);
        //Quaternion.AngleAxis(dir.magnitude, axis.normalized);
        transform.Rotate(axis, -dir.magnitude, Space.World);

        foreach (Transform t in objs)
        {
            t.rotation = Quaternion.identity;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, min_range);
        Gizmos.DrawWireSphere(transform.position, max_range);
    }
}
