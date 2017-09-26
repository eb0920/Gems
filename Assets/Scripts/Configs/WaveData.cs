using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class _tWaveMonster
{
	public int id;
	public int number;
}

[Serializable]
public class _tWave
{
	public _tWaveMonster[] monsters;

	public int monsterNumber { get { return monsters.Length; } }
	public _tWaveMonster this[int idx]
	{
		get
		{
			if (idx < monsterNumber)
				return monsters[idx];
			else
				return null;
		}
	}
}

[CreateAssetMenuAttribute(menuName = "Custom/WaveData")]
public class WaveData : ScriptableObject
{
	public _tWave[] waves;
	
	// get wave number
	public int waveNumber
	{
		get
		{
			return waves.Length;
		}
	}

	// get wave
	public _tWave this[int idx]
	{
		get
		{
			if (idx < waveNumber)
				return waves[idx];
			else
				return null;
		}
	}
}
