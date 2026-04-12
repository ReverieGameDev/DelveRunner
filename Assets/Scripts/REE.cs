using System.Collections.Generic;
using UnityEngine;

public class REE : MonoBehaviour
{
    private EmberSystem emberSystem;
    public List<GameObject> reeEncounterList = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emberSystem = FindFirstObjectByType<EmberSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !emberSystem.isFightNodeActive)
        {
            int randomEnemy = Random.Range(0, reeEncounterList.Count);
            Instantiate(reeEncounterList[randomEnemy], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
