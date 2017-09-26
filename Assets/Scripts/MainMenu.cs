using UnityEngine;

public enum MenuPage
{
	Home,
	Level,
	Skill
}


public class MainMenu : MonoBehaviour
{
	#region Variables
	
	public CanvasGroup m_MainPanel;
	public CanvasGroup m_LevelPanel;

	private CanvasGroup m_CurrentPanel;
	private MenuPage m_CurrentPage;

	#endregion


	void Start()
	{
		//m_CurrentPage = MenuPage.Home;
		//switch (m_CurrentPage)
		//{
		//	case MenuPage.Home:
		//	default:
		//		ShowMainPanel();
		//		break;
		//}
	}
	
	void Update()
	{

	}

	public void ShowPanel(CanvasGroup newPanel)
	{
		if (m_CurrentPanel != null)
		{
			m_CurrentPanel.gameObject.SetActive(false);
		}

		m_CurrentPanel = newPanel;
		if (m_CurrentPanel != null)
		{
			m_CurrentPanel.gameObject.SetActive(true);
		}
	}

	public void ShowMainPanel()
	{
		ShowPanel(m_MainPanel);
	}

	public void ShowLevelPanel()
	{
		ShowPanel(m_LevelPanel);
	}

	#region Button events

	public void OnPlayClicked()
	{
		ShowLevelPanel();
	}

	public void OnMainMenuClicked()
	{
		ShowMainPanel();
	}

	#endregion
}
