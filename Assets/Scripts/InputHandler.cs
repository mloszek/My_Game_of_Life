using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public UnityEvent OnStart;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnStart != null)
                OnStart.Invoke();
        }
    }

    private void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit)
        {
            Cell indicatedCell = hit.collider.gameObject.GetComponent<Cell>();

            if (indicatedCell)
                indicatedCell.ChangeCell();
        }
    }
}
