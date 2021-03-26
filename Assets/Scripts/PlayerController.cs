using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public Checker ontile;
    public float camspeed;
    
    public bool building;
    public GameObject tileplan; //move tile creation to item
    NewControls input;
    Vector3 pos = new Vector3();
    //public 
    
    // Start is called before the first frame update
    void Awake()
    {
        input = new NewControls();
    }

    // Update is called once per frame
    void Update()
    {
        var mousepos = Mouse.current.position.ReadValue();
        if(Mouse.current.middleButton.isPressed) {
            pos[0]+=mousepos[0]*camspeed;
            pos[1]+=mousepos[1]*camspeed;
            cam.transform.position = new Vector3(pos[0],pos[1],-10f);
        }
    }
    
    void StartBuilding()
    {
        building = false;
        tileplan.SetActive(true);
    }
    
    void HandlePointer()
    {
        
    }
    
    //void OnMouseClick()
    public void EndTurn() 
    {
        
    }
    
    //public void BuildTile()
    //{
    //    buildtile = Instantiate(tileplan,new Vector3(1,1,1), new Quaternion(0,0,0,0));
    //}
}
