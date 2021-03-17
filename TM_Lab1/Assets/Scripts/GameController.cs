using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[Header("GamePlay Variables & References")]
	public GameData PlayerGameData;
	[Space]
	public StateScriptableObject StartState;
	[Space]
	public GameObject MenuPanel;
	public GameObject GamePanel;
	public GameObject GlobalFaderPanel;
	[Space]
	public AudioSource GameAmbientAudioSource;
	public AudioSource GameMusicAudioSource;
	public AudioSource SoundAudioSource;
	public AudioController AudioController;

	[Header("Menu Variables & References")]
	public GameObject ButtonsTab;
	public GameObject SettingsTab;
	[Space]
	public Button ContinueButton;
	[Space]
	public Slider SoundVolumeSlider;
	public Slider MusicVolumeSlider;

	[Header("Game Variables & References")]
	public Image BackgroundImage;
	public GameObject LeftPersonHolder;
	public GameObject LeftReplicaHolder;
	public Image LeftPersonImage;
	public GameObject RightPersonHolder;
	public GameObject RightReplicaHolder;
	public Image RightPersonImage;
	public Text LeftPersonName;
	public Text LeftPersonText;
	public Text RightPersonName;
	public Text RightPersonText;
	public GameObject HintContentGameObject;
	public GameObject AnswersContentGameObject;
	[Space]
	public GameObject AnswerVariantPrefab;

	[SerializeField]
	private int currentReplica;
	private StateScriptableObject currentState;
	[SerializeField]
	private Coroutine textFillerCoroutine;
	[SerializeField]
	private bool replicaEnded = false;
	[SerializeField]
	private bool textFilling = false;
	private Text tempTextFillText;
	private string tempTextFillContent;

	void Start()
	{
		LoadData();
		SaveData();
		InitMenu();
	}

	void Update()
	{
		if (textFilling && Input.GetMouseButtonDown(0))
		{
			SkipReplica();
		}
		else if (replicaEnded && Input.GetMouseButtonDown(0))
		{
			LoadReplica();
		}
	}

	#region Game

	public void LoadState()
	{
		currentState = PlayerGameData.LastStateData;
		BackgroundImage.sprite = currentState.BackGroundSprite;

		LeftPersonText.text = "";
		RightPersonText.text = "";
		LeftPersonName.text = "";
		RightPersonName.text = "";
		LeftPersonHolder.SetActive(false);
		LeftReplicaHolder.SetActive(false);
		RightPersonHolder.SetActive(false);
		RightReplicaHolder.SetActive(false);
		AnswersContentGameObject.SetActive(false);
		HintContentGameObject.SetActive(true);

		//LeftPersonHolder.SetActive(state.LeftPersonSprite != null);
		//if (state.LeftPersonSprite != null)
		//{
		//	if (string.IsNullOrEmpty(state.LeftReplica)) { LeftReplicaHolder.SetActive(false); }
		//	else
		//	{
		//		LeftReplicaHolder.SetActive(true);
		//		LeftPersonName.text = state.LeftName;
		//		LeftPersonImage.sprite = state.LeftPersonSprite;
		//		LeftPersonText.text = state.LeftReplica;
		//	}
		//}

		//RightPersonHolder.SetActive(state.RightPersonSprite != null);
		//if (state.RightPersonSprite != null)
		//{
		//	if (string.IsNullOrEmpty(state.RightReplica)) { RightReplicaHolder.SetActive(false); }
		//	else
		//	{
		//		RightReplicaHolder.SetActive(true);
		//		RightPersonName.text = state.RightName;
		//		RightPersonImage.sprite = state.RightPersonSprite;
		//		RightPersonText.text = state.RightReplica;
		//	}
		//}

		foreach (Transform child in AnswersContentGameObject.transform) Destroy(child.gameObject);

		if (currentState.StateAnswers.Length > 0)
		{
			foreach (var answer in currentState.StateAnswers)
			{
				var answItem = Instantiate(AnswerVariantPrefab, AnswersContentGameObject.transform);
				var answText = answItem.GetComponentInChildren<Text>();
				var eventTrigger = answText.GetComponent<EventTrigger>();

				answText.text = answer.AnswerText;
				var entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener((eventData) => { SetNextState(answer.NextState); });
				eventTrigger.triggers.Add(entry);
			}
		}
		else
		{
			var answItem = Instantiate(AnswerVariantPrefab, AnswersContentGameObject.transform);
			var answText = answItem.GetComponentInChildren<Text>();
			var eventTrigger = answText.GetComponent<EventTrigger>();

			answText.text = "Game Over. Back to Menu!";
			var entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener((eventData) => { StartCoroutine(BackToMenu()); });
			eventTrigger.triggers.Add(entry);
		}

		AnswersContentGameObject.GetComponent<ContentSizeFitter>().SetLayoutVertical();

		if (currentState.StateAmbient != GameAmbientAudioSource.clip)
		{
			GameAmbientAudioSource.Stop();
			GameAmbientAudioSource.clip = currentState.StateAmbient;
			GameAmbientAudioSource.Play();
		}

		currentReplica = -1;
		LoadReplica();
	}

	public void LoadReplica()
	{
		replicaEnded = false;
		currentReplica += 1;
		if (currentReplica > currentState.Replicas.Length - 1)
		{
			HintContentGameObject.SetActive(false);
			AnswersContentGameObject.SetActive(true);
			return;
		}
		LeftReplicaHolder.SetActive(false);
		RightReplicaHolder.SetActive(false);

		var replica = currentState.Replicas[currentReplica];
		if (replica.SoundReplica.SoundClip == null)
		{
			var hasText = !string.IsNullOrEmpty(replica.CharReplica);
			if (replica.Side == ReplicaSide.Left)
			{
				LeftPersonHolder.SetActive(replica.CharReplica != "HIDE");
				if (hasText) LeftReplicaHolder.SetActive(replica.CharReplica != "HIDE");
				LeftPersonName.text = replica.CharName;
				LeftPersonImage.sprite = replica.CharSprite;
			}
			else
			{
				RightPersonHolder.SetActive(replica.CharReplica != "HIDE");
				if (hasText) RightReplicaHolder.SetActive(replica.CharReplica != "HIDE");
				RightPersonName.text = replica.CharName;
				RightPersonImage.sprite = replica.CharSprite;
			}

			if (hasText)
			{
				if (replica.CharReplica == "HIDE") { LoadReplica(); return; }
				textFillerCoroutine = StartCoroutine(TextFiller(replica.Side, replica.CharReplica));
			}
		}
		else
		{
			SoundAudioSource.PlayOneShot(replica.SoundReplica.SoundClip);
			Invoke(nameof(LoadReplica), replica.SoundReplica.SoundClip.length);
		}
	}

	private void EnableSkip()
	{
		replicaEnded = true;
	}

	private void SkipReplica()
	{
		textFilling = false;
		if (textFillerCoroutine != null)
			StopCoroutine(textFillerCoroutine);
		textFillerCoroutine = null;
		tempTextFillText.text = tempTextFillContent;
		replicaEnded = true;
	}

	public IEnumerator TextFiller(ReplicaSide side, string replicaText)
	{
		var textElement = side == ReplicaSide.Left ? LeftPersonText : RightPersonText;
		textElement.text = "";
		tempTextFillText = textElement;
		tempTextFillContent = replicaText;
		textFilling = true;
		foreach (var c in replicaText)
		{
			textElement.text += c;
			yield return new WaitForSeconds(0.05f);
		}
		textFilling = false;
		replicaEnded = true;
	}

	public void SetNextState(StateScriptableObject state)
	{
		AudioController.PlaySound("Swipe");
		PlayerGameData.LastStateData = state;
		StartCoroutine(ChangeState());
	}

	public IEnumerator ChangeState()
	{
		GlobalFaderPanel.GetComponent<Animator>().SetTrigger("Fade");
		yield return new WaitForSeconds(1f);
		LoadState();
	}

	public IEnumerator BackToMenu()
	{
		GameMusicAudioSource.enabled = true;
		GameAmbientAudioSource.enabled = false;
		AudioController.PlaySound("Swipe");
		GlobalFaderPanel.GetComponent<Animator>().SetTrigger("Fade");
		yield return new WaitForSeconds(1f);
		GamePanel.SetActive(false);
		MenuPanel.SetActive(true);
		InitMenu();
	}

	#endregion

	#region Menu

	public void InitMenu()
	{
		SoundVolumeSlider.value = PlayerGameData.SoundVolumeData;
		AudioController.SoundAudioSource.volume = PlayerGameData.SoundVolumeData;
		MusicVolumeSlider.value = PlayerGameData.MusicVolumeData;
		GameMusicAudioSource.volume = PlayerGameData.MusicVolumeData;

		if (PlayerGameData.LastStateData != StartState) ContinueButton.interactable = true;
	}

	public void ContinueGame()
	{
		AudioController.PlaySound("Button Click");
		StartCoroutine(FadeToGame());
	}

	public void StartNewGame()
	{
		AudioController.PlaySound("Button Click");
		PlayerGameData.LastStateData = StartState;
		StartCoroutine(FadeToGame());
	}

	public void ToggleSettings(bool state)
	{
		AudioController.PlaySound("Button Click");
		ButtonsTab.SetActive(!state);
		SettingsTab.SetActive(state);
		if (!state) SaveData();
	}

	public void OnSoundVolumeChanged()
	{
		AudioController.PlaySound("Button Click");
		AudioController.SoundAudioSource.volume = SoundVolumeSlider.value;
		PlayerGameData.SoundVolumeData = SoundVolumeSlider.value;
		GameAmbientAudioSource.volume = SoundAudioSource.volume / 3;
	}

	public void OnMusicVolumeChanged()
	{
		GameMusicAudioSource.volume = MusicVolumeSlider.value;
		PlayerGameData.MusicVolumeData = MusicVolumeSlider.value;
	}

	public void QuitGame()
	{
		AudioController.PlaySound("Button Click");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.ExitPlaymode();
#else
	Application.Quit();
#endif
	}

	IEnumerator FadeToGame()
	{
		GameMusicAudioSource.enabled = false;
		GameAmbientAudioSource.enabled = true;
		GlobalFaderPanel.GetComponent<Animator>().SetTrigger("Fade");
		yield return new WaitForSeconds(1f);
		LoadState();
		MenuPanel.SetActive(false);
		GamePanel.SetActive(true);
	}

	#endregion

	private void SaveData()
	{
		var data = JsonUtility.ToJson(PlayerGameData);
		PlayerPrefs.SetString("SaveData", data);
	}

	private void LoadData()
	{
		if (PlayerPrefs.HasKey("SaveData"))
		{
			PlayerGameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("SaveData"));
		}
		else
		{
			PlayerGameData.LastStateData = StartState;
			PlayerGameData.SoundVolumeData = 1;
			PlayerGameData.MusicVolumeData = 1;
		}
	}

	private void OnApplicationQuit()
	{
		SaveData();
	}
}

[Serializable]
public class GameData
{
	public StateScriptableObject LastStateData;
	public float MusicVolumeData;
	public float SoundVolumeData;
}