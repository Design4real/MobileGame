#pragma strict
var rudderSpeadBreak = false;
var rudderLeft = false;
var rudderRight = false;
var bodyFlapUp = false;
var bodyFlapDown = false;
var bayDoor1_Open = false;
var bayDoor2_Open = false;
//
var mainRudder : Transform;
var rudder : GameObject[];
var bodyFlap : Transform;
var bayDoors : Transform[];


function Start () {
	yield WaitForSeconds(1);
	rigidbody.AddRelativeTorque (Vector3(0, 1000,-100));
}

function Update () {

	//Rudder
	if(rudderSpeadBreak == true) {
		var target1 = Quaternion.Euler (0, 49.3, 0);
		var target2 = Quaternion.Euler (0, -49.3, 0);
	} else {
		target1 = Quaternion.Euler (0, 0, 0);
		target2 = Quaternion.Euler (0, 0, 0);
	}
	if(rudderLeft == true && rudderRight == false) {
		if(rudderSpeadBreak == false) {
		var target0 = Quaternion.Euler (0, 27, 0);
		} else {
		target0 = Quaternion.Euler (0, 0, 0);
		target2 = Quaternion.Euler (0, -25, 0);
		}
	} else if(rudderLeft == false && rudderRight == true) {
		if(rudderSpeadBreak == false) {
		target0 = Quaternion.Euler (0, -27, 0);
		} else {
		target0 = Quaternion.Euler (0, 0, 0);
		target1 = Quaternion.Euler (0, 25, 0);
		}
	} else {
		target0 = Quaternion.Euler (0, 0, 0);
	}
	mainRudder.localRotation = Quaternion.Slerp(mainRudder.localRotation, target0, Time.deltaTime * 2.0);
	rudder[0].transform.localRotation = Quaternion.Slerp(rudder[0].transform.localRotation, target1, Time.deltaTime * 2.0);
	rudder[1].transform.localRotation = Quaternion.Slerp(rudder[1].transform.localRotation, target2, Time.deltaTime * 2.0);
	rudder[2].transform.localRotation = Quaternion.Slerp(rudder[2].transform.localRotation, target1, Time.deltaTime * 1.5);
	rudder[3].transform.localRotation = Quaternion.Slerp(rudder[3].transform.localRotation, target2, Time.deltaTime * 1.5);
    //End Rudder
    
	//BodyFlap
	if(bodyFlapUp == true && bodyFlapDown == false) {
		var bodyflapTarget = Quaternion.Euler (-11.7, 0, 0);
	} else if(bodyFlapUp == false && bodyFlapDown == true) {
		bodyflapTarget = Quaternion.Euler (22.5, 0, 0);
	} else {
		bodyflapTarget = Quaternion.Euler (0, 0, 0);
	}
	bodyFlap.localRotation = Quaternion.Slerp(bodyFlap.localRotation, bodyflapTarget, Time.deltaTime * 1.0);
	//End BodyFlap
	
	//BayDoors
	if(bayDoor2_Open == true) {
		var bayDoorsTarget1 = Quaternion.Euler (-1.188232, 0, 0);
	} else {
		bayDoorsTarget1 = Quaternion.Euler (-1.188232, 0, 170);
	}
	if(bayDoor1_Open == true) {
		var bayDoorsTarget2 = Quaternion.Euler (-1.188232, 0, 0);
	} else {
		bayDoorsTarget2 = Quaternion.Euler (-1.188232, 0, -170);
	}
	bayDoors[0].localRotation = Quaternion.Slerp(bayDoors[0].localRotation, bayDoorsTarget1, Time.deltaTime * 0.2);
	bayDoors[1].localRotation = Quaternion.Slerp(bayDoors[1].localRotation, bayDoorsTarget2, Time.deltaTime * 0.2);
	//End BayDoors
}