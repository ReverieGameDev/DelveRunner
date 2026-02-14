using UnityEngine;

public class NPCTriggerDialogue : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueData currentDialogue;
    public bool isNearNPC = false;
    public GameObject canSpeakToNPCIndicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueManager.isDialogueActive == false && isNearNPC)
        {
            dialogueManager.StartDialogue(gameObject.tag, currentDialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSpeakToNPCIndicator.SetActive(true);
            isNearNPC = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSpeakToNPCIndicator.SetActive(false);
            isNearNPC = false;
        }
    }
}
