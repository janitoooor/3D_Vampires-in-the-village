using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableCollider : MonoBehaviour
{
    [SerializeField] private SelectableObject _selectableObject;
    public SelectableObject SelectableObject => _selectableObject;
}
