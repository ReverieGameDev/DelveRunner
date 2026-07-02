using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerCombat : MonoBehaviour
{
    #region References
    public static PlayerCombat Instance;

    public GameObject bloodHealPrefab;
    public GameObject gameOverScreen;

    public PlayerData playerData;
    private PlayerMovement playerMovement;
    private AugmentManager augmentManager;
    public AttackManager attackManager;
    private Animator anim;
    private EmberSystem emberSystem;
    private GameObject closestEnemy;
    private AbilityManager abilityManager;
    #endregion

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

    public int totalSiphonKills;
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

    void Start()
    {
        abilityManager = FindFirstObjectByType<AbilityManager>();
        if (playerGold != null) playerGold.text = ": " + (int)playerMoney;

        anim = GetComponent<Animator>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        augmentManager = FindFirstObjectByType<AugmentManager>();
        attackManager = FindFirstObjectByType<AttackManager>();
        emberSystem = FindFirstObjectByType<EmberSystem>();

        currentPlayerHealth = (int)maxHealth;
        currentPlayerMana = playerManaBase;
        playerHpBarNumber.text = currentPlayerHealth + " / " + (int)maxHealth;
        playerManaBarNumber.text = currentPlayerMana + " / " + playerManaBase;
        if (playerManaBar != null) playerManaBar.value = 1.0f;
        if (playerHpBar != null) playerHpBar.value = 1.0f;

    }

    private void Update()
    {
        if (emberSystem.aliveEnemies > 0 && hpRegenActive && !hpIsRegenning && currentPlayerHealth < maxHealth)
        {
            StartCoroutine("HpRegen");
        }
        if (emberSystem.aliveEnemies <= 0 && isRunAndHitActive)
        {
            RunAndHit(true);
        }
        if (playerManaBar != null) playerManaBar.value = currentPlayerMana / playerManaBase;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position = FindFirstObjectByType<EnemySpawnDetector>().transform.position;
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
        int rng = Random.Range(1, 101);
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
        int rng = Random.Range(1, 101);
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
        isEvasiveClimbing = true;
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
    public int CalcWeaponDamage(float damage, out bool crit)
    {
        int critRoll = Random.Range(0, 101);
        int processedDamage = 0;

        if (packAPunchIsActive)
        {
            damage *= 1 + Mathf.Min(.35f,(packAPunchDamagePerItem * ItemHotbar.Instance.NumberOfItems()));
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
        if (achillesHeelIsActive && Random.Range(1, 101) < achillesHeelChance)
        {
            damage *= achillesHeelDamage;
        }
        if (critRoll < critChance)
        {
            if (lightningStrikesTwiceActive)
            {
                lightningStrikesTwiceStacks++;
                processedDamage = (int)(Mathf.Round(damage * attack * (critDamage+(Mathf.Min(lightningStrikesTwiceCritDmgCap,lightningStrikesTwiceStacks*lightningStrikesTwiceDmg)))));
            }
            else if (!lightningStrikesTwiceActive)
            {
                processedDamage = (int)(Mathf.Round(damage * attack * critDamage));
            }
            
            crit = true;
        }
        else
        {
            lightningStrikesTwiceStacks = 0;
            crit = false;
            processedDamage = (int)(Mathf.Round(damage * attack));
        }
        if (emberSystem != null && emberSystem.emberAmount <= 0)
            processedDamage = (int)(processedDamage * 0.85f);
        return processedDamage;
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
            closestEnemy.GetComponent<Enemy>().reduceHp(reflectDamage*mADDoctrineReflectDamage);
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
        int dodgeChance = Random.Range(0, 101);
        if (dodgeChance <= dodge)
        {
            dodge = dodgeBase + dodgeBonus;
            if (isEvasiveManeuversActive && !isEvasiveClimbing) StartCoroutine(EvasiveManeuvers());
            if (isSurvivorshipBiasActive) SurvivorshipBias(survivorshipBiasXP);
            return;
        }
        if (isRunAndHitActive) RunAndHit(true);
        int damageTakenInt = (int)Mathf.Round(damageTaken);
        if (emberSystem != null && emberSystem.emberAmount <= 0)
            damageTakenInt = (int)(damageTakenInt * 1.15f);
        if (iFrames) return;
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
            currentPlayerHealth += healAmount;
        }
        else if (!rampingRegenActive)
        {
            currentPlayerHealth += (int)hpRegen;
        }
        yield return new WaitForSeconds(5f);
        rampingRegenCounter++;
        if (emberSystem.aliveEnemies > 0 && hpRegenActive)
        {
            StartCoroutine("HpRegen");
        }
        else if (emberSystem.aliveEnemies <= 0)
        {
            hpIsRegenning = false;
            rampingRegenCounter = 1;
        }
    }
    #endregion

    #region Soul Coins - On Kill
    public void OnEnemyKilled()
    {
        if (bloodSoulBarrierActive && currentPlayerHealth < (int)maxHealth)
        {
            HealPlayer(bloodSoulBarrierValue);
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
            Time.timeScale = 0;
            augmentManager.RandomAugmentGenerator();
        }
    }
    #endregion

    #region Augments
    public void ApplyAugment(string selectedAugment)
    {
        if (selectedAugment == "Attack")
            attack *= 1.05f;
        else if (selectedAugment == "AttackSpeed")
            attackSpeed *= 0.95f;
        else if (selectedAugment == "CritChance")
            critChance += 10;
        else if (selectedAugment == "CritDamage")
            critDamage *= 1.10f;
        else if (selectedAugment == "Armor")
            armor += 10f;
        else if (selectedAugment == "MaxHealth")
            maxHealth *= 1.05f;
        else if (selectedAugment == "Dodge")
            dodge += 4f;
        else if (selectedAugment == "MovementSpeed")
            movementSpeed *= 1.05f;
        else if (selectedAugment == "XPGain")
            xpGain *= 1.05f;
        else if (selectedAugment == "GoldGain")
            goldGain *= 1.10f;
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
        PlayerPrefs.DeleteAll();
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