using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDisableCursorOnStart : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.Keypad1;

    void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            Cursor.visible = !Cursor.visible;
        }
    }
}
