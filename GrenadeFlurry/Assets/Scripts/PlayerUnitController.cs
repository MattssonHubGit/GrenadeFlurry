using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnitController : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
   

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] private Material clientOwnerMaterial;

    private void Start()
    {
        //If this is not my player unit
        if (hasAuthority == false)
        {
            Debug.Log("PlayerUnitController::Start() - hasAuthority == false");
            meshRenderer.material = clientOwnerMaterial;
            return;
        }

    }

    private void Update()
    {
        //If this is not my player unit
        if (hasAuthority == false)
        {
            return;
        }

        MovementController();

    }

    private void MovementController()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * moveSpeed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(transform.right * -moveSpeed, ForceMode.Force);
        }
    }
}
