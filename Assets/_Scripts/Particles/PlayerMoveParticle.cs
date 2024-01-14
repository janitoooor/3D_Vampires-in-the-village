using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveParticle : MonoBehaviour
{
    private void Update()
    {
        transform.position = PlayerVampire.Instance.transform.position;
    }
}
