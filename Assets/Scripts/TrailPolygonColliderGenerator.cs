using UnityEngine;
using System.Collections;

public class TrailPolygonColliderGenerator : MonoBehaviour {

    public Transform target;
    public PolygonCollider2D _poly2d;
    public TrailRenderer trailRenderer;

    public ArrayList _segments = new ArrayList();

    public float lifeTime = 5f;
    //private bool _b_addSegPoints = true;
    
    //struct AABB_entry : IComparer {
    //    float p;
    //    int index;
    //    bool max;

    //    public AABB_entry(float _pos = 0f, int _index = 0, bool _max = false)
    //    {
    //        p = _pos;
    //        index = _index;
    //        max = _max;
    //    }

    //    public float coord
    //    {
    //        get { return p; }
    //        set { p = value; }
    //    }

    //    int IComparer.Compare(object x, object y)
    //    {
    //        AABB_entry a = (AABB_entry)x;
    //        AABB_entry b = (AABB_entry)y;
    //        if (a.p > b.p)
    //            return -1;
    //        else if (a.p == b.p)
    //            return 0;
    //        else
    //            return 1;
    //    }
    //}

    struct LineSegment
    {
        Vector2 point;
        Vector2 aabb_min, aabb_max;

        float time;

        public void SetTime(float t)
        {
            time = t;
        }
        public void SetPoint(Vector3 pos)
        {
            point = new Vector2();
            point.x = pos.x;
            point.y = pos.y;
        }
        public void SetAABB(Vector2 pos)
        {
            //aabb_min = new Vector2();
            //aabb_max = new Vector2();

            if (point.x < pos.x)
            {
                aabb_min.x = point.x;
                aabb_max.x = pos.x;
            }
            else
            {
                aabb_min.x = pos.x;
                aabb_max.x = point.x;
            }

            if (point.y < pos.y)
            {
                aabb_min.y = point.y;
                aabb_max.y = pos.y;
            }
            else
            {
                aabb_min.y = pos.y;
                aabb_max.y = point.y;
            }

            //Debug.Log("Point : " + point);
            //Debug.Log("Next Point : " + pos);
            //Debug.Log("AABB_min : " + aabb_min);
            //Debug.Log("AABB_max : " + aabb_max);

        }
        public bool IntersectP(Vector2 begin, Vector2 end)
        {
            //Debug.Log("AABB test Begin!");
            //Debug.Log("AABB min : " + aabb_min);
            //Debug.Log("AABB max : " + aabb_max);
            //Debug.Log("Begin : " + begin);
            //Debug.Log("End" + end);
            
            if (aabb_min.x < end.x && aabb_max.x > begin.x && aabb_min.y < end.y && aabb_max.y > begin.y)
                return true;
            else
                return false;
        }
        public bool PointDeadP()
        {
            return (time < Time.time);
        }

        public void DrawAABB(Color color)
        {
            //Gizmos.color = color;
            Vector2 aabb_lu = new Vector2(aabb_min.x, aabb_max.y);
            Vector2 aabb_rd = new Vector2(aabb_max.x, aabb_min.y);

            Gizmos.color = Color.gray;
            Gizmos.DrawLine(aabb_min, aabb_lu);
            Gizmos.DrawLine(aabb_min, aabb_rd);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(aabb_max, aabb_lu);
            Gizmos.DrawLine(aabb_max, aabb_rd);
        }

        public Vector2 pos
        {
            get { return point; }
        }

        public void PrintAABB()
        {
            Debug.Log("AABB min : " + aabb_min);
            Debug.Log("AABB max : " + aabb_max);
        }
    }



    //private ArrayList AABB = new ArrayList();

    // Use this for initialization
    void Start ()
    {
    }

    void OnDrawGizmos ()
    {
        
        //for (int i=0; i<_segments.Count-1; ++i)
        //{
        //    if (i % 2 == 0) Gizmos.color = Color.red;
        //    else Gizmos.color = Color.green;
        //    Vector2 p1 = ((LineSegment)_segments[i]).pos;
        //    Vector2 p2 = ((LineSegment)_segments[i+1]).pos;
        //    Gizmos.DrawLine(p1, p2);

        //    //((LineSegment)_segments[i]).DrawAABB(Color.gray);
        //}

        //if (_segments.Count > 2)
        //{
        //    Vector3 pos3 = target.transform.position;
        //    Vector2 pos = new Vector2(pos3.x, pos3.y);
        //    Vector2 end = ((LineSegment)_segments[_segments.Count - 1]).pos;

        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(end, pos);
        //}
    }
	
	// Update is called once per frame
	void Update () {

        //_segments.Add(target.position);
        //Debug.Log(target.position);

        if (_segments.Count > 2)
        {
            //Debug.Log("Segment intersection test begin!");

            Vector3 pos3 = target.transform.position;
            Vector2 pos = new Vector2(pos3.x, pos3.y);
            Vector2 end = ((LineSegment)_segments[_segments.Count - 1]).pos;

            Vector2 aabb_min, aabb_max;

            if (end.x < pos.x)
            {
                aabb_min.x = end.x;
                aabb_max.x = pos.x;
            }
            else
            {
                aabb_min.x = pos.x;
                aabb_max.x = end.x;
            }

            if (end.y < pos.y)
            {
                aabb_min.y = end.y;
                aabb_max.y = pos.y;
            }
            else
            {
                aabb_min.y = pos.y;
                aabb_max.y = end.y;
            }

            for (int i = 0; i < _segments.Count - 2; ++i)
            {
                LineSegment seg = (LineSegment)_segments[i];
                if (seg.IntersectP(aabb_min, aabb_max))
                {
                    CreatePolygon(pos, i + 1);
                    break;
                }
            }
        }

        //if (Input.GetButtonDown("Jump"))
        //{
        //    _b_addSegPoints = !_b_addSegPoints;
        //    //CreatePolygon();
        //}
	}

    public void AddSegmentsPoints(Vector3 p)
    {
        //if (!_b_addSegPoints) return;

        // called at LateUpdate in CCMove.cs(Player Movement)
        LineSegment seg = new LineSegment();
        seg.SetPoint(p);
        seg.SetTime(Time.time + lifeTime);
        _segments.Add(seg);

        if (_segments.Count > 1)
        {
            LineSegment seg_1 = (LineSegment)_segments[_segments.Count - 1];
            LineSegment seg_2 = (LineSegment)_segments[_segments.Count - 2];

            seg_2.SetAABB(seg_1.pos);

            _segments[_segments.Count - 2] = seg_2;

            //((LineSegment)_segments[_segments.Count - 2]).SetAABB(((LineSegment)_segments[_segments.Count - 1]).pos);
            //((LineSegment)_segments[_segments.Count - 2]).PrintAABB();
        }

        //_segments.Add(p);


        // delete dead segments
        if (((LineSegment)_segments[0]).PointDeadP())
        {
            _segments.RemoveAt(0);
        }
    }

    void CreatePolygon(Vector2 end, int offset = 0)
    {
        Debug.Log("Create Polygon");
        //Debug.Log("Segment size:" + _segments.Count);
        //Debug.Log("Offset:" + offset);

        int point_size = _segments.Count - offset;
        
        Vector2[] points = new Vector2[point_size+1];
        for (int i=0; i< point_size; ++i)
        {
            points[i] = ((LineSegment)_segments[i+offset]).pos;
            //Debug.Log(points[i]);
        }
        points[point_size] = end;

        //poly2d.SetPath(point_size-1, points);
        _poly2d.points = points;

        TestInside();

        _segments.Clear();
        //trailRenderer.Reset(this);
        trailRenderer.Clear();

        AddSegmentsPoints(end);
    }

    void TestInside()
    {
        Planet[] planet_list = GameObject.FindObjectsOfType<Planet>();
        foreach (Planet t in planet_list)
        {
            Vector3 p = t.transform.position;
            bool b = _poly2d.OverlapPoint(new Vector2(p.x, p.y));
            //Debug.Log(t);
            if (b)
            {
                Debug.Log(t.transform);

                // query to planet
                t.PickPlanet();
                //GameObject.Destroy(t.gameObject);
                
            }
        }
        //_poly2d.OverlapPoint(new Vector2(0, 0));
    }
}
