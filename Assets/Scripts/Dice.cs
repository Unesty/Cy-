using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dice : MonoBehaviour
{
    public PlayerController owner;
    public Camera cam;
    public void Use() {
        
    }

    void Awake() {
        cam = owner.cam;
    }
    void Update() {
        RaycastHit hit;
        Ray ray;
        if(Mouse.current.leftButton.isPressed) {
            var mousepos = Mouse.current.position.ReadValue();
            ray = cam.ScreenPointToRay(mousepos);
            if( Physics.Raycast(ray, out hit, 100f)) {
                print(hit.transform.gameObject.name);
                Checker clicked = hit.transform.gameObject.GetComponent<Checker>();
                if(clicked!=null) {
                    for(ushort i=0; i<clicked.connected.Length; i++) {
                        if(clicked.connected[i]==owner.ontile) {
                            owner.transform.position = hit.transform.position;
                            owner.ontile = clicked;
                        }
                    }
                }
            }
        }
    }
}
