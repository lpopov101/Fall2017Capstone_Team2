using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandIndicatorBehavior : MonoBehaviour {

	public float lifetime = 1f;

	private SpriteRenderer spriteRenderer;
	private bool fading;
	private float startTime;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		fading = false;
		startTime = 0f;
	}

	public void StartFading() {
		fading = true;
		startTime = Time.time;
	}

	void Update () {
		if(!fading)
			return;

		float alpha = 1f - (Time.time - startTime) / lifetime;
		spriteRenderer.color = new Color(1f, 1f, 1f, alpha);

		if(Time.time > startTime + lifetime) {
			Destroy(gameObject);
		}
	}
}
