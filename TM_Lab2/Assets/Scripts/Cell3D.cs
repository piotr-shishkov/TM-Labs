using UnityEngine;
using System.Collections;

public class Cell3D : Cell {

	public Material livingMaterial;
	public Material deadMaterial;

	private MeshRenderer meshRenderer;

	void Awake () {
		meshRenderer = GetComponent <MeshRenderer> ();
	}
		
	public override void UpdateLook () {
		if (state == States.Alive)
			meshRenderer.sharedMaterial = livingMaterial;
		else
			meshRenderer.sharedMaterial = deadMaterial;
	}
}
