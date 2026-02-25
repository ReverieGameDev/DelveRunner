using UnityEngine;

public class REE : MonoBehaviour
{
    public GameObject skeletonArcher;
    public GameObject skeletonWarrior;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("REE should be spawning");
            Instantiate(skeletonArcher, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
