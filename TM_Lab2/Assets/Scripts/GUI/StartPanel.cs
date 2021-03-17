using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartPanel : GenericPanel {

	public Button startButton;
	public Dropdown sizeDropDown;
	public Text progressText;

	void Start () {
		FillSizeDropDown (gui.spawner.sizes);
		progressText.text = "Press 'Generate Map' to start";
	}

	public void FillSizeDropDown (int[] values) {
		foreach (int v in values) {
			sizeDropDown.options.Add (new Dropdown.OptionData (string.Format("{0}x{0}", v)));
		}
		sizeDropDown.value = 0;
		sizeDropDown.value = 5;
	}

	public void UpdateSpawningProgress (int p) {
		progressText.text = $"Loading: {p}%";
	}

	public void SpawningFinish () {
		Invoke ("StartGame", 1f);
	}

	public void StartGame () {
		gui.ShowPanel (1);
		gui.cam.MoveToCenter(gui.spawner.size);
	}

	public void StartButtonClick () {
		sizeDropDown.interactable = false;
		startButton.interactable = false;
		gui.spawner.StartSpawning (sizeDropDown.value);
	}
}
