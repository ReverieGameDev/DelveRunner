using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Teleporter : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public GameObject delveDeeperScreen;
    public string currentAbility;
    private bool playerInRange = false;

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DelveDeeperMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }

    private void DelveDeeperMenu()
    {
        delveDeeperScreen.SetActive(true);
    }
    public void SkipToBoss()
    {
        PlayerPrefs.SetInt("StartWave", 10);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SampleScene");
    }
    public void DelveDeeper()
    {
        playerCombat.delveLevel++;
        PlayerPrefs.SetInt("DelveLevel", playerCombat.delveLevel);
        PlayerPrefs.SetInt("Gold", playerCombat.playerMoney);
        PlayerPrefs.SetString("CurrentAbility", currentAbility);
        if (PlayerPrefs.GetString("CurrentAbility") == "")
        {
            PlayerPrefs.SetString("CurrentAbility", "Dash");
        }
        PlayerPrefs.Save();
        SceneManager.LoadScene("SampleScene");
    }
    public void DontDelveDeeper()
    {
        delveDeeperScreen.SetActive(false);
    }
}