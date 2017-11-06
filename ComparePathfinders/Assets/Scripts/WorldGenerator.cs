using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

	public int worldHeight;
	public int worldWidth;
	public int worldDepth;
	[Range(0.001f,1)]
	public float noiseScale;
	private float[,] world;

	// Mesh data
	Vector3[] vertices;
	int[] triangles;

	// Actual mesh
	Mesh mesh;
	MeshFilter meshFilter;

	Renderer meshTexture;

	// Use this for initialization
	void Start () {
		meshTexture = GetComponent<MeshRenderer>();
		meshFilter = GetComponent<MeshFilter>();

		transform.localScale = new Vector3(worldWidth, worldHeight, worldDepth);

		GenerateNoise();
		GenerateTexture();
		GenerateMesh();
		meshFilter.mesh = mesh;
	}

	void GenerateNoise() {
		print("Generate noise");
		world = new float[worldWidth, worldDepth];

		for(int i=0;i<worldWidth;i++) {
			for(int j=0;j<worldWidth;j++) {
				world[i,j] = Mathf.PerlinNoise(i*noiseScale, j*noiseScale);
			}
		}
	}

	void GenerateTexture() {
		print("Generates texture");
		Texture2D texture= new Texture2D(worldWidth, worldDepth);
		
		Color[] colorMap = new Color[worldWidth * worldDepth];
		for(int i=0;i<worldWidth;i++) {
			for(int j=0;j<worldDepth;j++) {
				colorMap[i*worldDepth + j] = Color.Lerp(Color.black, Color.white, world[i,j]);
			}
		}
		texture.SetPixels(colorMap);
		texture.Apply();

		meshTexture.material.mainTexture = texture;
	}

	void GenerateMesh() {
		print("Generates mesh");
		// Mesh data
		vertices = new Vector3[worldWidth*worldDepth];
		triangles = new int[(worldWidth-1) * (worldDepth-1) * 6];

		int trianglesIndex = 0;
		int vertexIndex = 0;
		for(int i=0;i<worldWidth;i++) {
			for(int j=0;j<worldDepth;j++, vertexIndex++) {
				vertices[vertexIndex] = new Vector3(i,world[i,j],j);

				if(i<worldWidth-1 && j<worldDepth-1) {
					// Triangle 1
					triangles[trianglesIndex++] = vertexIndex;
					triangles[trianglesIndex++] = vertexIndex+worldWidth+1;
					triangles[trianglesIndex++] = vertexIndex+worldWidth;

					// Triangle 2
					triangles[trianglesIndex++] = vertexIndex+worldWidth+1;
					triangles[trianglesIndex++] = vertexIndex;
					triangles[trianglesIndex++] = vertexIndex+1;
				}
			}
		}

		mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}
