using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePendulum : MonoBehaviour {

	public float length;
	public float speed;
	public float angularRange;

	private Vector2 initialPos;
	private Vector2 anchorPos;
	private float elapsedTime;

	void Start () {
		initialPos = transform.position;
		anchorPos = initialPos + Vector2.up * length;
		elapsedTime = 0;
	}

	void Update () {
		elapsedTime += Time.deltaTime;

		float angle = Mathf.Sin(elapsedTime * speed) * angularRange * 0.5f;
		float movement = angle * Mathf.Deg2Rad;

		float x = anchorPos.x + length * Mathf.Sin(movement);
		float y = anchorPos.y - length * Mathf.Cos(movement);
		transform.position = new Vector2(x, y);

		Vector3 euler = transform.rotation.eulerAngles;
		euler.z = angle;
		transform.rotation = Quaternion.Euler(euler);
	}
}
