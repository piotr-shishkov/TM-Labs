using UnityEngine;
using System.Collections;

public class GenericPanel : MonoBehaviour {

	public GUI gui;

	public void Show () {
		gameObject.SetActive (true);
	}

	public void Hide () {
		gameObject.SetActive (false);
	}
}
