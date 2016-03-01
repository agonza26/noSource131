using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

	/* This script will be used to change the setting, if needed. 
	 *  and hopefully be used for persistence
	 *  when an outside factor changes the settings, they should access
	 *  this script, change the changedSetting boolean to true, and use an appropriate 
	 *  message for the switch, finally you can use the newWidth, and newHeight to specify
	 *  which resolution to use.
	 * 
	 * 
		*/
	//we can try to use this class for persistence 
	public bool changedSetting = false;
	public string message = "none";   //"none" for nothing, "res" for resolution, "full" for fullscreen, 
	public int newWidth;
	public int newHeight;
	public string currentLevel;




	private int defaultWidth;
	private int defaultHeight;
	private bool defaultScreen = false;






	// Use this for initialization
	void Start () {
		//if a default width or height arent specified
		if (defaultWidth == null) {
			defaultWidth = 640;
		}
		if (defaultHeight == null) {
			defaultHeight = 480;

		}
		//set resolution
		newWidth = defaultWidth;
		newHeight = defaultWidth;
		Screen.SetResolution(defaultWidth, defaultHeight, defaultScreen);


		//set the continue button's level
		currentLevel = "Level 1";


	}
	
	// Update is called once per frame
	void Update () {

		//switch to change appropriate settings
		if (changedSetting) {
			switch (message) {
				case "full":
					defaultScreen = !defaultScreen;
					Screen.SetResolution (newWidth, newHeight, defaultScreen);
					break;
				case "res":
					Screen.SetResolution (newWidth, newHeight, defaultScreen);
					break;
				case "none":
				default:
					break;

			}
			changedSetting = false;
		}
	}
}
