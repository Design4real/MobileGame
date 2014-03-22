#pragma strict

// if the player hits the object, the gravity will be turned on and if it is a light, the light will be turned off.

function Start () {
this.GetComponent(Rigidbody).useGravity = false;
}

function Update () {

}

function OnCollisionEnter (collision : Collision)
{
	this.GetComponent(Rigidbody).useGravity = true;
//	var lightvis = gameObject.GetComponentInChildren(LightVisibility);
//	lightvis.broken = true;
//	if (lightvis != null) {lightvis.broken = true;}
}