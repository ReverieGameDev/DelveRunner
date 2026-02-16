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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine("SendBackToAufburn");
        }
    }

    public IEnumerator SendBackToAufburn()
    {
        
        PlayerPrefs.SetInt("Gold", playerCombat.playerMoney);
        PlayerPrefs.SetInt("Exp", playerCombat.playerXp);
        PlayerPrefs.SetInt("DelveLevel", playerCombat.delveLevel);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("TradingHub");
    }
}
