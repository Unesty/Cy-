using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO item_so;
    public delegate void BeginTurnDelegate();
    public BeginTurnDelegate BeginTurn;

    public delegate void EndTurnDelegate();
    public EndTurnDelegate EndTurn;
}
