using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using EasyRoads3D;
using EasyRoads3DEditor;
public class EasyRoadsEditorMenu : ScriptableObject {







[MenuItem( "EasyRoads3D/New Object" )]
public static void  CreateEasyRoads3DObject ()
{

Terrain[] terrains = (Terrain[]) FindObjectsOfType(typeof(Terrain));
if(terrains.Length == 0){
EditorUtility.DisplayDialog("Alert", "No Terrain objects found! EasyRoads3D objects requires a terrain object to interact with. Please create a Terrain object first", "Close");
return;
}



if(NewEasyRoads3D.instance == null){
NewEasyRoads3D window = (NewEasyRoads3D)ScriptableObject.CreateInstance(typeof(NewEasyRoads3D));
window.ShowUtility();
}



}
[MenuItem( "EasyRoads3D/Back Up/Terrain Height Data" )]
public static void  GetTerrain ()
{
if(GetEasyRoads3DObjects()){

ODODQQODQC.ODCOOQQOCD(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "EasyRoads3D/Restore/Terrain Height Data" )]
public static void  SetTerrain ()
{
if(GetEasyRoads3DObjects()){

ODODQQODQC.OODDCCDDDQ(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "EasyRoads3D/Back Up/Terrain Splatmap Data" )]
public static void  ODCCCODQQQ()
{
if(GetEasyRoads3DObjects()){

ODODQQODQC.ODCCCODQQQ(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "EasyRoads3D/Restore/Terrain Splatmap Data" )]
public static void  OQOOQDODOD ()
{
if(GetEasyRoads3DObjects()){
string path = "";
if(EditorUtility.DisplayDialog("Road Splatmap", "Would you like to merge the terrain splatmap(s) with a road splatmap?", "Yes", "No")){
path = EditorUtility.OpenFilePanel("Select png road splatmap texture", "", "png");
}


ODODQQODQC.OOODQDDQDO(true, 100, 4, path, Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "EasyRoads3D/Back Up/Terrain Vegetation Data" )]
public static void  OODQDQQOQD()
{
if(GetEasyRoads3DObjects()){

ODODQQODQC.OODQDQQOQD(Selection.activeGameObject, null, "");
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "EasyRoads3D/Back Up/All Terrain Data" )]
public static void  GetAllData()
{
if(GetEasyRoads3DObjects()){

ODODQQODQC.ODCOOQQOCD(Selection.activeGameObject);
ODODQQODQC.ODCCCODQQQ(Selection.activeGameObject);
ODODQQODQC.OODQDQQOQD(Selection.activeGameObject, null,"");
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "EasyRoads3D/Restore/Terrain Vegetation Data" )]
public static void  OQDODDCDDC()
{
if(GetEasyRoads3DObjects()){

ODODQQODQC.OQDODDCDDC(Selection.activeGameObject);
}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}
}
[MenuItem( "EasyRoads3D/Restore/All Terrain Data" )]
public static void  RestoreAllData()
{
if(GetEasyRoads3DObjects()){

ODODQQODQC.OODDCCDDDQ(Selection.activeGameObject);
ODODQQODQC.OOODQDDQDO(true, 100, 4, "", Selection.activeGameObject);
ODODQQODQC.OQDODDCDDC(Selection.activeGameObject);

}else{
EditorUtility.DisplayDialog("Alert", "No EasyRoads3D objects found! Terrain functions cannot be accessed!", "Close");
}


}
public static bool GetEasyRoads3DObjects(){
RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
bool flag = false;
foreach (RoadObjectScript script in scripts) {
if(script.OOCCODDDDQ == null){
script.OOQCOOCCQO(null, null, null);
}
flag = true;
}
return flag;
}

static private void OQDQCCCOCQ(RoadObjectScript target){
EditorUtility.DisplayProgressBar("Build EasyRoads3D Object - " + target.gameObject.name,"Initializing", 0);

RoadObjectScript[] scripts = (RoadObjectScript[])FindObjectsOfType(typeof(RoadObjectScript));
ArrayList rObj = new ArrayList();
Undo.RegisterUndo(Terrain.activeTerrain.terrainData, "EasyRoads3D Terrain leveling");
foreach(RoadObjectScript script in scripts) {
if(script.transform != target.transform) rObj.Add(script.transform);
}
if(target.ODODQOQO == null){
target.ODODQOQO = target.OOCCODDDDQ.OOOOQDQODQ();
target.ODODQOQOInt = target.OOCCODDDDQ.OOODDQDDCQ();
}
target.ODDCDDDDCD(0.5f, true, false);

ArrayList hitODCCCOOCQQ = target.OOCCODDDDQ.OQQQOCOOCQ(Vector3.zero, target.raise, target.obj, target.OOQDOOQQ, rObj, target.handleVegetation);
ArrayList changeArr = new ArrayList();
float stepsf = Mathf.Floor(hitODCCCOOCQQ.Count / 10);
int steps = Mathf.RoundToInt(stepsf);
for(int i = 0; i < 10;i++){
changeArr = target.OOCCODDDDQ.ODOOQOODQC(hitODCCCOOCQQ, i * steps, steps, changeArr);
EditorUtility.DisplayProgressBar("Build EasyRoads3D Object - " + target.gameObject.name,"Updating Terrain", i * 10);
}

changeArr = target.OOCCODDDDQ.ODOOQOODQC(hitODCCCOOCQQ, 10 * steps, hitODCCCOOCQQ.Count - (10 * steps), changeArr);
target.OOCCODDDDQ.ODODDDDODC(changeArr, rObj);

target.OCQDQQOQQC();
EditorUtility.ClearProgressBar();

}
private static void SetWaterScript(RoadObjectScript target){
for(int i = 0; i < target.ODDQQDCODD.Length; i++){
if(target.OOCCODDDDQ.road.GetComponent(target.ODDQQDCODD[i]) != null && i != target.selectedWaterScript)DestroyImmediate(target.OOCCODDDDQ.road.GetComponent(target.ODDQQDCODD[i]));
}
if(target.ODDQQDCODD[0] != "None Available!"  && target.OOCCODDDDQ.road.GetComponent(target.ODDQQDCODD[target.selectedWaterScript]) == null){
target.OOCCODDDDQ.road.AddComponent(target.ODDQQDCODD[target.selectedWaterScript]);

}
}
public static Vector3 ReadFile(string file)
{
Vector3 pos = Vector3.zero;
if(File.Exists(file)){
StreamReader streamReader = File.OpenText(file);
string line = streamReader.ReadLine();
line = line.Replace(",",".");
string[] lines = line.Split("\n"[0]);
string[] arr = lines[0].Split("|"[0]);
float.TryParse(arr[0],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.x);
float.TryParse(arr[1],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.y);
float.TryParse(arr[2],System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out pos.z);
}
return pos;
}
}
