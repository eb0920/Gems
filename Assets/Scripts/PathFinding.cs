using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GridXY
{
	public static int width = 0;
	public static int height = 0;
	
	public static void SetSize(int w, int h)
	{
		width = w;
		height = h;
	}

	public int x;
	public int y;

	public bool valid { get { return x >= 0 && x < width && y >= 0 && y < height; } }
	public int idx { get { return x + y * width; } }
}

public class PathFinding
{
	private static PathFinding instance = null;

	// All nodes in graph.
	private List<Vector2> grids = new List<Vector2>();
	// Neighbors of each node.
	private Dictionary<Vector2, List<Vector2>> neighborsOfGrid = new Dictionary<Vector2, List<Vector2>>();
	// List of a Breadth First Search result;
	private List<Dictionary<Vector2, Vector2>> bfsList = new List<Dictionary<Vector2,Vector2>>();
	
	public static PathFinding GetInstance()
	{
		if (instance == null)
		{
			instance = new PathFinding();
		}
		return instance;
	}

	public void LoadMap(Texture2D map)
	{
		Vector2[] neighborsDir = {
			new Vector2(0, 1),
			new Vector2(1, 0),
			new Vector2(0, -1),
			new Vector2(-1, 0)
		};
		
		for (int y = 0; y < map.height; y++)
		{
			for (int x = 0; x < map.width; x++)
			{
				Color clr = map.GetPixel(x, y);
				if (clr.Equals(Color.white))
				{
					Vector2 grid = new Vector2(x, y);
					var neighbors = new List<Vector2>();
					foreach (var dir in neighborsDir)
					{
						Vector2 neighbor = dir + grid;
						if (neighbor.x < 0 || neighbor.x >= map.width ||
							neighbor.y < 0 || neighbor.y >= map.height)
							continue;

						Color nClr = map.GetPixel((int)neighbor.x, (int)neighbor.y);
						if (nClr.Equals(Color.white) || nClr.Equals(Color.black))
						{
							neighbors.Add(neighbor);
						}
					}

					grids.Add(grid);
					neighborsOfGrid[grid] = neighbors;
				}
			}
		}
	}

	public void CreateBFS(int id, Vector2 start)
	{
		while (id >= bfsList.Count)
		{
			bfsList.Add(null);
		}

		if (bfsList[id] == null)
		{
			bfsList[id] = new Dictionary<Vector2, Vector2>();
		}

		BreadthFirstSearch(start, bfsList[id]);
	}

	public int CreateBFS(GridXY start)
	{
		var bfs = new Dictionary<Vector2, Vector2>();
		bfsList.Add(bfs);

		BreadthFirstSearch(new Vector2(start.x, start.y), bfs);

		return bfsList.Count - 1;
	}

	public void ResetBFS(int id, int x, int y)
	{
		if (id >= 0 && id < bfsList.Count)
		{
			BreadthFirstSearch(new Vector2(x, y), bfsList[id]);
		}
	}

	void BreadthFirstSearch(Vector2 start, Dictionary<Vector2, Vector2> result)
	{
		Queue<Vector2> frontier = new Queue<Vector2>();
		frontier.Enqueue(start);

		result.Clear();
		result[start] = start;

		while (frontier.Count > 0)
		{
			Vector2 current = frontier.Dequeue();
			if (neighborsOfGrid.ContainsKey(current))
			{
				var neighbors = neighborsOfGrid[current];
				foreach (Vector2 neighbor in neighbors)
				{
					if (!result.ContainsKey(neighbor))
					{
						frontier.Enqueue(neighbor);
						result[neighbor] = current;
					}
				}
			}
		}
		
		result.Remove(start);
	}

	public bool GetPath(int id, Vector2 start, Queue<Vector3> path)
	{
		if (id >= bfsList.Count || bfsList[id] == null)
			return false;

		Vector3 dxy = new Vector3(0.5f, 0.5f, 0);

		path.Clear();
		path.Enqueue(dxy + (Vector3)start);

		var result = bfsList[id];
		Vector2 next = start;
		while (result.ContainsKey(next))
		{
			next = result[next];
			path.Enqueue(dxy + (Vector3)next);
		}

		return true;
	}
}



