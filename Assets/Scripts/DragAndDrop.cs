using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Vector3 mousePositionOffset;

    public bool allowed;

    private Vector3 GetMouseWorldPosition()
    {
        //capture mouse position & return WorldPoint
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        if (!allowed) return;

        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }
    private void OnMouseDrag()
    {
        if (!allowed) return;

        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
}
