using UnityEngine;
using UnityEngine.UIElements;

public class AttackIndicator : MonoBehaviour
{
    public GameObject attackIndicator;
    private PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        Instantiate(attackIndicator, playerMovement.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
