
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
                damageText.color = crit ? new Color32(240, 198, 74, 255) : new Color32(201, 196, 184, 255);
                break;
            case WeaponStatusEffect.Poison:
                damageText.color = new Color32(102, 179, 76, 255);
                break;
            case WeaponStatusEffect.Burn:
                damageText.color = new Color32(224, 112, 44, 255);
                break;
            case WeaponStatusEffect.Cinder:
                damageText.color = new Color32(232, 220, 168, 255);
                break;
            case WeaponStatusEffect.Shock:
                damageText.color = new Color32(61, 220, 255, 255);
                break;
            case WeaponStatusEffect.Bleed:
                damageText.color = new Color32(232, 86, 74, 255);
                break;
        }
    }
}
