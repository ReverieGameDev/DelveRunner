using UnityEngine;

public class ButtonPromptPulse : MonoBehaviour
{
    public GameObject buttonTrim;
    private float increaseOrDecrease = -.1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonTrim.transform.localScale.x >= 1)
        {
            increaseOrDecrease = -.01f;
        }
        else if (buttonTrim.transform.localScale.x <= 0.95)
        {
            increaseOrDecrease = .01f;
        }
        buttonTrim.transform.localScale = new Vector3(buttonTrim.transform.localScale.x + increaseOrDecrease *Time.deltaTime, buttonTrim.transform.localScale.y + increaseOrDecrease * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y + (increaseOrDecrease * 3)*Time.deltaTime);
    }
}
