using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float xInput;
    public float yInput;
    public float knockback = 10f;
    public Vector2 playerPosition;
    
    private Rigidbody2D playerRb;
    private PlayerCombat playerCombat;
    private SpriteRenderer spriteRenderer;
    private DialogueManager dialogueManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRb = GetComponent<Rigidbody2D>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        if (dialogueManager == null || dialogueManager.isDialogueActive == false)
        {
            GetComponent<Animator>().SetFloat("IsMoving", Mathf.Abs(xInput) + Mathf.Abs(yInput));

            if (xInput <= 0)
            {
                spriteRenderer.flipX = true;
            }
            if (xInput >= 0)
            {
                spriteRenderer.flipX = false;
            }
            playerRb.linearVelocity = (new Vector2(xInput, yInput) * playerCombat.movementSpeed);
            //transform.Translate(new Vector2(xInput, yInput) * playerCombat.movementSpeed * Time.deltaTime);
            playerPosition = transform.position;
        }

    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && playerCombat.iFrames == false)
            {
                Rigidbody2D enemyCollisionRb = collision.GetComponent<Rigidbody2D>();
                Vector2 enemyCollisionPosition = enemyCollisionRb.transform.position;
                playerRb.AddForce(((playerPosition - enemyCollisionPosition).normalized)*50, ForceMode2D.Impulse);
                playerCombat.DamagePlayer();
            }
        }
}
