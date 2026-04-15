using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    private Animator anim;
    private SendToAufburn sendToAufburn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sendToAufburn = FindFirstObjectByType<SendToAufburn>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PortalActiveState()
    {
        anim.SetTrigger("PortalActive");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            sendToAufburn.StartCoroutine("SendBackToAufburn");
        }
    }
}
