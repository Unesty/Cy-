using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter))]
public class Tileplan : MonoBehaviour
{
    public Checker[] connected = new Checker[4];
    public GameObject player;
    public Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;
    //public float[,] h_edge = new float[4,2];
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
    }
    // Update is called once per frame
    void Update()
    {
        var mousepos = Mouse.current.position.ReadValue();
        RaycastHit hit;
        Ray ray;
        if(Mouse.current.leftButton.isPressed) {
            ray = cam.ScreenPointToRay(mousepos); 
            if( Physics.Raycast(ray, out hit, 100f)) {
                print(hit.transform.name);
                uint sd;
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
                Vector3[] edg = new Vector3[2];
                for(int v=0; v<3;v++) {
                    int n=v+1;
                    if(n==4) n=0;
                    if(LineLineIntersection(connected[sd].vertices[v], connected[sd].vertices[n],connected[sd].transform.position,hit.point)) {
                        if(v==0) {
                            vertices[3]=connected[sd].vertices[0];
                            vertices[2]=connected[sd].vertices[1];
                        } else if(v==1) {
                            vertices[0]=connected[sd].vertices[1];
                            vertices[3]=connected[sd].vertices[2];
                        } else if(v==2) {
                            vertices[1]=connected[sd].vertices[2];
                            vertices[0]=connected[sd].vertices[3];
                        } else if(v==3) {
                            vertices[2]=connected[sd].vertices[3];
                            vertices[1]=connected[sd].vertices[0];
                        }
                        break;
                    }
                }
                
                
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
        }
    }
    public static bool LineLineIntersection(Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if( Mathf.Approximately(planarFactor, 0f) && 
            !Mathf.Approximately(crossVec1and2.sqrMagnitude, 0f))
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            //intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            //intersection = Vector3.zero;
            return false;
        }
    }
}
