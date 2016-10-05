using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {
    public Vector3 destination;
    public float rotation;

    Vector3 finalRotation;
    // Use this for initialization
    void Start () {
        finalRotation = new Vector3(transform.rotation.x, transform.rotation.y + rotation, transform.rotation.z);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButton("Fire1"))
        {
            transform.position = destination;
        }

        if (Input.GetButton("Fire2"))
        {
            transform.eulerAngles = finalRotation;
        }

        if (Input.GetButton("Fire3"))
        {
            transform.position = destination;
            transform.eulerAngles = finalRotation;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
