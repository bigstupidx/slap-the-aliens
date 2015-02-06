using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;

public class Menu : MonoBehaviour {

	public RawImage bg;
	public Animator curtain;

	public GameObject helpObj;
	public GameObject mainMenu;
	public GameObject options;
	public GameObject ownAdb;
	public Image[] objRecolor;
	public static bool isLogged =false;

	void Awake()
	{
		GameData.Get ();
		Color c = Config.bgColors [Random.Range (0, Config.bgColors.Length)];
		//bg.color = c;

		for(int i = 0; i < objRecolor.Length; i++)
		{
			objRecolor[i].color = c;
		}
	}

	void Start()
	{		
		if(curtain != null && GoTo.lastScene == "")
			curtain.Play("CurtainAnim");

		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();

		Social.localUser.Authenticate((bool success) => {
			isLogged = success;
		});
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && mainMenu.activeSelf)
		{
			if(ownAdb.activeSelf)
				OnExitClick();
			else
				ownAdb.SetActive(true);
		}
	}

	public void OnMoreGamesClick()
	{
		Application.OpenURL ("https://play.google.com/store/apps/developer?id=GGWP");
	}

	public void OnPlayClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		GameObject.Find ("AdvertsManager").GetComponent<AdvertsManager> ().HideBaner();
		GoTo.LoadGame ();
	}

	public void OnHighScoreClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		PlayGamesPlatform.Instance.ShowLeaderboardUI(Config.borderId);
	}

	public void OnHelpClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		mainMenu.SetActive (false);
		options.SetActive (false);
		helpObj.SetActive (true);
		//GoTo.LoadHelp ();
	}

	public void OnRateClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		UniRate.Instance.PromptIfNetworkAvailable();
	}

	public void OnMainMenuClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		mainMenu.SetActive (true);
		options.SetActive (false);
		helpObj.SetActive (false);
	}

	public void OnOptionsClick()
	{
		options.GetComponent<Options> ().Initialize ();
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		mainMenu.SetActive (false);
		options.SetActive (true);
		helpObj.SetActive (false);
	}

	public void OnExitClick()
	{
		AppSoundManager.Get ().PlaySfx (Sfx.Type.sfx_btn_click);
		Application.Quit ();
		Debug.Log("exit");
	}

	public void GoToOwnAdb()
	{
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.ggwpstudio.faster");
	}

}
