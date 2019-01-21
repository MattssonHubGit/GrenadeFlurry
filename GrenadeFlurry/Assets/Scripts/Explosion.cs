using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Explosion : NetworkBehaviour
{
    [SerializeField] private float lifeSpan = 3f;
    private float currentLifeCounter = 0f;

    void Update()
    {
        if (currentLifeCounter >= lifeSpan)
        {
            Destroy(this.gameObject);
        }
        else
        {
            currentLifeCounter += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerUnitController _hitPlayer = other.gameObject.GetComponent<PlayerUnitController>();

        if(_hitPlayer != null)
        {
            _hitPlayer.TakeDamage();
        }
    }
}
