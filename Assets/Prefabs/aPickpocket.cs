using UnityEngine;
[CreateAssetMenu(fileName = "aPickpocket", menuName = "Augments/aPickpocket")]
public class aPickpocket : AugmentData
{
    int currentPickpocketLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnHitDealt -= OnHit;
        playerCombat.OnHitDealt += OnHit;
        currentPickpocketLevel = currentLevel;
    }
    private void OnHit(Enemy enemy)
    {
        if (Random.Range(1,101)< (8 * currentPickpocketLevel) + 2)
        {
            PlayerCombat.Instance.ModifyGoldValue("pickup", Random.Range(1, 4));
        }
    }
}