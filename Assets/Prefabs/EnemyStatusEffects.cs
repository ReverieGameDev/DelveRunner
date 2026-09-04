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
    public int emberGain;
    public int currentShockStacks = 1;
    public int maxShockStacks = 3;
    public int bleedStacks = 1;
}

public class EnemyStatusEffects : MonoBehaviour
{
    [SerializeField]
    public List<ActiveStatusEffects> activeStatusEffects = new List<ActiveStatusEffects>();
    [SerializeField]
    GridLayoutGroup iconGrid;
    private Enemy enemy;
    public GameObject testIcon;
    public Sprite[] enfeebleIcon;
    public Sprite[] poisonIcon;
    public Sprite[] burnIcon;
    public Sprite[] cinderIcon;
    public Sprite[] bleedIcon;
    public Sprite[] shockIcon;
    public GameObject shockPrefab;
    public GameObject shockboltPrefab;

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
                    case WeaponStatusEffect.Cinder:
                    case WeaponStatusEffect.Shock:
                    case WeaponStatusEffect.Bleed:
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
                                int mult = 1;
                                if (activeStatusEffects[i].type == WeaponStatusEffect.Poison) mult = activeStatusEffects[i].poisonStacks;
                                else if (activeStatusEffects[i].type == WeaponStatusEffect.Bleed) mult = activeStatusEffects[i].bleedStacks;
                                enemy.reduceHp(activeStatusEffects[i].damage * mult, 1, false, activeStatusEffects[i].type);
                                if (activeStatusEffects[i].type == WeaponStatusEffect.Shock && PlayerCombat.Instance.aConductionActive) PlayerCombat.Instance.AddManaPlayer(PlayerCombat.Instance.aConductionManaPerTick);
                                if (activeStatusEffects[i].type == WeaponStatusEffect.Cinder) EmberSystem.Instance.AddEmber(activeStatusEffects[i].emberGain);
                                if (activeStatusEffects[i].type == WeaponStatusEffect.Shock)
                                { 
                                    Instantiate(shockPrefab,transform.position, Quaternion.identity);
                                    RelayShock(activeStatusEffects[i].currentShockStacks); 
                                }
                                
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
                newBurn.icon.GetComponentInChildren<StatusIconAnimator>().frames = burnIcon;
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
            newEnfeeble.icon.GetComponentInChildren<StatusIconAnimator>().frames = enfeebleIcon;
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
                int poisonCap = PlayerCombat.Instance.blightedSoulActive ? 15 : 5;
                if (activeStatuses.poisonStacks < poisonCap)
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
            newPoison.icon.GetComponentInChildren<StatusIconAnimator>().frames = poisonIcon;
            newPoison.duration = DurationChange(poisonDuration);
            newPoison.damage = poisonDamage;
            newPoison.tickRate = poisonTickRate;
            activeStatusEffects.Add(newPoison);
        }
    }
    public void ESEBleed(float bleedDuration, int bleedDamage, float bleedTickRate)
    {
        bool bleedAlreadyActive = false;

        foreach (ActiveStatusEffects activeStatuses in activeStatusEffects)
        {
            if (activeStatuses.type == WeaponStatusEffect.Bleed)
            {
                activeStatuses.damage = Mathf.Max(activeStatuses.damage, bleedDamage);
                bleedAlreadyActive = true;
                if (activeStatuses.bleedStacks < 5)
                {
                    activeStatuses.bleedStacks++;
                }
                if (DurationChange(bleedDuration) > activeStatuses.duration)
                {
                    activeStatuses.duration = DurationChange(bleedDuration);
                }
            }
        }
        if (!bleedAlreadyActive)
        {
            ActiveStatusEffects newBleed = new ActiveStatusEffects();
            newBleed.type = WeaponStatusEffect.Bleed;
            newBleed.icon = Instantiate(testIcon, iconGrid.transform, false);
            newBleed.timerText = newBleed.icon.GetComponentInChildren<TextMeshProUGUI>();
            newBleed.icon.GetComponentInChildren<StatusIconAnimator>().frames = bleedIcon;
            newBleed.duration = DurationChange(bleedDuration);
            newBleed.damage = bleedDamage;
            newBleed.tickRate = bleedTickRate;
            activeStatusEffects.Add(newBleed);
        }
    }
    public void ESECinder(float cinderDuration, int cinderDamage, float cinderTickRate, int emberAmount = 1)
    {
        {
            bool cinderAlreadyActive = false;
            foreach (ActiveStatusEffects activeStatuses in activeStatusEffects)
            {
                if (activeStatuses.type == WeaponStatusEffect.Cinder)
                {
                    cinderAlreadyActive = true;
                    if (DurationChange(cinderDuration) > activeStatuses.duration)
                    {
                        activeStatuses.duration = DurationChange(cinderDuration);
                    }
                }
            }
            if (!cinderAlreadyActive)
            {
                ActiveStatusEffects newCinder = new ActiveStatusEffects();
                newCinder.type = WeaponStatusEffect.Cinder;
                newCinder.icon = Instantiate(testIcon, iconGrid.transform, false);
                newCinder.timerText = newCinder.icon.GetComponentInChildren<TextMeshProUGUI>();
                newCinder.icon.GetComponentInChildren<StatusIconAnimator>().frames = cinderIcon;
                newCinder.duration = DurationChange(cinderDuration);
                newCinder.damage = cinderDamage;
                newCinder.emberGain = emberAmount;
                newCinder.tickRate = cinderTickRate;
                activeStatusEffects.Add(newCinder);
            }
        }
    }
    public void ESEShock(float shockDuration, int shockDamage, float shockTickRate, int shockMaxStacks = 3)
    {
        bool shockAlreadyActive = false;

        foreach (ActiveStatusEffects activeStatuses in activeStatusEffects)
        {
            if (activeStatuses.type == WeaponStatusEffect.Shock)
            {
                activeStatuses.damage = Mathf.Max(activeStatuses.damage, shockDamage);
                shockAlreadyActive = true;
                if (activeStatuses.currentShockStacks < shockMaxStacks)
                {
                    activeStatuses.currentShockStacks++;
                }
                if (DurationChange(shockDuration) > activeStatuses.duration)
                {
                    activeStatuses.duration = DurationChange(shockDuration);
                }
            }
        }
        if (!shockAlreadyActive)
        {
            ActiveStatusEffects newShock = new ActiveStatusEffects();
            newShock.type = WeaponStatusEffect.Shock;
            newShock.icon = Instantiate(testIcon, iconGrid.transform, false);
            newShock.timerText = newShock.icon.GetComponentInChildren<TextMeshProUGUI>();
            newShock.icon.GetComponentInChildren<StatusIconAnimator>().frames = shockIcon;
            newShock.duration = DurationChange(shockDuration);
            newShock.damage = shockDamage;
            newShock.tickRate = shockTickRate;
            activeStatusEffects.Add(newShock);
        }
    }

    public void RelayShock(int currentStacks)
    {
        
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemies = enemies.Where(inRange=> inRange != enemy.gameObject && Vector2.Distance(inRange.transform.position, enemy.transform.position) <= 10).ToList();
        for (int i = 0; i < currentStacks; i++)
        {
            if (enemies.Count == 0) break;
            int enemyHit = (Random.Range(0, enemies.Count));
            GameObject shockbolt = Instantiate(shockboltPrefab, transform.position, Quaternion.identity);
            ShockboltCarrier shockboltScript = shockbolt.GetComponent<ShockboltCarrier>();
            shockboltScript.enemyGameObjectHit = enemies[enemyHit];
            enemies.RemoveAt(enemyHit);
        }
    }
}
