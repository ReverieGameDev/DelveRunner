using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FreezeAnimation()
    {
        GetComponent<Animator>().speed = 0; // stop the animation so the spikes remain
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCombat.Instance.DamagePlayer(40f);
            GetComponent<Animator>().speed = 1; //continue the animation after the player gets hit.
        }
    }
    public void DestroySpikes()
    {
        Destroy(gameObject);//destroy the spikes when the animation is done
    }
}
