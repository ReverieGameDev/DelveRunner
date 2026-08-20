using UnityEngine;
using UnityEngine.UI;

public class StatusIconAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float frameTime = 0.15f;

    private Image image;
    private int index;
    private float timer;

    void Awake() => image = GetComponent<Image>();

    void Update()
    {
        if (frames.Length == 0) return;
        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer = 0f;
            index = (index + 1) % frames.Length;
            image.sprite = frames[index];
        }
    }
}