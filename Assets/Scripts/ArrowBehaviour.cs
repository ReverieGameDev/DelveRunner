using NUnit.Framework.Constraints;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    private string attackName;
    public Vector2 archerToPlayerPos;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Vector2 currentPos;
    private float speed = 50f;
    public int arrowOffset;
    private float arrowAngle;
    private bool isReadyToFire = false;
    private Vector2 fanAngle;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
        currentPos = (Vector2)transform.position;
        rb = GetComponent<Rigidbody2D>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        archerToPlayerPos = new Vector2(playerMovement.transform.position.x - currentPos.x, playerMovement.transform.position.y - currentPos.y).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg*Mathf.Atan2(archerToPlayerPos.y,archerToPlayerPos.x));
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (attackName == "arrowShot")
        {
            rb.MovePosition((Vector2)transform.position + archerToPlayerPos * speed * Time.fixedDeltaTime);
        }
        if (attackName == "arrowVolley" && isReadyToFire)
        {
            rb.MovePosition((Vector2)transform.position + archerToPlayerPos * speed * Time.fixedDeltaTime);
            
        }
    }

    public void AttackName(string attack, int offset)
    {
        attackName = attack;
        arrowOffset = offset;
        if (attackName == "arrowVolley")
        {
            arrowAngle = Mathf.Rad2Deg * Mathf.Atan2(archerToPlayerPos.y, archerToPlayerPos.x) + offset;
            transform.rotation = Quaternion.Euler(0, 0, arrowAngle);

            arrowAngle = Mathf.Deg2Rad * arrowAngle;

            archerToPlayerPos = new Vector2(Mathf.Cos(arrowAngle), Mathf.Sin(arrowAngle));
            Debug.Log(archerToPlayerPos.x + "x " + archerToPlayerPos.y);
            
        }
        isReadyToFire = true;
    }

}
