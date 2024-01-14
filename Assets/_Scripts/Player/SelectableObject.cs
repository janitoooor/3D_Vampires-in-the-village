using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    [SerializeField] private GameObject _selectionIndicator;

    public virtual void Start()
    {
        _selectionIndicator.SetActive(false);
    }

    public virtual void OnHover()
    {

    }

    public virtual void UnOnHover()
    {

    }

    public virtual void Select()
    {
        _selectionIndicator.SetActive(true);
    }

    public virtual void UnSelect()
    {
        _selectionIndicator.SetActive(false);
    }

    public abstract void ClickOnHoverObject(IHoverObject hoverObject);
    public abstract void ClickOnGround(Vector3 point);
}
