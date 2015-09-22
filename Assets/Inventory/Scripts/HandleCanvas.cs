using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandleCanvas : MonoBehaviour {

    /// <summary>
    /// A reference to the CanvasScaler
    /// </summary>
    private CanvasScaler scaler;

	// Use this for initialization
	void Start ()
    {
        //Creates a reference to the canvas scaler
	    scaler = GetComponent<CanvasScaler>();

        //Makes the canvas scale with the screensize
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}
}
