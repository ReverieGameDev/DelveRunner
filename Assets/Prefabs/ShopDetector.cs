using UnityEngine;

public class ShopDetector : MonoBehaviour
{
    public GameObject shopInterface;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            shopInterface.SetActive(true);
            PlayerMovement.Instance.playerFrozen = true;
        }
    }
    public void CloseShop()
    {
        shopInterface.SetActive(false);
        PlayerMovement.Instance.playerFrozen = false;
    }
}
