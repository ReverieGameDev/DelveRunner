using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 0;
    public int currentDelve = 1;
    
    public TextMeshProUGUI waveNumberGUI;
    public GameObject[] enemiesToDefeat;

    void Update()
    {
        FindAllEnemiesToDefeat();
    }

    public void FindAllEnemiesToDefeat()
    {
        enemiesToDefeat = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public int CountAliveEnemies()
    {
        FindAllEnemiesToDefeat();
        int count = 0;
        for (int i = 0; i < enemiesToDefeat.Length; i++)
        {
            if (enemiesToDefeat[i] != null)
            {
                count++;
            }
        }

        return count;

    }
}