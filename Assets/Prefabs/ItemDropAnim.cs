using UnityEngine;

public class ItemDropAnim : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItemAnim(float explosionForce)
    {

        rb.AddForce(Random.insideUnitCircle.normalized * explosionForce, ForceMode2D.Impulse);
    }
}
