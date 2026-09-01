
using UnityEngine;

public class SoulCoinTreeSelection : MonoBehaviour
{
    public SoulCoinTree tree;
    private SoulCoinManager soulCoinManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soulCoinManager = FindFirstObjectByType<SoulCoinManager>();
    }
    public void OnClick()
    {
        soulCoinManager.selectedTree = tree;
        soulCoinManager.Refresh(tree);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
