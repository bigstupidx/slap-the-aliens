using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdvertsManager : MonoBehaviour {

	public string bannerID = "";
	public string intersentialID = "";

	BannerView bannerView;
	InterstitialAd interstitial;

	delegate void FuncInt();
	System.EventHandler<System.EventArgs> myFunc;

	public bool show = true;

	void Start () 
	{
		DontDestroyOnLoad (gameObject);
		gameObject.name = "AdvertsManager";
		myFunc = LoadInterstitial;
		LoadBaner ();
		LoadInterstitial (null,null);
		if(interstitial != null)
			interstitial.AdClosed += myFunc;
	}

	public void BuyAdverts()
	{
		show = false;
		HideBaner ();
	}

	public void ShowBaner()
	{
		if(!show)
			return;
		bannerView.Show ();
	}

	public void HideBaner()
	{
		if(bannerView != null)
		bannerView.Hide ();
	}

	public void LoadBaner()
	{
		if(!show)
			return;

		bannerView = new BannerView (bannerID, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
	}

	public void LoadInterstitial(System.Object obj,  System.EventArgs arg)
	{
		if(!show)
			return;

		interstitial = new InterstitialAd(intersentialID);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	public void ShowInterstitial ()
	{
		if(!show)
			return;
		//if (interstitial.IsLoaded()) {
			interstitial.Show();
		//}
	}
}
