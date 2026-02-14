using UnityEngine;

public class RandomAnimationStartTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int randomRotation;
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, 0, Random.Range(0f, 1f));
        transform.localScale= transform.localScale * (Random.Range(.75f, 1.25f));
        randomRotation = Random.Range(0, 1);
        if (randomRotation == 0)
        {
            transform.Rotate(0, 180, 0);
        }
        else if (randomRotation == 1)
        {
            transform.Rotate(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
