using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TileID
{
	Red,
	Green,
	Blue
}

public struct MapCell
{
	public enum Type
	{
		None,
		Red,
		Green,
		Blue		
	}

	public Type type;
	public bool hasTree;
	public int towerID;
}

public class MapManager : MonoBehaviour
{
	public static MapManager Instance = null;

	private int m_Width;
	private int m_Height;
	private MapCell[] m_Cells;

	public Transform m_TileContainer;
	public GameObject[] m_TilePrefabs;

	public MapCell[] cells { get { return m_Cells; } }
	public int width { get { return m_Width; } }
	public int height { get { return m_Height; } }

	private TileController[,] m_Tiles;
	private List<Vector3> m_ThiefSpawnPositions = new List<Vector3>();
	
	void Awake()
	{
		Instance = this;
	}

	public void Init()
	{
		LoadDataFromImage();
		SetupTiles();
	}

	#region Load and process terrain data ---------------------------------

	public void LoadTerrain()
	{
		// Load data from resource.
		Texture2D terrain = Resources.Load<Texture2D>("Terrain04");

		// Process terrain data and create tile objects;
		m_Tiles = new TileController[terrain.height, terrain.width];
		for (int y = 0; y < terrain.height; y++)
		{
			for (int x = 0; x < terrain.width; x++)
			{
				ProcessTerrainData(terrain, x, y);
			}
		}

		// Initialise path finding model.
		GridXY.SetSize(terrain.width, terrain.height);
		PathFinding.GetInstance().LoadMap(terrain);

		foreach(var mt in MovingTarget.allTargets)
		{
			mt.InitBFS();
		}
	}

	void ProcessTerrainData(Texture2D terrain, int x, int y)
	{
		Color data = terrain.GetPixel(x, y);

		// Check spwan position
		if (data.Equals(Color.black))
		{
			m_ThiefSpawnPositions.Add(new Vector3(x, y, 0));
		}

		// create tile objects.
		if (data.Equals(Color.red))
		{
			CreateTileObject(TileID.Red, x, y);
		}
		else if (data.Equals(Color.green))
		{
			CreateTileObject(TileID.Green, x, y);
		}
		else if (data.Equals(Color.blue))
		{
			CreateTileObject(TileID.Blue, x, y);
		}
		else
		{
			m_Tiles[y, x] = null;
		}
	}
	
	void CreateTileObject(TileID id, int x, int y)
	{
		Vector3 position = new Vector3(x + 0.5f, y + 0.5f, 0);
		var tileObject = Instantiate(m_TilePrefabs[(int)id], position, Quaternion.identity);
		tileObject.transform.SetParent(m_TileContainer, true);

		m_Tiles[y, x] = tileObject.GetComponent<TileController>();
	}

	#endregion

	#region functions --------------------------------------

	public Vector3 GetRandomSpawnPosition()
	{
		int idx = Random.Range(0, m_ThiefSpawnPositions.Count - 1);
		return m_ThiefSpawnPositions[idx];
	}

	#endregion

	void LoadDataFromImage()
	{
		var tex2D = Resources.Load<Texture2D>("level04");

		m_Width = tex2D.width;
		m_Height = tex2D.height;

		m_Cells = new MapCell[m_Width * m_Height];
		
		var colors = tex2D.GetPixels();
		for (int i = 0; i < colors.Length; i++)
		{
			m_Cells[i] = TranslateColor2Cell(colors[i]);
		}
	}

	MapCell TranslateColor2Cell(Color clr)
	{
		MapCell cell;
		cell.type = MapCell.Type.None;
		cell.hasTree = false;
		cell.towerID = -1;

		if (clr == Color.red)
			cell.type = MapCell.Type.Red;
		else if (clr == Color.green)
			cell.type = MapCell.Type.Green;
		else if (clr == Color.blue)
			cell.type = MapCell.Type.Blue;
		
		return cell;
	}

	void SetupTiles()
	{
		Transform tiles = GameObject.Find("Tiles").transform; // container

		int index;
		Vector3 pos = Vector3.zero;

		m_Tiles = new TileController[m_Height, m_Width];

		for (int y = 0; y < m_Height; y++)
		{
			pos.y = y + 0.5f;
			for (int x = 0; x < m_Width; x++)
			{
				index = y * m_Width + x;
				
				if (m_Cells[index].type != MapCell.Type.None)
				{
					var prefab = m_TilePrefabs[(int)m_Cells[index].type - 1];

					pos.x = x + 0.5f;
					var tile = Instantiate(prefab, pos, Quaternion.identity);
					tile.transform.SetParent(tiles, true);

					m_Tiles[y, x] = tile.GetComponent<TileController>();
				}
				else
				{
					m_Tiles[y, x] = null;
				}
			}
		}
	}
	
	public TileController GetTile(Vector3 position)
	{
		return GetTile((int)position.x, (int)position.y);
	}

	public TileController GetTile(int x, int y)
	{
		if (x >= 0 && x < m_Width && y >= 0 && y < m_Height)
			return m_Tiles[y, x];
		else
			return null;
	}
}
