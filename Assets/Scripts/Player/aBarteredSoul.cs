using UnityEngine;
[CreateAssetMenu(fileName = "aBarteredSoul", menuName = "Augments/aBarteredSoul")]
public class aBarteredSoul : AugmentData
{
    
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.barteredSoulActive = true;
        playerCombat.barteredSoulLevel = currentLevel;
        playerCombat.playerMoney = 0;
    }
}