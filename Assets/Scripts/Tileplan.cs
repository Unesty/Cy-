using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter))]
public class Tileplan : MonoBehaviour
{
    public List<Checker> connected;
    public GameObject player;
    public Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;
    public float[] h_edge = new float[2];
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
                print(mousepos);
            }
        }
    }
}
