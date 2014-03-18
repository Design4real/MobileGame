// Pulse light's intensity over time
public var strength = 5;
var duration : float= 1.0;
var offset:float= 0.5;
function Update() {
    // argument for cosine
    
    var phi : float = (Time.time+offset) / duration * 2 * Mathf.PI;
    // get cosine and transform from -1..1 to 0..1 range
    var amplitude : float = Mathf.Cos( phi ) * 0.5 + 0.2;
    // set light color
    light.intensity = amplitude;
    
    
//    
//   var heading: Vector3 = gameObject.transform.position - Camera.main.transform.position;
//
// var dist: float = Vector3.Dot(heading, Camera.main.transform.forward);
//
// gameObject.GetComponent(LensFlare).brightness = strength/dist;
//Debug.Log(Time.time);
 
    
}