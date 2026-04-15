/*using UnityEngine;

public class DeathBossWave10Slash : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Vector3 initialPosition;
    private Vector3 playerPosition;
    public int speed = 210;
    private PlayerCombat playerCombat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        initialPosition = transform.position;
        playerPosition = playerMovement.transform.position;

        Vector3 direction = playerPosition - initialPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(((playerPosition - initialPosition).normalized) * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerCombat.iFrames == false)
        {
            Destroy(gameObject);
            playerCombat.DamagePlayer();
        }
    }
}
*/