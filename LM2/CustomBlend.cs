using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Blend))]
public class CustomBlend : MonoBehaviour {

    private Blend blendScript;
    public float cps = 0.5f; // Change per second
    public float maxChange = 1;
    private float accumChange = 0;
	// Update is called once per frame
	void Update() {

        if (maxChange > accumChange)
        {
            if (blendScript == null)
            {
                blendScript = this.GetComponent<Blend>();
                blendScript.LoadMaterials();
            }

            float change = cps * Time.deltaTime;// this will cause 50% change in a second 
            Texture2D target = LightmapSettings.lightmaps[this.renderer.lightmapIndex].lightmapFar;// the target texture in the lightmap settings
            Texture2D destination = blendScript.BlendTexture;// or a custom texture with the right texture importer settings

            accumChange += change;

            blendScript.BlendNow(target, destination, accumChange);
        }
	}
}
