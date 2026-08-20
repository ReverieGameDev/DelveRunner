using UnityEngine;

[CreateAssetMenu(fileName = "SoulCoinBloodlust", menuName = "Soul Coins/Bloodlust")]
public class SoulCoinBloodlust: SoulCoinNode
{
    public override void Apply(PlayerCombat player, int currentLevel)
    {
        player.bloodlustIsActive = true;
        player.OnEnemyKill -= OnKill;
        player.OnEnemyKill += OnKill;
        switch (currentLevel)
        {
            case 1:
                player.bloodlustDamage = .1f;
                player.bloodlustTime = 6f;
                break;
            case 2:
                player.bloodlustDamage = .15f;
                player.bloodlustTime = 6f;
                break;
            case 3:
                player.bloodlustDamage = .2f;
                player.bloodlustTime = 6f;
                break;
        }
    }
    private void OnKill(Enemy enemy)
    {
        PlayerCombat.Instance.bloodlustRemaining = PlayerCombat.Instance.bloodlustTime;
    }
}