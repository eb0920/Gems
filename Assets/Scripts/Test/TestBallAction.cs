using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBallAction : MonoBehaviour
{
	void Start()
	{

	}
	
	void Update()
	{
		transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
	}
}
