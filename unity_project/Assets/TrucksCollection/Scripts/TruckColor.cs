using UnityEngine;
using System.Collections;

public class TruckColor : MonoBehaviour {
//----------------------------------------------------------------
//
//  This script allows the user to change the main color of each 
//  instance of all cars without adding new materials.
//
//----------------------------------------------------------------
    public Color car_color;
    public Material car_main_color;
    //----------------------------
    Renderer car_renderer;
    int mat_index;

//----------------------------------------------------------------
void Start () 
{
    car_renderer = gameObject.GetComponent<Renderer>();           

    string user_mat_name = car_main_color + " ";
    for(int i=0; i<car_renderer.materials.Length; i++)
    {
        string obj_mat_name = car_renderer.transform.renderer.materials[i] + " ";
        bool match = true;
        for(int j=0;j<40;j++)
        {
            if(user_mat_name[j] != obj_mat_name[j])        
                match = false;
        }
        if(match == true)
            mat_index = i;
    }
}
//----------------------------------------------------------------
void Update ()
{
    car_renderer.transform.renderer.materials[mat_index].color = car_color;
}
//----------------------------------------------------------------
}
