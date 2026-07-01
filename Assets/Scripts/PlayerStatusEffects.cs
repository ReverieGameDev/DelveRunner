using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatusEffects : MonoBehaviour
{
    public bool isStunned = false;
    public List<GameObject> statusHudSlots = new List<GameObject>();
    public bool isPoisoned = false;
    private PlayerMovement playerMovement;
    public GameObject statusSlot;
    public Transform statusHudContainer;
    public Sprite stunIcon;
    private PlayerCombat playerCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyStatus(string effect, float duration, float damage)
    {
        switch (effect)
        {
            case "stun": StartCoroutine(StunPlayer(duration)); break;
            case "poison": StartCoroutine(PoisonPlayer(duration,damage)); break;
        }
            
    }

    IEnumerator StunPlayer(float duration)
    {
        if (playerCombat.isSovereignImmunityActive)
        {
            playerCombat.StartCoroutine(playerCombat.SovereignImmunity());
            yield break;
        }
        if (!isStunned)
        {
            playerMovement.anim.SetFloat("IsMoving", 0);
            isStunned = true;
            GameObject slot = Instantiate(statusSlot, statusHudContainer);
            StatusSlotScript slotScript = slot.GetComponent<StatusSlotScript>();
            slotScript.statusIcon.sprite = stunIcon;
            slotScript.effectName.text = "Stunned";
            slotScript.timerText.text = "0:"+ "0"+duration;
            int timeLeftStunned = (int)duration;
            for (int i = 0; i<duration; i++)
            {
                yield return new WaitForSeconds(1f);
                timeLeftStunned-=1;
                slotScript.timerText.text = "0:" + "0" + timeLeftStunned;
            }
            Destroy(slot);
            isStunned = false;
        }
        else
        {
            yield break;
        }
        
    }
    IEnumerator PoisonPlayer(float duration, float damage)
    {
        if (!isPoisoned)
        {
            isPoisoned = true;
            yield return new WaitForSecondsRealtime(duration);
            isPoisoned = false;
        }
        else
        {
            yield break;
        }

    }


}
