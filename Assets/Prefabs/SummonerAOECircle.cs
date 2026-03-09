using System.Collections;
using UnityEngine;

public class SummonerAOECircle : MonoBehaviour
{
    public float circleActiveTime = .5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("DestroyCircle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //deal damage to player
        }
    }
    IEnumerator DestroyCircle()
    {
        yield return new WaitForSeconds(circleActiveTime);
        Destroy(gameObject);
    }
}
