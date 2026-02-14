using UnityEngine;

public class AufburnOnLoad : MonoBehaviour
{
    private PlayerCombat playerCombat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();

        playerCombat.playerMoney = PlayerPrefs.GetInt("Gold", 0);
        playerCombat.playerXp = PlayerPrefs.GetInt("Exp", 0);
        playerCombat.delveLevel = PlayerPrefs.GetInt("DelveLevel", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
