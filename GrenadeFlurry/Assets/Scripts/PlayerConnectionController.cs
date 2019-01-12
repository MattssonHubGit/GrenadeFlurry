using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionController : NetworkBehaviour
{
    [SerializeField] private GameObject playerUnitPrefab;
    [SerializeField] public PlayerUnitController myPlayerUnit;

    void Start()
    {
        //Only run on own client
        if (isLocalPlayer == false)
        {
            return;
        }

        //Spawn and set a player
        CmdSpawnPlayer();
    }

    //////////////////////////////Commands
    //Commands are run on the server only
    [Command]
    private void CmdSpawnPlayer()
    {
        GameObject _player = Instantiate(playerUnitPrefab);

        myPlayerUnit = _player.GetComponent<PlayerUnitController>();

        NetworkServer.SpawnWithClientAuthority(_player, connectionToClient);

    }
}
