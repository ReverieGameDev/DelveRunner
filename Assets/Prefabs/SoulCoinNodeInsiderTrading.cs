using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinInsiderTrading", menuName = "Soul Coins/InsiderTrading")]
public class SoulCoinNodeInsiderTrading : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isInsiderTradingActive = true;
        
        switch (currentLevel)
        {
            case 1:
                player.insiderTradingPercent = 5;
                player.insiderTradingGoldAmount = 2;
                break;
            case 2:
                player.insiderTradingPercent = 8;
                player.insiderTradingGoldAmount = 3;
                break;
            case 3:
                player.insiderTradingPercent = 12;
                player.insiderTradingGoldAmount = 4;
                break;
        }
    }
}