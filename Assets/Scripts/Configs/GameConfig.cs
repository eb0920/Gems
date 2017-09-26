using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Custom/GameConfig")]
public class GameConfig : ScriptableObject
{
	public float waveInterval = 20f;
	public float firstWaveTime = 20f;
	public float minDelaySeconds = 0.8f;
	public float maxDelaySeconds = 1.5f;
}
