using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefHealth : MonoBehaviour
{
	public int m_Blood = 100;
	public bool death { private set; get; }
	
	void Start()
	{
		death = false;
	}

	public void TakeDamage(int damage)
	{
		if (!death)
		{
			m_Blood -= damage;
			death = m_Blood <= 0;

			if (death)
			{
				Death();
			}
		}
	}

	public void Death()
	{
		GetComponent<ThiefMovement>().enabled = false;
		//DestroyObject(gameObject, 0.1f);
		gameObject.SetActive(false);
	}
}
