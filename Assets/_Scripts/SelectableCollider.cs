using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SelectableCollider : MonoBehaviour
{
    [SerializeField] private SelectableObject _selectableObject;
    public SelectableObject SelectableObject => _selectableObject;
}
