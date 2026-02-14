using UnityEngine;

public class IndicatorBehaviour : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Vector3 mousePosition;
    private Vector3 playerPosition;
    private Vector3 indicatorPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        playerPosition = playerMovement.transform.position;
        indicatorPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        indicatorPosition = ((indicatorPosition - playerMovement.transform.position).normalized*2)+playerMovement.transform.position;
        transform.position = indicatorPosition;
        Vector3 indicatorDirection = indicatorPosition - playerMovement.transform.position;
        float angle = Mathf.Atan2(indicatorDirection.y, indicatorDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
