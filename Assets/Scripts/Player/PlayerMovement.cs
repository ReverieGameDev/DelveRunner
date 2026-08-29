using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float xInput;
    public float yInput;
    public float knockback = 10f;
    public Vector2 playerPosition;
    public static PlayerMovement Instance;
    private Rigidbody2D playerRb;
    private PlayerCombat playerCombat;
    private SpriteRenderer spriteRenderer;
    private DialogueManager dialogueManager;
    public bool isDashing = false;
    private Vector2 dashStartingPos;
    private bool dashingTimer = false;
    private float xInputSteer;
    private float yInputSteer;
    private float lastXInput;
    private float lastYInput;
    public Vector2 dashDirection;
    private float dashSpeed = 30f;
    public Animator anim;
    public bool playerFrozen = false;
    private PlayerStatusEffects playerStatusEffects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRb = GetComponent<Rigidbody2D>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        playerStatusEffects = FindAnyObjectByType<PlayerStatusEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerFrozen && (playerStatusEffects == null || !playerStatusEffects.isStunned))
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");

        if (isDashing)
        {
            float steerX = Input.GetAxis("Horizontal") * 0.6f;
            float steerY = Input.GetAxis("Vertical") * 0.6f;
            Vector2 dashVelocity = (dashDirection + new Vector2(steerX, steerY)).normalized;
            playerRb.linearVelocity = dashVelocity * dashSpeed;
        }
        else 
        {
            anim.SetFloat("IsMoving", Mathf.Abs(xInput) + Mathf.Abs(yInput));
            if (xInput <= 0) spriteRenderer.flipX = true;
            if (xInput >= 0) spriteRenderer.flipX = false;
            playerRb.linearVelocity = new Vector2(xInput, yInput) * playerCombat.movementSpeed;
            playerPosition = transform.position;
        }
        }
        else
        {
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(.5f);
        PlayerCombat.Instance.FlowState();
        isDashing = false;
        playerRb.linearVelocity = Vector2.zero;
    }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && playerCombat.iFrames == false)
            {
                Rigidbody2D enemyCollisionRb = collision.GetComponent<Rigidbody2D>();
                Vector2 enemyCollisionPosition = enemyCollisionRb.transform.position;
                playerRb.AddForce(((playerPosition - enemyCollisionPosition).normalized)*50, ForceMode2D.Impulse);
                playerCombat.DamagePlayer(15f);
            }
        }
}
