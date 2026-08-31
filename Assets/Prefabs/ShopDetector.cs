using UnityEngine;

public class ShopDetector : MonoBehaviour
{
    public GameObject shopInterface;
    private bool isInRange = false;
    private bool isShopping = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && !isShopping && Input.GetKeyDown(KeyCode.E))
        {
            isShopping = true;
            shopInterface.SetActive(true);
            PlayerMovement.Instance.playerFrozen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isInRange = false;
        }
    }
    public void CloseShop()
    {
        shopInterface.SetActive(false);
        PlayerMovement.Instance.playerFrozen = false;
        isShopping = false;
    }
}
