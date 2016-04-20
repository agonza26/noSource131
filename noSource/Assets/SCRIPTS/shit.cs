using UnityEngine;
using System.Collections;

public class shit : MonoBehaviour {
	public string sceneName = "MainMenu";
	public GameObject player;
	public GameObject prefab;
	public GameObject prefab2;

	public bool swithcThis = false;


	private bool toggle = false;
	// Use this for initialization
	void Start () {
		player =  GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void buttonthing(){
		if (player.GetComponent<RobotBrain> () != null) {
			if ((player.GetComponent<RobotBrain> ().error || toggle) && (!player.GetComponent<RobotBrain> ().beatLevel)) {
				Destroy (player);

					player = (GameObject)Instantiate (prefab, transform.position, transform.rotation);
				player.tag = "Player";
				toggle = false;
			} else if (player.GetComponent<RobotBrain> ().beatLevel) {
				Application.LoadLevel (sceneName);
			} else {
				toggle = true;
				player.GetComponent<RobotBrain> ().toggleRun ();


			}
		} else if (player.GetComponent<RobotBran> () != null) {
			if ((player.GetComponent<RobotBran> ().error || toggle) && (!player.GetComponent<RobotBrain> ().beatLevel)) {
				Destroy (player);
				player = (GameObject)Instantiate (prefab2, transform.position, transform.rotation);
				player.tag = "Player";
				toggle = false;
			} else if (player.GetComponent<RobotBran> ().beatLevel) {
				Application.LoadLevel (sceneName);
			} else {
				toggle = true;
				player.GetComponent<RobotBran> ().toggleRun ();


			}
		}
	}
}
