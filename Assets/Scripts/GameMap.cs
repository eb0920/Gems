using UnityEngine;

public class GameMap : MonoBehaviour
{
	public static GameMap Instance = null;

	public GameObject[] m_TilePrefabs;



	void Awake()
	{
		Instance = this;
	}

	public void Init()
	{
		Texture2D terrain = Resources.Load<Texture2D>("Terrain04");
	}
	
}
