using System.Collections.Generic;
using UnityEngine;

public class ThiefMovement : MonoBehaviour
{
	public float m_Speed;
	
	private Queue<Vector3> m_MovingPath;
	public bool isMoving { private set; get; }
	
	void Start()
	{
		m_MovingPath = new Queue<Vector3>();
		isMoving = false;
	}
	
	void Update()
	{
		isMoving = MoveToNextPosition(m_Speed * Time.deltaTime);
	}

	bool MoveToNextPosition(float moveDistance)
	{
		if (m_MovingPath.Count == 0)
			return false;

		var nextPosition = m_MovingPath.Peek();
		var moveVector = nextPosition - transform.position;

		if (moveVector.magnitude < moveDistance)
		{
			transform.position = nextPosition;
			m_MovingPath.Dequeue();

			MoveToNextPosition(moveDistance - moveVector.magnitude);
		}
		else
		{
			//PS: 这儿先用moveVector来旋转，因为等它*moveDistance后，
			//会因为moveDistance很小而变成0向量。
			RotateUpdate(moveVector.normalized);

			moveVector = moveVector.normalized * moveDistance;
			transform.Translate(moveVector, Space.World);
		}

		return true;
	}

	//public void SetTargetPosition(Vector3 position)
	//{
	//	m_MovingPath.Clear();
	//	PathFinder.Instance.SearchPath(
	//			(int)transform.position.x, (int)transform.position.y,
	//			(int)position.x, (int)position.y,
	//			m_MovingPath
	//		);
	//}

	//public void SetTarget(int id)
	//{
	//	Vector2 start = new Vector2((int)transform.position.x, (int)transform.position.y);
	//	PathFinding.GetInstance().GetPath(id, start, m_MovingPath);
	//}
	
	void RotateUpdate(Vector3 moveDir)
	{
		float angle = Vector3.Angle(Vector3.down, moveDir);
		if (moveDir.x < 0)
		{
			angle = -angle;
		}	
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	// 

	public void SetMoving(MovingTarget mt, int x, int y, bool isBack = false)
	{
		mt.GetPathFrom(x, y, m_MovingPath);
	}

	public void SetTarget(MovingTarget mt)
	{
		int x = (int)transform.position.x;
		int y = (int)transform.position.y;
		mt.GetPathFrom(x, y, m_MovingPath);
	}
}
