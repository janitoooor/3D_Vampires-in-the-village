using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = PlayerVampire.Instance.gameObject.transform.position + new Vector3(-32, 18, 17);
    }
}
