using UnityEngine;

public class ZapWindup : MonoBehaviour
{
    public EnvironmentThreat environmentThreat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnWindupComplete()
    {
        environmentThreat.EnvironmentThreatZapper();
    }
}
