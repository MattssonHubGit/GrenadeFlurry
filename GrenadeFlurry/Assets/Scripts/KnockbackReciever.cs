using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackReciever : MonoBehaviour
{
    [SerializeField] private float mass = 1.0f; // defines the character mass
    private Vector3 impact = Vector3.zero;
    private CharacterController cc;
    private bool beingKnocked = false;
    private PlayerUnitController player;
    [SerializeField] private float magLimit = 0.2f;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        player = GetComponent<PlayerUnitController>();
    }

    private void Update()
    {
        if (beingKnocked == true)
        {
            KnockbackExecution();
        }
    }

    private void KnockbackExecution()
    {

        if (impact.magnitude > magLimit)
        {
            if (!cc.isGrounded && player.isBoosted)
            {
                impact.y -= 50f*Time.deltaTime;
                Debug.Log(impact.y);
            }
            cc.Move(impact * Time.deltaTime); // apply the impact force:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime); // consumes the impact energy each cycle:
        }
        else
        {
            beingKnocked = false;
            player.isBoosted = false;
        }
    }


    /// <summary>
    /// Adds knockback in similar way to force
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    public void AddImpact(float force, Vector3 dir, bool isBoost)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * (force * 0.1f) / mass;
        beingKnocked = true;
        player.isBoosted = isBoost;
    }
}
