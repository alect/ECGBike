using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public enum TrickMode {
		None = 0,
		FrontWheelUp = 1,
		BackWheelUp = 2,
		AllWheelsUp = 3,
	}
	
	public tk2dSprite sprite;
	public double CurrentScore = 0;
	public GameObject Player = null;
	private Rigidbody _rb = null;
	private int air_id = 0;
	private int ground_id = 0;
	WheelCollider left = null, right = null;
	BoxCollider body = null;
	TrickMode trick = TrickMode.None;
	bool bodyisgrounded = false;
	float airtime = 0;
	float wheelietime = 0;
	float scoretime = 0;
	
	public TrickMode Trick {
		get {
			return trick;
		}
	}
	
	public void Reset () {
		CurrentScore = 0;	
		trick = TrickMode.None;
		airtime = 0;
		wheelietime = 0;
		scoretime = 0;
	}
	
	// Use this for initialization
	void Start () {
		var wheels = Player.GetComponentsInChildren<WheelCollider>();
		left = wheels[0];
		right = wheels[1];
		body = Player.GetComponent<BoxCollider>();
		_rb = Player.GetComponent<Rigidbody>();
		air_id = sprite.GetSpriteIdByName("big_air");
		ground_id = sprite.GetSpriteIdByName("bike_rider");
		Reset ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float scoreboost = 0;
		if (left != null && right != null) {
			bool leftgrounded = left.isGrounded;
			bool rightgrounded = right.isGrounded;
			trick = !rightgrounded ? (trick | TrickMode.FrontWheelUp) : (trick & ~TrickMode.FrontWheelUp);
			trick = !leftgrounded ? (trick | TrickMode.BackWheelUp) : (trick & ~TrickMode.BackWheelUp);
			if ( trick == TrickMode.AllWheelsUp ) {
				airtime += Time.fixedDeltaTime;
				if ( airtime > 1 ) {
					scoreboost += airtime;
				}
				//RaycastHit hit = new RaycastHit();
				//Physics.Raycast(new Ray(transform.position, transform.InverseTransformDirection(new Vector3(0, -1, 0))), out hit);
				if (airtime > 4 || sprite.spriteId == air_id) {
					sprite.spriteId = air_id;
				} 
				//sprite.spriteId = air_id;
			}
			else if ( trick == TrickMode.None ) {
				airtime = 0;
				wheelietime = 0;
				if (sprite.spriteId == air_id) {
					sprite.spriteId = ground_id;
				}
			}
			else {
				wheelietime = Time.fixedDeltaTime;
				if (wheelietime > 1) {
					scoreboost += wheelietime / 3;
				}
				if (sprite.spriteId == air_id) {
					sprite.spriteId = ground_id;
				}
			}
		}
		else {
			airtime = 0;
			wheelietime = 0;
			scoreboost = 0;	
			trick = TrickMode.None;
			sprite.spriteId = ground_id;
		}
		if ( bodyisgrounded ) {
			airtime = 0;
			wheelietime = 0;
			scoreboost = 0;
			sprite.spriteId = ground_id;
		}
		else if (Player.rigidbody.velocity.magnitude < 5) {
			airtime = 0;
			wheelietime = 0;
			scoreboost = 0;	
			//sprite.spriteId = ground_id;
		}
		scoretime += scoreboost != 0 ? Time.fixedDeltaTime : 0;
		CurrentScore += scoreboost;
	}
	
	void OnTriggerEnter ( Collider c ) {
		if (!c.isTrigger)
			bodyisgrounded = true;
	}
	
	void OnTriggerExit ( Collider c ) {
		if (!c.isTrigger)
			bodyisgrounded = false;
	}
	
	void OnGUI () {
		
	}
}
