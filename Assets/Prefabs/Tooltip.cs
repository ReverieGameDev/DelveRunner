using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI secondaryText;
    public GameObject panel;
    float offsetX = 150f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    void Update()
    {
        if (Input.mousePosition.x > Screen.width * 0.5f) offsetX = -150f;
        if (Input.mousePosition.x <= Screen.width * 0.5f) offsetX = 150f;
        if (panel.activeSelf) panel.transform.position = Input.mousePosition + new Vector3(offsetX, 80f, 0f); ;
    }
    public void Show(string title, string body, string secondary)
    {
        titleText.text = title;
        bodyText.text = body;
        secondaryText.text = secondary;
        panel.SetActive(true);
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
    }
}