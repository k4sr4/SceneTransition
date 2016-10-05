using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class SceneTransition : MonoBehaviour {
    public Vector3 destination;
    public float rotation;
    public bool fadeToBlack = false;
    public bool fade = false;
    public float blackTime = 4;
    public float fadeTime = 2;    

    public GameObject centerEye;

    Vector3 finalRotation;

    public enum State { Teleport, Animated, Pulsed};
    public State state = State.Teleport;

    // Use this for initialization
    void Start () {
        finalRotation = new Vector3(transform.rotation.x, transform.rotation.y + rotation, transform.rotation.z);
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Teleport)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (fadeToBlack)
                {
                    centerEye.GetComponent<OVRScreenFade>().Teleport(blackTime);
                    transform.position = destination;
                }
                else if (fade)
                {
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(1));
                }
                else
                {
                    transform.position = destination;
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                if (fadeToBlack)
                {
                    centerEye.GetComponent<OVRScreenFade>().Teleport(blackTime);
                    transform.eulerAngles = finalRotation;
                }
                else if (fade)
                {
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(2));
                }
                else
                {
                    transform.eulerAngles = finalRotation;
                }
            }

            if (Input.GetButtonDown("Fire3"))
            {
                if (fadeToBlack)
                {
                    centerEye.GetComponent<OVRScreenFade>().Teleport(blackTime);
                    transform.position = destination;
                    transform.eulerAngles = finalRotation;
                }
                else if (fade)
                {
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(3));
                }
                else
                {
                    transform.position = destination;
                    transform.eulerAngles = finalRotation;
                }
            }
        }
        else if (state == State.Animated)
        {

        }
        else if (state == State.Pulsed)
        {

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    IEnumerator FadeImage(int state)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            yield return new WaitForSeconds (fadeTime / 100);
            elapsedTime ++;
            centerEye.GetComponent<Blur>().iterations = (int)(2 * elapsedTime);
        }

        if (state == 1)
        {
            transform.position = destination;
        }
        else if (state == 2)
        {
            transform.eulerAngles = finalRotation;
        }
        else if (state == 3)
        {
            transform.position = destination;
            transform.eulerAngles = finalRotation;
        }

        while (elapsedTime > 0)
        {
            yield return new WaitForSeconds(fadeTime / 100);
            elapsedTime--;
            centerEye.GetComponent<Blur>().iterations = (int)(2 * elapsedTime);
        }

        centerEye.GetComponent<Blur>().enabled = false;
    }
}
