using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private LookAtCameraMode _mode;

    private void LateUpdate()
    {

        switch (_mode)
        {
            case LookAtCameraMode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case LookAtCameraMode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.forward;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case LookAtCameraMode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case LookAtCameraMode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
