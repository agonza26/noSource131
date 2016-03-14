using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RobotBrain : MonoBehaviour {



	public float wait = 1f;


	private Dictionary<string,   Tuple<bool, int, int> > layers =
		new Dictionary<string, Tuple<bool, int, int> > ();


	public bool beatLevel = false;
	private float lifetime = 0f;

	private string storedKey = "Normal";
	public bool error = false;
	public GameObject err;
	public GameObject don;




	private float walkSpeed = 0f;
	private bool newHit = false;
	private bool changeHit = false;
	private float nextSpeed = 0f;
	private bool useYellow = true;
	private bool run = false;
	private Vector3 startPos;
	private Rigidbody2D bod;


	private bool startSwitch = false;








	// Use this for initialization
	void Start () {
		err.SetActive (false);
		don.SetActive (false);


		bod = GetComponent<Rigidbody2D> ();
		startPos = transform.position;
		layers.Add("Normal", new Tuple<bool, int, int>(false,0,1));
		layers.Add("Blue", new Tuple<bool, int, int>(false,0,1));
		layers.Add("Green", new Tuple<bool, int, int>(false,0,1));
		startSwitch = true;

	}














	// Update is called once per frame
	void FixedUpdate () {
		if (startSwitch&&run) {

			InputField Input = GameObject.Find("arrows").GetComponent<InputField>();
			Interpreter.Interpreter.RunInterpreter(gameObject.GetComponent<RobotBrain>(), Input.text);
			startSwitch = false;
		}

		walkSpeed = layers ["Normal"].b;
		if (error) {
			err.SetActive (true);
			exit ();
		}
		if (run) {
			walk ();
			lifetime += Time.deltaTime;
			float tempT = Mathf.Round (lifetime * 100f) / 100f;



		}
			
	}






	public void canJump(string layer = "Normal" , bool activated = true, int height = 1 ){
		layers [layer].a = activated;
		layers [layer].c = height;
	}






	public void setSpeed(string layer = "Normal", int dist = 1){
		Debug.Log ("we got here");
		layers [layer].b = dist;
	}



	public void activateYellow(){
		useYellow = true;

	}

















	private void jump(string key){
		if (layers [key].a) {
			bod.velocity += Vector2.up * layers [key].c * 16;

		}
	}



	private void walk(){
		//if we aren't at our maximum speed, get there to continue walking
		if (walkSpeed > 0) {
			if (bod.velocity.x < walkSpeed * 16) {
				bod.velocity += Vector2.right * 3;
			}

			if (bod.velocity.x > walkSpeed * 10){
				bod.velocity = new Vector2(walkSpeed * 10,bod.velocity.y);

			}
		} else {

			if (bod.velocity.x > walkSpeed * 16) {
				bod.velocity += Vector2.left * 3;
			}

			if (bod.velocity.x < walkSpeed * 10){
				bod.velocity = new Vector2(walkSpeed * 10,bod.velocity.y);

			}
		}
	}














	public void toggleRun(){
<<<<<<< HEAD
        InputField Input = GameObject.Find("arrows").GetComponent<InputField>();
        Interpreter.Interpreter.RunInterpreter(Input.text);
		run = !run;
=======
		if (!error) {
			run = !run;

			if (!run) {
				bod.velocity = Vector2.zero;
				transform.position = startPos;
			}
		} else {
			error = false;
			if (!run) {
				bod.velocity = Vector2.zero;
				transform.position = startPos;
			}
>>>>>>> refs/remotes/origin/master

		}
	}





	public void exit(){

		bod.velocity = Vector2.zero;

		run = false;
		bod.isKinematic = true;

	}
















	void OnTriggerEnter2D(Collider2D other){
		GameObject it = other.gameObject;


		switch (it.tag) {
		case "Ground":
			if(changeHit)
				layers["Normal"].b=layers[storedKey].b;
				nextSpeed=layers[storedKey].b;
				
			break;
		case "Spike":
			Debug.Log ("we hit spikes");
			error = true;
			exit ();
			break;
		case "Terminal":
			beatLevel = true;
			don.SetActive (true);
			exit ();
			break;

		
		case "Yellow":
			if(useYellow)
				layers ["Normal"].b= layers ["Normal"].b * -1;
			break;
		case "Blue":
		case "Green":
		case "Normal":
			storedKey = it.tag;
			newHit = true;
			jump (it.tag);break;
			

		}
			

	}

	void OnTriggerExit2D (Collider2D other){

		if(newHit){
			
			changeHit = true;
			newHit = false;

		}

	}



	public bool isRunning(){
		return run;

	}













}





























class Tuple<t1,t2,t3> {

	public t1 a;
	public t2 b;
	public t3 c;

	public Tuple(t1 v1,t2 v2,t3 v3)
	{
		a = v1;
		b = v2;
		c = v3;
		// Add code here
	}


}
