using UnityEngine;

public class CombineWalls : MonoBehaviour
{
    [ContextMenu("Kombiniere W�nde")]
    void Combine()
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
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Falls sehr viele W�nde

        combinedMesh.CombineMeshes(combine);

        GameObject combined = new GameObject("CombinedWalls");
        combined.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
        combined.AddComponent<MeshRenderer>().sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
        combined.isStatic = true;

        // Originale W�nde deaktivieren oder l�schen
        foreach (Transform child in transform)
        {
            if (child != transform)
                child.gameObject.SetActive(false);
        }

        //Debug.Log("W�nde erfolgreich kombiniert!");
    }
}
