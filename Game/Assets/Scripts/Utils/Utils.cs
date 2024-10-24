using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-20, 20), 4, Random.Range(-20, 20)); // give a set area location to place the spawn point randomly on the x and z axis 
    }

    public static void SetRenderLayerInChildren(Transform transform, int layerNumber) //sets objects to be rendered or not. 
    {
        foreach (Transform trans in transform.GetComponentsInChildren<Transform>(true))
        {
           if (trans.CompareTag("IgnoreLayerChange"))
                continue; //if object has the tag ignore layer it loops back from this point. Does not chang layer

            trans.gameObject.layer = layerNumber;

        }
           
    }

}


