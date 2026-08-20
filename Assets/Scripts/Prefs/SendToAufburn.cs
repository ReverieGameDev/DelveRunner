using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendToAufburn : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private SpawnManager spawnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        spawnManager = FindFirstObjectByType<SpawnManager>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(SendBackToAufburn());
        }
    }

    public IEnumerator SendBackToAufburn()
    {
        if (playerCombat.soulMixActive)
        {
            PlayerPrefs.SetInt("SoulMixTotal",playerCombat.soulMixPreviousTotal + Mathf.Min(playerCombat.soulMixCap,(int)(playerCombat.soulMixPercent*(playerCombat.totalSiphonKills))));
        }
        else if (!playerCombat.soulMixActive) 
        {
            PlayerPrefs.SetInt("SoulMixTotal", playerCombat.soulMixPreviousTotal);
        }
            PlayerPrefs.SetFloat("Gold", playerCombat.playerMoney);
        PlayerPrefs.SetInt("Exp", playerCombat.playerXp);
        PlayerPrefs.SetInt("DelveLevel", playerCombat.delveLevel);
        string json = PlayerPrefs.GetString("SoulSave", "");
        SoulSaveData data = json == "" ? new SoulSaveData() : JsonUtility.FromJson<SoulSaveData>(json);
        data.soulCoins = playerCombat.soulCoins;
        PlayerPrefs.SetString("SoulSave", JsonUtility.ToJson(data));
        PlayerPrefs.Save();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("TradingHub");
    }
}
