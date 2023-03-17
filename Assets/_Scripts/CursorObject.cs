using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private SelectableObject _hovered;
    public List<SelectableObject> selectedObjects = new();

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.GetComponent<SelectableCollider>())
            {
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
                if (_hovered)
                {
                    if (_hovered != hitSelectable)
                    {
                        _hovered.UnOnHover();
                        _hovered = hitSelectable;
                        _hovered.OnHover();
                    }
                }
                else
                {
                    _hovered = hitSelectable;
                    _hovered.OnHover();
                }
            }
            else
            {
                UnHoverCurrent();
            }
        }
        else
        {
            UnHoverCurrent();
        }
    }

    private void UnHoverCurrent()
    {
        if (_hovered)
        {
            _hovered.UnOnHover();
            _hovered = null;
        }
    }
}
