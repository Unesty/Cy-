using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    List<PlayerController> players;
    List<Item> items;
    uint players_finished=0;
    ulong turn=0;
    public delegate void BeginTurnDelegate();
    public BeginTurnDelegate BeginTurnFunc;
    void BeginTurn()
    {
        /*
        foreach(PlayerController player in players) {
            player.BeginTurn();
        }
        foreach(Item item in items) {
            item.BeginTurn();
        }
        */
        BeginTurnFunc();
    }
    public delegate void EndTurnDelegate();
    public EndTurnDelegate EndTurnFunc;
    void EndTurn()
    {        
        /*
        foreach(PlayerController player in players) {
            player.EndTurn();
        }
        foreach(Item item in items) {
            item.EndTurn();
        }
        */
        //Call EndTurn for each item and player
        EndTurnFunc();
        print("end turn");
        turn++;
    }
    public void CallEndTurn(PlayerController player)
    {
        players_finished++;
        print("player has finished");
        if(players_finished >= players.Count) {
            EndTurn();
        }
    }
}
