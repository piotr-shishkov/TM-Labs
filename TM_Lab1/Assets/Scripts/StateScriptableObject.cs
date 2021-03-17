using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StateScriptableObject", order = 1)]
public class StateScriptableObject : ScriptableObject
{
	[Header("Main Vars")]
	public Sprite BackGroundSprite;
	[Header("Replicas")]
	public Replica[] Replicas;
	[Header("Answers")]
	public Answer[] StateAnswers;
	[Header("Misc")]
	public AudioClip StateAmbient;
}

[Serializable]
public class Replica
{
	public string CharName;
	public ReplicaSide Side;
	[TextArea(1, 5)]
	public string CharReplica;
	public Sprite CharSprite;
	public SoundReplica SoundReplica;
}

[Serializable]
public class SoundReplica
{
	public string SoundCaption;
	public AudioClip SoundClip;
}

public enum ReplicaSide
{
	Left=1,
	Right=2,
}

[Serializable]
public class Answer
{
	public string AnswerText;
	public StateScriptableObject NextState;
}