using UnityEngine;

public class ObstacleColliderRebuild : MonoBehaviour
{
    public void Rebuild()
    {
        GetComponent<CompositeCollider2D>().GenerateGeometry();
    }
}