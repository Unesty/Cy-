using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Checker ontile;
    
    public bool building;
    public GameObject tileplan; //move tile creation to item
    NewControls input;
    //public 
    
    // Start is called before the first frame update
    void Awake()
    {
        input = new NewControls();
    }

    // Update is called once per frame
    void Update()
    {
        if(building==true)
            StartBuilding();
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
