using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    //public Image[] frames; 
    public GameObject[] frames;
    int framesPerSecond = 3;
    int currentFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        frames[1].SetActive(false);
        frames[2].SetActive(false);
        InvokeRepeating("CycleFrame", 0f, 1/framesPerSecond);
    }

    // Update is called once per frame

    void Update() { 
        //currentFrame = (Time.time * framesPerSecond) % frames.Length; 
        //imageRef.image = frames[currentFrame]; 
    }

    void CycleFrame() {
        if (currentFrame < 2) {
            currentFrame++;
        }
        else {
            currentFrame = 0;
        }

        switch(currentFrame) {
            case 0:
                frames[0].SetActive(true);
                frames[1].SetActive(false);
                frames[2].SetActive(false);
                break;
            case 1:
                frames[0].SetActive(false);
                frames[1].SetActive(true);
                frames[2].SetActive(false);
                break;
            case 2:
                frames[0].SetActive(false);
                frames[1].SetActive(false);
                frames[2].SetActive(true);
                break;
        }
    }
}
