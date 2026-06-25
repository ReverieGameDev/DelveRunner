using UnityEngine;
[CreateAssetMenu(fileName = "SoulCoinNodeBloodSoulBarrier", menuName = "Soul Coins/Blood Soul Barrier Node")]
public class SoulCoinNodeBloodSoulBarrier : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.bloodSoulBarrierActive = true;
        switch (currentLevel)
        {
            case 1:
                player.bloodSoulBarrierValue = 1;
                break;
            case 2:
                player.bloodSoulBarrierValue = 2;
                break;
            case 3:
                player.bloodSoulBarrierValue = 3;
                break;
            case 4:
                player.bloodSoulBarrierValue = 5;
                break;
            case 5:
                player.bloodSoulBarrierValue = 7;
                break;
        }
    }
}