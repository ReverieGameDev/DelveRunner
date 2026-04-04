using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
public class RERCurse : MonoBehaviour
{
    private int chosenStat;
    public string chosenStatueStat;
    private PlayerCombat playerCombat;

    public GameObject statueDialogue;
    public TextMeshProUGUI statueText;
    private bool hasAccepted = false;
    public GameObject acceptButton;
    public GameObject declineButton;
    public float maxHealthPenalty = .8f;
    private string cursedStat;
    private bool playerInRange = false;
    private ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        chosenStat = Random.Range(0, playerCombat.playerStats.Count);
        List<string> remaining = new List<string>(playerCombat.playerStats);
        chosenStatueStat = playerCombat.playerStats[chosenStat];
        remaining.Remove(chosenStatueStat);
        cursedStat = remaining[Random.Range(0, remaining.Count)];
    }

   

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasAccepted)
        {
            Time.timeScale = 0;
            statueDialogue.SetActive(true);
            int randomDialogue = Random.Range(0, 5);

            switch (randomDialogue)
            {
                case 0:
                    statueText.text = "The statue radiates dark energy. It offers to enhance your <color=#FFD700>" + chosenStatueStat + "</color>, but a curse will weaken your <color=#FF4444>" + cursedStat + "</color>.";
                    break;
                case 1:
                    statueText.text = "A sinister aura pulses from the statue. It promises to bolster your <color=#FFD700>" + chosenStatueStat + "</color>, but your <color=#FF4444>" + cursedStat + "</color> will suffer.";
                    break;
                case 2:
                    statueText.text = "The statue hums with forbidden power. Your <color=#FFD700>" + chosenStatueStat + "</color> will surge, but your <color=#FF4444>" + cursedStat + "</color> will wither.";
                    break;
                case 3:
                    statueText.text = "The statue whispers a dark bargain. It will bless your <color=#FFD700>" + chosenStatueStat + "</color> and curse your <color=#FF4444>" + cursedStat + "</color>.";
                    break;
                case 4:
                    statueText.text = "The statue's eyes glow. Accept its gift to your <color=#FFD700>" + chosenStatueStat + "</color>, but your <color=#FF4444>" + cursedStat + "</color> pays the price.";
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
                statueText.text = "The curse takes hold. Your <color=#FFD700>" + chosenStatueStat + "</color> swells with power as your <color=#FF4444>" + cursedStat + "</color> withers.";
                break;
            case 1:
                statueText.text = "Dark tendrils wrap around you. Your <color=#FFD700>" + chosenStatueStat + "</color> sharpens, but your <color=#FF4444>" + cursedStat + "</color> grows feeble.";
                break;
            case 2:
                statueText.text = "The statue laughs. Your <color=#FFD700>" + chosenStatueStat + "</color> floods with strength as your <color=#FF4444>" + cursedStat + "</color> dulls.";
                break;
            case 3:
                statueText.text = "It is done. The statue bolsters your <color=#FFD700>" + chosenStatueStat + "</color> and saps your <color=#FF4444>" + cursedStat + "</color>.";
                break;
            case 4:
                statueText.text = "The pact is sealed. Your <color=#FFD700>" + chosenStatueStat + "</color> burns bright. Your <color=#FF4444>" + cursedStat + "</color> falters.";
                break;
        }
        yield return new WaitForSecondsRealtime(2f);
        playerCombat.CurseStatueMods(chosenStatueStat, cursedStat);
        Time.timeScale = 1;
        ps.Stop();
        statueDialogue.SetActive(false);
    }
}
