using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameObject player;

	private Vector2 vel;
	public float smoothX;
	public float smoothY;

	public float xOffset = 0f;
	public float yOffset = 0f;


	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");

	
	}
	
	// Update is called once per frame
	void Update () {


		if (player.GetComponent<RobotBrain> ().isRunning()) {
			float pX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x + xOffset, ref vel.x, smoothX);
			float pY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y + yOffset, ref vel.y, smoothY);

			transform.position = new Vector3 (pX, pY, transform.position.z);
		}
	}
}
