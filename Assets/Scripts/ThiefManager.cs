using UnityEngine;

public class ThiefManager : MonoBehaviour
{
	enum State
	{
		None,
		Start,
		SteelMoving,
		BackMoving,
		Dead,
		Win,
	}

	public float startDelaySeconds = 0f;

	private State m_State;
	private GemManager m_TakingGem;
	private float m_Timer;
	private Vector3 m_StartPosition;

	private ThiefMovement m_Movement;
	private ThiefHealth m_Heath;
	
	void Start()
	{
		m_Movement = GetComponent<ThiefMovement>();
		m_Heath = GetComponent<ThiefHealth>();

		m_State = State.Start;
		m_Timer = startDelaySeconds; // Random.Range(0f, m_MaxWaitTimeSec);
		m_StartPosition = transform.position;
	//	Debug.Log("Start.................");
	}
	
	void Update()
	{
		if (m_Heath.death)
			return;

		switch (m_State)
		{
			case State.Start:
				m_Timer -= Time.deltaTime;
				if (m_Timer <= 0f)
				{
					GotoSteel();
				}
				break;

			case State.SteelMoving:
				OnStateSteelMoving();
				break;

			case State.BackMoving:
				if (!m_Movement.isMoving)
				{
					if (m_TakingGem != null)
					{
						m_TakingGem.Steel();
					}
					m_State = State.Win;
				}
				break;

			case State.Win:
				gameObject.SetActive(false);
				break;
		}
	}
	
	void GotoSteel()
	{
		//	Vector3 targetPos = GameManager.instance.defaultGemPosition;
		//	m_Movement.SetTargetPosition(targetPos);

		MovingTarget mt = MovingTarget.GetRandomTarget();
		m_Movement.SetTarget(mt);

		m_State = State.SteelMoving;
	}

	void OnStateSteelMoving()
	{
		// 查看四周有没有钻石
		var gems = GameManager.instance.gems;
		foreach (var gem in gems)
		{
			if (TrySteelGem(gem))
			{
				m_TakingGem = gem;
				m_TakingGem.Take();
				break;
			}
		}

		// 偷到钻石了，往回跑
		if (m_TakingGem != null)
		{
			GoBack();
		}

		// 如果没有移动了，表示走到矿洞了，往回走
		if (!m_Movement.isMoving)
		{
			GoBack();
		}
	}

	bool TrySteelGem(GemManager gem)
	{
		if (!gem.isVisible)
			return false;

		if (Vector3.Distance(gem.transform.position, transform.position) < 0.5f)
			return true;

		return false;
	}

	void GoBack()
	{
	//	m_Movement.SetTargetPosition(m_StartPosition);
	//	m_State = State.BackMoving;




	}
}
