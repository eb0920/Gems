using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
	Vector3 m_StartPosition;
	SpriteRenderer m_Render;
	bool m_Taking = false;
	bool m_Stolen = false;
	bool m_Original = true;

	public bool isVisible { get { return !m_Taking && !m_Stolen; } }
	public bool original {  get { return m_Original; } }
	public bool stolen {  get { return m_Stolen; } }

	void Awake()
	{
		m_Render = GetComponent<SpriteRenderer>();
		m_StartPosition = transform.position;
	}
	
	public void Take()
	{
		if (m_Taking)
			return;

		m_Original = false;
		m_Taking = true;
		m_Render.enabled = false;
	}

	public void Drop(Vector3 pos)
	{
		if (!m_Taking)
			return;

		m_Taking = false;
		m_Render.enabled = true;
		transform.position = pos;
	}

	public void Steel()
	{
		if (m_Taking)
		{
			m_Stolen = true;
			m_Render.enabled = true;
		}
	}
}
