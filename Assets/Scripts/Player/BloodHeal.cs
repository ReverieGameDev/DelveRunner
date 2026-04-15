using UnityEngine;

public class BloodHeal : MonoBehaviour
{
    private PlayerCombat playerCombat;

    private void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }
    private void Update()
    {
        transform.position = playerCombat.transform.position;
    }
    public void DestroyAfterAnim()
    {
        Destroy(gameObject);
    }
}
