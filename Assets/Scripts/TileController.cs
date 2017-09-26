using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
	public SpriteRenderer m_RangeCircle;
	public int m_RangeSize = 4;

	public GameObject tower { set; get; }

	void Start()
	{
		tower = null;

		m_RangeCircle.size = new Vector2(m_RangeSize, m_RangeSize);
		OnTowerDrag(false);
	}

	public void OnTowerDrag(bool isIn)
	{
		m_RangeCircle.enabled = isIn;
	}
}
