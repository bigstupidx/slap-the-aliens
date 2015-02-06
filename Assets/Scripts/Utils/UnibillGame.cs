using UnityEngine;
using System.Collections;
using System.IO;
using Unibill.Impl;

public class UnibillGame : MonoBehaviour {

	public delegate void CallBack();
	CallBack callBack;
	private PurchasableItem[] items;
	public AdvertsManager adb;
	void Awake () 
	{
		GameObject.DontDestroyOnLoad (gameObject);
		if (UnityEngine.Resources.Load ("unibillInventory.json") == null) 
		{
			Debug.LogError("You must define your purchasable inventory within the inventory editor!");
			this.gameObject.SetActive(false);
			return;
		}
		
		// We must first hook up listeners to Unibill's events.
		Unibiller.onBillerReady += onBillerReady;
		Unibiller.onTransactionsRestored += onTransactionsRestored;
		Unibiller.onPurchaseCancelled += onCancelled;
		Unibiller.onPurchaseFailed += onFailed;
		Unibiller.onPurchaseCompleteEvent += onPurchased;
		Unibiller.onPurchaseDeferred += onDeferred;
		Unibiller.onDownloadProgressedEvent += (item, progress) => {
			Debug.Log(item + " " + progress);
		};
		
		Unibiller.onDownloadFailedEvent += (arg1, arg2) => {
			Debug.LogError(arg2);
		};
		
		Unibiller.onDownloadCompletedEventString += (obj, dir) => {
			Debug.Log("Completed download: " + obj);
			#if !(UNITY_WP8 || UNITY_METRO || UNITY_WEBPLAYER)
			foreach (var f in  new DirectoryInfo(dir).GetFiles()) {
				Debug.Log(f.Name);
				if (f.Name.EndsWith("txt") && f.Length < 10000) {
					#if !(UNITY_WP8 || UNITY_METRO || UNITY_WEBPLAYER)
					Debug.Log(Util.ReadAllText(f.FullName));
					#endif
				}
			}
			#endif
		};
		
		// Now we're ready to initialise Unibill.
		Unibiller.Initialise();
	}

	public void unlockKit (CallBack f)
	{
		callBack = f;
		items = Unibiller.AllPurchasableItems;
		Unibiller.initiatePurchase (items[0]);
	}

	public bool isUnlock()
	{
		items = Unibiller.AllPurchasableItems;
		if(Unibiller.GetPurchaseCount(items[0]) == 0)
			return false;
		else
			return true;
	}

	/// <summary>
	/// This will be called when Unibill has finished initialising.
	/// </summary>
	private void onBillerReady(UnibillState state) {
		if(isUnlock())
			adb.BuyAdverts();//GameObject.Find("AdvertsManager").GetComponent<AdvertsManager>().BuyAdverts();
		UnityEngine.Debug.Log("onBillerReady:" + state);

	}
	
	/// <summary>
	/// This will be called after a call to Unibiller.restoreTransactions().
	/// </summary>
	private void onTransactionsRestored (bool success) {
		Debug.Log("Transactions restored.");
	}
	
	/// <summary>
	/// This will be called when a purchase completes.
	/// </summary>
	private void onPurchased(PurchaseEvent e) {
		callBack ();
		Debug.Log("Purchase OK: " + e.PurchasedItem.Id);
		Debug.Log ("Receipt: " + e.Receipt);
		Debug.Log(string.Format ("{0} has now been purchased {1} times.",
		                         e.PurchasedItem.name,
		                         Unibiller.GetPurchaseCount(e.PurchasedItem)));
	}
	
	/// <summary>
	/// This will be called if a user opts to cancel a purchase
	/// after going to the billing system's purchase menu.
	/// </summary>
	private void onCancelled(PurchasableItem item) {
		Debug.Log("Purchase cancelled: " + item.Id);
	}
	
	/// <summary>
	/// iOS Specific.
	/// This is called as part of Apple's 'Ask to buy' functionality,
	/// when a purchase is requested by a minor and referred to a parent
	/// for approval.
	/// 
	/// When the purchase is approved or rejected, the normal purchase events
	/// will fire.
	/// </summary>
	/// <param name="item">Item.</param>
	private void onDeferred(PurchasableItem item) {
		Debug.Log ("Purchase deferred blud: " + item.Id);
	}
	
	/// <summary>
	/// This will be called is an attempted purchase fails.
	/// </summary>
	private void onFailed(PurchasableItem item) {
		Debug.Log("Purchase failed: " + item.Id);
	}
}
