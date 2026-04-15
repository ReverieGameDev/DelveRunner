using UnityEngine;

public class RERGamble : MonoBehaviour
{
    private int chosenStat;
    public string chosenStatueStat;
    private PlayerCombat playerCombat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        chosenStat = Random.Range(0, playerCombat.playerStats.Count);
        chosenStatueStat = playerCombat.playerStats[chosenStat];
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
