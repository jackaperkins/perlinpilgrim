using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour
{

	public static GameObject cubes (Vector3 pos)
	{
		GameObject o = new GameObject ("cube");
		o.transform.position = pos;
		MeshFilter mf = o.AddComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		MeshRenderer mr = o.AddComponent<MeshRenderer> ();
		MeshCollider mc =  o.AddComponent<MeshCollider> ();

		Material mat = Resources.Load ("face", typeof(Material)) as Material;
		print (mat);
		mf.mesh =  cube ();
		mc.sharedMesh = mf.mesh;

		mr.material = mat;

		return o;
	}

	private static Mesh cube () {
		Mesh mesh = new Mesh ();
		ArrayList data = meshData (new bool[3], new Vector3());

		mesh.vertices = (Vector3[]) data[0];
		mesh.triangles = (int[]) data[1];
		mesh.normals = (Vector3[]) data[2];
		mesh.uv = (Vector2[]) data[3];
		
		mesh.RecalculateBounds ();
		mesh.Optimize ();
		return mesh;
	}

	private static ArrayList meshData (bool[] sides, Vector3 offset)
	{
			
		float length = 2f;
		float width = 2f;
		float height = 2f;

	
		ArrayList a = new ArrayList ();
		a.Add (new Vector3[0]);
		a.Add (new int[0]);
		a.Add (new Vector3[0]);
		a.Add (new Vector2[0]);


	
		Vector3 p0 = new Vector3 (-length * .5f, -width * .5f, height * .5f) + offset;
		Vector3 p1 = new Vector3 (length * .5f, -width * .5f, height * .5f)+ offset;
		Vector3 p2 = new Vector3 (length * .5f, -width * .5f, -height * .5f)+ offset;
		Vector3 p3 = new Vector3 (-length * .5f, -width * .5f, -height * .5f)+ offset;	
		
		Vector3 p4 = new Vector3 (-length * .5f, width * .5f, height * .5f)+ offset;
		Vector3 p5 = new Vector3 (length * .5f, width * .5f, height * .5f)+ offset;
		Vector3 p6 = new Vector3 (length * .5f, width * .5f, -height * .5f)+ offset;
		Vector3 p7 = new Vector3 (-length * .5f, width * .5f, -height * .5f)+ offset;
		
		Vector3[] vertices = new Vector3[]
		{
		// Bottom
			p0, p1, p2, p3,
			
		// Left
			p7, p4, p0, p3,
			
		// Front
			p4, p5, p1, p0,
			
		// Back
			p6, p7, p3, p2,
			
		// Right
			p5, p6, p2, p1,
			
		// Top
			p7, p6, p5, p4
		};

		Vector3 up = Vector3.up;
		Vector3 down = Vector3.down;
		Vector3 front = Vector3.forward;
		Vector3 back = Vector3.back;
		Vector3 left = Vector3.left;
		Vector3 right = Vector3.right;
		
		Vector3[] normals = new Vector3[]
		{
		// Bottom
			down, down, down, down,
			
		// Left
			left, left, left, left,
			
		// Front
			front, front, front, front,
			
		// Back
			back, back, back, back,
			
		// Right
			right, right, right, right,
			
		// Top
			up, up, up, up
		};

		Vector2 _00 = new Vector2 (0f, 0f);
		Vector2 _10 = new Vector2 (1f, 0f);
		Vector2 _01 = new Vector2 (0f, 1f);
		Vector2 _11 = new Vector2 (1f, 1f);
		
		Vector2[] uvs = new Vector2[]
		{
		// Bottom
			_11, _01, _00, _10,
			
		// Left
			_11, _01, _00, _10,
			
		// Front
			_11, _01, _00, _10,
			
		// Back
			_11, _01, _00, _10,
			
		// Right
			_11, _01, _00, _10,
			
		// Top
			_11, _01, _00, _10,
		};

		int[] triangles = new int[]
		{
		// Bottom
			3, 1, 0,
			3, 2, 1,			
			
		// Left
			3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
			3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
			
		// Front
			3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
			3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
			
		// Back
			3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
			3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
			
		// Right
			3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
			3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
			
		// Top
			3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
			3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
			
		};


		a [0] = vertices;
		a[1] =  triangles;
		a[2] = normals;
		a[3] =  uvs;

		return a;
	}

	/*
	static int[] combineInt (int[] a, int[] b) {
		int[] c = new int[a.Length + b.Length];
		a.CopyTo (c, 0);
		b.CopyTo (c, a.Length);
		return c;
	}

	static Vector2[] combineVector2 (Vector2[] a, Vector2[] b) {
		Vector2[] c = new Vector2[a.Length + b.Length];
		a.CopyTo (c, 0);
		b.CopyTo (c, a.Length);
		return c;
	}

	static Vector3[] combineVector3 (Vector3[] a, Vector3[] b) {
		Vector3[] c = new Vector3[a.Length + b.Length];
		a.CopyTo (c, 0);
		b.CopyTo (c, a.Length);
		return c;
	}
	*/
}




