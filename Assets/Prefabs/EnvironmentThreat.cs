
using System.Collections;
using UnityEngine;
using static EnvironmentThreat;

public class EnvironmentThreat : MonoBehaviour
{
    private float totalChargeTime;
    private float cooldownTime;
    private float interruptPenaltyTime;
    private float cooldownAfterFire;

    private float chargeCounter;
    private float stateTimer;
    public EnvironmentState environmentState = EnvironmentState.Idle;
    public EnvironmentThreatName currentEnvironmentThreatName;
    
    public enum EnvironmentThreatName
    {
        Zapper,
        HealingTotem,
        NullObelisk
    }
    public enum EnvironmentState
    {
        Idle,
        Charging,
        Firing,
        Interrupted
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (currentEnvironmentThreatName)
        {
            case EnvironmentThreatName.Zapper:
                    totalChargeTime = 10f;
                    cooldownTime = 4f;
                    interruptPenaltyTime = 2f;
                cooldownAfterFire = 4f;
                break;
            case EnvironmentThreatName.HealingTotem:
                    totalChargeTime = 5f;
                    cooldownTime = 5f;
                    interruptPenaltyTime = 5f;
                cooldownAfterFire = 3f;
                break;
            case EnvironmentThreatName.NullObelisk:
                    totalChargeTime = 8f;
                    cooldownTime = 6f;
                    interruptPenaltyTime = 2f;
                cooldownAfterFire = 2f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
            switch (environmentState)
            {
                case EnvironmentState.Idle: //ET is unoccupied, idle.
                    break;
                case EnvironmentState.Charging: //ET is occupied by an enemy, is charging
                    chargeCounter += Time.deltaTime;
                    if (chargeCounter >= totalChargeTime)
                    {
                        environmentState = EnvironmentState.Firing;
                        stateTimer = cooldownAfterFire;
                    }
                    break;
                case EnvironmentState.Firing: //ET has reached the total charge, fires.
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0)
                    {
                        environmentState = EnvironmentState.Charging;
                        chargeCounter = 0;
                    }
                    break;
                case EnvironmentState.Interrupted: //ET is interrupted by a stun
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0) 
                    {
                        environmentState = EnvironmentState.Charging;
                    }
                    break;
            }
        }
    public void InterruptEnvironment()
    {
        if (chargeCounter >= totalChargeTime)
        {
            return;
        }
        chargeCounter = Mathf.Max(0,chargeCounter - interruptPenaltyTime);
        stateTimer = cooldownTime;
        environmentState = EnvironmentState.Interrupted;
    }
    public float EnvironmentChargePercent()
    {
        return chargeCounter / totalChargeTime;
    }
}

