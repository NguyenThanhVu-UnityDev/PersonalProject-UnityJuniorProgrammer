using UnityEngine;

public interface IHittable
{
    public void OnMinorHit(GameObject hitObj, Collider collider);
    public void OnMajorHit(GameObject hitObj);
}
