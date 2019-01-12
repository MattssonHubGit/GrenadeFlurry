using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCheckIfGrounded : MonoBehaviour
{
    [SerializeField] private PlayerUnitController checkerFor;

    private void OnTriggerStay(Collider other)
    {
        checkerFor.isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        checkerFor.isGrounded = false;
    }
}
