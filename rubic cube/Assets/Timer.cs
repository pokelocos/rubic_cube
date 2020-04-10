using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int seconds = 1;
    public Text text;

    void Start()
    {
        StartCoroutine(TimerRutine());
        text.text = seconds.ToString();
    }

    public IEnumerator TimerRutine()
    {
        
        while(seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
            text.text = seconds.ToString();
        }
        this.gameObject.SetActive(false);
    }
}
