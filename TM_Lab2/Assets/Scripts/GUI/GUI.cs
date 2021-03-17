using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public CameraController cam;

	public CellEngine engine;
	public CellSpawner spawner;

	public StartPanel startPanel;
	public GamePanel gamePanel;

	void Start () {
		engine.updateFinish += gamePanel.UpdateFromGame;
		spawner.spawnProgress += startPanel.UpdateSpawningProgress;
		spawner.spawnFinish += startPanel.SpawningFinish;

		ShowPanel (0);
	}

	public void ShowPanel (int i) {
		if (i == 0) {
			gamePanel.Hide ();
			startPanel.Show ();
		} else {
			gamePanel.Show ();
			startPanel.Hide ();
		}
	}
}