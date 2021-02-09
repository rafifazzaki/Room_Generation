using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tripolygon.UModeler;

public class MeshBuild : MonoBehaviour
{
    void Start()
    {
        var modeler = this.GetComponent<UModeler>();
        if (modeler != null)
        {
            modeler.BuildEdMesh();
        }
    }
}
