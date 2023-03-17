using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampir : Unit
{
    private const string IS_MOVE_BOOL = "IsMove";

    [SerializeField] private Animator _animator;

    public override void Start()
    {
        base.Start();
        _animator.SetBool(IS_MOVE_BOOL, false);
    }

    private void Update()
    {

    }
}
