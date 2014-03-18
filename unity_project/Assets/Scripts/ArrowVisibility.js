#pragma strict

function Start () {

}

var player : Transform;

function Update () {
var uipanel = gameObject.GetComponent("UIPanel") as Behaviour;
var distancex = this.transform.position.x-player.transform.position.x;
var distancey = this.transform.position.y-player.transform.position.y;
var distancez = this.transform.position.z-player.transform.position.z;
var distance = Mathf.Sqrt(distancex*distancex+distancey*distancey+distancez*distancez);
if (distance > 20) {uipanel.enabled = false;} else {uipanel.enabled = true;}
}
