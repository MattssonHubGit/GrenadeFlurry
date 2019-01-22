using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPositions : MonoBehaviour
{
    [SerializeField] public List<Transform> spawnPositions = new List<Transform>();

    public static RespawnPositions Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
