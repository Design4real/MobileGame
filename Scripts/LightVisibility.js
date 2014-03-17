#pragma strict

// if the player is to far away, the light will be turned off.

var player : Transform;
var broken : boolean = false;

function Start () {

}

function Update () {
var light = gameObject.GetComponent(Light) as Light;
var distancex = this.transform.position.x-player.transform.position.x;
var distancey = this.transform.position.y-player.transform.position.y;
var distancez = this.transform.position.z-player.transform.position.z;
var distance = Mathf.Sqrt(distancex*distancex+distancey*distancey+distancez*distancez);

if (distance < 50 && broken == false) {light.enabled = true;} else {light.enabled = false;}
}
