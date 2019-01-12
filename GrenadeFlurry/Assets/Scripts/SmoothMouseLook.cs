using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [Range(0f, 10f)] [SerializeField] private float sensitivityX = 2f;
    [Range(0f, 10f)] [SerializeField] private float sensitivityY = 2f;

    [Header("Restrictions")]
    [SerializeField] private RotationAxes axes = RotationAxes.MouseXAndY;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

    [Space]

    [Range(-360f, 360f)] [SerializeField] private float minimumX = -360f;
    [Range(-360f, 360f)] [SerializeField] private float maximumX = 360f;
    [Range(-360f, 360f)] [SerializeField] private float minimumY = -60f;
    [Range(-360f, 360f)] [SerializeField] private float maximumY = 60f;

    [Header("Misc")]
    [Range(-0f, 20f)] [SerializeField] private float smootheningFrames = 5f;
    public bool allowedControlByCurrentClient = false;

    [Header("Trackers")]
    private float rotationX = 0f;
    private float rotationY = 0f;

    private List<float> rotArrayX = new List<float>();
    private float rotAverageX = 0f;

    private List<float> rotArrayY = new List<float>();
    private float rotAverageY = 0f;

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        //Only work for owner of unit
        if (allowedControlByCurrentClient == false)
        {
            return;
        }

        RotationController();
    }

    private void RotationController()
    {
        rotAverageY = 0f;
        rotAverageX = 0f;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;

        rotArrayY.Add(rotationY);
        rotArrayX.Add(rotationX);

        if (rotArrayY.Count >= smootheningFrames)
        {
            rotArrayY.RemoveAt(0);
        }
        if (rotArrayX.Count >= smootheningFrames)
        {
            rotArrayX.RemoveAt(0);
        }

        for (int j = 0; j < rotArrayY.Count; j++)
        {
            rotAverageY += rotArrayY[j];
        }
        for (int i = 0; i < rotArrayX.Count; i++)
        {
            rotAverageX += rotArrayX[i];
        }

        rotAverageY /= rotArrayY.Count;
        rotAverageX /= rotArrayX.Count;

        rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
        rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

        Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

        if (axes == RotationAxes.MouseXAndY)
        {
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        if (axes == RotationAxes.MouseX)
        {
            transform.localRotation = originalRotation * xQuaternion;
        }
        if (axes == RotationAxes.MouseY)
        {
            transform.localRotation = originalRotation * yQuaternion;
        }

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

}
