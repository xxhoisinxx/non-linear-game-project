using System.Collections;
using System.Collections.Generic;

using log4net;

using UnityEngine;

public class Scrapbook : MonoBehaviour {
    /// <summary>
    /// The logger for this class.
    /// </summary>
    private static readonly ILog Log = log4net.LogManager.GetLogger(
        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    // Use this for initialization
    void Start () {
        Log.Debug(Vuforia.VuforiaRuntime.Instance.HasInitialized);

	}

	// Update is called once per frame
	void Update () {

	}
}
