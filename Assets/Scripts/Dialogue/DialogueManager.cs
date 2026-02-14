using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // State
    public bool isDialogueActive = false;
    private bool isTyping = false;
    private bool skipRequested = false;
    private bool waitingForInput = false;
    private string npcYoureSpeakingTo;

    // UI References
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueTextField;
    public TextMeshProUGUI speakerNameField;
    public GameObject speakerPortraitImage;

    // Tracking
    private int currentLineIndex = 0;
    private DialogueData currentDialogue;

    // Other References
    private PlayerMovement playerMovement;
    private ShopManager shopManager;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        shopManager = FindFirstObjectByType<ShopManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTyping && !waitingForInput)
        {
            skipRequested = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isTyping && waitingForInput)
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(string npcTag, DialogueData dialogueData)
    {
        npcYoureSpeakingTo = npcTag;
        currentDialogue = dialogueData;
        currentLineIndex = 0;
        speakerPortraitImage.GetComponent<UnityEngine.UI.Image>().sprite = currentDialogue.portrait;
        speakerNameField.text = currentDialogue.npcName;
        playerMovement.GetComponent<Animator>().SetFloat("IsMoving", 0);
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        dialogueTextField.text = "";
        StartCoroutine(DisplayText(currentDialogue.lines[currentLineIndex]));
    }

    IEnumerator DisplayText(string line)
    {
        skipRequested = false;
        waitingForInput = false;
        isTyping = false;
        yield return new WaitForSeconds(.2f);
        isTyping = true;
        foreach (char c in line)
        {
            dialogueTextField.text += c;
            if (skipRequested)
            {
                dialogueTextField.text = line;
                break;
            }
            yield return new WaitForSeconds(.05f);
        }
        isTyping = false;
        waitingForInput = true;
    }

    private void AdvanceDialogue()
    {
        currentLineIndex++;
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            CloseDialogue();
        }
        else
        {
            dialogueTextField.text = "";
            StartCoroutine(DisplayText(currentDialogue.lines[currentLineIndex]));
        }
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        waitingForInput = false;
        currentLineIndex = 0;
        ActionPostDialogue(npcYoureSpeakingTo);
        StartCoroutine(DialogueCooldown());
    }

    IEnumerator DialogueCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        isDialogueActive = false;
    }

    private void ActionPostDialogue(string npcTag)
    {
        if (npcTag == "Shop")
        {
            shopManager.OpenShop();
        }
        if (npcTag == "Wizard")
        {
            shopManager.OpenWizardShop();
        }
        if (npcTag == "QuestBoard")
        {
            shopManager.OpenQuestBoard();
        }
    }
}