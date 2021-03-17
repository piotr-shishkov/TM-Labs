using UnityEngine;
using System.Collections;

public abstract class Cell : MonoBehaviour {

	public enum States {
		Dead, Alive
	}

	public int x, y;
	public Cell[] neighbours;

	public States state;
	private States nextState;
		
	public void CellUpdate () {
		nextState = state;
		int aliveCells = GetAliveCells ();
		if (state == States.Alive) {
			if (aliveCells != 2 && aliveCells != 3) 
				nextState = States.Dead;
		} else { 
			if (aliveCells == 3) 
				nextState = States.Alive;
		}
	}
		
	public void CellApplyUpdate () {
		state = nextState;
		UpdateLook ();
	}
		
	public void Init (Transform parent, int x, int y) {
		transform.parent = parent;

		this.x = x;
		this.y = y;
	}
		
	public void SetRandomState () {
		state = (Random.Range (0, 2) == 0) ? States.Dead : States.Alive;
		UpdateLook ();
	}

	public void SetState (States s) {
		state = s;
	}

	public void ToggleState () {
		if (state == States.Alive)
			state = States.Dead;
		else
			state = States.Alive;
	}
		
	public abstract void UpdateLook ();
		
	private int GetAliveCells () {
		int ret = 0;
		for (int i = 0; i < neighbours.Length; i++) {
			if (neighbours [i].state == States.Alive)
					ret++;
		}
		return ret;
	}
}
