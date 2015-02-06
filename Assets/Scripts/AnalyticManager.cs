using UnityEngine;
using System.Collections;

public class AnalyticManager : MonoBehaviour {

    public GoogleAnalyticsV3 analytic;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(analytic.gameObject);

        analytic.StartSession();

	}

    void OnLevelWasLoaded(int level)
    {
        analytic.LogScreen("screen_" + level);
    }
	

    public void OnApplicationQuit()
    {
        analytic.StopSession();
    }
}
