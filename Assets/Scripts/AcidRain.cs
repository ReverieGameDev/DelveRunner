using UnityEngine;

public class AcidRain : MonoBehaviour
{
    public bool isReadyToDamage = false;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isReadyToDamage)
        {
            PlayerCombat.Instance.DamagePlayer(25f);
        }
    }

    public void EnableDamage()
    {
        isReadyToDamage = true;
    }
    public void DestroyAcidRain()
    {
        Destroy(gameObject);
    }
}
