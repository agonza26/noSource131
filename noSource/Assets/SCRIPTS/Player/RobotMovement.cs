using UnityEngine;
using System.Collections;

public class RobotMovement : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy")
			coll.gameObject.SendMessage("ApplyDamage", 10);

	}

	public void walk(int steps){
		transform.Translate (transform.right * steps/100);
	}

	public void jump(int steps){
		//walk 

	}



	public void debug(int steps, string message){
		//debug

	}






}
