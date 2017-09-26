using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
	public GameObject m_Bullet;

	public float m_Range = 1.5f;
	public float m_Interval = 0.5f;
	
	float m_IntervalTimer = 0f;
		
	void Update()
	{
		m_IntervalTimer += Time.deltaTime;
		if (m_IntervalTimer >= m_Interval)
		{
			Shoot();
		}
	}

	void Shoot()
	{
		var thieves = GameObject.Find("Thieves").transform;

		for (int i = 0; i < thieves.childCount; i++)
		{
			var health = thieves.GetChild(i).GetComponent<ThiefHealth>();
			if (!health.death)
			{
				var distance = Vector3.Distance(transform.position, health.transform.position);
				if (distance <= m_Range)
				{
					ShootTarget(health);
					break;
				}
			}
		}
	}

	void ShootTarget(ThiefHealth target)
	{
		m_IntervalTimer = 0;

		var bullet = Instantiate(m_Bullet, transform.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().ShootTarget(target);
	}
}
