using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] string playerName;
    public int maxLevel = 50, playerLevel = 1, currentXP, maxHP = 100, currentHP, maxMana = 30, currentMana, dexterity, defence;
    public int weaponPower, armorDefence;
    [SerializeField] int[] xpForNextLevel;
    [SerializeField] int baseLevelXP = 100;

    // Start is called before the first frame update
    void Start()
    {
        xpForNextLevel = new int[maxLevel];
        xpForNextLevel[1] = baseLevelXP;

        for(int i = 2; i< xpForNextLevel.Length; i++)
        {
            xpForNextLevel[i] = (int)(0.02f * i * i * i + 3.06f * i * i + 105.6f * i); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddXP(100);
        }
    }

    public void AddXP(int amountofXP)
    {
        currentXP += amountofXP;
        if(currentXP> xpForNextLevel[playerLevel])
        {
            currentXP -= xpForNextLevel[playerLevel];
            playerLevel++;

            if(playerLevel % 2 == 0)
            {
                dexterity++;
            }
            else
            {
                defence++;
            }
            maxHP = Mathf.FloorToInt(maxHP * 1.18f);
            currentHP = maxHP;
            maxMana = Mathf.FloorToInt(maxMana * 1.06f);
            currentMana = maxMana;
        }
    }

    public string PlayerName()
    {
        return this.playerName;
    }

}
