using UnityEngine;

public class ETZapLightning : MonoBehaviour
{
    void OnEnable()
    {
        transform.position = PlayerCombat.Instance.transform.position;
    }
    public void DisableOnAnimationFinish()
    {
        gameObject.SetActive(false);
    }
}
