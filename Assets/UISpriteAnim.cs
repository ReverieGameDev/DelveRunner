using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnim : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 12f;
    private Image img;
    private float t;
    private int i;

    void Start() { img = GetComponent<Image>(); }

    void Update()
    {
        t += Time.deltaTime;
        if (t >= 1f / frameRate)
        {
            t = 0;
            i = (i + 1) % frames.Length;
            img.sprite = frames[i];
        }
    }
}