using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public GameObject m_ThiefPrefab;
	public GameObject m_TowerPrefab;

	GemManager[] m_Gems;
	
	private bool m_LoseGame = false;

	public Vector3 defaultGemPosition { private set; get; }
	public MapManager mapManager { private set; get; }
	public PathFinder pathHelper { private set; get; }
	public GemManager[] gems { get { return m_Gems; } }

	GameUIManager m_UIMng;
	
	// Waves
	WaveData m_WaveData;
	public WaveData waveData { get { return m_WaveData; } }

//	float m_TotalWaveTime = 0;
	float m_CurrentWaveTime = 0;
	int m_CurrentWave = -1;

	public float curWaveTime { get { return m_CurrentWaveTime; } }
	public int curWave { get { return m_CurrentWave; } }

	//
	GameConfig m_Configuration;
	public GameConfig configuration {  get { return m_Configuration; } }

	void Awake()
	{
		// 简单的赋值，暂时不考虑异常情况
		instance = this;

		mapManager = GetComponent<MapManager>();
		pathHelper = GetComponent<PathFinder>();
		m_UIMng = GetComponent<GameUIManager>();

		// 暂时这样获取宝石的列表
		var gems = GameObject.Find("Gems");
		if (gems != null)
		{
			m_Gems = gems.GetComponentsInChildren<GemManager>();
		}

		if (m_Gems.Length > 0)
			defaultGemPosition = m_Gems[0].transform.position;
		else
			defaultGemPosition = Vector3.zero;
	}

	void Start()
	{
		//mapManager.Init();
		//pathHelper.Init();
		MapManager.Instance.LoadTerrain();

		m_Configuration = Resources.Load<GameConfig>("GameConfiguration");
		m_WaveData = Resources.Load<WaveData>("WaveData01");

	//	m_TotalWaveTime = configuration.firstWaveTime + (waveData.waveNumber - 1) * configuration.waveInterval;
		
		m_UIMng.Init();
	}

	
	void Update()
	{

		UpdateWave();

		bool loseAll = true;
		foreach(var gem in m_Gems)
		{
			if (!gem.stolen)
			{
				loseAll = false;
				break;
			}
		}

		if (loseAll)
		{
			m_LoseGame = true;
			Debug.Log("You lose.");
		}
	}
	
	public GemManager GetVisibleGem()
	{
		foreach (var gm in m_Gems)
		{
			if (gm.isVisible)
				return gm;
		}

		return null;
	}

	public void TryCreateTower(Vector3 position, int towerType)
	{
		var tile = mapManager.GetTile(position);
		if (tile == null || tile.tower != null)
			return;

		var tower = Instantiate(m_TowerPrefab, tile.transform.position, Quaternion.identity);
		tile.tower = tower;
	}

	

	#region waves --------------------------------------------

	void UpdateWave()
	{
		m_CurrentWaveTime += Time.deltaTime;

		int wave = -1;
		float ft = m_CurrentWaveTime - configuration.firstWaveTime;
		if (ft >= 0)
		{
			wave = (int)(ft / configuration.waveInterval);
		}
			
		if (wave > m_CurrentWave)
		{
			NewWave(wave);
		}
	}

	void NewWave(int wave)
	{
		if (wave < 0 || wave >= waveData.waveNumber)
			return;

		m_CurrentWave = wave;

		float delaySeconds = 0f;
		GameObject thieves = GameObject.Find("Thieves");
		_tWave wd = waveData[m_CurrentWave];
		foreach (_tWaveMonster m in wd.monsters)
		{
			for (int i = 0; i < m.number; i++)
			{
				CreateThiefObject(thieves.transform, delaySeconds);
				delaySeconds += Random.Range(configuration.minDelaySeconds, configuration.maxDelaySeconds);
			}
		}
	}

	void CreateThiefObject(Transform parent, float delaySeconds)
	{
		Vector3 position = MapManager.Instance.GetRandomSpawnPosition();
		position += new Vector3(0.5f, 0.5f, 0);

		var thief = Instantiate(m_ThiefPrefab, position, Quaternion.identity);
		thief.transform.SetParent(parent, true);
		thief.GetComponent<ThiefManager>().startDelaySeconds = delaySeconds;
	}

	#endregion


}
