using UnityEngine;
using System.Collections;

public class Cell2D : Cell {

	public Color aliveColor;
	public Color deadColor;

	private SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public override void UpdateLook() {
		if (state == States.Alive)
			spriteRenderer.color = aliveColor;
		else
			spriteRenderer.color = deadColor;
	}
}
