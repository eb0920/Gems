using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUIController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

	public GameObject m_TowerPrefab;

	GameObject m_DragObject;
	TileController m_LastOverTile;

	public void OnBeginDrag(PointerEventData evt)
	{
		Debug.Log(evt.position);

		Vector3 pos = Camera.main.ScreenToWorldPoint(evt.position);
		pos.z = 0;

		m_DragObject = Instantiate(m_TowerPrefab, pos, Quaternion.identity);
		m_LastOverTile = null;
	}

	public void OnDrag(PointerEventData evt)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(evt.position);
		pos.z = 0;
		m_DragObject.transform.position = pos;

		TileController t = GameManager.instance.mapManager.GetTile(pos);
		if (t != null && t == m_LastOverTile)
			return;

		if (m_LastOverTile != null)
		{
			m_LastOverTile.OnTowerDrag(false);
		}

		m_LastOverTile = t;
		if (m_LastOverTile != null)
		{
			m_LastOverTile.OnTowerDrag(true);
		}
	}

	public void OnEndDrag(PointerEventData evt)
	{
		if (m_LastOverTile != null)
		{
			m_LastOverTile.OnTowerDrag(false);
			m_LastOverTile = null;
		}

		DestroyObject(m_DragObject);
		m_DragObject = null;

		Vector3 pos = Camera.main.ScreenToWorldPoint(evt.position);
		pos.z = 0;

		GameManager.instance.TryCreateTower(pos, 0);
	}
}
