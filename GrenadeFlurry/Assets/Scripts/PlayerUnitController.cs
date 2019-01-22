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

    [Header("Grenades")]
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwPower;

    [Header("Components")]
    [SerializeField] private CharacterController cc;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] private Material clientOwnerMaterial;
    [SerializeField] private Camera myCam;
    [SerializeField] private Transform head;
    [SerializeField] private Transform gfx;
    [SerializeField] private KnockbackReciever kbr;

    private void Start()
    {
        if (!isServer)
        {
            CmdSetUpUnitOnLocal();
        }
        else
        {
            RpcSetUpUnitOnLocal();
        }

        //If this is not my player unit exit
        if (hasAuthority == false)
        {
            return;
        }
    }

    private void Update()
    {
        Debug.Log(NetworkServer.active.ToString());
        //If this is not my player unit
        if (hasAuthority == false)
        {
            return;
        }
        
        MovementController();
        GrenadeController();

    }

    private void FixedUpdate()
    {
        //If this is not my player unit
        if (hasAuthority == false)
        {
            return;
        }
    }
    
    private void GrenadeController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isServer)
            {
                CmdThrowGrenade(head.up);
            }
            else
            {
                RpcThrowGrenade(head.up);
            }
        }
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

        if (cc.isGrounded && Input.GetKey(KeyCode.Space)/* && !isBoosted*/)
        {
            Knockback(jumpPower, transform.up, false);
        }   

       // if (!isBoosted)
       // {
            cc.Move(move * Time.deltaTime);
       // }


    }

    public void Knockback(float force, Vector3 direction, bool isBoost)
    {
        kbr.AddImpact(force, direction, isBoost);
    }


    public void TakeDamage()
    {
        if (!isServer)
        {
            CmdRespawn();
        }
        else
        {
            RpcRespawn();
        }
    }


    //////////////////////////COMMAND
    //Commands are ran on the server
    [Command]
    private void CmdThrowGrenade(Vector3 dir)
    {
        /*GameObject _grenade = Instantiate(grenadePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody _grb = _grenade.GetComponent<Rigidbody>();
        _grb.AddForce(dir * throwPower, ForceMode.Impulse);

        NetworkServer.Spawn(_grenade);   */

        RpcThrowGrenade(dir);
    }

    [Command]
    private void CmdRespawn()
    {
        RpcRespawn();
    }

    [Command]
    private void CmdSetUpUnitOnLocal()
    {
        RpcSetUpUnitOnLocal();
    }

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

    [ClientRpc]
    private void RpcThrowGrenade(Vector3 dir)
    {
        if (!isServer)
        {
            return;
        }

        GameObject _grenade = Instantiate(grenadePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody _grb = _grenade.GetComponent<Rigidbody>();
        _grb.AddForce(dir * throwPower, ForceMode.Impulse);

        NetworkServer.Spawn(_grenade);
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        this.transform.position = RespawnPositions.Instance.spawnPositions[0].position;
    }
}
