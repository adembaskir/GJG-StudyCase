using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [Header("Variables:")]
    [SerializeField] private float shapeSpeed = 4f;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private List<Vector3> upperSideVertices = new List<Vector3>();
    private Mesh deformingMesh;
    private Vector3[] originalVertices;
    private Vector3[] originalVerticesPositions;
    private float maxDistanceForVertex = 170;
    private const float maxDistanceConstant = 170;
    
    private int selectedVerticeCount = 0;
    private void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshRenderer>().material.renderQueue = 2998;

        originalVertices = deformingMesh.vertices;
        originalVerticesPositions = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            originalVerticesPositions[i] = originalVertices[i];
            originalVertices[i] = new Vector3(originalVertices[i].x, originalVertices[i].y, originalVertices[i].z + 3.2f);
        }
        deformingMesh.vertices = originalVertices;

        upperSideVertices = new List<Vector3>(new Vector3[originalVertices.Length]);
        for (int i = 0; i < upperSideVertices.Count; i++)
        {
            upperSideVertices[i] = new Vector3(0, 2, 5);

        }
        
    }
    private void Update()
    {
       
        if (Input.GetMouseButton(0))
        {
            if (maxDistanceForVertex < 250)
            {
                maxDistanceForVertex += Time.deltaTime * 25;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            maxDistanceForVertex = maxDistanceConstant;
            deformingMesh.RecalculateNormals();
            
        }
    }
    public void DetectUpperSideOfMesh()
    {
        Mesh upperSideMesh = new Mesh();
        upperSideMesh.vertices = upperSideVertices.ToArray();
        upperSideMesh.triangles = deformingMesh.triangles;
        upperSideMesh.RecalculateBounds();
        upperSideMesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = upperSideMesh;
    }
    private float GetComplateRatio()
    {
        return (float)selectedVerticeCount / (float)upperSideVertices.Count;
    }
    public void AddForceToNormal(Vector3 point, float force)
    {
        point = transform.InverseTransformPoint(point);
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 pointToVertex = originalVertices[i] - point;
            if (pointToVertex.sqrMagnitude < maxDistanceForVertex && originalVertices[i] != originalVerticesPositions[i])
            {
                float speedAccordingDistance = (50 / pointToVertex.sqrMagnitude);
                if (pointToVertex.sqrMagnitude < 150)
                {
                    
                    originalVertices[i] = Vector3.Lerp(originalVertices[i], originalVerticesPositions[i],
                    (shapeSpeed * 3 * Time.deltaTime));
                }
                else
                {
                    
                    originalVertices[i] = Vector3.Lerp(originalVertices[i], originalVerticesPositions[i],
                    speedAccordingDistance * (shapeSpeed * Time.deltaTime));
                }


                if (!upperSideVertices.Contains(originalVertices[i]))
                {
                    
                    if (originalVertices[i] == originalVerticesPositions[i])
                    {
                        float complateRatio = GetComplateRatio();
                        
                        if (complateRatio>0.22f)
                        {
                            Debug.Log("Win");
                            eventManager.CallFirstStageComplatedEvent();
                        }

                        upperSideVertices[i] = originalVertices[i];
                        selectedVerticeCount++;
                    }

                    deformingMesh.RecalculateNormals();
                }
            }
        }
        deformingMesh.vertices = originalVertices;
    }
}