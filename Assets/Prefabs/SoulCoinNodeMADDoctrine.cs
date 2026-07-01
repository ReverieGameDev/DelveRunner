using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeMADDoctrine", menuName = "Soul Coins/MADDoctrine")]
public class SoulCoinNodeMADDoctrine : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isMADDoctrineActive = true;
        switch (currentLevel)
        {
            case 1:
                player.mADDoctrineCooldown = 15;
                player.mADDoctrineReflectDamage = 1.5f;
                break;
            case 2:
                player.mADDoctrineCooldown = 13;
                player.mADDoctrineReflectDamage = 1.75f;
                break;
            case 3:
                player.mADDoctrineCooldown = 11;
                player.mADDoctrineReflectDamage = 2f;
                break;
            case 4:
                player.mADDoctrineCooldown = 9;
                player.mADDoctrineReflectDamage = 2.5f;
                break;
            case 5:
                player.mADDoctrineCooldown = 7;
                player.mADDoctrineReflectDamage = 3f;
                break;
        }
    }
}