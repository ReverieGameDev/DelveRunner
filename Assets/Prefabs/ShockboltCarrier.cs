using UnityEngine;

public class ShockboltCarrier : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector2 destination;
    public GameObject enemyGameObjectHit;
    private float angle;
    public float speed;
    void Start()
    {
        destination = enemyGameObjectHit.transform.position;
        angle = Mathf.Atan2(enemyGameObjectHit.transform.position.y-transform.position.y, enemyGameObjectHit.transform.position.x - transform.position.x);
        angle = Mathf.Rad2Deg*angle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, destination) < 0.1f) 
        {
            if (enemyGameObjectHit == null) { Destroy(gameObject); return; }
            enemyGameObjectHit.GetComponent<Enemy>().reduceHp(enemyGameObjectHit.GetComponent<Enemy>().enemyHealth * 0.03f, 1, false, WeaponStatusEffect.Shock);
            if (PlayerCombat.Instance.aStaticCarrierActive )
            {
                if (Random.Range(0, 101) < PlayerCombat.Instance.aStaticCarrierChance)
                {
                    AddExtraStatus();
                }
            }
            Destroy(gameObject);
        }
    }

    public void AddExtraStatus()
    {
    EnemyStatusEffects targetEse = enemyGameObjectHit.GetComponent<EnemyStatusEffects>();
    int randomStatus = Random.Range(0, 4);
                switch (randomStatus)
                {
                    case 0:
                        targetEse.ESEBurn(4f, 3, 1f);
                        break;
                    case 1:
                        targetEse.ESEPoison(5f, 2, 1f);
                        break;
                    case 2:
                        targetEse.ESEEnfeeble(4f, 10f);
                        break;
                    case 3:
                        targetEse.ESECinder(4f, 3, 1f, 1);
                        break;
                }
    }
}
