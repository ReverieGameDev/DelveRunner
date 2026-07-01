using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeFlowState", menuName = "Soul Coins/FlowState")]
public class SoulCoinNodeFlowState : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isFlowStateActive = true;
        switch (currentLevel)
        {
            case 1:
                player.flowStateDamage = .05f;
                break;
            case 2:
                player.flowStateDamage = .1f;
                break;
            case 3:
                player.flowStateDamage = .15f;
                break;
        }
    }
}