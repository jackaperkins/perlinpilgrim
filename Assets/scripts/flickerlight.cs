using UnityEngine;
using System.Collections;

public class flickerlight : MonoBehaviour {

	private float lookup;
	private Light l;
	private float bright;
	// Use this for initialization
	void Start () {
		l = GetComponent<Light>();
		bright = l.intensity;

	}
	
	// Update is called once per frame
	void Update () {
		lookup += Time.deltaTime * 2;

		l.intensity = bright * (0.7f + 0.5f * Mathf.PerlinNoise (lookup , 3.0f));
	}
}
