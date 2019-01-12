using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionController : NetworkBehaviour
{
    [SerializeField] private GameObject playerUnitPrefab;

    void Start()
    {

        //Only run on own client
        if (isLocalPlayer == false)
        {
            return;
        }

        //Spawn and set a player
        CmdSpawnPlayer(this.gameObject);
    }

    //////////////////////////////Commands
    //Commands are run on the server only
    [Command]
    private void CmdSpawnPlayer(GameObject spawner)
    {
        GameObject _player = Instantiate(playerUnitPrefab);
        

        NetworkServer.SpawnWithClientAuthority(_player, connectionToClient);
    }

    ///////////////////////////RPCs
    //RPCs are called on all clients induvidually

    
}
