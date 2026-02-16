using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public GameObject delveDeeperScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("player entered teleporter");
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("Player attempting to access teleporter UI");
                DelveDeeperMenu();
            }

        }
    }

    private void DelveDeeperMenu()//menu that says "delve deeper?"
    {
        delveDeeperScreen.SetActive(true);
    }

    public void DelveDeeper()
    {
        playerCombat.delveLevel++;  // Increment first
        PlayerPrefs.SetInt("DelveLevel", playerCombat.delveLevel);  // Now saves 1
        PlayerPrefs.SetInt("Gold", playerCombat.playerMoney);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SampleScene");
    }

    public void DontDelveDeeper()//if player hits no
    {
        delveDeeperScreen.SetActive(false);
    }
}
