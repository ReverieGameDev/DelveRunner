using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
public class REROfferingRoom : MonoBehaviour
{
    private int chosenStat;
    public string chosenStatueStat;
    private PlayerCombat playerCombat;
    
    public GameObject statueDialogue;
    public TextMeshProUGUI statueText;
    private bool hasAccepted = false;
    public GameObject acceptButton;
    public GameObject declineButton;
    public GameObject leaveButton;
    private bool playerInRange = false;
    private int goldOffering = 25;
    private ParticleSystem ps;
    private PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        chosenStat = Random.Range(0, playerCombat.playerStats.Count);
        List<string> remaining = new List<string>(playerCombat.playerStats);
        chosenStatueStat = playerCombat.playerStats[chosenStat];
        remaining.Remove(chosenStatueStat);
    }



    void Update()
    {
        
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasAccepted)
        {
            Time.timeScale = 0;
            playerMovement.playerFrozen = true;
            statueDialogue.SetActive(true);
            int randomDialogue = Random.Range(0, 5);
            if (playerCombat.playerMoney < goldOffering)
            {
                NotEnoughMoney();
                return;
            }
            acceptButton.SetActive(true);
            declineButton.SetActive(true);
            leaveButton.SetActive(false);
            switch (randomDialogue)
            {
                case 0:
                    statueText.text = "The shrine glows faintly. It demands <color=#FFD700>" + goldOffering + " gold</color> in exchange for a blessing to your <color=#FFD700>" + chosenStatueStat + "</color>.";
                    break;
                case 1:
                    statueText.text = "The shrine hums with warmth. Place <color=#FFD700>" + goldOffering + " gold</color> at its base and your <color=#FFD700>" + chosenStatueStat + "</color> will be strengthened.";
                    break;
                case 2:
                    statueText.text = "The shrine awaits an offering. <color=#FFD700>" + goldOffering + " gold</color> will grant a boon to your <color=#FFD700>" + chosenStatueStat + "</color>.";
                    break;
                case 3:
                    statueText.text = "A gentle light pulses from the shrine. It offers to enhance your <color=#FFD700>" + chosenStatueStat + "</color> for <color=#FFD700>" + goldOffering + " gold</color>.";
                    break;
                case 4:
                    statueText.text = "The shrine whispers a simple bargain. <color=#FFD700>" + goldOffering + " gold</color> for the favor of your <color=#FFD700>" + chosenStatueStat + "</color>.";
                    break;
            }
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
    public void DeclineButton()
    {
        Time.timeScale = 1;
        statueDialogue.SetActive(false);
        playerMovement.playerFrozen = false;
    }
    public void LeaveButton()
    {
        hasAccepted = false;
        Time.timeScale = 1;
        statueDialogue.SetActive(false);
        playerMovement.playerFrozen = false;
    }
    public void AcceptButton()
    {
        if (!hasAccepted)
        {
            StartCoroutine("AcceptOffer");
            hasAccepted = true;
            acceptButton.SetActive(false);
            declineButton.SetActive(false);
        }
    }

    IEnumerator AcceptOffer()
    {
        int randomDialogue = Random.Range(0, 5);
        switch (randomDialogue)
        {
            case 0:
                statueText.text = "The gold vanishes. Your <color=#FFD700>" + chosenStatueStat + "</color> stirs with new strength.";
                break;
            case 1:
                statueText.text = "The shrine absorbs your offering. Your <color=#FFD700>" + chosenStatueStat + "</color> sharpens.";
                break;
            case 2:
                statueText.text = "Gold well spent. Your <color=#FFD700>" + chosenStatueStat + "</color> swells with newfound power.";
                break;
            case 3:
                statueText.text = "The shrine dims, satisfied. Your <color=#FFD700>" + chosenStatueStat + "</color> has been blessed.";
                break;
            case 4:
                statueText.text = "A fair trade. The shrine grants its favor to your <color=#FFD700>" + chosenStatueStat + "</color>.";
                break;
        }
        yield return new WaitForSecondsRealtime(2f);
        playerCombat.OfferingStatueMods(chosenStatueStat, goldOffering);
        Time.timeScale = 1;
        ps.Stop();
        statueDialogue.SetActive(false);
        GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
        playerMovement.playerFrozen = false;
    }

    private void NotEnoughMoney()
    {
        statueText.text = "This shrine requires an offering of <color=#FFD700>" + goldOffering + " gold</color>. You don't have enough.";
        leaveButton.SetActive(true);
        acceptButton.SetActive(false);
        declineButton.SetActive(false);
    }
}
