using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter))]
public class Tileplan : MonoBehaviour
{
    enum states {
        edge,
        vertex,
        modify
    };
    public float area;
    public float min_edge_size=2.0f;
    states state=states.edge;
    public GameObject ground;
    [SerializeField]
    Checker[] connected = new Checker[4];
    ushort side;
    public GameObject checker_prefab;
    public GameObject player;
    public Mesh mesh;
    public Vector3[] vertices = new Vector3[4];
    public int[] triangles = new int[6];
    //public float[,] h_edge = new float[4,2];
    ushort tick=0;
    public Camera cam;
    void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    void OnEnable()
    {
        print("tileplan enabled");
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 ipoint;
        if(LineLineIntersection(out ipoint, new Vector3(1f,0f,0f),new Vector3(0f,0f,0f),new Vector3(0.4f,-1f,0f),new Vector3(0.6f,1f,0f))) {
            print("line test intersect passed" + ipoint);
        } else {
            print("line test intersect not passed");
        }
        if(false == LineLineIntersection(out ipoint, new Vector3(1f,0f,0f),new Vector3(0f,0f,0f),new Vector3(-0.4f,1000f,0f),new Vector3(0.5f,1000.1f,0f))) {
            print("line test nonintersect passed" + ipoint);
            if(ipoint!=Vector3.zero) print("actually no");
        } else {
            print("line test nonintersect not passed");
        }
    }
    // Update is called once per frame
    void Update()
    {
        var mousepos = Mouse.current.position.ReadValue();
        RaycastHit hit;
        Ray ray;
        if(Mouse.current.leftButton.isPressed) {
            ray = cam.ScreenPointToRay(mousepos);
            area = GetArea();
            if( Physics.Raycast(ray, out hit, 100f)) {
                uint sd;
                if(hit.transform.gameObject == ground){
                    switch(state) {
                    case states.edge:
                    for(int v=0; v<4;v++) {
                        int n=v+1;
                        if(n==4) n=0;
                        Vector3 ipoint;
                        if(LineLineIntersection(out ipoint, player.GetComponent<PlayerController>().ontile.vertices[v]+player.GetComponent<PlayerController>().ontile.transform.position, player.GetComponent<PlayerController>().ontile.vertices[n]+player.GetComponent<PlayerController>().ontile.transform.position,player.GetComponent<PlayerController>().ontile.transform.position,hit.point)) {
                            print(hit.point);
                            print("intersected at" + ipoint);
                            //TODO detect new triangle overlaps with vertices, edges, colliders
                            if(v==0) {
                                vertices[3]=player.GetComponent<PlayerController>().ontile.vertices[0];
                                vertices[2]=player.GetComponent<PlayerController>().ontile.vertices[1];
                                vertices[1] = Vector3.back;
                                vertices[0] = Vector3.back;
                                connected[2]=player.GetComponent<PlayerController>().ontile;
                                side=0;
                                print("top");
                            } else if(v==1) {
                                vertices[0]=player.GetComponent<PlayerController>().ontile.vertices[1];
                                vertices[3]=player.GetComponent<PlayerController>().ontile.vertices[2];
                                vertices[2] = Vector3.back;
                                vertices[1] = Vector3.back;
                                connected[3]=player.GetComponent<PlayerController>().ontile;
                                side=1;
                                print("right");
                            } else if(v==2) {
                                vertices[1]=player.GetComponent<PlayerController>().ontile.vertices[2];
                                vertices[0]=player.GetComponent<PlayerController>().ontile.vertices[3];
                                vertices[2] = Vector3.back;
                                vertices[3] = Vector3.back;
                                connected[0]=player.GetComponent<PlayerController>().ontile;
                                side=2;
                                print("bottom");
                            } else if(v==3) {
                                vertices[2]=player.GetComponent<PlayerController>().ontile.vertices[3];
                                vertices[1]=player.GetComponent<PlayerController>().ontile.vertices[0];
                                vertices[3] = Vector3.back;
                                vertices[0] = Vector3.back;
                                connected[1]=player.GetComponent<PlayerController>().ontile;
                                side=3;
                                print("left");
                            }
                            state=states.vertex;
                            break;
                        } else {
                            if(v==3) {
                                print("no edge chosen");
                            }
                        }
                    }
                    break;
                    case states.vertex:
                    for(ushort i=0;i<4;i++) {
                        if(vertices[i]==Vector3.back) {  
                            if(Vector3.Distance(hit.point,vertices[0])<min_edge_size && vertices[0]!=Vector3.back) break;
                            if(Vector3.Distance(hit.point,vertices[1])<min_edge_size && vertices[1]!=Vector3.back) break;
                            if(Vector3.Distance(hit.point,vertices[2])<min_edge_size && vertices[2]!=Vector3.back) break;
                            if(Vector3.Distance(hit.point,vertices[3])<min_edge_size && vertices[3]!=Vector3.back) break;
                            vertices[i]=hit.point;
                            print(Vector3.Distance(hit.point,vertices[2]));
                            print("vert" + i + "set" + hit.point);
                            mesh.Clear();
                            mesh.vertices = vertices;
                            mesh.triangles = triangles;
                            break;
                        } else {
                            if(i==3) {
                                mesh.Clear();
                                mesh.vertices = vertices;
                                mesh.triangles = triangles;
                                print("all verts set");
                                state=states.modify;
                                tick=0;
                            }
                        }
                    }
                    break;
                    case states.modify:
                        if(tick==1){
                            if(state==states.modify) {
                                Vector3 newtransform = MiddlePoint();
                                GameObject newtile = Instantiate(checker_prefab, newtransform, new Quaternion(0,0,0,0));
                                Checker newchecker = newtile.GetComponent<Checker>();
                                print("new tile"+newtransform);
                                for(ushort i=0;i<4;i++) {
                                    newchecker.vertices[i]=vertices[i]-newtransform;
                                }
                                newchecker.UpdateMesh();
                                newchecker.connected=(Checker[])connected.Clone();
                                player.GetComponent<PlayerController>().ontile.connected[side]=newchecker;
                                vertices[0]=Vector3.zero;
                                vertices[1]=Vector3.zero;
                                vertices[2]=Vector3.zero;
                                vertices[3]=Vector3.zero;
                                connected[0]=null;
                                connected[1]=null;
                                connected[2]=null;
                                connected[3]=null;
                                mesh.Clear();
                                mesh.vertices = vertices;
                                mesh.triangles = triangles;
                                gameObject.SetActive(false);
                                state=states.edge;
                            }
                        }
                    break;
                    }
                } else {
                    print(hit.transform.gameObject.name);
                }
                /*
                if(hit.transform.position[0]<transform.position[0]) {
                    if(hit.transform.position[1]<transform.position[1]) {
                        sd = 3;
                    } else {
                        sd = 0;
                    }
                } else {
                    if(hit.transform.position[1]<transform.position[1]) {
                        sd = 1;
                    } else {
                        sd = 2;
                    }
                }
                connected[sd]=hit.collider.gameObject.GetComponent<Checker>();
                */
                
                
                //foreach(Vector3 vert in connected[sd].vertices) {
                //    if(edg[0]==null) {
                //        edg[0]=vert;
                //    } else {
                //        if(edg[1]==null) {
                //            edg[1]=vert;
                //        } else {
                //            if (edg[0][0]<edg[1][0]) {
                //                
                //            } else {
                //                
                //            }
                //        }
                //    }
                //}
            }
        } else {
            tick=1;
        }
        if(Mouse.current.rightButton.isPressed) {
            vertices[0]=Vector3.zero;
            vertices[1]=Vector3.zero;
            vertices[2]=Vector3.zero;
            vertices[3]=Vector3.zero;
            state=states.edge;
            connected[0]=null;
            connected[1]=null;
            connected[2]=null;
            connected[3]=null;
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
    }
    public static bool LineLineIntersection(out Vector3 intersection, Vector3 p1,
        Vector3 p2, Vector3 p3, Vector3 p4){
            intersection = Vector3.zero;

            var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

            if (d == 0.0f)
            {
                return false;
            }

            var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
            var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

            if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
            {
                return false;
            }

            intersection.x = p1.x + u * (p2.x - p1.x);
            intersection.y = p1.y + u * (p2.y - p1.y);

            return true;
    }
    float GetArea()
    {
        float Area=0;
        for(int ii = 0 ; ii < triangles.Length; ii+=3)
        {
            Vector3 A = vertices[triangles[ii]];
            Vector3 B = vertices[triangles[ii+1]];
            Vector3 C = vertices[triangles[ii+2]];
            Vector3 V = Vector3.Cross(A-B, A-C);
            Area += V.magnitude * 0.5f;
        }
        return Area;
    }
    Vector3 MiddlePoint()
    {
        float x = (vertices[0][0]+vertices[1][0]+vertices[2][0]+vertices[3][0])*0.25f;
        float y = (vertices[0][1]+vertices[1][1]+vertices[2][1]+vertices[3][1])*0.25f;

        return new Vector3(x,y,0f);
    }
}
