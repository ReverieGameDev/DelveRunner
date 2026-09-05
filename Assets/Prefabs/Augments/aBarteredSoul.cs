using UnityEngine;
[CreateAssetMenu(fileName = "aBarteredSoul", menuName = "Augments/aBarteredSoul")]
public class aBarteredSoul : AugmentData
{

    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.barteredSoulActive = true;
        playerCombat.barteredSoulLevel = currentLevel;

        if (currentLevel == 1)
        {
            playerCombat.moneyText.text = ": 0";
            playerCombat.playerMoney = 0;
        }
    }
}