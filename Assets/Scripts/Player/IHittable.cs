using UnityEngine;

public interface IHittable
{
    public void OnMinorHit(GameObject hitObj);
    public void OnMajorHit(GameObject hitObj);
}
