
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EnemyDamageNumbers : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public Image damageTypeIcon;
    public Sprite critIcon;
    public Sprite regularDamageIcon;
    private float floatSpeed = 1f;
    private float fadeSpeed = .66f;
    private CanvasGroup visibility;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        visibility = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x,transform.position.y + floatSpeed * Time.deltaTime);
        visibility.alpha -= fadeSpeed * Time.deltaTime;
        if (visibility.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DamageNumberSetup(int Damage, bool crit)
    {
        if (crit == true)
        {
            damageTypeIcon.sprite = critIcon;
        }
        else if (crit == false)
        {
            damageTypeIcon.sprite = regularDamageIcon;
        }
        damageText.text = ("" + Damage);
    }
}
