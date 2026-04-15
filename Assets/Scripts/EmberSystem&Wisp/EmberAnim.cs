using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EmberAnim : MonoBehaviour
{
    public Sprite[] emberFrames;
    private Image emberImage;
    public float frameRate = 0.1f;
    private int currentFrame = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emberImage = GetComponent<Image>();
        StartCoroutine("AnimateEmber");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AnimateEmber()
    {
        while (true)
        {
            emberImage.sprite = emberFrames[currentFrame];
            currentFrame = (currentFrame + 1) % emberFrames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}
