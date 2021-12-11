using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   //means you can use it anywhere
public class BattleMoves
{
    public string moveName;
    public int movePower;
    public int manaCost;
    public AttackEffect theEffectToUse;
}
