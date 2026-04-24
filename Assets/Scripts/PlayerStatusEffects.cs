using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatusEffects : MonoBehaviour
{
    private bool isStunned = false;
    public List<GameObject> statusHudSlots = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyStatus(string effect, float duration)
    {
        switch (effect)
        {
            case "stun": StunPlayer(duration); break;
        }
            
    }

    IEnumerator StunPlayer(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            yield return new WaitForSecondsRealtime(duration);
            isStunned = false;
        }
        else
        {
            yield break;
        }
        
    }

   /* IEnumerator UpdateHUD(string effect, float duration)
    {
        if (statusHudSlots.Count > 6) //idk how to break out of the coroutine;
        switch (effect)
        {
            case "stun": break;
        }
    }*/
}
