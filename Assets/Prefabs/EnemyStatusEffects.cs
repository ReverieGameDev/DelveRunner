using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;
[System.Serializable]
public class ActiveStatusEffects
{
    public WeaponStatusEffect type;
    public float duration;
    public int damage;
    public float tickRate;
    public float tickTimer;
    public float effectPercentage;
}

public class EnemyStatusEffects : MonoBehaviour
{
    [SerializeField]
    private List<ActiveStatusEffects> activeStatusEffects = new List<ActiveStatusEffects>();
    private Enemy enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeStatusEffects != null && activeStatusEffects.Count > 0)
        {
            for (int i = activeStatusEffects.Count - 1; i >= 0; i--)
            {
                switch (activeStatusEffects[i].type)
                {
                    case WeaponStatusEffect.Burn:
                    case WeaponStatusEffect.Poison:
                        activeStatusEffects[i].tickTimer += Time.deltaTime;
                        if (activeStatusEffects[i].tickTimer >= activeStatusEffects[i].tickRate)
                        {
                            activeStatusEffects[i].duration -= activeStatusEffects[i].tickRate;
                            activeStatusEffects[i].tickTimer = 0;
                            if (activeStatusEffects[i].damage > 0)
                            {
                                enemy.reduceHp(activeStatusEffects[i].damage, 1, false);
                            }
                            if (activeStatusEffects[i].duration <= 0)
                            {
                                activeStatusEffects.RemoveAt(i);
                            }
                        }
                        break;
                    case WeaponStatusEffect.Enfeeble:
                        activeStatusEffects[i].duration -= Time.deltaTime;
                        if (activeStatusEffects[i].duration <= 0)
                        {
                            enemy.enfeebled = false;
                            activeStatusEffects.RemoveAt(i);
                        }
                        break;
                }
                
            }
        }
    }

    public void ESEBurn(float burnDuration, int burnDamage, float burnTickRate)
    {
        ActiveStatusEffects newBurn = new ActiveStatusEffects();
        newBurn.type = WeaponStatusEffect.Burn;
        newBurn.duration = burnDuration;
        newBurn.damage = burnDamage;
        newBurn.tickRate = burnTickRate;
        activeStatusEffects.Add(newBurn);
    }
    public void ESEEnfeeble(float enfeebleDuration, float enfeebleExtraDmgPercent)
    {
        bool enfeebleAlreadyActive = false;
        foreach (ActiveStatusEffects activeStatuses in activeStatusEffects)
        {
            if (activeStatuses.type == WeaponStatusEffect.Enfeeble)
            {
                enfeebleAlreadyActive = true;
                if (enfeebleDuration > activeStatuses.duration)
                {
                    activeStatuses.duration = enfeebleDuration;
                }
            }
        }
        if (!enfeebleAlreadyActive)
        {
            ActiveStatusEffects newEnfeeble = new ActiveStatusEffects();
            enemy.enfeebled = true;
            enemy.enfeebleBonusDamage = enfeebleExtraDmgPercent;
            newEnfeeble.type = WeaponStatusEffect.Enfeeble;
            newEnfeeble.duration = enfeebleDuration;
            newEnfeeble.effectPercentage = enfeebleExtraDmgPercent;
            activeStatusEffects.Add(newEnfeeble);
        }
    }
    public void ESEStun(float stunDuration)
    {

    }
    public void ESEPoison(float poisonDuration, int poisonDamage, float poisonTickRate)
    {

    }
}
