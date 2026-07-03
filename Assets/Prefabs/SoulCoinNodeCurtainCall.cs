using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeCurtainCall", menuName = "Soul Coins/CurtainCall")]
public class SoulCoinNodeCurtainCall : SoulCoinNode
{

    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.curtainCallActive = true;
        switch (currentLevel)
        {
            case 1:
                player.curtainCallExecute = .1f;
                break;
            case 2:
                player.curtainCallExecute = .125f;
                break;
            case 3:
                player.curtainCallExecute = .15f;
                break;
        }
    }
}