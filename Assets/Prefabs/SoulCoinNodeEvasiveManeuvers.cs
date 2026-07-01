using UnityEngine;


[CreateAssetMenu(fileName = "SoulCoinNodeEvasiveManeuvers", menuName = "Soul Coins/EvasiveManeuvers")]
public class SoulCoinEvasiveManeuvers : SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.isEvasiveManeuversActive = true;
        switch (currentLevel)
        {
            case 1:
                player.evasiveManeuversValue = .01f;
                player.StartCoroutine("EvasiveManeuvers");
                break;
            case 2:
                player.evasiveManeuversValue = .02f;
                player.StartCoroutine("EvasiveManeuvers");
                break;
            case 3:
                player.evasiveManeuversValue = .03f;
                player.StartCoroutine("EvasiveManeuvers");
                break;
        }
    }
}