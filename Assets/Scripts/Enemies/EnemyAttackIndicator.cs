using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackIndicator : MonoBehaviour
{
    public Image fillImage;
    public Image iconImage;
    public GameObject attackIndicatorUI;
    private float attackWindup;
    public bool isAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIndicator(Sprite indicatorIcon, float attackWindupTime)
    {
        attackWindup = attackWindupTime;
        iconImage.sprite = indicatorIcon;
        attackIndicatorUI.SetActive(true);
        StartCoroutine(FillIndicator());
    }

    IEnumerator FillIndicator()
    {
        fillImage.fillAmount = 0;
        float elapsed = 0;

        while (elapsed < attackWindup)
        {
            elapsed += Time.deltaTime;
            fillImage.fillAmount = elapsed / attackWindup;
            yield return null;
        }
        attackIndicatorUI.SetActive(false);
    }
}
