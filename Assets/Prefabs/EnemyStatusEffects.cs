using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    public int poisonStacks = 1;
}

public class EnemyStatusEffects : MonoBehaviour
{
    [SerializeField]
    public List<ActiveStatusEffects> activeStatusEffects = new List<ActiveStatusEffects>();
    [SerializeField]
    GridLayoutGroup iconGrid;
    private Enemy enemy;
    public GameObject testIcon;
    public Sprite enfeebleIcon;
    public Sprite poisonIcon;
    public Sprite burnIcon;

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
                        activeStatusEffects[i].duration -= Time.deltaTime;
                        activeStatusEffects[i].timerText.text = ("" + activeStatusEffects[i].duration.ToString("F1"));
                        if (activeStatusEffects[i].duration <= 0)
                        {
                            Destroy(activeStatusEffects[i].icon);
                            activeStatusEffects.RemoveAt(i);
                            continue;
                        }
                        if (activeStatusEffects[i].tickTimer >= activeStatusEffects[i].tickRate)
                        {
                            activeStatusEffects[i].tickTimer = 0;
                            if (activeStatusEffects[i].damage > 0)
                            {
                                if (PlayerCombat.Instance.harvestActive && Random.Range(0, 101) <= PlayerCombat.Instance.harvestChance)
                                {
                                    PlayerCombat.Instance.HealPlayer(PlayerCombat.Instance.harvestHeal);
                                }
                                enemy.reduceHp(activeStatusEffects[i].damage * activeStatusEffects[i].poisonStacks, 1, false, activeStatusEffects[i].type);
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
    private float DurationChange(float durationToMod)
    {
        if (PlayerCombat.Instance.aEmberWickActive && EmberSystem.Instance.emberAmount / EmberSystem.Instance.baseEmber > .5f)
        {
            durationToMod = PlayerCombat.Instance.aEmberWickStatusExtend * durationToMod;
        }
        return durationToMod * PlayerCombat.Instance.statusDurationMultiplier;
    }
    public void ESEBurn(float burnDuration, int burnDamage, float burnTickRate)
    {
        {
            bool burnAlreadyActive = false;
            foreach (ActiveStatusEffects activeStatuses in activeStatusEffects)
            {
                if (activeStatuses.type == WeaponStatusEffect.Burn)
                {
                    burnAlreadyActive = true;
                    if (DurationChange(burnDuration) > activeStatuses.duration)
                    {
                        activeStatuses.duration = DurationChange(burnDuration);
                    }
                }
            }
            if (!burnAlreadyActive)
            {
                ActiveStatusEffects newBurn = new ActiveStatusEffects();
                newBurn.type = WeaponStatusEffect.Burn;
                newBurn.icon = Instantiate(testIcon, iconGrid.transform, false);
                newBurn.timerText = newBurn.icon.GetComponentInChildren<TextMeshProUGUI>();
                newBurn.icon.GetComponent<Image>().sprite = burnIcon;
                newBurn.duration = DurationChange(burnDuration);
                newBurn.damage = burnDamage;
                newBurn.tickRate = burnTickRate;
                activeStatusEffects.Add(newBurn);
            }
        }
    }
    public void ESEEnfeeble(float enfeebleDuration, float enfeebleExtraDmgPercent)
    {
        bool enfeebleAlreadyActive = false;
        foreach (ActiveStatusEffects activeStatuses in activeStatusEffects)
        {
            if (activeStatuses.type == WeaponStatusEffect.Enfeeble)
            {
                enfeebleAlreadyActive = true;
                if (DurationChange(enfeebleDuration) > activeStatuses.duration)
                {
                    activeStatuses.duration = DurationChange(enfeebleDuration);
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
            newEnfeeble.duration = DurationChange(enfeebleDuration);
            newEnfeeble.effectPercentage = enfeebleExtraDmgPercent;
            activeStatusEffects.Add(newEnfeeble);
        }
    }
    public void ESEStun(float stunDuration)
    {

    }
    public void ESEPoison(float poisonDuration, int poisonDamage, float poisonTickRate)
    {
        bool poisonAlreadyActive = false;

        foreach (ActiveStatusEffects activeStatuses in activeStatusEffects)
        {
            if (activeStatuses.type == WeaponStatusEffect.Poison)
            {
                activeStatuses.damage = Mathf.Max(activeStatuses.damage, poisonDamage);
                poisonAlreadyActive = true;
                if (PlayerCombat.Instance.blightedSoulActive || activeStatuses.poisonStacks < 5)
                {
                    activeStatuses.poisonStacks++;
                }
                if (DurationChange(poisonDuration) > activeStatuses.duration)
                {
                    activeStatuses.duration = DurationChange(poisonDuration);
                }
            }
        }
        if (!poisonAlreadyActive)
        {
            ActiveStatusEffects newPoison = new ActiveStatusEffects();
            newPoison.type = WeaponStatusEffect.Poison;
            newPoison.icon = Instantiate(testIcon, iconGrid.transform, false);
            newPoison.timerText = newPoison.icon.GetComponentInChildren<TextMeshProUGUI>();
            newPoison.icon.GetComponent<Image>().sprite = poisonIcon;
            newPoison.duration = DurationChange(poisonDuration);
            newPoison.damage = poisonDamage;
            newPoison.tickRate = poisonTickRate;
            activeStatusEffects.Add(newPoison);
        }
    }
}
