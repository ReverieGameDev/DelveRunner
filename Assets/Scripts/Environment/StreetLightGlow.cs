using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StreetLightGlow : MonoBehaviour
{
    private Light2D light2d;
    private float lightValue;
    public float lightStrength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light2d = GetComponent<Light2D>();
        StartCoroutine("LightGlow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LightGlow()
    {
        for (int i = 0; i < 40; i++)
        {
            light2d.pointLightOuterRadius += .03f+lightStrength;
            yield return new WaitForSeconds(.07f);
        }
        StartCoroutine("LightDim");
    }

    IEnumerator LightDim()
    {
        for (int i = 0; i < 40; i++)
        {
            light2d.pointLightOuterRadius -= .03f+ lightStrength;
            yield return new WaitForSeconds(.07f);
        }
        StartCoroutine("LightGlow");
    }
}
