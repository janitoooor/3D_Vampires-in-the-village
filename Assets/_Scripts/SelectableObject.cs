using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    [SerializeField] private float _scaleObjectOnHover = 1.1f;
    [SerializeField] private GameObject _selectionIndicator;

    public virtual void Start()
    {
        _selectionIndicator.SetActive(false);
    }

    public virtual void OnHover()
    {
        transform.localScale = Vector3.one * _scaleObjectOnHover;
    }

    public virtual void UnOnHover()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void Select()
    {
        _selectionIndicator.SetActive(true);
    }

    public virtual void UnSelect()
    {
        _selectionIndicator.SetActive(false);
    }
}
