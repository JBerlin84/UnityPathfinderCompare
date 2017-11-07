using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(NavMeshSurface))]
public class WorldGenerator : MonoBehaviour {

	public int worldHeight = 5;
	public int worldWidth = 50;
	public int worldDepth = 50;
	[Range(0.001f,1)]
	public float noiseScale = 0.1f;
	private float[,] world;
	public float[,] World {
		get { return world; }
	}

	// Just some fun stuff
	public float flowSpeedX = 0.1f;
	public float flowSpeedY = 0.1f;
	float offsetX = 0;
	float offsetY = 0;

	// Mesh data
	Vector3[] vertices;
	int[] triangles;

	// Actual mesh
	Mesh mesh;
	MeshFilter meshFilter;

	Renderer meshTexture;

	NavMeshSurface navMeshSurface;

	// Use this for initialization
	void Awake () {
		meshTexture = GetComponent<MeshRenderer>();
		meshFilter = GetComponent<MeshFilter>();
		navMeshSurface = GetComponent<NavMeshSurface>();

		GenerateNoise();
		GenerateTexture();
		GenerateMesh();
		meshFilter.mesh = mesh;

		//transform.localScale = new Vector3(worldWidth, worldHeight, worldDepth);
		navMeshSurface.BuildNavMesh();
	}

	void Update() {
		// GenerateNoise();
		// GenerateTexture();
		// GenerateMesh();
		// meshFilter.mesh = mesh;
	}

	void GenerateNoise() {
		world = new float[worldWidth, worldDepth];

		for(int i=0;i<worldWidth;i++) {
			for(int j=0;j<worldWidth;j++) {
				world[i,j] = Mathf.PerlinNoise(i*noiseScale+offsetY, j*noiseScale+offsetX) * worldHeight;
			}
		}
		offsetX += flowSpeedX;
		offsetY += flowSpeedY;
	}

	void GenerateTexture() {
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
