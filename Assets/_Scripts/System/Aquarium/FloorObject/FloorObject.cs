using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorObject : MonoBehaviour
{
    [SerializeField]
    public Vector2 floorSize;

    private Mesh floor_mesh;

    private Vector3[] vertices;
    private int[] triangles;

    private BoxCollider2D boxCollider;

    void Start()
    {
        floor_mesh = new Mesh();
        vertices = new Vector3[]{ new Vector3(0,0,0),
                                  new Vector3(0,floorSize.y,0),
                                  new Vector3(floorSize.x,0,0),
                                  new Vector3(floorSize.x,floorSize.y,0) };
        triangles = new int[]{ 0,1,2, 2,1,3 };
        floor_mesh.vertices = vertices;
        floor_mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = floor_mesh;

        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = floorSize;
        boxCollider.offset = new Vector2(floorSize.x/2, floorSize.y/2);
    }
}
