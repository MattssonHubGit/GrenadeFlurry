using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    [SerializeField] private float boostAmount = 500f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerUnitController playerToBoost = other.gameObject.GetComponent<PlayerUnitController>();

        if (playerToBoost != null)
        {

            playerToBoost.Knockback(boostAmount, transform.forward, true);
        }
    }
}
