using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dice : MonoBehaviour
{
    public PlayerController owner;
    public Camera cam;
    public int moves=1;
    public bool used=false;
    Item controler;
    ItemSO item_so;
    public Game gameManager;
    public void Use() {
        if(used==false) {
            moves = Random.Range(1,6);
            used=true;
        }
    }

    void Awake() {
        cam = owner.cam;
        gameManager.BeginTurnFunc += BeginTurn;
        gameManager.EndTurnFunc += EndTurn;
        //controler = GetComponent<Item>();
        //item_so.BeginTurn = new item_so.BeginTurn(BeginTurn);
        //controler.EndTurn += EndTurn;
        //controler.BeginTurn += BeginTurn;
        //item_so.EndTurnEvt += EndTurn;
    }

    void OnDestroy() {
        gameManager.BeginTurnFunc -= BeginTurn;
        gameManager.EndTurnFunc -= EndTurn;
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
                            if(moves>0) {
                                owner.transform.position = hit.transform.position;
                                owner.ontile = clicked;
                                moves--;
                            }
                        }
                    }
                }
            }
        }
    }
    void BeginTurn() {

    }
    void EndTurn() {
        used=false;
    }
}
