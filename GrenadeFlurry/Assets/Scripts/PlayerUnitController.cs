using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnitController : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [HideInInspector] public bool isGrounded = true;
    [SerializeField] private float maxSpeed;
    [HideInInspector] public bool isBoosted = false;

    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] private Material clientOwnerMaterial;
    [SerializeField] private Camera myCam;
    [SerializeField] private Transform head;
    [SerializeField] private Transform gfx;

    private void Start()
    {

        RpcSetUpUnitOnLocal();
        //If this is not my player unit exit
        if (hasAuthority == false)
        {
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

    private void FixedUpdate()
    {
        //If this is not my player unit
        if (hasAuthority == false)
        {
            return;
        }
        LimitMaxSpeed();
    }

    private void MovementController()
    {
        gfx.eulerAngles = new Vector3(0, myCam.transform.eulerAngles.y, 0);

        if (Input.GetKey(KeyCode.D) && isGrounded)
        {
            rb.AddForce(myCam.transform.right * moveSpeed * Time.deltaTime, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A) && isGrounded)
        {
            rb.AddForce(myCam.transform.right * -moveSpeed * Time.deltaTime, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (!isGrounded)
            {
                rb.AddForce(myCam.transform.forward * moveSpeed * Time.deltaTime, ForceMode.Force);
            }
            else
            {
                rb.AddForce(gfx.forward * moveSpeed * Time.deltaTime, ForceMode.Force);
            }
        }
        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            rb.AddForce(myCam.transform.forward * -moveSpeed * Time.deltaTime, ForceMode.Force);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void LimitMaxSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed && !isBoosted)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    
    //////////////////////////COMMAND
    //Commands are ran on the server


    /////////////////////////RPC
    //RPCs are called on all clients induvidually
    [ClientRpc]
    private void RpcSetUpUnitOnLocal()
    {
        //If this is not my player unit
        if (hasAuthority == false)
        {
            myCam.enabled = false;
            myCam.gameObject.SetActive(true);
            return;
        }

        meshRenderer.material = clientOwnerMaterial;
        myCam.gameObject.SetActive(true);
        myCam.GetComponent<SmoothMouseLook>().allowedControlByCurrentClient = true;
    }


}
