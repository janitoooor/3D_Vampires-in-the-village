using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    public const string GROUND_TAG = "Ground";

    [SerializeField] private Camera _camera;

    private SelectableObject _hovered;
    public List<SelectableObject> _selectedObjectsList = new();

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            HoverObject(hit);
        else
            UnHoverCurrent();

        ClickOnObject(hit);

    }

    private void HoverObject(RaycastHit hit)
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

    private void ClickOnObject(RaycastHit hit)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_hovered)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                    UnSelectAll();

                SelectObject(_hovered);
            }

            if (hit.collider.CompareTag(GROUND_TAG))
                foreach (var item in _selectedObjectsList)
                    item.ClickOnGround(hit.point);
        }

        if (Input.GetMouseButtonDown(1))
            UnSelectAll();
    }

    private void UnSelectAll()
    {
        foreach (var item in _selectedObjectsList)
            item.UnSelect();

        _selectedObjectsList.Clear();
    }

    private void SelectObject(SelectableObject selectableObject)
    {
        if (!_selectedObjectsList.Contains(selectableObject))
        {
            _selectedObjectsList.Add(selectableObject);
            selectableObject.Select();
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
