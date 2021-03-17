using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStateChanger : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public CellEngine cellEngine;

    public Camera cam;

    private void Start()
    {
	    cam = Camera.main;
    }

    void Update()
    {
        if(cellEngine.state == CellEngine.States.Running) return;

        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
	        if (Input.GetMouseButtonDown(0))
	        {
                var cell =  hit.transform.gameObject.GetComponent<Cell3D>();
		        if (cell)
		        {
                    cell.ToggleState();
                    cell.UpdateLook();
		        }
	        }
        }
    }
}
