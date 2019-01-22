using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grenade : NetworkBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private void Start()
    {
        Destroy(this.gameObject, 30f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isServer == false)
        {
            CmdCreateExplosion(this.transform.position);
        }
        else
        {
            RpcCreateExplosion(this.transform.position);
        }

    }

    [ClientRpc]
    private void RpcCreateExplosion(Vector3 pos)
    {
        if (!isServer)
        {
            return;
        }
        GameObject _explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);

        NetworkServer.Spawn(_explosion);
        Destroy(this.gameObject);
    }

    [Command]
    private void CmdCreateExplosion(Vector3 pos)
    {
        RpcCreateExplosion(pos);
    }

}
