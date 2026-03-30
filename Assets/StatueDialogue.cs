using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class StatueDialogue : MonoBehaviour
{
    private PlayerCombat playerCombat;
    public GameObject statueDialogue;
    public TextMeshProUGUI statueText;
    private RERGamble rerGamble;
    public int gambleInt;
    public GameObject dice;
    public Sprite[] diceFaces; // assign all 6 face sprites in Inspector, index 0 = face 1
    public Sprite[] diceFacesRolling;
    private bool hasAccepted = false;
    public GameObject acceptButton;
    public GameObject declineButton;
    private bool playerInRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        rerGamble = GetComponent<RERGamble>();
    }

    // Update is called once per frame
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
                    statueText.text = "The jester sneers at you. He wants to gamble with your <color=#FFD700>" + rerGamble.chosenStatueStat + "</color> stat";
                    break;
                case 1:
                    statueText.text = "The jester cackles and points at you. He dares you to wager your <color=#FFD700>" + rerGamble.chosenStatueStat + "</color> stat";
                    break;
                case 2:
                    statueText.text = "The jester's grin widens. He beckons you to roll the dice on your <color=#FFD700>" + rerGamble.chosenStatueStat + "</color> stat";
                    break;
                case 3:
                    statueText.text = "The jester twirls mockingly. He's itching to gamble with your <color=#FFD700>" + rerGamble.chosenStatueStat + "</color> stat";
                    break;
                case 4:
                    statueText.text = "The jester bows with a wicked smile. He's set his sights on your <color=#FFD700>" + rerGamble.chosenStatueStat + "</color> stat.";
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
            gambleInt = Random.Range(1, 7);
            StartCoroutine("DiceRoll");
            hasAccepted = true;
            acceptButton.SetActive(false);
            declineButton.SetActive(false);
        }

    }

    IEnumerator DiceRoll()
    {
        
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSecondsRealtime(.05f);
            dice.GetComponent<UnityEngine.UI.Image>().sprite = diceFacesRolling[i % 6];
            
        }

        dice.GetComponent<UnityEngine.UI.Image>().sprite = diceFaces[gambleInt - 1];

        
        if (gambleInt < 3)
        {
            statueText.text = "You rolled higher than a 3, the jester raises your " + rerGamble.chosenStatueStat + " by 5%, the statue goes dormant";
            playerCombat.StatueStatMods(rerGamble.chosenStatueStat, -0.05f);

        }
        else if (gambleInt > 3)
        {
            statueText.text = "You rolled less than a 3. The jester lowers your " + rerGamble.chosenStatueStat + " by 5%, the statue goes dormant";
            playerCombat.StatueStatMods(rerGamble.chosenStatueStat, 0.05f);
        }
        else if (gambleInt == 3)
        {
            statueText.text = "You rolled a 3, the jester does nothing, the statue goes dormant.";
        }
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        statueDialogue.SetActive(false);
    }
}
