using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Utility;
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
    Vector3 initialPos;
    Vector3 finalPos;

    float xDiv;
    float zDiv;
    float rotateDiv;
    Vector3 newPos;
    Vector3 newRot;
    public float pulsedWait = 1;
    public int pulseNum = 5;

    public enum State { Teleport, Animated, Pulsed};
    public State current = State.Teleport;

    public enum Environment { SmallShort, BigShort, BigLarge };
    public Environment env = Environment.SmallShort;

    // Use this for initialization
    void Start () {
        initialPos = transform.position;
        finalPos = destination;

        finalRotation = new Vector3(transform.rotation.x, transform.rotation.y + rotation, transform.rotation.z);

        float xDifference = destination.x - transform.position.x;
        float zDifference = destination.z - transform.position.z;
        float rotateDifference = rotation - transform.eulerAngles.y;

        xDiv = xDifference / pulseNum;
        zDiv = zDifference / pulseNum;
        rotateDiv = rotateDifference / pulseNum;

        newPos = transform.position;
        newRot = transform.eulerAngles;        
	}
	
	// Update is called once per frame
	void Update () {
        if (env == Environment.BigLarge)
        {
            if (transform.position != initialPos)
            {
                destination = initialPos;
            }
            else
            {
                destination = new Vector3(375f, 1.01f, 200f);
            }
        }

        else
        {
            if (transform.position != initialPos)
            {
                destination = initialPos;
            }
            else
            {
                destination = finalPos;
            }
        }

        if (current == State.Teleport)
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
                    StartCoroutine(FadeImage(1, destination, finalRotation));
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
                    StartCoroutine(FadeImage(2, destination, finalRotation));
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
                    StartCoroutine(FadeImage(3, destination, finalRotation));
                }
                else
                {
                    transform.position = destination;
                    transform.eulerAngles = finalRotation;
                }
            }
        }
        else if (current == State.Animated)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GetComponent<AutoMoveAndRotate>().move = true;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                GetComponent<AutoMoveAndRotate>().rotate = true;
            }

            if (Input.GetButtonDown("Fire3"))
            {
                GetComponent<AutoMoveAndRotate>().move = true;
                GetComponent<AutoMoveAndRotate>().rotate = true;
            }
        }
        else if (current == State.Pulsed)
        {            
            if (Input.GetButtonDown("Fire1"))
            {                                
                newPos = new Vector3(newPos.x + xDiv, newPos.y, newPos.z + zDiv);
                centerEye.GetComponent<Blur>().enabled = true;
                StartCoroutine(FadeImage(1, newPos, finalRotation));                
            }

            if (Input.GetButtonDown("Fire2"))
            {
                newRot = new Vector3(newRot.x, newRot.y + rotateDiv, newRot.z);
                centerEye.GetComponent<Blur>().enabled = true;
                StartCoroutine(FadeImage(2, destination, newRot));
            }

            if (Input.GetButtonDown("Fire3"))
            {
                newPos = new Vector3(newPos.x + xDiv, newPos.y, newPos.z + zDiv);
                newRot = new Vector3(newRot.x, newRot.y + rotateDiv, newRot.z);
                centerEye.GetComponent<Blur>().enabled = true;
                StartCoroutine(FadeImage(3, newPos, newRot)); 
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    IEnumerator FadeImage(int state, Vector3 dest, Vector3 rot)
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
            transform.position = dest;
        }
        else if (state == 2)
        {
            transform.eulerAngles = rot;
        }
        else if (state == 3)
        {
            transform.position = dest;
            transform.eulerAngles = rot;
        }
        
        while (elapsedTime > 0)
        {
            yield return new WaitForSeconds(fadeTime / 100);
            elapsedTime--;
            centerEye.GetComponent<Blur>().iterations = (int)(2 * elapsedTime);
        }
        
        centerEye.GetComponent<Blur>().enabled = false;
        
        if (current == State.Pulsed)
        {            
            if (state == 1)
            {                
                if (transform.position.x < destination.x)
                {                    
                    yield return new WaitForSeconds(pulsedWait);
                    
                    newPos = new Vector3(newPos.x + xDiv, newPos.y, newPos.z + zDiv);
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(1, newPos, newRot));
                }
            }
            else if (state == 2)
            {
                if (transform.eulerAngles.y < rotation)
                {
                    yield return new WaitForSeconds(pulsedWait);

                    Debug.Log("2");
                    newRot = new Vector3(newRot.x, newRot.y + rotateDiv, newRot.z);
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(2, destination, newRot));
                }
            }
            else if (state == 3)
            {
                if ((transform.eulerAngles.y < rotation) && (transform.position.x < destination.x))
                {
                    yield return new WaitForSeconds(pulsedWait);

                    newPos = new Vector3(newPos.x + xDiv, newPos.y, newPos.z + zDiv);
                    newRot = new Vector3(newRot.x, newRot.y + rotateDiv, newRot.z);
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(3, newPos, newRot));
                }
                else if (transform.position.x < destination.x)
                {
                    yield return new WaitForSeconds(pulsedWait);

                    newPos = new Vector3(newPos.x + xDiv, newPos.y, newPos.z + zDiv);
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(1, newPos, newRot));
                }
                else if (transform.eulerAngles.y < rotation)
                {
                    yield return new WaitForSeconds(pulsedWait);

                    newRot = new Vector3(newRot.x, newRot.y + rotateDiv, newRot.z);
                    centerEye.GetComponent<Blur>().enabled = true;
                    StartCoroutine(FadeImage(2, newPos, newRot));
                }
            }            
        }
    }
}
