using UnityEngine;
using System.Collections;

public class TimedTrailRenderer : MonoBehaviour
{
    public PolygonCollider2D _poly2d;
    //private TimedTrailRenderer trailRenderer;

    public ArrayList _segments = new ArrayList();

    public float lifeTime = 5f;
    public float deadTime = 1f;
    public float highlightTime = 2f;

    [HideInInspector]
    public new Transform transform;


    public bool emit = true;
    public float emitTime = 0.00f;
    public Material material;

    public Color[] colors;
    public float[] sizes;

    public float uvLengthScale = 0.01f;
    public bool higherQualityUVs = true;

    public int movePixelsForRebuild = 6;
    public float maxRebuildTime = 0.1f;

    public float minVertexDistance = 0.10f;

    public float maxVertexDistance = 10.00f;
    public float maxAngle = 3.00f;

    public bool autoDestruct = false;

    private ArrayList points = new ArrayList();
    private GameObject o;
    private Vector3 lastPosition;
    private Vector3 lastCameraPosition1;
    private Vector3 lastCameraPosition2;
    private float lastRebuildTime = 0.00f;
    private bool lastFrameEmit = true;

    public class Point
    {
        public float timeCreated = 0.00f;
        public Vector3 position;
        public bool lineBreak = false;

        public bool dead = false;
        public float transTime;
        public bool highlight = false;
    }

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

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Start()
    {
        lastPosition = transform.position;
        o = new GameObject("Trail");
        o.transform.parent = null;
        o.transform.position = Vector3.zero;
        o.transform.rotation = Quaternion.identity;
        o.transform.localScale = Vector3.one;
        o.AddComponent(typeof(MeshFilter));
        o.AddComponent(typeof(MeshRenderer));
        o.GetComponent<Renderer>().material = material;
    }

    void OnEnable()
    {
        lastPosition = transform.position;
        o = new GameObject("Trail");
        o.transform.parent = null;
        o.transform.position = Vector3.zero;
        o.transform.rotation = Quaternion.identity;
        o.transform.localScale = Vector3.one;
        o.AddComponent(typeof(MeshFilter));
        o.AddComponent(typeof(MeshRenderer));
        o.GetComponent<Renderer>().material = material;
    }

    void OnDisable()
    {
        Destroy(o);
    }

    void Update()
    {

        // for intersection checking sample points
        if (_segments.Count > 2)
        {
            //Debug.Log("Segment intersection test begin!");

            Vector3 pos3 = transform.transform.position;
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








        if (emit && emitTime != 0)
        {
            emitTime -= Time.deltaTime;
            if (emitTime == 0) emitTime = -1;
            if (emitTime < 0) emit = false;
        }

        if (!emit && points.Count == 0 && autoDestruct)
        {
            Destroy(o);
            Destroy(gameObject);
        }

        // early out if there is no camera
        if (!Camera.main) return;

        bool re = false;

        // if we have moved enough, create a new vertex and make sure we rebuild the mesh
        float theDistance = (lastPosition - transform.position).magnitude;
        if (emit)
        {
            if (theDistance > minVertexDistance)
            {
                bool make = false;
                if (points.Count < 3)
                {
                    make = true;
                }
                else
                {
                    Vector3 l1 = ((Point)points[points.Count - 2]).position - ((Point)points[points.Count - 3]).position;
                    Vector3 l2 = ((Point)points[points.Count - 1]).position - ((Point)points[points.Count - 2]).position;
                    if (Vector3.Angle(l1, l2) > maxAngle || theDistance > maxVertexDistance) make = true;
                }

                if (make)
                {
                    Point p = new Point();
                    p.position = transform.position;
                    p.timeCreated = Time.time;
                    points.Add(p);
                    lastPosition = transform.position;

                    AddSegmentsPoints(transform.position);
                }
                else
                {
                    ((Point)points[points.Count - 1]).position = transform.position;
                    ((Point)points[points.Count - 1]).timeCreated = Time.time;
                }
            }
            else if (points.Count > 0)
            {
                ((Point)points[points.Count - 1]).position = transform.position;
                ((Point)points[points.Count - 1]).timeCreated = Time.time;
            }
        }

        if (!emit && lastFrameEmit && points.Count > 0) ((Point)points[points.Count - 1]).lineBreak = true;
        lastFrameEmit = emit;

        // approximate if we should rebuild the mesh or not
        if (points.Count > 1)
        {
            Vector3 cur1 = Camera.main.WorldToScreenPoint(((Point)points[0]).position);
            lastCameraPosition1.z = 0;
            Vector3 cur2 = Camera.main.WorldToScreenPoint(((Point)points[points.Count - 1]).position);
            lastCameraPosition2.z = 0;

            float distance = (lastCameraPosition1 - cur1).magnitude;
            distance += (lastCameraPosition2 - cur2).magnitude;

            if (distance > movePixelsForRebuild || Time.time - lastRebuildTime > maxRebuildTime)
            {
                re = true;
                lastCameraPosition1 = cur1;
                lastCameraPosition2 = cur2;
            }
        }
        else
        {
            re = true;
        }


        if (re)
        {
            lastRebuildTime = Time.time;

            ArrayList remove = new ArrayList();
            int i = 0;
            foreach (Point p in points)
            {
                // cull old points first
                if (p.dead && Time.time - p.transTime > deadTime) remove.Add(p);
                else if (p.highlight && Time.time - p.transTime > highlightTime) remove.Add(p);
                else if (Time.time - p.timeCreated > lifeTime) remove.Add(p);

                i++;
            }

            foreach (Point p in remove) points.Remove(p);
            remove.Clear();

            if (points.Count > 1)
            {
                Vector3[] newVertices = new Vector3[points.Count * 2];
                Vector2[] newUV = new Vector2[points.Count * 2];
                int[] newTriangles = new int[(points.Count - 1) * 6];
                Color[] newColors = new Color[points.Count * 2];

                i = 0;
                float curDistance = 0.00f;

                foreach (Point p in points)
                {
                    float time = (Time.time - p.timeCreated) / lifeTime;
                    time = time * 0.5f + 0.5f;
                    if (p.dead && p.transTime + deadTime < p.timeCreated + lifeTime)
                    {
                        time = (p.transTime - p.timeCreated) / lifeTime + (Time.time - p.transTime) / deadTime;
                        time = time * 0.5f + 0.5f;
                    }
                    else if (p.highlight)
                    {
                        time = (Time.time - p.transTime) / highlightTime;
                        if (time > 1) time = 1f;
                    }
                    

                    Color color = Color.Lerp(Color.white, Color.clear, time);
                    if (colors != null && colors.Length > 0)
                    {
                        float colorTime = time * (colors.Length - 1);
                        float min = Mathf.Floor(colorTime);
                        float max = Mathf.Clamp(Mathf.Ceil(colorTime), 1, colors.Length - 1);
                        float lerp = Mathf.InverseLerp(min, max, colorTime);
                        if (min >= colors.Length) min = colors.Length - 1; if (min < 0) min = 0;
                        if (max >= colors.Length) max = colors.Length - 1; if (max < 0) max = 0;
                        color = Color.Lerp(colors[(int)min], colors[(int)max], lerp);
                    }

                    float size = 1f;
                    if (sizes != null && sizes.Length > 0)
                    {
                        float sizeTime = time * (sizes.Length - 1);
                        float min = Mathf.Floor(sizeTime);
                        float max = Mathf.Clamp(Mathf.Ceil(sizeTime), 1, sizes.Length - 1);
                        float lerp = Mathf.InverseLerp(min, max, sizeTime);
                        if (min >= sizes.Length) min = sizes.Length - 1; if (min < 0) min = 0;
                        if (max >= sizes.Length) max = sizes.Length - 1; if (max < 0) max = 0;
                        size = Mathf.Lerp(sizes[(int)min], sizes[(int)max], lerp);
                    }

                    Vector3 lineDirection = Vector3.zero;
                    if (i == 0) lineDirection = p.position - ((Point)points[i + 1]).position;
                    else lineDirection = ((Point)points[i - 1]).position - p.position;

                    Vector3 vectorToCamera = Camera.main.transform.position - p.position;
                    Vector3 perpendicular = Vector3.Cross(lineDirection, vectorToCamera).normalized;

                    newVertices[i * 2] = p.position + (perpendicular * (size * 0.5f));
                    newVertices[(i * 2) + 1] = p.position + (-perpendicular * (size * 0.5f));

                    newColors[i * 2] = newColors[(i * 2) + 1] = color;

                    newUV[i * 2] = new Vector2(curDistance * uvLengthScale, 0);
                    newUV[(i * 2) + 1] = new Vector2(curDistance * uvLengthScale, 1);

                    if (i > 0 && !((Point)points[i - 1]).lineBreak)
                    {
                        if (higherQualityUVs) curDistance += (p.position - ((Point)points[i - 1]).position).magnitude;
                        else curDistance += (p.position - ((Point)points[i - 1]).position).sqrMagnitude;

                        newTriangles[(i - 1) * 6] = (i * 2) - 2;
                        newTriangles[((i - 1) * 6) + 1] = (i * 2) - 1;
                        newTriangles[((i - 1) * 6) + 2] = i * 2;

                        newTriangles[((i - 1) * 6) + 3] = (i * 2) + 1;
                        newTriangles[((i - 1) * 6) + 4] = i * 2;
                        newTriangles[((i - 1) * 6) + 5] = (i * 2) - 1;
                    }

                    i++;
                }

                Mesh mesh = (o.GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;
                mesh.Clear();
                mesh.vertices = newVertices;
                mesh.colors = newColors;
                mesh.uv = newUV;
                mesh.triangles = newTriangles;
            }
        }
    }

    public void Reset()
    {

        //GameObject trailOther = Instantiate(o);
        //Mesh mesh_ = Instantiate<Mesh>(mesh);
        //for (int i = 0; i < mesh_.colors.Length; ++i)
        //{
        //    mesh_.colors[i] = Color.black;
        //    mesh_.uv[i] = new Vector2(0f, 0f);
        //}
        //o.GetComponent<MeshFilter>().mesh = mesh_;
        //points = new ArrayList();
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

        int i;



        int point_size = _segments.Count - offset;

        Vector2[] points_ = new Vector2[point_size + 1];
        for (i = 0; i < point_size; ++i)
        {
            points_[i] = ((LineSegment)_segments[i + offset]).pos;
            //Debug.Log(points[i]);
        }
        points_[point_size] = end;

        //poly2d.SetPath(point_size-1, points);
        _poly2d.points = points_;

        bool picked = TestInside();
        Debug.Log(picked);

        i = 0;
        foreach (Point p in points)
        {
            if (p.dead || p.highlight) continue;
            p.transTime = Time.time;
            if (i < offset - 2 || !picked)
            {
                p.dead = true;
                p.highlight = false;
            }
            else {
                p.highlight = true;
                p.timeCreated = Time.time;
            }
            i++;
        }

        _segments.Clear();
        //trailRenderer.Reset(this);
        //trailRenderer.Reset();
        //trail.Reset();

        AddSegmentsPoints(end);
    }

    bool TestInside()
    {
        bool retval = false;
        Planet[] planet_list = FindObjectsOfType<Planet>();
        foreach (Planet t in planet_list)
        {
            Vector3 p = t.transform.position;
            bool b = _poly2d.OverlapPoint(new Vector2(p.x, p.y));
            //Debug.Log(t);
            if (b)
            {
                retval = true;
                Debug.Log(t.transform);

                // query to planet
                t.PickPlanet();
                //GameObject.Destroy(t.gameObject);

            }
        }
        return retval;
        //_poly2d.OverlapPoint(new Vector2(0, 0));
    }
}