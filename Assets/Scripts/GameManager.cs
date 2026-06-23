using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float acceleration;

    private void Update()
    {
        if (PlayerController.CurrentPlayer != null)
        {
            PlayerController.CurrentPlayer.AddRunSpeed(acceleration * Time.deltaTime);
        }
    }
}
