using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float m_Speed = 1.0f;
	public int m_Damage = 20;
	
	ThiefHealth m_Thief;
	
	void Update()
	{
		if (m_Thief == null)
			return;

		transform.position = Vector3.MoveTowards(transform.position, m_Thief.transform.position, m_Speed * Time.deltaTime);

		if (transform.position.Equals(m_Thief.transform.position))
		{
			m_Thief.TakeDamage(m_Damage);
			m_Thief = null;

			Destroy(gameObject, 0.1f);
		}
	}

	public void ShootTarget(ThiefHealth thief)
	{
		m_Thief = thief;
	}
}
