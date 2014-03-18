using UnityEngine;
using System.Collections;

public class Relightning : MonoBehaviour {
  
    public Color ambient;
    public Light[] lights;
    
    void Start() {

        LightmapSettings.lightProbes = Instantiate(LightmapSettings.lightProbes) as LightProbes;

        if (lights == null || lights.Length==0)
         lights = (Light[])   GameObject.FindObjectsOfType(typeof(Light));

        float[] coefficients = LightmapSettings.lightProbes.coefficients;
        int coefficientsPerProbe = 27;
        int probeCount = LightmapSettings.lightProbes.count;
        Vector3[] probePositions = LightmapSettings.lightProbes.positions;
        int i = 0;
        i = 0;

        while (i < probeCount) {
            AddSHAmbientLight(ambient, coefficients, i * coefficientsPerProbe);
            i++;
        }
        // cast rays from the sun and find bounces
        // cast rays of every light and find bounces
        foreach (Light l in lights) {
            if (l.type == LightType.Directional) {
                i = 0;
                while (i < probeCount) {
                    AddSHDirectionalLight(l.color, -l.transform.forward, l.intensity, coefficients, i * coefficientsPerProbe);
                    i++;
                }
            } else
                if (l.type == LightType.Point) {
                    i = 0;
                    while (i < probeCount) {
                        AddSHPointLight(l.color, l.transform.position, l.range, l.intensity, coefficients, i * coefficientsPerProbe, probePositions[i]);
                        i++;
                    }
                }
        }
        LightmapSettings.lightProbes.coefficients = coefficients;
    }
    void AddSHAmbientLight(Color color, float[] coefficients, int index) {
        float k2SqrtPI = 3.54490770181F;
        coefficients[index + 0] += color.r * k2SqrtPI;
        coefficients[index + 1] += color.g * k2SqrtPI;
        coefficients[index + 2] += color.b * k2SqrtPI;
    }
    void AddSHDirectionalLight(Color color, Vector3 direction, float intensity, float[] coefficients, int index) {
        float kInv2SqrtPI = 0.28209479177F;
        float kSqrt3Div2SqrtPI = 0.4886025119F;
        float kSqrt15Div2SqrtPI = 1.09254843059F;
        float k3Sqrt5Div4SqrtPI = 0.94617469576F;
        float kSqrt15Div4SqrtPI = 0.5462742153F;
        float kOneThird = 0.33333333333F;
        float[] dirFactors = new float[9];
        dirFactors[0] = kInv2SqrtPI;
        dirFactors[1] = -direction.y * kSqrt3Div2SqrtPI;
        dirFactors[2] = direction.z * kSqrt3Div2SqrtPI;
        dirFactors[3] = -direction.x * kSqrt3Div2SqrtPI;
        dirFactors[4] = direction.x * direction.y * kSqrt15Div2SqrtPI;
        dirFactors[5] = -direction.y * direction.z * kSqrt15Div2SqrtPI;
        dirFactors[6] = (direction.z * direction.z - kOneThird) * k3Sqrt5Div4SqrtPI;
        dirFactors[7] = -direction.x * direction.z * kSqrt15Div2SqrtPI;
        dirFactors[8] = (direction.x * direction.x - direction.y * direction.y) * kSqrt15Div4SqrtPI;
        float kNormalization = 2.95679308573F;
        intensity *= 2.0F;
        float rscale = color.r * intensity * kNormalization;
        float gscale = color.g * intensity * kNormalization;
        float bscale = color.b * intensity * kNormalization;
        int i = 0;
        while (i < 9) {
            float c = dirFactors[i];
            coefficients[index + 3 * i + 0] += c * rscale;
            coefficients[index + 3 * i + 1] += c * gscale;
            coefficients[index + 3 * i + 2] += c * bscale;
            ++i;
        }
    }
    void AddSHPointLight(Color color, Vector3 position, float range, float intensity, float[] coefficients, int index, Vector3 probePosition) {
        Vector3 probeToLight = position - probePosition;
        float attenuation = 1.0F / (1.0F + 25.0F * probeToLight.sqrMagnitude / range * range);
        AddSHDirectionalLight(color, probeToLight.normalized, intensity * attenuation, coefficients, index);
    }
}
