using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "aBloodletter", menuName = "Augments/aBloodletter")]
public class aBloodletter : AugmentData
{
    int aCritsplosionCurrentLevel;

    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.bloodletterActive = true;
    }
}