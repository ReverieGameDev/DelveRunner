
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EnemyDamageNumbers : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public Image damageTypeIcon;
    public Sprite critIcon;
    public Sprite regularDamageIcon;
    private float floatSpeed = 1.5f;
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

    public void DamageNumberSetup(int Damage, bool crit, WeaponStatusEffect type = WeaponStatusEffect.None, bool enfeebled = false)
    {
        damageText.text = ("" + Damage);
        if (enfeebled) { damageText.fontMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, new Color32(0, 0, 135, 255)); }
        else { damageText.fontMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, new Color32(0, 0, 0, 255)); }

        switch (type)
        {
            case WeaponStatusEffect.None:
                if (crit)
                damageText.color = Color.white;
                if (!crit)
                damageText.color = Color.red;
                break;
            case WeaponStatusEffect.Poison:
                damageText.color = new Color32(0, 175, 0, 255);
                break;
            case WeaponStatusEffect.Burn:
                damageText.color = new Color32(255, 90, 0, 255);
                break;
            case WeaponStatusEffect.Cinder:
                damageText.color = new Color32(255, 170, 40, 255);
                break;
            case WeaponStatusEffect.Shock:
                damageText.color = new Color32(120, 200, 255, 255);
                break;
        }
    }
}
