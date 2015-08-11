using ExternalLibrary;
using UnityEngine;

public class TestBehaviourScript : MonoBehaviour
{

	private void Start()
	{
		int result = ExternalClass.Add(40, 2);
		Debug.Log("Result: " + result);
	}
}
