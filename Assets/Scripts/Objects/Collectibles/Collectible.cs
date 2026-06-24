using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public virtual void Collect(CollectiblesDetector target)
    {
        ProcessCollect(target);
        gameObject.SetActive(false);
    }

    protected virtual void ProcessCollect(CollectiblesDetector target) { }
}
