#pragma strict

function Start () {

}
var Donut : Transform;

function OnCollisionEnter (collision : Collision)
{
	Donut.GetComponent(Rigidbody).useGravity = true;
}