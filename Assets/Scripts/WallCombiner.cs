using UnityEditor;
using UnityEngine;

public class CombineWalls : MonoBehaviour
{
    public void Combine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].transform == transform) continue;

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine);

        GameObject combined = new GameObject("CombinedWalls");
        combined.transform.position = transform.position;
        combined.isStatic = true;

        MeshFilter mf = combined.AddComponent<MeshFilter>();
        mf.sharedMesh = combinedMesh;

        MeshRenderer mr = combined.AddComponent<MeshRenderer>();
        mr.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

        MeshCollider mc = combined.AddComponent<MeshCollider>();
        mc.sharedMesh = combinedMesh;

        combined.layer = LayerMask.NameToLayer("Obstacle");

        foreach (Transform child in transform)
        {
            if (child != transform)
                child.gameObject.SetActive(false);
        }

        Debug.Log("combine ist fertig");
    }
}
