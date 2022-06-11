using System.Collections;
using UnityEngine;

public static class MeshGenerator {
    public static MeshData GenerateTerrainMesh(float[,] heightMap){
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;
        for (int y = 0; y< height; y++){
            for (int x = 0; x < width; x++){
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y], topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                //skip bottom and left ones
                if(x < width - 1 && y < height - 1){
                    //main,bottom left, bottom;
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);

                    //bottom left, main, left;
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }

        return meshData;
    }
}

// W is array row length(width)
//  i  , i+1
//  i+w, i+w+1
public class MeshData{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;
    public MeshData(int meshWidht, int meshHeight){
        vertices = new Vector3[meshWidht * meshHeight];
        uvs = new Vector2[meshWidht * meshHeight];
        triangles = new int[(meshWidht - 1) * (meshHeight - 1) * 6];
    }
    public void AddTriangle(int a, int b, int c){
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}