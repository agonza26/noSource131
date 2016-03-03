using UnityEngine;
using System.Collections;

public class RobotBrain : MonoBehaviour {


	public float walkSpeed = 0f;
	public float jumpHeight = 0f;
	public float wait = 1f;
	public bool error = false;
	public GameObject err;
	public GameObject don;



	private bool run = false;
	private bool jumped = false;
	private float lifetime = 0f;
	private Vector3 startPos;
	private Rigidbody2D bod;





	// Use this for initialization
	void Start () {
		err.SetActive (false);
		don.SetActive (false);
		bod = GetComponent<Rigidbody2D> ();
		startPos = transform.position;
	}








	// Update is called once per frame
	void Update () {
		if (error) {
			err.SetActive (true);
			exit ();
		}
		if (run) {




			//if we aren't at our maximum speed, get there to continue walking
			if (bod.velocity.x < walkSpeed) {
				bod.velocity+= Vector2.right*2;
			}





			lifetime += Time.deltaTime;
			float tempT = Mathf.Round (lifetime * 100f) / 100f;

			if (tempT >= wait && !jumped) {
				jumped = true;
				jump ();
				//error = true;

			}
		}
			
	}






	public void toggleRun(){
		run = !run;

		if (!run) {
			bod.velocity = Vector2.zero;
			lifetime = 0;
			transform.position = startPos;
		}
	}



	public void exit(){

		bod.velocity = Vector2.zero;

		run = false;
		bod.isKinematic = true;

	}



	void jump(){
		
		bod.velocity+= Vector2.up*jumpHeight;

	}




	void OnTriggerEnter2D(Collider2D other){
		GameObject it = other.gameObject;


		switch (it.tag) {
		case "Spike":
			Debug.Log ("we hit spikes");
			error = true;
			exit ();
			break;
		case "Terminal":
			don.SetActive (true);
			exit ();
			break;

		}

	}

	public bool isRunning(){
		return run;

	}








}
