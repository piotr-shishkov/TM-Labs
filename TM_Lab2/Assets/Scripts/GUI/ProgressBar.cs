using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public Slider slider;
	public Text text;

	public void SetProgress (float p) {
		slider.value = p;
		text.text = string.Format ("Instantiating cells... {0}%", p);
	}

	public void Show () {
		gameObject.SetActive (true);
	}

	public void Hide () {
		gameObject.SetActive (false);
	}
}
