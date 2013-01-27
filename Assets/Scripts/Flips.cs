using UnityEngine;
using System.Collections;

public class Flips : MonoBehaviour {
	
	private Rigidbody _body; 
	
	private int _numCollisions; 
	
	// The quadrants we use to tell if we've had a flip
	private bool _firstQuad, _secondQuad, _thirdQuad, _fourthQuad; 
	
	private int _flips; 
	
	private Collider _bodyCol; 
	private Health _bodyHealth; 
	
	// Use this for initialization
	void Start () {
		_body = GetComponent<Rigidbody>(); 
		_numCollisions = 0; 
		_firstQuad = false; 
		_secondQuad = false; 
		_thirdQuad = false; 
		_fourthQuad = false; 
		_flips = 0; 
		
		_bodyCol = transform.FindChild("body").GetComponent<Collider>(); 
		_bodyHealth = GetComponentInChildren<Health>(); 
	}
	
	void OnCollisionEnter(Collision col) { 
		_numCollisions++; 
		// See if one of the contacts was our head 
		foreach (ContactPoint contact in col.contacts) { 
			if (contact.thisCollider == _bodyCol) { 
				_bodyHealth.CurrentHealth -= col.impactForceSum.magnitude;
			} 
		} 
	}
	
	void OnCollisionExit(Collision other) { 
		_numCollisions--;	
	}
	
	// Update is called once per frame
	void Update () {
		
		float angle = transform.rotation.eulerAngles.z; 
		if (angle >= 0 && angle < 90) 
			_firstQuad = true; 
		if (angle >= 90 && angle < 180) 
			_secondQuad = true; 
		if (angle >= 180 && angle < 270)
			_thirdQuad = true; 
		if (angle >= 270 && angle < 360) 
			_fourthQuad = true; 	
	
		if (_numCollisions > 0) { 
			_firstQuad = false; 
			_secondQuad = false; 
			_thirdQuad = false; 
			_fourthQuad = false; 
		}
		
		// If we've seen all the quads, it was a flip 
		if (_firstQuad && _secondQuad && _thirdQuad && _fourthQuad) { 
			_flips++; 
			_firstQuad = false; 
			_secondQuad = false; 
			_thirdQuad = false; 
			_fourthQuad = false; 
		}
		
	}
	
	void OnGUI() { 
		GUI.skin.label.fontSize = 24; 
		GUI.Label(new Rect(270, 20, 100, 48), "Glips: " + _flips.ToString()); 
	} 
}
