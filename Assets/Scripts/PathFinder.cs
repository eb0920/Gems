using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
	class Grid
	{
		public short x;
		public short y;
		public short g;
		public short h;
		public bool valid;
		public int visit;

		public Grid parent;

		public short f { get { return (short)(g + h); } }

		public Grid(short x, short y, bool bValid)
		{
			this.x = (short)x;
			this.y = (short)y;
			this.g = 0;
			this.h = 0;
			this.visit = 0;
			this.valid = bValid;
			this.parent = null;
		}
	}


	private Grid[,] m_Grids;
	private List<Grid> m_OpenList;
	private int m_VisitValue = 0;
	private int m_Width;
	private int m_Height;

	public static PathFinder Instance
	{
		private set;
		get;
	}

	void Awake()
	{
		Instance = this;
	}

	public void Init()
	{
		var map = GetComponent<MapManager>();

		m_Width = map.width;
		m_Height = map.height;

		m_Grids = new Grid[m_Height, m_Width];
		m_OpenList = new List<Grid>();

		int index = 0;
		for (int row = 0; row < map.height; row++)
		{
			for (int col = 0; col < m_Width; col++)
			{
				m_Grids[row, col] = new Grid((short)col, (short)row, map.cells[index++].type == MapCell.Type.None);
			}
		}
	}

	public bool SearchPath(int fx, int fy, int tx, int ty, Queue<Vector3> path)
	{
		var oo = GetGrid(fx, fy);
		if (oo == null)
			return false;

		m_VisitValue++;

		m_OpenList.Clear();

		Grid findGrid = null;

		Grid first = (Grid)oo;
		first.parent = null;
		first.visit = m_VisitValue;
		m_OpenList.Add(first);

		while (m_OpenList.Count > 0)
		{
			var grid = m_OpenList[0];
			m_OpenList.RemoveAt(0);

			if (grid.x == tx && grid.y == ty)
			{
				findGrid = grid;
				break;
			}

			int x = grid.x;
			int y = grid.y;
			for (int dy = -1; dy <= 1; dy++)
			{
				for (int dx = -1; dx <= 1; dx++)
				{
					if (dx == 0 && dy == 0)
						continue;

					x = grid.x + dx;
					y = grid.y + dy;

					var g = GetGrid(x, y);
					if (g != null && g.visit < m_VisitValue && g.valid)
					{
						g.parent = grid;
						g.visit = m_VisitValue;
						m_OpenList.Add(g);
					}
				}
			}
		}

		m_OpenList.Clear();

		if (findGrid == null)
			return false;

		do
		{
			m_OpenList.Add(findGrid);
			findGrid = findGrid.parent;
		}
		while (findGrid != null);

		path.Clear();
		for (int i = m_OpenList.Count - 1; i >= 0; i--)
		{
			path.Enqueue(new Vector3(m_OpenList[i].x + 0.5f, m_OpenList[i].y + 0.5f, 0));

			if (i > 0)
			{
				CheckPath(m_OpenList[i], m_OpenList[i - 1], path);
			}
		}
		m_OpenList.Clear();

		return true;
	}

	Grid GetGrid(int x, int y)
	{
		if (x >= 0 && x < m_Width &&
			y >= 0 && y < m_Height)
		{
			return m_Grids[y, x];
		}
		else
		{
			return null;
		}
	}

	void CheckPath(Grid p1, Grid p2, Queue<Vector3> path)
	{
		//if (Math.Abs(p1.x - p2.x) != 1 || Math.Abs(p1.y - p2.y) != 1)
		//	return;

		if (p2.x < p1.x && p2.y > p1.y)
		{
			if (!GetGrid(p1.x, p2.y).valid)
			{
				path.Enqueue(new Vector3(p1.x, p1.y + 0.5f, 0));
				path.Enqueue(new Vector3(p2.x + 0.5f, p2.y, 0));
			}
			else if (!GetGrid(p2.x, p1.y).valid)
			{
				path.Enqueue(new Vector3(p1.x + 0.5f, p1.y + 1f, 0));
				path.Enqueue(new Vector3(p2.x + 1f, p2.y + 0.5f, 0));
			}
		}
		else if (p2.x > p1.x && p2.y > p1.y)
		{
			if (!GetGrid(p1.x, p2.y).valid)
			{
				path.Enqueue(new Vector3(p1.x + 1f, p1.y + 0.5f, 0));
				path.Enqueue(new Vector3(p2.x + 0.5f, p2.y, 0));
			}
			else if (!GetGrid(p2.x, p1.y).valid)
			{
				path.Enqueue(new Vector3(p1.x + 0.5f, p1.y + 1f, 0));
				path.Enqueue(new Vector3(p2.x, p2.y + 0.5f, 0));
			}
		}
		else if (p2.x < p1.x && p2.y < p1.y)
		{
			if (!GetGrid(p1.x, p2.y).valid)
			{
				path.Enqueue(new Vector3(p1.x, p1.y + 0.5f, 0));
				path.Enqueue(new Vector3(p2.x + 0.5f, p2.y + 1f, 0));
			}
			else if (!GetGrid(p2.x, p1.y).valid)
			{
				path.Enqueue(new Vector3(p1.x + 0.5f, p1.y, 0));
				path.Enqueue(new Vector3(p2.x + 1f, p2.y + 0.5f, 0));
			}
		}
		else if (p2.x > p1.x && p2.y < p1.y)
		{
			if (!GetGrid(p1.x, p2.y).valid)
			{
				path.Enqueue(new Vector3(p1.x + 1f, p1.y + 0.5f, 0));
				path.Enqueue(new Vector3(p2.x + 0.5f, p2.y + 1f, 0));
			}
			else if (!GetGrid(p2.x, p1.y).valid)
			{
				path.Enqueue(new Vector3(p1.x + 0.5f, p1.y, 0));
				path.Enqueue(new Vector3(p2.x, p2.y + 0.5f, 0));
			}
		}
	}
}
