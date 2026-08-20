using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System;
using JetBrains.Annotations;

public class PlayerCombat : MonoBehaviour
{
    public GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI statsText;
    public bool chargeAttackFired = false;
    public bool aOverchargeActive = false;
    public bool aConductionActive = false;
    public int aConductionManaPerTick;
    public bool aStaticCarrierActive = false;
    public int aStaticCarrierChance;
    public float lastHitTime;
    public int currentBackstabbersStacks = 0;
    public int backstabbersSoulFlatCrit;
    public bool backstabbersSoulActive = false;
    public float backstabbersSoulDamageMult;
    public int backstabbersSoulCritGain;
    public float backstabbersSoulCritDamageGain;
    public int backstabbersSoulMaxStacks = 25;
    public bool blitzSoulActive = false;
    public bool blitzToggledOn = true;
    public int blitzManaCost;
    public int blitzShockDamage;
    public float blitzShockDuration;
    public float blitzShockTickRate;
    public int blitzMaxStacks;
    public float blitzWeakAutoMult;
    public bool aStarCallerNovaActive = false;
    public float aStarCallerNovaDamageMult;
    public int aStarCallerNovaCinderChance;
    public int aStarCallerNovaEmberCost;
    public bool aBloodMoneyActive = false;
    public int aBloodMoneyHeal;
    public bool aProspectorActive = false;
    public int aProspectorChance;
    public bool barterSoulPaid = false;
    public bool barteredSoulActive;
    public int barteredSoulLevel;
    public GameObject emberGainPrefab;
    public float burningSoulMaxDamage;
    public bool burningSoulActive = false;
    public int burningSoulLevel;
    public bool blightedSoulActive = false;
    public bool cullTheMeekActive = false;
    public float cullTheMeekBonusDmg;
    public bool isRekindleActive;
    public int rekindleEmberPerKill;
    public DropManager dropManager;
    #region References
    public DropTableData scholarDropTable;
    public static PlayerCombat Instance;
    public float statusDurationMultiplier = 1;
    public GameObject bloodHealPrefab;
    public GameObject gameOverScreen;
    public int scholarXPAmount;
    public bool scholarActive = false;
    public PlayerData playerData;
    private PlayerMovement playerMovement;
    private AugmentManager augmentManager;
    public AttackManager attackManager;
    private Animator anim;
    private EmberSystem emberSystem;
    private GameObject closestEnemy;
    public event Action<Enemy> OnHitDealt;
    public event Action<Enemy> OnEnemyKill;
    private AbilityManager abilityManager;
    public float bloodlustRemaining;
    #endregion
    public int harvestChance;
    public int harvestHeal;
    public bool harvestActive = false;
    public bool aEmberWickActive = false;
    public float aEmberWickStatusExtend;

    public GameObject critSplosionPrefab;
    #region Stats
    // Attack
    public float attackBase = 1f;
    public float attackBonus = 0f;
    public float attack = 1f;            // current

    // Attack Speed
    public float attackSpeedBase = 1f;
    public float attackSpeedBonus = 0f;
    public float attackSpeed = 1f;       // current

    // Crit Chance
    public int critChanceBase = 5;
    public int critChanceBonus = 0;
    public int critChance = 5;           // current

    // Crit Damage
    public float critDamageBase = 1.5f;
    public float critDamageBonus = 0f;
    public float critDamage = 1.5f;      // current

    // Armor
    public float armorBase = 0f;
    public float armorBonus = 0f;
    public float armor = 0f;             // current

    // Dodge
    public float dodgeBase = 1f;
    public float dodgeBonus = 0f;
    public float dodge = 1f;             // current

    // Movement Speed
    public float movementSpeedBase = 16f;
    public float movementSpeedBonus = 0f;
    public float movementSpeed = 16f;     // current

    // XP Gain
    public float xpGainBase = 1f;
    public float xpGainBonus = 0f;
    public float xpGain = 1f;            // current

    // Gold Gain
    public float goldGainBase = 1f;
    public float goldGainBonus = 0f;
    public float goldGain = 1f;          // current

    public float statusResist = 1f;
    public float statusResistBonus = 0f;
    public float statusResistBase = 1f;

    //Consumable effectiveness
    public float consumableEffectiveness = 1f;
    public float consumableEffectivenessBonus = 0f;
    public float consumableEffectivenessBase = 1f;

    public int currentPlayerHealth;
    public float maxHealth = 100f;       // max health DURING delves
    public float baseMaxHealth = 100f;          // max health BETWEEN delves, after soul mix calcs.
    public float hpRegen;
    public float playerManaBase = 100;
    public float currentPlayerMana = 100f;
    public int soulCoins = 0;

    public List<string> playerStats = new List<string>();
    #endregion

    #region Soul Coin State
    private bool hpIsRegenning = false;
    public bool hpRegenActive = false;
    public bool rampingRegenActive = false;
    public float rampingRegenValue;
    private float rampingRegenCounter = 1;

    public bool isSovereignImmunityActive = false;
    public int sovereignImmunityCooldown;

    public bool bloodSoulBarrierActive = false;
    public int bloodSoulBarrierValue;

    public bool soulMixActive = false;
    public float soulMixPercent;
    public int soulMixCap;
    public int soulMixPreviousTotal;

    public bool isMADDoctrineActive = false;
    public int mADDoctrineCooldown;
    public float mADDoctrineReflectDamage;

    public int hindsightBiasReturnTime;
    public int hindsightBiasHealthReturn;
    public bool isHindsightBiasActive = false;

    public float hitandRunValue;
    public bool isHitandRunActive = false;
    public float hitandRunTime;
    private bool hitandRunBool = false;

    public bool isDynamicDensityActive = false;
    public bool dynamicDensity = false;

    public float evasiveManeuversValue;
    public bool isEvasiveManeuversActive = false;
    private bool isEvasiveClimbing = false;

    public bool isSurvivorshipBiasActive = false;
    public int survivorshipBiasXP;

    public int totalSiphonKills = 0;
    public int siphonCounter;
    public int soulSiphonLevel;

    public bool isFlowStateActive = false;
    public float flowStateDamage;
    private bool flowState = false;

    public bool isRunAndHitActive = false;
    public float runAndHitDamage;
    public float runAndHitCap;
    private int runAndHitCurrentStacks;

    public bool isSchrodingersCatActive = false;
    public int schrodingersCatCurrentLevel;

    public bool isClinicalTrialsActive = false;

    public bool isInsiderTradingActive = false;
    public int insiderTradingPercent;
    public int insiderTradingGoldAmount;

    public float packAPunchDamagePerItem;
    public bool packAPunchIsActive = false;

    public bool bitterPillIsActive = false;
    public float bitterPillDamage;
    public float bitterPillDuration;
    public bool bitterPill = false;
    public float bitterPillEndTime;

    public float achillesHeelChance;
    public float achillesHeelDamage;
    public bool achillesHeelIsActive = false;

    public bool doOrDieIsActive = false;
    public float doOrDieHpThreshold;
    public float doOrDieAS;
    public bool doOrDieActivated = false;

    public float bloodlustTime;
    public float bloodlustDamage;
    public bool bloodlustIsActive = false;

    public bool lightningStrikesTwiceActive = false;
    public float lightningStrikesTwiceCritDmgCap = .75f;
    public float lightningStrikesTwiceDmg = 0.02f;
    public int lightningStrikesTwiceStacks;
    private float lightningStrikesTwiceApplied = 0f;

    public bool strikeGoldActive = false;
    public int strikeGoldChance;
    public int strikeGoldAmount;

    public bool curtainCallActive = false;
    public float curtainCallExecute = .1f;

    public bool gamblersFallacyActive = false;
    public float gamblersFallacyPayout;

    public bool jackpotActive = true;
    public int jackpotChance;
    public int jackpotCritDamage = 30;
    public int jackpotGoldCost = 1;
    #endregion

    #region Progression & State
    public float playerMoney = 100;
    public bool iFrames = false;
    public int playerXp;
    public int playerLevel = 1;
    public int delveLevel = 0;
    public int augmentsOwed = 0;
    private string statueStatToMod;
    private bool abilityInUse = false;
    #endregion

    #region UI
    public TextMeshProUGUI playerGold;
    public Slider playerHpBar;
    public Slider playerManaBar;
    public Slider playerXpBar;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI playerHpBarNumber;
    public TextMeshProUGUI playerManaBarNumber;
    #endregion

    private bool gamePaused = false;
    #region Unity Lifecycle
    void Awake()
    {
        Instance = this;
        playerStats.Add("attack");
        playerStats.Add("attack speed");
        playerStats.Add("crit chance");
        playerStats.Add("crit damage");
        playerStats.Add("armor");
        playerStats.Add("max health");
        playerStats.Add("dodge");
        playerStats.Add("movement speed");
        playerStats.Add("xp gain");
        playerStats.Add("gold gain");
        playerStats.Add("status resist");
        playerStats.Add("consumable effectiveness");
    }
    public void ResetStatBonuses()
    {
        lightningStrikesTwiceApplied = 0f;
        attackBonus = 0f; attack = attackBase;
        attackSpeedBonus = 0f; attackSpeed = attackSpeedBase;
        critChanceBonus = 0; critChance = critChanceBase;
        critDamageBonus = 0f; critDamage = critDamageBase;
        armorBonus = 0f; armor = armorBase;
        dodgeBonus = 0f; dodge = dodgeBase;
        movementSpeedBonus = 0f; movementSpeed = movementSpeedBase;
        xpGainBonus = 0f; xpGain = xpGainBase;
        goldGainBonus = 0f; goldGain = goldGainBase;
        statusResistBonus = 0f; statusResist = statusResistBase;
        consumableEffectivenessBonus = 0f; consumableEffectiveness = consumableEffectivenessBase;

        maxHealth = baseMaxHealth;        // no bonus field — reset to base directly
    }
    void Start()
    {
        soulMixPreviousTotal = PlayerPrefs.GetInt("SoulMixTotal");
        abilityManager = FindFirstObjectByType<AbilityManager>();
        if (playerGold != null) playerGold.text = ": " + (int)playerMoney;

        anim = GetComponent<Animator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        augmentManager = FindFirstObjectByType<AugmentManager>();
        attackManager = FindFirstObjectByType<AttackManager>();
        emberSystem = FindFirstObjectByType<EmberSystem>();

        baseMaxHealth += soulMixPreviousTotal;
        currentPlayerHealth = (int)maxHealth;
        currentPlayerMana = playerManaBase;
        if (playerHpBarNumber != null) playerHpBarNumber.text = currentPlayerHealth + " / " + (int)maxHealth;
        if (playerHpBarNumber != null) playerManaBarNumber.text = currentPlayerMana + " / " + playerManaBase;
        if (playerManaBar != null) playerManaBar.value = 1.0f;
        if (playerHpBar != null) playerHpBar.value = 1.0f;

    }

    private void Update()
    {
        if (bloodlustIsActive)
        {
            if (bloodlustRemaining > 0)
            {
                bloodlustRemaining -= Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            Time.timeScale = gamePaused ? 0f : 1f;
            pausePanel.SetActive(gamePaused);
            if (gamePaused) RefreshStats();
        }
        if (blitzSoulActive && Input.GetKeyDown(KeyCode.G))
        {
            BlitzSoulToggle();
        }
        if (emberSystem != null && emberSystem.aliveEnemies > 0 && hpRegenActive && !hpIsRegenning && currentPlayerHealth < maxHealth)
        {
            StartCoroutine("HpRegen");
        }
        if (emberSystem != null && emberSystem.aliveEnemies > 0 && isEvasiveManeuversActive && !isEvasiveClimbing && dodge < 101)
        {
            isEvasiveClimbing = true;
            StartCoroutine(EvasiveManeuvers());
        }
        if (emberSystem != null && emberSystem.aliveEnemies <= 0 && isRunAndHitActive)
        {
            RunAndHit(true);
        }
        if (playerManaBar != null) playerManaBar.value = currentPlayerMana / playerManaBase;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            FightNodeIndicator fightNodeIndicator;
            fightNodeIndicator = FindFirstObjectByType<FightNodeIndicator>();
            transform.position = fightNodeIndicator.currentActiveFightNodeCoords;
        }
        if (isDynamicDensityActive)
        {
            UpdateDynamicDensity();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Money1"))
        {

        }
    }
    #endregion

    #region Stat/gold Recalculation
    public void ModifyStat(string stat, float statmod)
    {
        switch (stat)
        {
            case "attack":
                attackBonus += (attackBase * statmod);
                attack = attackBase + attackBonus;
                break;
            case "attack speed":
                attackSpeedBonus += (attackSpeedBase * statmod);
                attackSpeed = attackSpeedBase + attackSpeedBonus;
                break;
            case "crit chance":
                critChanceBonus += (int)(statmod*100);
                critChance = Mathf.Clamp((critChanceBase + critChanceBonus),1,100);
                break;
            case "crit damage":
                critDamageBonus += (critDamageBase * statmod);
                critDamage = Mathf.Max(critDamageBase + critDamageBonus, 1f);
                break;
            case "armor":
                armorBonus += statmod;
                armor = armorBase + armorBonus;
                break;
            case "dodge":
                dodgeBonus += (dodgeBase * statmod);
                dodge = dodgeBase + dodgeBonus;
                break;
            case "movement speed":
                movementSpeedBonus += (movementSpeedBase * statmod);
                movementSpeed = movementSpeedBase + movementSpeedBonus;
                break;
            case "xp gain":
                xpGainBonus += (xpGainBase * statmod);
                xpGain = xpGainBase + xpGainBonus;
                break;
            case "gold gain":
                goldGainBonus += (goldGainBase * statmod);
                goldGain = goldGainBase + goldGainBonus;
                break;
            case "max health":
                int healthChange = (int)(maxHealth * statmod);
                maxHealth += (maxHealth*statmod);
                RefreshHpBar(healthChange, "max");
                if (currentPlayerHealth > maxHealth)
                {
                    currentPlayerHealth = (int)maxHealth;
                }
                break;
            case "consumable effectiveness":
                consumableEffectivenessBonus += (consumableEffectivenessBase * statmod);
                consumableEffectiveness = consumableEffectivenessBase + consumableEffectivenessBonus;
                break;
        }
        
    }
    public bool InsiderTrading()
    {
        int rng = UnityEngine.Random.Range(1, 101);
        if (rng <= insiderTradingPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SchrodingersCat()
    {
        int rng = UnityEngine.Random.Range(1, 101);
        if (rng <= schrodingersCatCurrentLevel)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public void FlowState()
    {
        ModifyStat("attack", flowStateDamage);
    }

    public void SurvivorshipBias(int xpAmount)
    {
        AddXp(xpAmount);
    }
    IEnumerator EvasiveManeuvers()
    {
        while (dodge < 100)
        {
            yield return new WaitForSeconds(3f);
            dodge = Mathf.Min(dodge + evasiveManeuversValue, 100);
        }
        isEvasiveClimbing = false;   
    }
    public void ModifyGoldValue(string transactionType,float goldValue)
    {
        if (transactionType == "shop")
        {
            playerMoney -= goldValue;
            moneyText.text = ": " + (int)playerMoney;
        }
        if (transactionType == "pickup")
        {
            if (isSchrodingersCatActive && SchrodingersCat() == true)
            {
                goldValue *= 2;
            }
            playerMoney += (goldValue * goldGain);
            moneyText.text = ": " + (int)playerMoney;
        }
    }

    private void UpdateDynamicDensity()
    {
        if (currentPlayerHealth / maxHealth >= .75f && !dynamicDensity)
        {
            armor += 25;
            dynamicDensity = true;
        }
        else if (currentPlayerHealth / maxHealth < .75f && dynamicDensity)
        {
            armor -= 25;
            dynamicDensity = false;
        }
    }

    IEnumerator HitAndRun()
    {
        if (!hitandRunBool)
        {
            float hitandRunMS = movementSpeed * hitandRunValue;
            movementSpeed += hitandRunMS;
            hitandRunBool = true;
            yield return new WaitForSeconds(hitandRunTime);
            hitandRunBool = false;
            ModifyStat("movement speed", 0);
        }
    }

    public IEnumerator SovereignImmunity()
    {
        isSovereignImmunityActive = false;
        yield return new WaitForSeconds(sovereignImmunityCooldown);
        isSovereignImmunityActive = true;
    }
    #endregion
    #region Combat - Damage Dealing

    public void DealDamage(Enemy target, float baseDamage, int hitCount = 1, bool maxCharge = false)
    {
        if (aOverchargeActive && chargeAttackFired)
        {
            target.GetComponent<EnemyStatusEffects>().ESEShock(8f, 3, 3f, 2);
            
        }
        if (blitzSoulActive)
        {
            if (currentPlayerMana >= blitzManaCost && blitzToggledOn)
            {
                currentPlayerMana -= blitzManaCost;
                target.GetComponent<EnemyStatusEffects>().ESEShock(blitzShockDuration, blitzShockDamage, blitzShockTickRate, blitzMaxStacks);
            }
            else
            {
                baseDamage *= blitzWeakAutoMult;
            }
        }

        int dmg = CalcWeaponDamage(baseDamage, out bool crit);
        target.reduceHp(dmg, hitCount, crit);
        OnHitDealt?.Invoke(target);
    }
    public void BlitzSoulToggle()
    {
        blitzToggledOn = !blitzToggledOn;
    }
    public IEnumerator PlayerTempSpeedBoost(float speedBoost, float time)
    {
        ModifyStat("movement speed", speedBoost);
        yield return new WaitForSeconds(time);
        ModifyStat("movement speed", -speedBoost);
    }
    public int CalcWeaponDamage(float damage, out bool crit)
    {
        int critRoll = UnityEngine.Random.Range(0, 101);
        int processedDamage = 0;

        int effectiveCritChance = critChance;
        float effectiveCritDamage = critDamage;

        if (backstabbersSoulActive)
        {
            if (Time.time - lastHitTime > 2f)
            {
                currentBackstabbersStacks = 0;
            }

            damage *= backstabbersSoulDamageMult;

            effectiveCritChance += backstabbersSoulFlatCrit + (currentBackstabbersStacks * backstabbersSoulCritGain);
            effectiveCritDamage += currentBackstabbersStacks * backstabbersSoulCritDamageGain;
            effectiveCritChance = Mathf.Min(effectiveCritChance, 100);
        }
        if (cullTheMeekActive && emberSystem.aliveEnemies < 5)
        {
            damage *= cullTheMeekBonusDmg;
        }
        if (burningSoulActive)
        {
            float emberPercent = emberSystem.emberAmount / emberSystem.baseEmber;
            damage *= Mathf.LerpUnclamped(0.5f, burningSoulMaxDamage, emberPercent);
        }

        if (barteredSoulActive)
        {
            damage *= 0.25f + (playerMoney / (4 - barteredSoulLevel)) * 0.01f;
        }
        if (packAPunchIsActive)
        {
            damage *= 1 + Mathf.Min(.35f,(packAPunchDamagePerItem * ItemHotbar.Instance.NumberOfItems()));
        }
        if (bloodlustIsActive && bloodlustRemaining > 0)
        {
            damage *= 1 + bloodlustDamage;
        }
        if (bitterPill)
        {
            damage *= (1 + bitterPillDamage);
        }
        if (isFlowStateActive && !flowState && (abilityManager.shadowEchoActive || abilityManager.timeDilation || playerMovement.isDashing))
        {
            FlowState();
            flowState = true;
        }
        else if (isFlowStateActive && flowState && (!abilityManager.shadowEchoActive && !abilityManager.timeDilation && !playerMovement.isDashing))
        {
            flowState = false;
            ModifyStat("attack", -flowStateDamage);
        }
        if (isRunAndHitActive) RunAndHit(false);
        if (achillesHeelIsActive && UnityEngine.Random.Range(1, 101) <= achillesHeelChance)
        {
            damage *= achillesHeelDamage;
        }
        if (critRoll < effectiveCritChance)
        {
            float critMult = effectiveCritDamage;
            if (gamblersFallacyActive)
            {
                ModifyGoldValue("pickup", gamblersFallacyPayout);
            }
            if (jackpotActive && playerMoney >= jackpotGoldCost && UnityEngine.Random.Range(1, 1001) < jackpotChance)
            {
                critDamageBonus -= lightningStrikesTwiceApplied;
                lightningStrikesTwiceApplied = 0f;
                critDamage = Mathf.Max(critDamageBase + critDamageBonus, 1f);
                lightningStrikesTwiceStacks = 0;
            }
            else if (lightningStrikesTwiceActive)
            {
                lightningStrikesTwiceStacks++;
                float target = Mathf.Min(lightningStrikesTwiceCritDmgCap, lightningStrikesTwiceStacks * lightningStrikesTwiceDmg);
                critMult = effectiveCritDamage + (target - lightningStrikesTwiceApplied);
                critDamageBonus += target - lightningStrikesTwiceApplied;
                lightningStrikesTwiceApplied = target;
                critDamage = Mathf.Max(critDamageBase + critDamageBonus, 1f);
            }
            else
            {
                critMult = effectiveCritDamage;
            }

            processedDamage = (int)Mathf.Round(damage * attack * critMult);

            if (strikeGoldActive && UnityEngine.Random.Range(1, 1001) <= strikeGoldChance)
                StrikeGold();

            crit = true;
        }
        else
        {
            lightningStrikesTwiceStacks = 0;
            crit = false;
            processedDamage = (int)(Mathf.Round(damage * attack));
        }
        if (blightedSoulActive)
        {
            processedDamage = (int)(processedDamage*.6f);
        }
        if (emberSystem != null && emberSystem.emberAmount <= 0)
            processedDamage = (int)(processedDamage * 0.85f);
        return processedDamage;
    }
    private void StrikeGold()
    {
        ModifyGoldValue("pickup",strikeGoldAmount);
    }
    public void TriggerBitterPill()
    {
        bitterPillEndTime = Time.time + bitterPillDuration;
        if (!bitterPill) StartCoroutine(BitterPill());
    }

    public IEnumerator BitterPill()
    {
        bitterPill = true;
        while (Time.time < bitterPillEndTime)
            yield return null;
        bitterPill = false;
    }

    private void RunAndHit(bool wasHitOrOOC)
    {
        if (!wasHitOrOOC)
        {
            if (runAndHitCurrentStacks*runAndHitDamage < runAndHitCap)
            {
                runAndHitCurrentStacks++;
                ModifyStat("attack", runAndHitDamage);
            }
        }
        else if (wasHitOrOOC) 
        {
            ModifyStat("attack", -(runAndHitCurrentStacks*runAndHitDamage));//what we actually need to put here is the current run and hit value
            runAndHitCurrentStacks = 0;
        }
    }

    public void MADDoctrine(int reflectDamage)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        isMADDoctrineActive = false;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = dist;
            }
        }

        if (closestEnemy != null)
        {
            closestEnemy.GetComponent<Enemy>().reduceHp(reflectDamage*mADDoctrineReflectDamage,1);
            StartCoroutine("MADDoctrineCD");
        }
            
    }

    IEnumerator MADDoctrineCD()
    {
        yield return new WaitForSeconds(mADDoctrineCooldown);
        isMADDoctrineActive = true;
    }

    IEnumerator HindsightBias(int health, int returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        HealPlayer(health);
    }
    #endregion

    #region Combat - Damage Taken
    public void DamagePlayer(float damageTaken)
    {
        if (iFrames) return;
        
        int dodgeChance = UnityEngine.Random.Range(0, 101);
        if (dodgeChance <= dodge)
        {
            dodge = dodgeBase + dodgeBonus;
            if (isSurvivorshipBiasActive) SurvivorshipBias(survivorshipBiasXP);
            return;
        }
        currentBackstabbersStacks = 0;
        if (isRunAndHitActive) RunAndHit(true);
        int damageTakenInt = (int)Mathf.Round(damageTaken);
        if (emberSystem != null && emberSystem.emberAmount <= 0)
            damageTakenInt = (int)(damageTakenInt * 1.15f);
        StartCoroutine("IFrames");
        float reduction = armor / (armor + 100f);
        int finalDamage = (int)Mathf.Round(damageTakenInt * (1f - reduction));
        currentPlayerHealth -= finalDamage;
        playerHpBar.value = currentPlayerHealth / maxHealth;
        playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
        if (currentPlayerHealth <= 0)
        {
            anim.SetTrigger("Death");
            GameOver();
            return;
        }
        if (isHindsightBiasActive) StartCoroutine(HindsightBias(hindsightBiasHealthReturn, hindsightBiasReturnTime));
        if (isMADDoctrineActive)
        {
            MADDoctrine(finalDamage);
        }
        if (isHitandRunActive)
        {
            StartCoroutine("HitAndRun");
        }
        if (doOrDieIsActive) DoOrDieToggle();
    }
    public void EnvironmentDamagePlayer(float damageTaken)
    {
        if (iFrames) return;
        int damageTakenInt = (int)Mathf.Round(damageTaken);
        currentPlayerHealth -= damageTakenInt;
        playerHpBar.value = currentPlayerHealth / maxHealth;
        playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
        if (currentPlayerHealth <= 0)
        {
            anim.SetTrigger("Death");
            GameOver();
            return;
        }
    }
    public void RefreshStats()
    {
        statsText.text =
        $"Attack        {attackBase:F2} + {attackBonus:F2} = {attack:F2}\n" +
        $"Attack Speed  {attackSpeedBase:F2} + {attackSpeedBonus:F2} = {attackSpeed:F2}\n" +
        $"Crit Chance   {critChanceBase} + {critChanceBonus} = {critChance}\n" +
        $"Crit Damage   {critDamageBase:F2} + {critDamageBonus:F2} = {critDamage:F2}\n" +
        $"Armor         {armorBase:F2} + {armorBonus:F2} = {armor:F2}\n" +
        $"Dodge         {dodgeBase:F2} + {dodgeBonus:F2} = {dodge:F2}\n" +
        $"Move Speed    {movementSpeedBase:F2} + {movementSpeedBonus:F2} = {movementSpeed:F2}\n" +
        $"XP Gain       {xpGainBase:F2} + {xpGainBonus:F2} = {xpGain:F2}\n" +
        $"Gold Gain     {goldGainBase:F2} + {goldGainBonus:F2} = {goldGain:F2}\n" +
        $"Status Resist {statusResistBase:F2} + {statusResistBonus:F2} = {statusResist:F2}\n" +
        $"Consumable    {consumableEffectivenessBase:F2} + {consumableEffectivenessBonus:F2} = {consumableEffectiveness:F2}\n" +
        $"Max Health    {baseMaxHealth:F0} → {maxHealth:F0}\n" +
        $"Soul Mix Bank {soulMixPreviousTotal}";
    }
    IEnumerator IFrames()
    {
        iFrames = true;
        StartCoroutine("IFrameAnimation");
        yield return new WaitForSeconds(.66f);
        iFrames = false;
    }

    IEnumerator IFrameAnimation()
    {
        anim.SetTrigger("Hurt");
        return null;
    }
    #endregion

    #region Healing & Regen
    public void BloodHeal(int damageHealed)
    {
        Instantiate(bloodHealPrefab, transform.position, Quaternion.identity);
        HealPlayer(damageHealed);
    }
    public void RefreshHpBar(int healthRegained, string currentOrMaxHealth)
    {
        playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
    }
    public void HealPlayer(float damageHealed)
    {
        if (currentPlayerHealth > 0 && currentPlayerHealth < (int)maxHealth)
        {
            int damageHealedInt = (int)Mathf.Round(damageHealed);
            currentPlayerHealth = Mathf.Min(currentPlayerHealth + damageHealedInt, (int)maxHealth);
            playerHpBar.value = currentPlayerHealth / maxHealth;
            playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
            if (doOrDieIsActive) DoOrDieToggle();
        }
    }

    public void AddManaPlayer(float manaRestored)
    {
        if (currentPlayerMana < (int)playerManaBase)
        {
            int manaRegainedInt = (int)Mathf.Round(manaRestored);
            currentPlayerMana = Mathf.Min(currentPlayerMana + manaRegainedInt, (int)playerManaBase);
            playerManaBar.value = currentPlayerMana / playerManaBase;
            playerManaBarNumber.text = currentPlayerMana + " / " + playerManaBase;
        }
    }
    public void DoOrDieToggle()
    {
        if (currentPlayerHealth/maxHealth <= doOrDieHpThreshold && !doOrDieActivated)
        {
            ModifyStat("attack speed", -doOrDieAS);
            doOrDieActivated = true;
        }
        if (currentPlayerHealth / maxHealth >= doOrDieHpThreshold && doOrDieActivated)
        {
            ModifyStat("attack speed", doOrDieAS);
            doOrDieActivated = false;
        }
    }
    IEnumerator HpRegen()
    {
        if (currentPlayerHealth >= maxHealth)
        {
            hpIsRegenning = false;
            rampingRegenCounter = 1;
            yield break;
        }
        hpIsRegenning = true;
        if (rampingRegenActive)
        {
            int healAmount = (int)hpRegen + (int)(rampingRegenValue * Mathf.Sqrt(rampingRegenCounter) / 2f);
            healAmount = Mathf.Min(healAmount, 14);
            currentPlayerHealth = Mathf.Min(currentPlayerHealth + healAmount, (int)maxHealth);
            RefreshHPUI();
        }
        else if (!rampingRegenActive)
        {
            currentPlayerHealth = Mathf.Min(currentPlayerHealth + (int)hpRegen, (int)maxHealth);
            RefreshHPUI();
        }
        yield return new WaitForSeconds(5f);
        rampingRegenCounter++;
        if (emberSystem.aliveEnemies > 0 && hpRegenActive)
        {
            StartCoroutine(HpRegen());
        }
        else
        {
            hpIsRegenning = false;
            rampingRegenCounter = 1;
        }
    }

    public void RefreshHPUI()
    {
        playerHpBar.value = currentPlayerHealth / (int)maxHealth;
        playerHpBarNumber.text = currentPlayerHealth + " / " + (int)maxHealth;
    }
    #endregion

    #region Soul Coins - On Kill
    public void OnEnemyKilled(Enemy enemy)
    {
        OnEnemyKill?.Invoke(enemy);
        if (enemy.GetComponent<EnemyStatusEffects>().activeStatusEffects.Count > 0)
        {
            if (scholarActive)
            {
                for (int i = 0; i < scholarXPAmount; i++)
                {
                    DropManager.Instance.RollDropTable(scholarDropTable, enemy.transform.position);
                }
            }
            if (isRekindleActive)
            {
                emberSystem.AddEmber(10 * rekindleEmberPerKill);
            }
        }
        if (bloodSoulBarrierActive && currentPlayerHealth < (int)maxHealth)
        {
            BloodHeal(bloodSoulBarrierValue);
        }
        if (soulSiphonLevel > 0)
        {
            if (siphonCounter < 3)
            {
                siphonCounter++;
            }
            if (siphonCounter >= 3)
            {
                siphonCounter = 0;
                totalSiphonKills++;
                maxHealth++;
                currentPlayerHealth++;
                Debug.Log("health should be added, soul siphon level: " + soulSiphonLevel);
                playerHpBarNumber.text = currentPlayerHealth + " / " + maxHealth;
            }
        }
    }
    #endregion

    #region Experience & Leveling
    public void AddXp(int xpValue)
    {
        Debug.Log("addxp " + xpValue);
        if (isSchrodingersCatActive && SchrodingersCat() == true)
        {
            xpValue *= 2;
        }
        xpValue = (int)(Mathf.Floor(xpValue*xpGain));
        playerXp += xpValue;
        playerXpBar.value = playerXp / 100f;

        while (playerXp >= 100)
        {
            playerLevel++;
            playerXp -= 100;
            augmentsOwed++;
            playerXpBar.value = playerXp / 100f;
            playerLevelText.text = ("" + playerLevel);
        }

        if (augmentsOwed > 0 && Time.timeScale != 0)
        {
            augmentManager.AugmentSelectionStart();
        }
    }
    #endregion



    #region Statues
    public void OfferingStatueMods(string statToMod, int goldOffered)
    {
        ModifyGoldValue("shop", goldOffered);
        switch (statToMod)
        {
            case "attack":
                ModifyStat("attack", .1f);
                break;
            case "attack speed":
                ModifyStat("attack speed", -.1f);
                break;
            case "crit chance":
                ModifyStat("crit chance", .1f);
                break;
            case "crit damage":
                ModifyStat("crit damage", .1f);
                break;
            case "armor":
                ModifyStat("armor", 10f);
                break;
            case "max health":
                ModifyStat("max health", .1f);
                break;
            case "dodge":
                ModifyStat("dodge", .1f);
                break;
            case "movement speed":
                ModifyStat("movement speed", .1f);
                break;
            case "xp gain":
                ModifyStat("xp gain", .1f);
                break;
            case "gold gain":
                ModifyStat("gold gain", .1f);
                break;
        }
    }

    public void CurseStatueMods(string blessedStat, string cursedStat)
    {
        switch (blessedStat)
        {
            case "attack":
                ModifyStat("attack", .04f);
                break;
            case "attack speed":
                ModifyStat("attack speed", -0.04f);
                break;
            case "crit chance":
                ModifyStat("crit chance", .04f);
                break;
            case "crit damage":
                ModifyStat("crit damage", .04f);
                break;
            case "armor":
                ModifyStat("armor", 8f);
                break;
            case "max health":
                ModifyStat("max health", .04f);
                break;
            case "dodge":
                ModifyStat("dodge", .04f);
                break;
            case "movement speed":
                ModifyStat("movement speed", .04f);
                break;
            case "xp gain":
                ModifyStat("xp gain", .02f);
                break;
            case "gold gain":
                ModifyStat("gold gain", .04f);
                break;
        }
        switch (cursedStat)
        {
            case "attack":
                ModifyStat("attack", -.04f);
                break;
            case "attack speed":
                ModifyStat("attack speed", 0.96f);
                break;
            case "crit chance":
                ModifyStat("crit chance", -.04f);
                break;
            case "crit damage":
                ModifyStat("crit damage", -.04f);
                break;
            case "armor":
                ModifyStat("armor", -8f);
                break;
            case "max health":
                ModifyStat("max health", -.1f);
                break;
            case "dodge":
                ModifyStat("dodge", -.04f);
                break;
            case "movement speed":
                ModifyStat("movement speed", -.04f);
                break;
            case "xp gain":
                ModifyStat("xp gain", -.02f);
                break;
            case "gold gain":
                ModifyStat("gold gain", -.04f);
                break;
        }
    }

    public void StatueStatMods(string statToMod, float statMod)
    {
        switch (statToMod)
        {
            case "attack":
                ModifyStat("attack", (statMod>0) ? .07f : -.07f);
                break;
            case "attack speed":
                ModifyStat("attack speed", (statMod > 0) ? -0.07f : .07f);
                break;
            case "crit chance":
                ModifyStat("crit chance", (statMod > 0) ? .07f : -.07f);
                break;
            case "crit damage":
                ModifyStat("crit damage",(statMod > 0) ? .07f : -.07f);
                break;
            case "armor":
                ModifyStat("armor", (statMod > 0) ? 7f : -7f);
                break;
            case "max health":
                ModifyStat("max health", (statMod > 0) ? .07f : -.07f);
                break;
            case "dodge":
                ModifyStat("dodge", (statMod > 0) ? .04f : -.04f);
                break;
            case "movement speed":
                ModifyStat("movement speed", (statMod > 0) ? .05f : -.05f);
                break;
            case "xp gain":
                ModifyStat("xp gain", (statMod > 0) ? .04f : -.04f);
                break;
            case "gold gain":
                ModifyStat("gold gain", (statMod > 0) ? .05f : -.05f);
                break;
        }
    }
    #endregion

    #region Scene / Game Flow
    public void GameOver()
    {
        PlayerPrefs.DeleteKey("Gold");
        PlayerPrefs.DeleteKey("Exp");
        PlayerPrefs.DeleteKey("DelveLevel");
        PlayerPrefs.DeleteKey("StartWave");
        PlayerPrefs.DeleteKey("CurrentAbility");
        PlayerPrefs.DeleteKey("SoulMixTotal");
        PlayerPrefs.Save();
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("TradingHub");
        Time.timeScale = 1;
    }
    #endregion
}