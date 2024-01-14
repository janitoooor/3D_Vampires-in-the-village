using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPointObject : MonoBehaviour
{
    private const string CLICK_TRIGGER = "Click";

    [SerializeField] private Animator _animator;

    public void ClickOnPoint(Vector3 position)
    {
        transform.position = position;
        _animator.SetTrigger(CLICK_TRIGGER);
    }
}
