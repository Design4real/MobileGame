@CustomEditor(EasyRoads3DTerrainID)
class TerrainEditorScript extends Editor
{
function OnSceneGUI()
{
if(Event.current.shift && RoadObjectScript.OCDDDQQOCD != null) Selection.activeGameObject = RoadObjectScript.OCDDDQQOCD.gameObject;
else RoadObjectScript.OCDDDQQOCD = null;
}
}
