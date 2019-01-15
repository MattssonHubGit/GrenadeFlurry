using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnitController : NetworkBehaviour, IKnockbackable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float jumpPower;
    [HideInInspector] public bool isBoosted = false;


    [Header("Components")]
    //[SerializeField] private Rigidbody rb;
    [SerializeField] private CharacterController cc;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] private Material clientOwnerMaterial;
    [SerializeField] private Camera myCam;
    [SerializeField] private Transform head;
    [SerializeField] private Transform gfx;
    [SerializeField] private KnockbackReciever kbr;

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

        //Debug.Log("Is boosted: " + isBoosted + " Duration: " + boostDuration + " current: " + currentBoostTimer);
        MovementController();
        //BoostedTimer();
    }

    private void FixedUpdate()
    {
        //If this is not my player unit
        if (hasAuthority == false)
        {
            return;
        }
        //LimitMaxSpeed();
    }
    
    private void MovementController()
    {

        gfx.eulerAngles = new Vector3(0, myCam.transform.eulerAngles.y, 0);

        float x = 0;
        float z = 0;


        if (Input.GetKey(KeyCode.W)) //Forward
        {
            z++;
        }
        if (Input.GetKey(KeyCode.S)) //Backward
        {
            z--;
        }
        if (Input.GetKey(KeyCode.D)) //Right
        {
            x++;
        }
        if (Input.GetKey(KeyCode.A)) //Left
        {
            x--;
        }

        Vector3 move = new Vector3(x, -fallSpeed, z) * moveSpeed;
        move = gfx.transform.TransformDirection(move);

        if (cc.isGrounded && Input.GetKey(KeyCode.Space) && !isBoosted)
        {
            Knockback(jumpPower, transform.up, false);
        }   

        if (!isBoosted)
        {
            cc.Move(move * Time.deltaTime);
        }


        /*if (!isGrounded)
        {
            cc.Move(Vector3.down * fallSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W)) //Forward
        {
            cc.Move(gfx.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) //Backward
        {
            cc.Move(gfx.forward * -moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) //Right
        {
            cc.Move(gfx.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) //Left
        {
            cc.Move(gfx.right * -moveSpeed * Time.deltaTime);
        }*/

        #region legacy
        /*//direction = transform.InverseTransformDirection(rb.velocity);



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

            rb.AddForce(gfx.forward * moveSpeed * Time.deltaTime, ForceMode.Force);


        }
        if (Input.GetKey(KeyCode.S) && isGrounded)
        {

            rb.AddForce(gfx.forward * -moveSpeed * Time.deltaTime, ForceMode.Force);

        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded) 
        {
            rb.velocity = Vector3.zero;
        }*/
        #endregion
    }

    public void Knockback(float force, Vector3 direction, bool isBoost)
    {
        kbr.AddImpact(force, direction, isBoost);
    }

    /*private void BoostedTimer()
    {
        if (isBoosted)
        {
            if (currentBoostTimer > boostDuration)
            {
                isBoosted = false;
                currentBoostTimer = 0;
                boostDuration = 0;
            }
            else
            {
                currentBoostTimer += Time.deltaTime;
            }
        }
    }

    public void SetBoost(float time)
    {
        boostDuration = time;
        currentBoostTimer = 0;

    }*/

    /*private void LimitMaxSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed && !isBoosted)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }*/

    
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
