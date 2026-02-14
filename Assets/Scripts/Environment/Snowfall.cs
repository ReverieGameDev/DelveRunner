using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Snowfall : MonoBehaviour
{
    public List<Sprite> snowParticleSprites = new List<Sprite>();
    public GameObject snowParticle;
    private Sprite randomSnowParticle;
    private Vector3 snowSpawnLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("MakeItSnow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MakeItSnow()
    {
        snowSpawnLocation = new Vector3(-48, 37, 0);
        for (int i = 0; i < 150; i++)
        {
            snowSpawnLocation.x++;
            randomSnowParticle = snowParticleSprites[Random.Range(0, snowParticleSprites.Count)];
            snowParticle.GetComponent<SpriteRenderer>().sprite = randomSnowParticle;
            Instantiate(snowParticle, snowSpawnLocation, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("MakeItSnow");
    }
}
