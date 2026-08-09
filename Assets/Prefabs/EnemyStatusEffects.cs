using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;
[System.Serializable]
public class ActiveStatusEffects
{
    public GameObject icon;
    public TextMeshProUGUI timerText;
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
    [SerializeField]
    GridLayoutGroup iconGrid;
    private Enemy enemy;
    public GameObject testIcon;
    public Sprite enfeebleIcon;

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
                                Destroy(activeStatusEffects[i].icon);
                                activeStatusEffects.RemoveAt(i);
                            }
                        }
                        break;
                    case WeaponStatusEffect.Enfeeble:
                        activeStatusEffects[i].duration -= Time.deltaTime;
                        activeStatusEffects[i].timerText.text = ("" + activeStatusEffects[i].duration.ToString("F1"));
                        if (activeStatusEffects[i].duration <= 0)
                        {
                            enemy.enfeebled = false;
                            Destroy(activeStatusEffects[i].icon);
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
        Debug.Log(enemy);
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
            newEnfeeble.icon = Instantiate(testIcon, iconGrid.transform, false);
            newEnfeeble.timerText = newEnfeeble.icon.GetComponentInChildren<TextMeshProUGUI>();
            newEnfeeble.icon.GetComponent<Image>().sprite = enfeebleIcon;
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
