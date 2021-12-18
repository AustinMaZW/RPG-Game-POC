using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleNotifications : MonoBehaviour
{
    [SerializeField] float timeAlive;
    [SerializeField] TextMeshProUGUI textNotice;
    
    public void SetText(string text)
    {
        textNotice.text = text;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        StartCoroutine(MakeNoticeDisappear());
    }

    public IEnumerator MakeNoticeDisappear()
    {
        yield return new WaitForSeconds(timeAlive);
        gameObject.SetActive(false);
    }
}
