using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeLightningStrikesTwice ", menuName = "Soul Coins/LightningStrikesTwice")]
public class SoulCoinNodeLightningStrikesTwice : SoulCoinNode
{
    
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.lightningStrikesTwiceActive = true;
        player.lightningStrikesTwiceCritDmgCap = .75f;
        switch (currentLevel)
        {
            case 1:
                player.lightningStrikesTwiceDmg = 0.02f;
                break;
            case 2:
                player.lightningStrikesTwiceDmg = 0.04f;
                break;
            case 3:
                player.lightningStrikesTwiceDmg = 0.06f;
                break;
            case 4:
                player.lightningStrikesTwiceDmg = 0.09f;
                break;
            case 5:
                player.lightningStrikesTwiceDmg = 0.15f;
                break;
        }
    }
}