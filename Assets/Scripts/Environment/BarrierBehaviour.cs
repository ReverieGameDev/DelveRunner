using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseBarrierAnim()
    {
        anim.speed = 0;
    }

    public void RetractSpikesBarrierAnim()
    {
        anim.speed = 1;
    }

    public void DestroyBarrier()
    {
        Destroy(gameObject);
        Destroy(transform.parent.gameObject);
    }
}
