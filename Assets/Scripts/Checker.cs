using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Checker : MonoBehaviour
{
    public Checker[] connected = new Checker[4];//clockwise from the top
    public List<PlayerController> players;
    public Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;
    void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape(0,0,0,1,1,1,1,0);
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    void OnEnable()
    {
        UpdateMesh();
    }
    public void DrawShape()
    {
        
    }
    void CreateShape(float a,float b,float c,float d, float e, float f, float g, float h)
    {
        vertices = new Vector3[]
        {
            new Vector3(a,b,0),
            new Vector3(c,d,0),
            new Vector3(e,f,0),
            new Vector3(g,h,0),
        };
        triangles = new int[3*2];
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    void Update()
    {
    //    UpdateMesh();
    }
}
