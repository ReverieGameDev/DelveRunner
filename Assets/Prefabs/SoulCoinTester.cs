using UnityEngine;

public class SoulCoinTester : MonoBehaviour
{
    public SoulCoinNode testNode;     
    public PlayerCombat player;       
    public int testLevel = 3;

    void Start()
    {
        Debug.Log($"Before: maxHealth = {player.maxHealth}");
        testNode.Apply(player, testLevel);
        Debug.Log($"After: maxHealth = {player.maxHealth}");
    }
}