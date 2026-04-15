using UnityEngine;

public class AufburnOnLoad : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private int firstLoad = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        firstLoad = PlayerPrefs.GetInt("FirstLoad");
        if (firstLoad == 0)
        {
            
            playerCombat = FindFirstObjectByType<PlayerCombat>();
            playerCombat.playerMoney = PlayerPrefs.GetInt("Gold", 50);
            playerCombat.playerXp = PlayerPrefs.GetInt("Exp", 0);
            playerCombat.delveLevel = PlayerPrefs.GetInt("DelveLevel", 0);  // Default 0
            firstLoad = PlayerPrefs.GetInt("FirstLoad", 1);
            PlayerPrefs.Save();
        }
        else if (firstLoad == 1)
        {
            playerCombat.playerMoney = PlayerPrefs.GetInt("Gold");
            playerCombat.playerXp = PlayerPrefs.GetInt("Exp");
            playerCombat.delveLevel = PlayerPrefs.GetInt("DelveLevel");  // Default 0
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
