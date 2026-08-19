using UnityEngine;

[CreateAssetMenu(fileName = "aBackstabbersSoul", menuName = "Augments/aBackstabbersSoul")]
public class aBackstabbersSoul : AugmentData
{
    int backstabbersSoulMaxStacks = 25;
    int currentBackstabbersStacks = 0;
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.maxHealth = (int)(playerCombat.maxHealth * .6f);
        playerCombat.currentPlayerHealth = (int)playerCombat.maxHealth;
        playerCombat.backstabbersSoulActive = true;
        playerCombat.backstabbersSoulDamageMult = .7f;
        playerCombat.backstabbersSoulCritGain = 1;
        playerCombat.backstabbersSoulCritDamageGain = .3f;
        playerCombat.backstabbersSoulFlatCrit = 25;
        playerCombat.OnHitDealt += OnHit;
    }
    private void OnHit(Enemy enemy)
    {
        if (PlayerCombat.Instance.currentBackstabbersStacks <25)
        {
            PlayerCombat.Instance.currentBackstabbersStacks++;
        }
        PlayerCombat.Instance.lastHitTime = Time.time;
    }
}