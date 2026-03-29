using UnityEngine;

public class PortalSpawnAura : MonoBehaviour
{
    private Animator anim;
    public GameObject aufburnPortal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPortal()
    {
        Instantiate(aufburnPortal, transform.position, Quaternion.identity);
    }

    public void DestroyAura()
    {
        Destroy(gameObject);
    }
}
