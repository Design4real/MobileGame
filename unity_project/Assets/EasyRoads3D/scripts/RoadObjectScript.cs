using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;
using EasyRoads3D;

public class RoadObjectScript : MonoBehaviour {
static public string version = "";
public int objectType = 0;
public bool displayRoad = true;
public float roadWidth = 5.0f;
public float indent = 3.0f;
public float surrounding = 5.0f;
public float raise = 1.0f;
public float raiseMarkers = 0.5f;
public bool OOQDOOQQ = false;
public bool renderRoad = true;
public bool beveledRoad = false;
public bool applySplatmap = false;
public int splatmapLayer = 4;
public bool autoUpdate = true;
public float geoResolution = 5.0f;
public int roadResolution = 1;
public float tuw =  15.0f;
public int splatmapSmoothLevel;
public float opacity = 1.0f;
public int expand = 0;
public int offsetX = 0;
public int offsetY = 0;
private Material surfaceMaterial;
public float surfaceOpacity = 1.0f;
public float smoothDistance = 1.0f;
public float smoothSurDistance = 3.0f;
private bool handleInsertFlag;
public bool handleVegetation = true;
public float OCQOOOCQDQ = 2.0f;
public float OCDOOCCDCO = 1f;
public int materialType = 0;
String[] materialStrings;
private MarkerScript[] mSc;

private bool OCQQDQDDCO;
private bool[] ODQDQOODDC = null;
private bool[] OQDOCDDQDD = null;
public string[] OOOODDQODQ;
public string[] ODODQOQO;
public int[] ODODQOQOInt;
public int ODCQDQQCOQ = -1;
public int ODOOQQOCDD = -1;
static public GUISkin OQCOOQODQQ;
static public GUISkin OOODQCQOQQ;
public bool OCQOQDCODC = false;
private Vector3 cPos;
private Vector3 ePos;
public bool OOODOOCDOD;
static public Texture2D OCCQOOODCO;
public int markers = 1;
public OQOCCOOOQD OOCCODDDDQ;
private GameObject ODOQDQOO;
public bool OCCQDCDOQD;
public bool doTerrain;
private Transform OODOCOOCOC = null;
public GameObject[] OODOCOOCOCs;
private static string OOCDQQOQDO = null;
public Transform obj;
private string OCQQOCODQQ;
public static string erInit = "";
static public Transform OCDDDQQOCD;
private RoadObjectScript ODOOOQCDCD;
public bool flyby;


private Vector3 pos;
private float fl;
private float oldfl;
private bool OCCDODODDQ;
private bool ODDQCQQDDO;
private bool OODDQCQQOQ;
public Transform OQODCDQQQQ;
public int OdQODQOD = 1;
public float OOQQQDOD = 0f;
public float OOQQQDODOffset = 0f;
public float OOQQQDODLength = 0f;
public bool ODODDDOO = false;
static public string[] ODOQDOQO;
static public string[] ODODOQQO; 
static public string[] ODODQOOQ;
public int ODQDOOQO = 0;
public string[] ODQQQQQO;  
public string[] ODODDQOO; 
public bool[] ODODQQOD; 
public int[] OOQQQOQO; 
public int ODOQOOQO = 0; 

public bool forceY = false;
public float yChange = 0f;
public float floorDepth = 2f;
public float waterLevel = 1.5f; 
public bool lockWaterLevel = true;
public float lastY = 0f;
public string distance = "0";
public string markerDisplayStr = "Hide Markers";
static public string[] objectStrings;
public string objectText = "Road";
public bool applyAnimation = false;
public float waveSize = 1.5f;
public float waveHeight = 0.15f;
public bool snapY = true;

private TextAnchor origAnchor;
public bool autoODODDQQO;
public Texture2D roadTexture;
public Texture2D roadMaterial;
public string[] OOODCOOQCC;
public string[] ODDQQDCODD;
public int selectedWaterMaterial;
public int selectedWaterScript;
private bool doRestore = false;
public bool doFlyOver;
public static GameObject tracer;
public Camera goCam;
public float speed = 1f;
public float offset = 0f;
public bool camInit;
public GameObject customMesh = null;
static public bool disableFreeAlerts = true;
public bool multipleTerrains;
public bool editRestore = true;
public Material roadMaterialEdit;
static public int backupLocation = 0;
public string[] backupStrings = new string[2]{"Outside Assets folder path","Inside Assets folder path"};
public Vector3[] leftVecs = new Vector3[0];
public Vector3[] rightVecs = new Vector3[0];
public void OOQCOOCCQO(ArrayList arr, String[] DOODQOQO, String[] OODDQOQO){

OOQOOCCOQD(transform, arr, DOODQOQO, OODDQOQO);
}
public void ODQCOQDQOC(MarkerScript markerScript){

OODOCOOCOC = markerScript.transform;



List<GameObject> tmp = new List<GameObject>();
for(int i=0;i<OODOCOOCOCs.Length;i++){
if(OODOCOOCOCs[i] != markerScript.gameObject)tmp.Add(OODOCOOCOCs[i]);
}




tmp.Add(markerScript.gameObject);
OODOCOOCOCs = tmp.ToArray();
OODOCOOCOC = markerScript.transform;

OOCCODDDDQ.OOQQCDQODC(OODOCOOCOC, OODOCOOCOCs, markerScript.OCDDOQOCOC, markerScript.OOCQQQQQOO, OQODCDQQQQ, out markerScript.OODOCOOCOCs, out markerScript.trperc, OODOCOOCOCs);

ODOOQQOCDD = -1;
}
public void ODCCOCODDO(MarkerScript markerScript){
if(markerScript.OOCQQQQQOO != markerScript.ODOOQQOO || markerScript.OOCQQQQQOO != markerScript.ODOOQQOO){
OOCCODDDDQ.OOQQCDQODC(OODOCOOCOC, OODOCOOCOCs, markerScript.OCDDOQOCOC, markerScript.OOCQQQQQOO, OQODCDQQQQ, out markerScript.OODOCOOCOCs, out markerScript.trperc, OODOCOOCOCs);
markerScript.ODQDOQOO = markerScript.OCDDOQOCOC;
markerScript.ODOOQQOO = markerScript.OOCQQQQQOO;
}
if(ODOOOQCDCD.autoUpdate) ODDCDDDDCD(ODOOOQCDCD.geoResolution, false, false);
}
public void ResetMaterials(MarkerScript markerScript){
if(OOCCODDDDQ != null)OOCCODDDDQ.OOQQCDQODC(OODOCOOCOC, OODOCOOCOCs, markerScript.OCDDOQOCOC, markerScript.OOCQQQQQOO, OQODCDQQQQ, out markerScript.OODOCOOCOCs, out markerScript.trperc, OODOCOOCOCs);
}
public void OQCCQCQDDD(MarkerScript markerScript){
if(markerScript.OOCQQQQQOO != markerScript.ODOOQQOO){
OOCCODDDDQ.OOQQCDQODC(OODOCOOCOC, OODOCOOCOCs, markerScript.OCDDOQOCOC, markerScript.OOCQQQQQOO, OQODCDQQQQ, out markerScript.OODOCOOCOCs, out markerScript.trperc, OODOCOOCOCs);
markerScript.ODOOQQOO = markerScript.OOCQQQQQOO;
}
ODDCDDDDCD(ODOOOQCDCD.geoResolution, false, false);
}
private void OOCDDDOQCD(string ctrl, MarkerScript markerScript){
int i = 0;
foreach(Transform tr in markerScript.OODOCOOCOCs){
MarkerScript wsScript = (MarkerScript) tr.GetComponent<MarkerScript>();
if(ctrl == "rs") wsScript.LeftSurrounding(markerScript.rs - markerScript.ODOQQOOO, markerScript.trperc[i]);
else if(ctrl == "ls") wsScript.RightSurrounding(markerScript.ls - markerScript.DODOQQOO, markerScript.trperc[i]);
else if(ctrl == "ri") wsScript.LeftIndent(markerScript.ri - markerScript.OOQOQQOO, markerScript.trperc[i]);
else if(ctrl == "li") wsScript.RightIndent(markerScript.li - markerScript.ODODQQOO, markerScript.trperc[i]);
else if(ctrl == "rt") wsScript.LeftTilting(markerScript.rt - markerScript.ODDQODOO, markerScript.trperc[i]);
else if(ctrl == "lt") wsScript.RightTilting(markerScript.lt - markerScript.ODDOQOQQ, markerScript.trperc[i]);
else if(ctrl == "floorDepth") wsScript.FloorDepth(markerScript.floorDepth - markerScript.oldFloorDepth, markerScript.trperc[i]);
i++;
}
}
public void ODODDDOOOD(){
if(markers > 1) ODDCDDDDCD(ODOOOQCDCD.geoResolution, false, false);
}
public void OOQOOCCOQD(Transform tr, ArrayList arr, String[] DOODQOQO, String[] OODDQOQO){
version = "2.4.7";
OQCOOQODQQ = (GUISkin)Resources.Load("ER3DSkin", typeof(GUISkin));


OCCQOOODCO = (Texture2D)Resources.Load("ER3DLogo", typeof(Texture2D));
if(RoadObjectScript.objectStrings == null){
RoadObjectScript.objectStrings = new string[3];
RoadObjectScript.objectStrings[0] = "Road Object"; RoadObjectScript.objectStrings[1]="River Object";RoadObjectScript.objectStrings[2]="Procedural Mesh Object";
}
obj = tr;
OOCCODDDDQ = new OQOCCOOOQD();
ODOOOQCDCD = obj.GetComponent<RoadObjectScript>();
foreach(Transform child in obj){
if(child.name == "Markers") OQODCDQQQQ = child;
}
OQOCCOOOQD.terrainList.Clear();
Terrain[] terrains = (Terrain[])FindObjectsOfType(typeof(Terrain));
foreach(Terrain terrain in terrains) {
Terrains t = new Terrains();
t.terrain = terrain;
if(!terrain.gameObject.GetComponent<EasyRoads3DTerrainID>()){
EasyRoads3DTerrainID terrainscript = (EasyRoads3DTerrainID)terrain.gameObject.AddComponent("EasyRoads3DTerrainID");
string id = UnityEngine.Random.Range(100000000,999999999).ToString();
terrainscript.terrainid = id;
t.id = id;
}else{
t.id = terrain.gameObject.GetComponent<EasyRoads3DTerrainID>().terrainid;
}
OOCCODDDDQ.OQDCCQCCCO(t);
}
ODODQQODQC.OQDCCQCCCO();
if(roadMaterialEdit == null){
roadMaterialEdit = (Material)Resources.Load("materials/roadMaterialEdit", typeof(Material));
}
if(objectType == 0 && GameObject.Find(gameObject.name + "/road") == null){
GameObject road = new GameObject("road");
road.transform.parent = transform;
}

OOCCODDDDQ.OQDCOOQCCD(obj, OOCDQQOQDO, ODOOOQCDCD.roadWidth, surfaceOpacity, out OOODOOCDOD, out indent, applyAnimation, waveSize, waveHeight);
OOCCODDDDQ.OCDOOCCDCO = OCDOOCCDCO;
OOCCODDDDQ.OCQOOOCQDQ = OCQOOOCQDQ;
OOCCODDDDQ.OdQODQOD = OdQODQOD + 1;
OOCCODDDDQ.OOQQQDOD = OOQQQDOD;
OOCCODDDDQ.OOQQQDODOffset = OOQQQDODOffset;
OOCCODDDDQ.OOQQQDODLength = OOQQQDODLength;
OOCCODDDDQ.objectType = objectType;
OOCCODDDDQ.snapY = snapY;
OOCCODDDDQ.terrainRendered = OCCQDCDOQD;
OOCCODDDDQ.handleVegetation = handleVegetation;
OOCCODDDDQ.raise = raise;
OOCCODDDDQ.roadResolution = roadResolution;
OOCCODDDDQ.multipleTerrains = multipleTerrains;
OOCCODDDDQ.editRestore = editRestore;
OOCCODDDDQ.roadMaterialEdit = roadMaterialEdit;
OOCCODDDDQ.renderRoad = renderRoad;
if(backupLocation == 0)OCCDCOQQDD.backupFolder = "/EasyRoads3D";
else OCCDCOQQDD.backupFolder =  "/Assets/EasyRoads3D/backups";

ODODQOQO = OOCCODDDDQ.OOOOQDQODQ();
ODODQOQOInt = OOCCODDDDQ.OOODDQDDCQ();


if(OCCQDCDOQD){




doRestore = true;
}


ODOOQCODCD();

if(arr != null || ODODQOOQ == null) OQOCCQDODQ(arr, DOODQOQO, OODDQOQO);


if(doRestore) return;
}
public void UpdateBackupFolder(){
}
public void OCOCOQODQD(){
if(!ODODDDOO || objectType == 2){
if(ODQDQOODDC != null){
for(int i = 0; i < ODQDQOODDC.Length; i++){
ODQDQOODDC[i] = false;
OQDOCDDQDD[i] = false;
}
}
}
}

public void OCQQDCDQOO(Vector3 pos){


if(!displayRoad){
displayRoad = true;
OOCCODDDDQ.OQDOCOODQQ(displayRoad, OQODCDQQQQ);
}
pos.y += ODOOOQCDCD.raiseMarkers;
if(forceY && ODOQDQOO != null){
float dist = Vector3.Distance(pos, ODOQDQOO.transform.position);
pos.y = ODOQDQOO.transform.position.y + (yChange * (dist / 100f));
}else if(forceY && markers == 0) lastY = pos.y;
GameObject go = null;
if(ODOQDQOO != null) go = (GameObject)Instantiate(ODOQDQOO);
else go = (GameObject)Instantiate(Resources.Load("marker", typeof(GameObject)));
Transform newnode = go.transform;
newnode.position = pos;
newnode.parent = OQODCDQQQQ;
markers++;
string n;
if(markers < 10) n = "Marker000" + markers.ToString();
else if (markers < 100) n = "Marker00" + markers.ToString();
else n = "Marker0" + markers.ToString();
newnode.gameObject.name = n;
MarkerScript scr = newnode.GetComponent<MarkerScript>();
scr.OOODOOCDOD = false;
scr.objectScript = obj.GetComponent<RoadObjectScript>();
if(ODOQDQOO == null){
scr.waterLevel = ODOOOQCDCD.waterLevel;
scr.floorDepth = ODOOOQCDCD.floorDepth;
scr.ri = ODOOOQCDCD.indent;
scr.li = ODOOOQCDCD.indent;
scr.rs = ODOOOQCDCD.surrounding;
scr.ls = ODOOOQCDCD.surrounding;
scr.tension = 0.5f;
if(objectType == 1){

pos.y -= waterLevel;
newnode.position = pos;
}
}
if(objectType == 2){
if(scr.surface != null)scr.surface.gameObject.active = false;
}
ODOQDQOO = newnode.gameObject;
if(markers > 1){
ODDCDDDDCD(ODOOOQCDCD.geoResolution, false, false);
if(materialType == 0){
OOCCODDDDQ.OCDDQCQDOC(materialType);
}
}
}
public void ODDCDDDDCD(float geo, bool renderMode, bool camMode){
OOCCODDDDQ.OQOCCDCCOQ.Clear();
int ii = 0;
OOOODDQCQQ k;
foreach(Transform child  in obj)
{
if(child.name == "Markers"){
foreach(Transform marker   in child) {
MarkerScript markerScript = marker.GetComponent<MarkerScript>();
markerScript.objectScript = obj.GetComponent<RoadObjectScript>();
if(!markerScript.OOODOOCDOD) markerScript.OOODOOCDOD = OOCCODDDDQ.OQQCCCCOQD(marker);
k  = new OOOODDQCQQ();
k.position = marker.position;
k.num = OOCCODDDDQ.OQOCCDCCOQ.Count;
k.object1 = marker;
k.object2 = markerScript.surface;
k.tension = markerScript.tension;
k.ri = markerScript.ri;
if(k.ri < 1)k.ri = 1f;
k.li =markerScript.li;
if(k.li < 1)k.li = 1f;
k.rt = markerScript.rt;
k.lt = markerScript.lt;
k.rs = markerScript.rs;
if(k.rs < 1)k.rs = 1f;
k.OCCQDDQOQO = markerScript.rs;
k.ls = markerScript.ls;
if(k.ls < 1)k.ls = 1f;
k.OCQDCODCDC = markerScript.ls;
k.renderFlag = markerScript.bridgeObject;
k.OCDDOQCOOC = markerScript.distHeights;
k.newSegment = markerScript.newSegment;
k.floorDepth = markerScript.floorDepth;
k.waterLevel = waterLevel;
k.lockWaterLevel = markerScript.lockWaterLevel;
k.sharpCorner = markerScript.sharpCorner;
k.OQCCQQOODO = OOCCODDDDQ;
markerScript.markerNum = ii;
markerScript.distance = "-1";
markerScript.OCDCQQQDOQ = "-1";
OOCCODDDDQ.OQOCCDCCOQ.Add(k);
ii++;
}
}
}
distance = "-1";

OOCCODDDDQ.OQCCDDQDQD = ODOOOQCDCD.roadWidth;

OOCCODDDDQ.OQDDQQDQQD(geo, obj, ODOOOQCDCD.OOQDOOQQ, renderMode, camMode, objectType);
if(OOCCODDDDQ.leftVecs.Count > 0){
leftVecs = OOCCODDDDQ.leftVecs.ToArray();
rightVecs = OOCCODDDDQ.rightVecs.ToArray();
}
}
public void StartCam(){

ODDCDDDDCD(0.5f, false, true);

}
public void ODOOQCODCD(){
int i = 0;
foreach(Transform child  in obj)
{
if(child.name == "Markers"){
i = 1;
string n;
foreach(Transform marker in child) {
if(i < 10) n = "Marker000" + i.ToString();
else if (i < 100) n = "Marker00" + i.ToString();
else n = "Marker0" + i.ToString();
marker.name = n;
ODOQDQOO = marker.gameObject;
i++;
}
}
}
markers = i - 1;

ODDCDDDDCD(ODOOOQCDCD.geoResolution, false, false);
}
public void OQODCCCDDO(){
RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
ArrayList rObj = new ArrayList();
foreach (RoadObjectScript script in scripts) {
if(script.transform != transform) rObj.Add(script.transform);
}
if(ODODQOQO == null){
ODODQOQO = OOCCODDDDQ.OOOOQDQODQ();
ODODQOQOInt = OOCCODDDDQ.OOODDQDDCQ();
}
ODDCDDDDCD(0.5f, true, false);

OOCCODDDDQ.OQQQOCOOCQ(Vector3.zero, ODOOOQCDCD.raise, obj, ODOOOQCDCD.OOQDOOQQ, rObj, handleVegetation);
OCQDQQOQQC();
}
public ArrayList RebuildObjs(){
RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
ArrayList rObj = new ArrayList();
foreach (RoadObjectScript script in scripts) {
if(script.transform != transform) rObj.Add(script.transform);
}
return rObj;
}
public void OCQOOQCCDD(){

ODDCDDDDCD(ODOOOQCDCD.geoResolution, false, false);
if(OOCCODDDDQ != null) OOCCODDDDQ.OCQOOQCCDD();
ODODDDOO = false;
}
public void OCQDQQOQQC(){
OOCCODDDDQ.OCQDQQOQQC(ODOOOQCDCD.applySplatmap, ODOOOQCDCD.splatmapSmoothLevel, ODOOOQCDCD.renderRoad, ODOOOQCDCD.tuw, ODOOOQCDCD.roadResolution, ODOOOQCDCD.raise, ODOOOQCDCD.opacity, ODOOOQCDCD.expand, ODOOOQCDCD.offsetX, ODOOOQCDCD.offsetY, ODOOOQCDCD.beveledRoad, ODOOOQCDCD.splatmapLayer, ODOOOQCDCD.OdQODQOD, OOQQQDOD, OOQQQDODOffset, OOQQQDODLength);
}
public void OCQOQCDDOQ(){
OOCCODDDDQ.OCQOQCDDOQ(ODOOOQCDCD.renderRoad, ODOOOQCDCD.tuw, ODOOOQCDCD.roadResolution, ODOOOQCDCD.raise, ODOOOQCDCD.beveledRoad, ODOOOQCDCD.OdQODQOD, OOQQQDOD, OOQQQDODOffset, OOQQQDODLength);
}
public void OCDOOQDCCC(Vector3 pos, bool doInsert){


if(!displayRoad){
displayRoad = true;
OOCCODDDDQ.OQDOCOODQQ(displayRoad, OQODCDQQQQ);
}

int first = -1;
int second = -1;
float dist1 = 10000;
float dist2 = 10000;
Vector3 newpos = pos;
OOOODDQCQQ k;
OOOODDQCQQ k1 = (OOOODDQCQQ)OOCCODDDDQ.OQOCCDCCOQ[0];
OOOODDQCQQ k2 = (OOOODDQCQQ)OOCCODDDDQ.OQOCCDCCOQ[1];

OOCCODDDDQ.OCCDQDODQO(pos, out first, out second, out dist1, out dist2, out k1, out k2, out newpos);
pos = newpos;
if(doInsert && first >= 0 && second >= 0){
if(ODOOOQCDCD.OOQDOOQQ && second == OOCCODDDDQ.OQOCCDCCOQ.Count - 1){
OCQQDCDQOO(pos);
}else{
k = (OOOODDQCQQ)OOCCODDDDQ.OQOCCDCCOQ[second];
string name = k.object1.name;
string n;
int j = second + 2;
for(int i = second; i < OOCCODDDDQ.OQOCCDCCOQ.Count - 1; i++){
k = (OOOODDQCQQ)OOCCODDDDQ.OQOCCDCCOQ[i];
if(j < 10) n = "Marker000" + j.ToString();
else if (j < 100) n = "Marker00" + j.ToString();
else n = "Marker0" + j.ToString();
k.object1.name = n;
j++;
}
k = (OOOODDQCQQ)OOCCODDDDQ.OQOCCDCCOQ[first];
Transform newnode = (Transform)Instantiate(k.object1.transform, pos, k.object1.rotation);
newnode.gameObject.name = name;
newnode.parent = OQODCDQQQQ;
MarkerScript scr = newnode.GetComponent<MarkerScript>();
scr.OOODOOCDOD = false;
float	totalDist = dist1 + dist2;
float perc1 = dist1 / totalDist;
float paramDif = k1.ri - k2.ri;
scr.ri = k1.ri - (paramDif * perc1);
paramDif = k1.li - k2.li;
scr.li = k1.li - (paramDif * perc1);
paramDif = k1.rt - k2.rt;
scr.rt = k1.rt - (paramDif * perc1);
paramDif = k1.lt - k2.lt;
scr.lt = k1.lt - (paramDif * perc1);
paramDif = k1.rs - k2.rs;
scr.rs = k1.rs - (paramDif * perc1);
paramDif = k1.ls - k2.ls;
scr.ls = k1.ls - (paramDif * perc1);
ODDCDDDDCD(ODOOOQCDCD.geoResolution, false, false);
if(materialType == 0)OOCCODDDDQ.OCDDQCQDOC(materialType);
if(objectType == 2) scr.surface.gameObject.active = false;
}
}
ODOOQCODCD();
}
public void OCOQOQQOCC(){

DestroyImmediate(ODOOOQCDCD.OODOCOOCOC.gameObject);
OODOCOOCOC = null;
ODOOQCODCD();
}
public void ODQQQQOQOC(){

if(OOCCODDDDQ == null){
OOQOOCCOQD(transform, null, null, null);
}
OQOCCOOOQD.ODQQCQQOOO = true;
if(!OCCQDCDOQD){
geoResolution = 0.5f;
OCCQDCDOQD = true;
doTerrain = false;
ODOOQCODCD();
if(objectType < 2) OQODCCCDDO();
OOCCODDDDQ.terrainRendered = true;
OCQDQQOQQC();



}
if(displayRoad && objectType < 2){
Material mat = (Material)Resources.Load("roadMaterial", typeof(Material));
if(OOCCODDDDQ.road.renderer != null){

OOCCODDDDQ.road.renderer.material = mat;
}
foreach(Transform t in OOCCODDDDQ.road.transform){
if(t.gameObject.renderer != null){
t.gameObject.renderer.material = mat;
}
}
OOCCODDDDQ.road.transform.parent = null;
OOCCODDDDQ.road.layer = 0;
OOCCODDDDQ.road.name = gameObject.name;
}
else if(OOCCODDDDQ.road != null)DestroyImmediate(OOCCODDDDQ.road);
}
public void OQODOODCDD(){
}
public ArrayList OQCQDCQQOO(){
ArrayList param = new ArrayList();
foreach(Transform child in obj){
if(child.name == "Markers"){
foreach(Transform marker in child){
MarkerScript markerScript = marker.GetComponent<MarkerScript>();

param.Add(markerScript.ODDGDOOO);
param.Add(markerScript.ODDQOODO);
if(marker.name == "Marker0003"){



}
param.Add(markerScript.ODDQOOO);
}
}
}
return param;
}
public void ODQOQDDOCC(){
ArrayList arrNames = new ArrayList();
ArrayList arrInts = new ArrayList();
ArrayList arrIDs = new ArrayList();

for(int i=0;i<ODODOQQO.Length;i++){
if(ODODQQOD[i] == true){
arrNames.Add(ODODQOOQ[i]);
arrIDs.Add(ODODOQQO[i]);
arrInts.Add(i);
}
}
ODODDQOO = (string[]) arrNames.ToArray(typeof(string));
OOQQQOQO = (int[]) arrInts.ToArray(typeof(int));
}
public void OQOCCQDODQ(ArrayList arr, String[] DOODQOQO, String[] OODDQOQO){
if(arr == null)return;


bool saveSOs = false;
ODODOQQO = DOODQOQO;
ODODQOOQ = OODDQOQO;






ArrayList markerArray = new ArrayList();
if(obj == null)OOQOOCCOQD(transform, null, null, null);
foreach(Transform child  in obj) {
if(child.name == "Markers"){
foreach(Transform marker  in child) {
MarkerScript markerScript = marker.GetComponent<MarkerScript>();
markerScript.OQODQQDO.Clear();
markerScript.ODOQQQDO.Clear();
markerScript.OQQODQQOO.Clear();
markerScript.ODDOQQOO.Clear();
markerArray.Add(markerScript);
}
}
}
mSc = (MarkerScript[]) markerArray.ToArray(typeof(MarkerScript));





ArrayList arBools = new ArrayList();



int counter1 = 0;
int counter2 = 0;

if(ODQQQQQO != null){

if(arr.Count == 0) return;



for(int i = 0; i < ODODOQQO.Length; i++){
ODODDQQO so = (ODODDQQO)arr[i];

for(int j = 0; j < ODQQQQQO.Length; j++){
if(ODODOQQO[i] == ODQQQQQO[j]){
counter1++;


if(ODODQQOD.Length > j ) arBools.Add(ODODQQOD[j]);
else arBools.Add(false);

foreach(MarkerScript scr  in mSc) {


int l = -1;
for(int k = 0; k < scr.ODDOOQDO.Length; k++){
if(so.id == scr.ODDOOQDO[k]){
l = k;
break;
}
}
if(l >= 0){
scr.OQODQQDO.Add(scr.ODDOOQDO[l]);
scr.ODOQQQDO.Add(scr.ODDGDOOO[l]);
scr.OQQODQQOO.Add(scr.ODDQOOO[l]);

if(so.sidewaysDistanceUpdate == 0 || (so.sidewaysDistanceUpdate == 2 && (float)scr.ODDQOODO[l] != so.oldSidwaysDistance)){
scr.ODDOQQOO.Add(scr.ODDQOODO[l]);

}else{
scr.ODDOQQOO.Add(so.splinePosition);

}




}else{
scr.OQODQQDO.Add(so.id);
scr.ODOQQQDO.Add(so.markerActive);
scr.OQQODQQOO.Add(true);
scr.ODDOQQOO.Add(so.splinePosition);
}

}
}
}
if(so.sidewaysDistanceUpdate != 0){



}
saveSOs = false;
}
}


for(int i = 0; i < ODODOQQO.Length; i++){
ODODDQQO so = (ODODDQQO)arr[i];
bool flag = false;
for(int j = 0; j < ODQQQQQO.Length; j++){

if(ODODOQQO[i] == ODQQQQQO[j])flag = true;
}
if(!flag){
counter2++;

arBools.Add(false);

foreach(MarkerScript scr  in mSc) {
scr.OQODQQDO.Add(so.id);
scr.ODOQQQDO.Add(so.markerActive);
scr.OQQODQQOO.Add(true);
scr.ODDOQQOO.Add(so.splinePosition);
}

}
}

ODODQQOD = (bool[]) arBools.ToArray(typeof(bool));


ODQQQQQO = new String[ODODOQQO.Length];
ODODOQQO.CopyTo(ODQQQQQO,0);





ArrayList arInt= new ArrayList();
for(int i = 0; i < ODODQQOD.Length; i++){
if(ODODQQOD[i]) arInt.Add(i);
}
OOQQQOQO  = (int[]) arInt.ToArray(typeof(int));


foreach(MarkerScript scr  in mSc) {
scr.ODDOOQDO = (string[]) scr.OQODQQDO.ToArray(typeof(string));
scr.ODDGDOOO = (bool[]) scr.ODOQQQDO.ToArray(typeof(bool));
scr.ODDQOOO = (bool[]) scr.OQQODQQOO.ToArray(typeof(bool));
scr.ODDQOODO = (float[]) scr.ODDOQQOO.ToArray(typeof(float));

}
if(saveSOs){

}



}
public bool CheckWaterHeights(){
if(ODODQQODQC.terrain == null) return false;
bool flag = true;

float y = ODODQQODQC.terrain.transform.position.y;
foreach(Transform child  in obj) {
if(child.name == "Markers"){
foreach(Transform marker  in child) {
//MarkerScript markerScript = marker.GetComponent<MarkerScript>();
if(marker.position.y - y <= 0.1f) flag = false;
}
}
}
return flag;
}
}
