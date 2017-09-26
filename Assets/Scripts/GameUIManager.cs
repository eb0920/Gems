using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	public RectTransform m_GamePanelRT;
	public RectTransform m_WavePanelRT;

	public GameObject m_WaveUIPrefab;
	public float m_PixelPerSeconds = 10f;
	float m_WavePanelOrginY = 0;
	int m_CurWave = -1;
	List<WaveInfoUI> m_WaveUIs = new List<WaveInfoUI>();

	void Start()
	{
		
	}

	void Update()
	{
		MoveWavePanel();
	}

	public void Init()
	{
		AdaptScreenSize();
		InitWaveUI();
	}

	#region Screen Adaption --------------------------------

	void AdaptScreenSize()
	{
	//	float pixelPerUnit = Screen.height / (Camera.main.orthographicSize * 2);
		float dwInPixel = Screen.width - (Screen.height * 4 / 3); //dwInPixel is positive by default
	//	float dwInUnit = dwInPixel / pixelPerUnit;

		// Camera
		//Camera.main.transform.position = new Vector3(
		//		Camera.main.orthographicSize * 4 / 3, // - (dwInUnit / 2),
		//		Camera.main.orthographicSize,
		//		-10
		//	);

		// UI Panel
		m_GamePanelRT.position = new Vector3(
				Screen.width - (dwInPixel / 2),
				Screen.height / 2,
				0
			);
	}

	#endregion
	
	#region Show level waves -------------------------------

	public void InitWaveUI()
	{
		WaveData data = GameManager.instance.waveData;

		m_WaveUIs.Clear();
		for (int i = 0; i < data.waveNumber; i++)
		{
			CreateWaveUI(i, data[i]);
		}

		m_CurWave = -1;
		m_WavePanelOrginY = m_WavePanelRT.localPosition.y;
	}

	void CreateWaveUI(int idx, _tWave w)
	{
		GameConfig cfg = GameManager.instance.configuration;
		float y = cfg.firstWaveTime * m_PixelPerSeconds;
		y += idx * (cfg.waveInterval * m_PixelPerSeconds);

		// Instantiate ui element
		var item = Instantiate(m_WaveUIPrefab, new Vector3(0, y), Quaternion.identity) as GameObject;
		item.transform.SetParent(m_WavePanelRT, false);
		
		// Initial ui element
		WaveInfoUI wiu = item.GetComponent<WaveInfoUI>();
		wiu.InitWithData(idx, w);

		m_WaveUIs.Add(wiu);
	}

	void MoveWavePanel()
	{
		Vector3 pos = m_WavePanelRT.localPosition;
		pos.y = m_WavePanelOrginY - (GameManager.instance.curWaveTime * m_PixelPerSeconds);

		m_WavePanelRT.localPosition = pos;

		if (m_CurWave < GameManager.instance.curWave)
		{
			m_CurWave = GameManager.instance.curWave;
			if (m_CurWave >= 0 && m_CurWave < m_WaveUIs.Count)
			{
				m_WaveUIs[m_CurWave].gameObject.SetActive(false);
			}
		}
	}

	#endregion
}
