using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingTag
{
	Gem,
	Cave
}

public class MovingTarget : MonoBehaviour
{
	private static List<MovingTarget> s_mtList = new List<MovingTarget>();
	public static List<MovingTarget> allTargets { get { return s_mtList; } }

	public MovingTag type = MovingTag.Gem;
	public GridXY gridCoord;

	private int m_PathID = -1;
	
	void Awake()
	{
		s_mtList.Add(this);
	}

	//void Start()
	//{
	//	gridCoord.x = (int)transform.position.x;
	//	gridCoord.y = (int)transform.position.y;
	//}

	void OnDestroy()
	{
		s_mtList.Remove(this);
	}
	
	public static MovingTarget GetRandomTarget()
	{
		MovingTarget mt = GetRandomTarget(MovingTag.Gem);
		if (mt != null)
			return mt;

		mt = GetRandomTarget(MovingTag.Cave);
		return mt;
	}

	public static MovingTarget GetRandomTarget(MovingTag type)
	{
		if (s_mtList.Count == 0)
			return null;

		MovingTarget[] temp = new MovingTarget[s_mtList.Count];
		int num = 0;
		foreach (var mt in s_mtList)
		{
		//	if (mt.enabled && mt.type == type)
			if (mt.type == type && mt.gameObject.activeSelf)
			{
				temp[num++] = mt;
			}
		}

		if (num == 0)
			return null;

		int idx = UnityEngine.Random.Range(0, num - 1);
		return temp[idx];
	}


	public void InitBFS()
	{
		if (m_PathID == -1)
		{
			m_PathID = PathFinding.GetInstance().CreateBFS(gridCoord);
		}
	}

	public void SetGridXY(int x, int y)
	{
		gridCoord.x = x;
		gridCoord.y = y;

		if (m_PathID == -1)
		{
			m_PathID = PathFinding.GetInstance().CreateBFS(gridCoord);
		}
		else
		{
			PathFinding.GetInstance().ResetBFS(m_PathID, x, y);
		}
	}

	public bool GetPathFrom(int x, int y, Queue<Vector3> path)
	{
		if (m_PathID == -1)
			return false;
		return PathFinding.GetInstance().GetPath(m_PathID, new Vector2(x, y), path);
	}
}
