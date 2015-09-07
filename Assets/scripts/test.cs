using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
	int gridSize = 15;
	float perlinScale = 0.125f;
	public Transform box;
	public Transform root;
	public Transform player;
	public Transform light;
	Perlin perlin = new Perlin ();
	bool once = true;
	bool[,,] grid;
	Vector3 playerVoxel = new Vector3 (0, 0, 0);

	// Use this for initialization
	void Start ()
	{
	}

	bool[,,] fillGrid (Vector3 voxel)
	{
		float vx = voxel.x;
		float vy = voxel.y;
		float vz = voxel.z;
		int cubes = 0;
		bool[,,] g = new bool[gridSize + 2, gridSize + 2, gridSize + 2];
		for (int x = 0; x < gridSize + 2; x++) {
			for (int y = 0; y < gridSize + 2; y++) {
				for (int z = 0; z < gridSize + 2; z++) {
					float v = perlin.Noise ((x + vx * gridSize - 1) * perlinScale, (y + vy * gridSize - 1) * perlinScale, (z + vz * gridSize - 1) * perlinScale);
					if (v + 0.5 > 0.34) {
						cubes++;
						g [x, y, z] = true;
					} 
				}
			}
		}
		print (cubes + " total points in grid");
		return g;
	}

	bool checkPositionEdges (int x, int y, int z)
	{
		bool left = grid [x - 1, y, z];
		bool right = grid [x + 1, y, z];
		bool front = grid [x, y + 1, z];
		bool back = grid [x, y - 1, z];
		bool top = grid [x, y, z + 1];
		bool bottom = grid [x, y, z - 1];

		if (grid [x, y, z]) {
			if (left && right && top && bottom && front && back) {
				// we're totally BOXED in, no need to render
				return false;
			} else {
				// we have at least once face exposed, show ourselves
				return true;
			}
		} else {
			return false;
		}
	}

	Vector3 findSafeSpot ()
	{
		for (int x = 10; x < gridSize; x++) {
			for (int y = 4; y < gridSize - 3; y++) {
				for (int z = 10; z < gridSize; z++) {
					if (grid [x, y, z] && !grid [x, y + 1, z] && !grid [x, y + 2, z]) {
						return  new Vector3 (x * 2, y * 2 + 2, z * 2);
					}
				}
			}
		}
		return new Vector3 (10, 200, 10);
	}

	void generate (Vector3 voxel)
	{
		int cubes = 0;
		float vx = voxel.x;
		float vy = voxel.y;
		float vz = voxel.z;
		grid = fillGrid (voxel);

		string targetName = "voxel " + vx + "," + vy + "," + vz;

		GameObject target = GameObject.Find (targetName);

		if (target) {
			target.transform.Find ("geometry").gameObject.SetActive (true);
			print ("target voxel already exists!");
			return;
		}

		GameObject node = new GameObject (targetName);
		node.transform.parent = root;

		GameObject geometry = new GameObject ("geometry");
		geometry.transform.parent = node.transform;
		geometry.AddComponent<MeshFilter> ();
		MeshRenderer mr = geometry.AddComponent<MeshRenderer> ();
		geometry.AddComponent<MeshCollider> ();


		for (int x = 1; x < gridSize +1; x++) {
			for (int y = 1; y < gridSize +1; y++) {
				for (int z = 1; z < gridSize +1; z++) {
					if (checkPositionEdges (x, y, z)) {
						cubes++;

						Transform child = Instantiate (box, new Vector3 ((x + vx * gridSize) * 2, (y + vy * gridSize) * 2, (z + + vz * gridSize) * 2), Quaternion.identity) as Transform;
						child.parent = geometry.transform;

						child.gameObject.isStatic = true;
					}
				}
			}
		}

		MeshFilter[] meshFilters = geometry.GetComponentsInChildren<MeshFilter> ();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length) {
			combine [i].mesh = meshFilters [i].sharedMesh;
			combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
			meshFilters [i].gameObject.active = false;
			i++;
		}
		Mesh m = geometry.transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
		geometry.transform.GetComponent<MeshCollider> ().sharedMesh = m;

		Material mat = Resources.Load ("rock", typeof(Material)) as Material;
		mr.material = mat;

		geometry.transform.GetComponent<MeshFilter> ().mesh.CombineMeshes (combine);
		geometry.transform.gameObject.active = true;

		//node.isStatic = true;
		print (cubes + " total places boxes");
	}

	Vector3 getColor (float x, float  y, float z)
	{
		float a = perlin.Noise (x * perlinScale, y * perlinScale, z * perlinScale);
		float b = perlin.Noise (x * perlinScale + 100, y * perlinScale, z * perlinScale);
		float c = perlin.Noise (x * perlinScale, y * perlinScale - 100, z * perlinScale);
		Vector3 v = new Vector3 (a + 0.5f, b + 0.5f, c + 0.5f);
		print (v);
		return v;
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 voxel = player.position;
		voxel /= (gridSize * 2);
		voxel.x = Mathf.FloorToInt (voxel.x);
		voxel.y = Mathf.FloorToInt (voxel.y);
		voxel.z = Mathf.FloorToInt (voxel.z);

		if (voxel != playerVoxel) {
			print ("player changed voxel");
			print (voxel);
			playerVoxel = voxel;

			// turn off all nodes
			for (int i = 0; i < root.GetChildCount(); ++i) {
				GameObject node = root.GetChild (i).gameObject;
				node.transform.Find ("geometry").gameObject.SetActive (false);
			}

			for (int x = -1; x < 2; x++) {
				for (int y = -1; y < 2; y++) {
					for (int z = -1; z < 2; z++) {
						generate (playerVoxel + new Vector3 (x, y, z));
					}	
				}
			}
		}

		//------- once
		if (once) {
			generate (new Vector3 (0, 0, 0));
			print ("placing player");
			player.position = findSafeSpot ();
			once = false;
		}

		// place light
		if (Input.GetMouseButtonDown (0)) {
			GameObject g = (GameObject)Instantiate (light, player.position, Quaternion.identity);
			g.transform.parent = root.Find (voxelName (playerVoxel)).transform;
		}
	}

	string voxelName (Vector3 v)
	{
		float vx = v.x;
		float vy = v.y;
		float vz = v.z;
		
		string s = "voxel " + vx + "," + vy + "," + vz;
		return s;
	}


}
