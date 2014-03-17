import EasyRoads3D;
@CustomEditor(MarkerScript)
@CanEditMultipleObjects
class MarkerEditorScript extends Editor
{
var oldPos : Vector3;
var pos : Vector3;
var OQCOOQODQQ : GUISkin;
var OOODQCQOQQ : GUISkin;
var showGui : int;
var ODDQCQQDDO : boolean;
var count:int = 0;
function OnEnable(){
if(target.objectScript == null) target.SetObjectScript();
}
function OnInspectorGUI()
{


showGui = EasyRoadsGUIMenu(false, false, target.objectScript);
if(showGui != -1 && !target.objectScript.ODODDQOO) Selection.activeGameObject =  target.transform.parent.parent.gameObject;
else if(target.objectScript.OODOCOOCOCs.length <= 1  && !target.objectScript.ODODDDOO) ERMarkerGUI(target);
else  if(target.objectScript.OODOCOOCOCs.length == 2 && !target.objectScript.ODODDDOO) MSMarkerGUI(target);
else if(target.objectScript.ODODDDOO)TRMarkerGUI(target);


}
function OnSceneGUI() {
if(target.objectScript.OOCCODDDDQ == null || target.objectScript.erInit == "") Selection.activeGameObject =  target.transform.parent.parent.gameObject;
else MarkerOnScene(target);
}
function EasyRoadsGUIMenu(flag : boolean, senderIsMain : boolean,  nRoadScript : RoadObjectScript) : int {
if((target.objectScript.ODQDQOODDC == null || target.objectScript.OQDOCDDQDD == null || target.objectScript.ODOOOQCDCD == null) && target.objectScript.OOCCODDDDQ){
target.objectScript.ODQDQOODDC = new boolean[5];
target.objectScript.OQDOCDDQDD = new boolean[5];
target.objectScript.ODOOOQCDCD = nRoadScript;

target.objectScript.OOOODDQODQ = target.objectScript.OOCCODDDDQ.OCQCDDQQCO();
target.objectScript.ODODQOQO = target.objectScript.OOCCODDDDQ.OOOOQDQODQ();
target.objectScript.ODODQOQOInt = target.objectScript.OOCCODDDDQ.OOODDQDDCQ();
}else if(target.objectScript.OOCCODDDDQ == null) return;

if(target.objectScript.OQCOOQODQQ == null){
target.objectScript.OQCOOQODQQ = Resources.Load("ER3DSkin", GUISkin);
target.objectScript.OCCQOOODCO = Resources.Load("ER3DLogo", Texture2D);
}
if(!flag) target.objectScript.OCOCOQODQD();
GUI.skin = target.objectScript.OQCOOQODQQ;
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal ();
GUILayout.FlexibleSpace();
target.objectScript.ODQDQOODDC[0] = GUILayout.Toggle(target.objectScript.ODQDQOODDC[0] ,new GUIContent("", " Add road markers. "),"AddMarkers",GUILayout.Width(40), GUILayout.Height(22));
if(target.objectScript.ODQDQOODDC[0] == true && target.objectScript.OQDOCDDQDD[0] == false) {
target.objectScript.OCOCOQODQD();
target.objectScript.ODQDQOODDC[0] = true;  target.objectScript.OQDOCDDQDD[0] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.ODQDQOODDC[1]  = GUILayout.Toggle(target.objectScript.ODQDQOODDC[1] ,new GUIContent("", " Insert road markers. "),"insertMarkers",GUILayout.Width(40),GUILayout.Height(22));
if(target.objectScript.ODQDQOODDC[1] == true && target.objectScript.OQDOCDDQDD[1] == false) {
target.objectScript.OCOCOQODQD();
target.objectScript.ODQDQOODDC[1] = true;  target.objectScript.OQDOCDDQDD[1] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.ODQDQOODDC[2]  = GUILayout.Toggle(target.objectScript.ODQDQOODDC[2] ,new GUIContent("", " Process the terrain and create road geometry. "),"terrain",GUILayout.Width(40),GUILayout.Height(22));

if(target.objectScript.ODQDQOODDC[2] == true && target.objectScript.OQDOCDDQDD[2] == false) {
if(target.objectScript.markers < 2){
EditorUtility.DisplayDialog("Alert", "A minimum of 2 road markers is required before the terrain can be leveled!", "Close");
target.objectScript.ODQDQOODDC[2] = false;
}else{
target.objectScript.ODQDQOODDC[2] = false;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;





}
}
if(target.objectScript.ODQDQOODDC[2] == false && target.objectScript.OQDOCDDQDD[2] == true){

target.objectScript.OQDOCDDQDD[2] = false;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.ODQDQOODDC[3]  = GUILayout.Toggle(target.objectScript.ODQDQOODDC[3] ,new GUIContent("", " General settings. "),"settings",GUILayout.Width(40),GUILayout.Height(22));
if(target.objectScript.ODQDQOODDC[3] == true && target.objectScript.OQDOCDDQDD[3] == false) {
target.objectScript.OCOCOQODQD();
target.objectScript.ODQDQOODDC[3] = true;  target.objectScript.OQDOCDDQDD[3] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
target.objectScript.ODQDQOODDC[4]  = GUILayout.Toggle(target.objectScript.ODQDQOODDC[4] ,new GUIContent("", "Version and Purchase Info"),"info",GUILayout.Width(40),GUILayout.Height(22));
if(target.objectScript.ODQDQOODDC[4] == true && target.objectScript.OQDOCDDQDD[4] == false) {
target.objectScript.OCOCOQODQD();
target.objectScript.ODQDQOODDC[4] = true;  target.objectScript.OQDOCDDQDD[4] = true;
Selection.activeGameObject =  target.transform.parent.parent.gameObject;
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
GUI.skin = null;
target.objectScript.ODCQDQQCOQ = -1;
for(var i : int  = 0; i < 5; i++){
if(target.objectScript.ODQDQOODDC[i]){
target.objectScript.ODCQDQQCOQ = i;
target.objectScript.ODOOQQOCDD = i;
}
}
if(target.objectScript.ODCQDQQCOQ == -1) target.objectScript.OCOCOQODQD();
var markerMenuDisplay : int = 1;
if(target.objectScript.ODCQDQQCOQ == 0 || target.objectScript.ODCQDQQCOQ == 1) markerMenuDisplay = 0;
else if(target.objectScript.ODCQDQQCOQ == 2 || target.objectScript.ODCQDQQCOQ == 3 || target.objectScript.ODCQDQQCOQ == 4) markerMenuDisplay = 0;
if(target.objectScript.OCCQDCDOQD && !target.objectScript.ODQDQOODDC[2] && !target.objectScript.ODODDQOO){
target.objectScript.OCCQDCDOQD = false;
if(target.objectScript.objectType != 2)target.objectScript.OCQOOQCCDD();
if(target.objectScript.objectType == 2 && target.objectScript.OCCQDCDOQD){
Debug.Log("restore");
target.objectScript.OOCCODDDDQ.OQCCCQOOQQ(target.transform, true);
}
}
GUI.skin.box.alignment = TextAnchor.UpperLeft;
if(target.objectScript.ODCQDQQCOQ >= 0 && target.objectScript.ODCQDQQCOQ != 4){
if(target.objectScript.OOOODDQODQ == null || target.objectScript.OOOODDQODQ.Length == 0){

target.objectScript.OOOODDQODQ = target.objectScript.OOCCODDDDQ.OCQCDDQQCO();
target.objectScript.ODODQOQO = target.objectScript.OOCCODDDDQ.OOOOQDQODQ();
target.objectScript.ODODQOQOInt = target.objectScript.OOCCODDDDQ.OOODDQDDCQ();
}
EditorGUILayout.BeginHorizontal();
GUILayout.Box(target.objectScript.OOOODDQODQ[target.objectScript.ODCQDQQCOQ], GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(50));
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
}
return target.objectScript.ODCQDQQCOQ;
}
function ERMarkerGUI( markerScript : MarkerScript){
EditorGUILayout.Space();
GUILayout.Box(" Marker: " + (target.markerNum + 1).ToString(), GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
if(target.distance == "-1" && target.objectScript.OOCCODDDDQ != null){
var arr = target.objectScript.OOCCODDDDQ.ODOODQCOCQ(target.markerNum);
target.distance = arr[0];
target.OQOQOCODOQ = arr[1];
target.OCDCQQQDOQ = arr[2];
}
GUILayout.Label(" Total Distance to Marker: " + target.distance.ToString() + " km");
GUILayout.Label(" Segment Distance to Marker: " + target.OQOQOCODOQ.ToString() + " km");
GUILayout.Label(" Marker Distance: " + target.OCDCQQQDOQ.ToString() + " m");
EditorGUILayout.Space();
GUILayout.Box(" Marker Settings", GUILayout.MinWidth(253), GUILayout.MaxWidth(1500), GUILayout.Height(20));
var oldss : boolean = markerScript.OCDDOQOCOC;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Soft Selection", "When selected, the settings of other road markers within the selected distance will change according their distance to this marker."),  GUILayout.Width(105));
GUI.SetNextControlName ("OCDDOQOCOC");
markerScript.OCDDOQOCOC = EditorGUILayout.Toggle (markerScript.OCDDOQOCOC);
EditorGUILayout.EndHorizontal();
if(markerScript.OCDDOQOCOC){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("         Distance", "The soft selection distance within other markers should change too."),  GUILayout.Width(105));
markerScript.OOCQQQQQOO = EditorGUILayout.Slider(markerScript.OOCQQQQQOO, 0, 500);
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
}
if(oldss != markerScript.OOCQQQQQOO) target.objectScript.ResetMaterials(markerScript);
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Left Indent", "The distance from the left side of the road to the part of the terrain levelled at the same height as the road") ,  GUILayout.Width(105));
GUI.SetNextControlName ("ri");
oldfl = markerScript.ri;
markerScript.ri = EditorGUILayout.Slider(markerScript.ri, target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.ri){
target.objectScript.OOCDDDOQCD("ri", markerScript);
markerScript.OOQOQQOO = markerScript.ri;
}
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Right Indent", "The distance from the right side of the road to the part of the terrain levelled at the same height as the road") ,  GUILayout.Width(105));
oldfl = markerScript.li;
markerScript.li =  EditorGUILayout.Slider(markerScript.li, target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.li){
target.objectScript.OOCDDDOQCD("li", markerScript);
markerScript.ODODQQOO = markerScript.li;
}
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Left Surrounding", "This represents the distance over which the terrain will be gradually leveled on the left side of the road to the original terrain height"),  GUILayout.Width(105));
oldfl = markerScript.rs;
GUI.SetNextControlName ("rs");
markerScript.rs = EditorGUILayout.Slider(markerScript.rs,  target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.rs){
target.objectScript.OOCDDDOQCD("rs", markerScript);
markerScript.ODOQQOOO = markerScript.rs;
}
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Right Surrounding", "This represents the distance over which the terrain will be gradually leveled on the right side of the road to the original terrain height"),  GUILayout.Width(105));
oldfl = markerScript.ls;
markerScript.ls = EditorGUILayout.Slider(markerScript.ls,  target.objectScript.indent, 100);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.ls){
target.objectScript.OOCDDDOQCD("ls", markerScript);
markerScript.DODOQQOO = markerScript.ls;
}
if(target.objectScript.objectType == 0){
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
oldfl = markerScript.rt;
GUILayout.Label(new GUIContent("    Left Tilting", "Use this setting to tilt the road on the left side (m)."),  GUILayout.Width(105));
markerScript.rt = EditorGUILayout.Slider(markerScript.rt, 0, target.objectScript.roadWidth * 0.5f);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.rt){
target.objectScript.OOCDDDOQCD("rt", markerScript);
markerScript.ODDQODOO = markerScript.rt;
}
GUI.enabled = true;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Right Tilting", "Use this setting to tilt the road on the right side (cm)."),  GUILayout.Width(105));
oldfl = markerScript.lt;
markerScript.lt = EditorGUILayout.Slider(markerScript.lt, 0, target.objectScript.roadWidth * 0.5f);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.lt){
target.objectScript.OOCDDDOQCD("lt", markerScript);
markerScript.ODDOQOQQ = markerScript.lt;
}
GUI.enabled = false;
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Bridge Object", "When selected this road segment will be treated as a bridge segment."),  GUILayout.Width(105));
GUI.SetNextControlName ("bridgeObject");
markerScript.bridgeObject = EditorGUILayout.Toggle (markerScript.bridgeObject);
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
if(markerScript.bridgeObject){
GUILayout.Label(new GUIContent("      Distribute Heights", "When selected the terrain, the terrain will be gradually leveled between the height of this road segment and the current terrain height of the inner bridge segment."),  GUILayout.Width(135));
GUI.SetNextControlName ("distHeights");
markerScript.distHeights = EditorGUILayout.Toggle (markerScript.distHeights);
}
EditorGUILayout.EndHorizontal();
GUI.enabled = true;
}else{
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Floor Depth", "Use this setting to change the floor depth for this marker."),  GUILayout.Width(105));
oldfl = markerScript.floorDepth;
markerScript.floorDepth = EditorGUILayout.Slider(markerScript.floorDepth, 0, 50);
EditorGUILayout.EndHorizontal();
if(oldfl != markerScript.floorDepth){
target.objectScript.OOCDDDOQCD("floorDepth", markerScript);
markerScript.oldFloorDepth = markerScript.floorDepth;
}
}
EditorGUILayout.Space();
GUI.enabled = false;
if(target.objectScript.objectType == 0){
EditorGUILayout.BeginHorizontal();
GUILayout.Label(new GUIContent("    Start New LOD Segment", "Use this to split the road or river object in segments to use in LOD system."),  GUILayout.Width(170));
markerScript.newSegment = EditorGUILayout.Toggle (markerScript.newSegment);
EditorGUILayout.EndHorizontal();
}
GUI.enabled = true;
EditorGUILayout.Space();
if(!markerScript.autoUpdate){
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button ("Refresh Surface", GUILayout.Width(225))){
target.objectScript.ODODDDOOOD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
}
if (GUI.changed && !target.objectScript.OCCDODODDQ){
target.objectScript.OCCDODODDQ = true;
}else if(target.objectScript.OCCDODODDQ){
target.objectScript.ODCCOCODDO(markerScript);
target.objectScript.OCCDODODDQ = false;
}
oldfl = markerScript.rs;
}
function MSMarkerGUI( markerScript : MarkerScript){
EditorGUILayout.Space();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Align XYZ", "Click to distribute the x, y and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OOCCODDDDQ.OOOCOQDQOO(target.objectScript.OODOCOOCOCs, 0);
target.objectScript.ODODDDOOOD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Align XZ", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OOCCODDDDQ.OOOCOQDQOO(target.objectScript.OODOCOOCOCs, 1);
target.objectScript.ODODDDOOOD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Align XZ  Snap Y", "Click to distribute the x and z values of all markers in between the selected markers in a line between the selected markers and snap the y value to the terrain height at the new position."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OOCCODDDDQ.OOOCOQDQOO(target.objectScript.OODOCOOCOCs, 2);
target.objectScript.ODODDDOOOD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.BeginHorizontal();
GUILayout.FlexibleSpace();
if(GUILayout.Button (new GUIContent(" Average Heights ", "Click to distribute the heights all markers in between the selected markers."), GUILayout.Width(150))){
Undo.RegisterUndo(target.transform.parent.GetComponentsInChildren(typeof(Transform)), "Marker align");
target.objectScript.OOCCODDDDQ.OOOCOQDQOO(target.objectScript.OODOCOOCOCs, 3);
target.objectScript.ODODDDOOOD();
}
GUILayout.FlexibleSpace();
EditorGUILayout.EndHorizontal();
EditorGUILayout.Space();
EditorGUILayout.Space();
}
function TRMarkerGUI(markerScript : MarkerScript){
EditorGUILayout.Space();
}
function MarkerOnScene(markerScript : MarkerScript){
var cEvent : Event = Event.current;

if(!target.objectScript.ODODDDOO || target.objectScript.objectType == 2){
if(cEvent.shift && (target.objectScript.ODOOQQOCDD == 0 || target.objectScript.ODOOQQOCDD == 1)) {
Selection.activeGameObject =  markerScript.transform.parent.parent.gameObject;
}else if(cEvent.shift && target.objectScript.OODOCOOCOC != target.transform){
target.objectScript.ODQCOQDQOC(markerScript);
Selection.objects = target.objectScript.OODOCOOCOCs;
}else if(target.objectScript.OODOCOOCOC != target.transform && count == 0){
if(!target.InSelected()){
target.objectScript.OODOCOOCOCs = new GameObject[0];
target.objectScript.ODQCOQDQOC(markerScript);
Selection.objects = target.objectScript.OODOCOOCOCs;


count++;
}

}else{
pos = markerScript.oldPos;
if(pos  != oldPos && !markerScript.changed){
oldPos = pos;
if(!cEvent.shift){
target.objectScript.OQCCQCQDDD(markerScript);
}
}
}
if(cEvent.shift && markerScript.changed){
ODDQCQQDDO = true;
}
markerScript.changed = false;
if(!cEvent.shift && ODDQCQQDDO){
target.objectScript.OQCCQCQDDD(markerScript);
ODDQCQQDDO = false;
}
}

}
}
