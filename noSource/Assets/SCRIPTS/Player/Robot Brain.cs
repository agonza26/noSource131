using UnityEngine;
using System.Collections;

public class RobotBrain : MonoBehaviour {


	private string command;
	private int steps;
	private string message;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	










	}



	public bool giveCommand(string command, int steps, string message = "none"){
		RobotMovement body = GetComponent<RobotMovement> ();




		switch (command) {
		case "walk":
			body.walk(
			break;
		case "walk":
			break;
		case "walk":
			break;
		case "walk":
		default:
			break;


		}



	}
}
