using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    [SerializeField] private float boostAmount = 5f;
    [SerializeField] private float boostDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerUnitController playerToBoost = other.gameObject.GetComponent<PlayerUnitController>();

        if (playerToBoost != null)
        {

            Rigidbody _rb = playerToBoost.GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.velocity = Vector3.zero;
                _rb.AddForce(this.transform.forward * boostAmount, ForceMode.Impulse);
                playerToBoost.isBoosted = true;
                playerToBoost.SetBoost(boostDuration);
            }
        }
    }
}
