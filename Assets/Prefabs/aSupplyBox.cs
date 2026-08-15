using UnityEngine;
[CreateAssetMenu(fileName = "aSupplyBox", menuName = "Augments/aSupplyBox", order = 1)]
public class aSupplyBox : AugmentData
{
    int augmentCurrentLevel;
    public DropTableData supplyBoxTable;
    public override void Apply(PlayerCombat playerCombat, int currentLevel)
    {
        playerCombat.OnEnemyKill -= OnKill;
        playerCombat.OnEnemyKill += OnKill;
        augmentCurrentLevel = currentLevel;
    }
    private void OnKill(Enemy enemy)
    {
        Debug.Log("ONKILL FIRING");
        if (Random.Range(1,101)<99)
        {
            for (int i = 0; i<augmentCurrentLevel; i++)
            {
                Debug.Log("DROPPING EXTRA ITEM");
                DropManager.Instance.RollDropTable(supplyBoxTable, enemy.transform.position);
            }
        }
    }
}