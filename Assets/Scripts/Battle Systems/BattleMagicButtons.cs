using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleMagicButtons : MonoBehaviour
{
    public string spellName;
    public int spellCost;

    public TextMeshProUGUI spellNameText, spellCostText;


    public void Press()
    {
        if(BattleManager.instance.GetCurrentActiveCharacter().currentMana >= spellCost)
        {
            BattleManager.instance.magicPanel.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.GetCurrentActiveCharacter().currentMana -= spellCost;
        }
        else
        {
            BattleManager.instance.battleNotice.SetText("Not Enough Mana");
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.magicPanel.SetActive(false);
        }
    }

}
