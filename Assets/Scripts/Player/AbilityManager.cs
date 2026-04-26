using System.Collections;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    // Ability State
    public string currentAbility = "Dash";
    private PlayerMovement playerMovement;
    private bool initialManaPaid = false;
    public float currentTimeScale = 1f;

    // Dash
    public float dashManaCost = 10f;
    public float dashDuration = 0.5f;
    private bool canDash = true;

    // Time Dilation
    public float timeDilationActivationCost = 20f;
    public float timeDilationDrainCost = 5f;
    public float timeDilationTickRate = 1f;
    public float timeDilationInitialCD = 0.25f;
    public float timeDilationRecastCD = 1f;
    public float timeDilationTimeScale = 0.66f;
    public GameObject timeDilationPrefab;
    public bool timeDilation = false;
    private bool canTimeDilation = true;

    // Shadow Echo
    public float shadowEchoActivationCost = 15f;
    public float shadowEchoDrainCost = 3f;
    public float shadowEchoTickRate = 1f;
    public float shadowEchoInitialCD = 0.25f;
    public float shadowEchoRecastCD = 1f;
    public GameObject shadowEchoPrefab;
    public bool shadowEchoActive = false;
    private bool canShadowEcho = true;
    public float updatedTimeScale;
    private PlayerStatusEffects playerStatusEffects;
    void Start()
    {
        playerStatusEffects = FindFirstObjectByType<PlayerStatusEffects>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        SwitchAbility(PlayerPrefs.GetString("CurrentAbility", currentAbility));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !playerMovement.playerFrozen && !playerStatusEffects.isStunned)
        {
            switch (currentAbility)
            {
                case "Dash":
                    AbilityDash();
                    break;
                case "Shadow Echo":
                    AbilityShadowEcho();
                    break;
                case "Time Dilation":
                    AbilityTimeDilation();
                    break;
            }
        }
    }

    public void SwitchAbility(string abilityToSwitchTo)
    {
        currentAbility = abilityToSwitchTo;
    }

    public void AbilityDash()
    {
        if (!playerMovement.isDashing && PlayerCombat.Instance.currentPlayerMana >= dashManaCost && canDash)
        {
            canDash = false;
            playerMovement.isDashing = true;
            playerMovement.dashDirection = new Vector2(playerMovement.xInput, playerMovement.yInput).normalized;
            PlayerCombat.Instance.GetComponent<Animator>().SetTrigger("Dash");
            StartCoroutine(ManaConsume(dashManaCost));
            StartCoroutine(Cooldown(dashDuration));
        }
    }

    public void AbilityShadowEcho()
    {
        if (!canShadowEcho) return;
        shadowEchoActive = !shadowEchoActive;
        if (shadowEchoActive && PlayerCombat.Instance.currentPlayerMana >= shadowEchoActivationCost && canShadowEcho)
        {
            canShadowEcho = false;
            shadowEchoPrefab.SetActive(true);
            StartCoroutine(InitialCooldown(shadowEchoInitialCD));
            StartCoroutine(ManaDrainAbility(shadowEchoActivationCost, shadowEchoDrainCost, shadowEchoTickRate));
        }
        else
        {
            shadowEchoActive = false;
            shadowEchoPrefab.SetActive(false);
            initialManaPaid = false;
            StopAllCoroutines();
            StartCoroutine(Cooldown(shadowEchoRecastCD));
        }
    }

    public void AbilityTimeDilation()
    {
        if (!canTimeDilation) return;
        timeDilation = !timeDilation;
        if (timeDilation && PlayerCombat.Instance.currentPlayerMana >= timeDilationActivationCost && canTimeDilation)
        {
            canTimeDilation = false;
            timeDilationPrefab.SetActive(true);
            Time.timeScale = timeDilationTimeScale;
            currentTimeScale = timeDilationTimeScale;
            StartCoroutine(InitialCooldown(timeDilationInitialCD));
            StartCoroutine(ManaDrainAbility(timeDilationActivationCost, timeDilationDrainCost, timeDilationTickRate));
        }
        else
        {
            timeDilation = false;
            timeDilationPrefab.SetActive(false);
            Time.timeScale = 1;
            initialManaPaid = false;
            currentTimeScale = 1f;
            StopAllCoroutines();
            StartCoroutine(Cooldown(timeDilationRecastCD));
        }
    }

    IEnumerator ManaDrainAbility(float activateManaCost, float persistManaCost, float manaTickRate)
    {
        if (!initialManaPaid)
        {
            PlayerCombat.Instance.currentPlayerMana -= activateManaCost;
            initialManaPaid = true;
        }
        yield return new WaitForSecondsRealtime(manaTickRate);
        if (PlayerCombat.Instance.currentPlayerMana <= persistManaCost)
        {
            switch (currentAbility)
            {
                case "Time Dilation":
                    timeDilation = false;
                    timeDilationPrefab.SetActive(false);
                    Time.timeScale = 1f;
                    initialManaPaid = false;
                    currentTimeScale = 1f;
                    StartCoroutine(Cooldown(timeDilationRecastCD));
                    yield break;
                case "Shadow Echo":
                    shadowEchoActive = false;
                    shadowEchoPrefab.SetActive(false);
                    initialManaPaid = false;
                    StartCoroutine(Cooldown(shadowEchoRecastCD));
                    yield break;
            }
        }
        PlayerCombat.Instance.currentPlayerMana -= persistManaCost;
        StartCoroutine(ManaDrainAbility(activateManaCost, persistManaCost, manaTickRate));
    }

    IEnumerator ManaConsume(float manaActivateCost)
    {
        PlayerCombat.Instance.currentPlayerMana -= manaActivateCost;
        yield break;
    }

    IEnumerator Cooldown(float cooldown)
    {
        yield return new WaitForSecondsRealtime(cooldown);
        switch (currentAbility)
        {
            case "TimeDilation":
                canTimeDilation = true;
                yield break;
            case "Shadow Echo":
                canShadowEcho = true;
                yield break;
            case "Dash":
                playerMovement.isDashing = false;
                playerMovement.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                canDash = true;
                yield break;
        }
    }

    IEnumerator InitialCooldown(float initialRecastCooldown)
    {
        yield return new WaitForSecondsRealtime(initialRecastCooldown);
        switch (currentAbility)
        {
            case "Time Dilation":
                canTimeDilation = true;
                yield break;
            case "Shadow Echo":
                canShadowEcho = true;
                yield break;
            case "Dash":
                canDash = true;
                yield break;
        }
    }


}