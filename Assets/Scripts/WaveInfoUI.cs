using UnityEngine;
using UnityEngine.UI;

public class WaveInfoUI : MonoBehaviour
{
	public Text m_IndexText;

	public GameObject[] m_ThiefUIPrefabs;
	
	public void InitWithData(int idx, _tWave wave)
	{
		m_IndexText.text = (idx + 1).ToString();

		var rt = GetComponent<RectTransform>();
		float sx = 0 - (wave.monsterNumber - 1) * 100 * 0.5f;

		Vector3 pos = new Vector3(sx, rt.rect.height * 0.25f, 0);
		foreach (var m in wave.monsters)
		{
			// create thief ui from prefab.
			var thief = Instantiate(m_ThiefUIPrefabs[0], pos, Quaternion.identity) as GameObject;
			thief.transform.SetParent(transform, false);

			// set the Text of monster number.
			var numTxt = thief.GetComponentInChildren<Text>();
			if (numTxt)
			{
				numTxt.text = m.number.ToString();
			}

			pos.x += 100;
		}
	}
}
