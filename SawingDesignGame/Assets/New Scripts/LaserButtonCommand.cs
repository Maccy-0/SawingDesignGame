using TMPro;
using UnityEngine;

public class LaserButtonCommand : MonoBehaviour
{
    public Vector3 newDirection;
    public bool buttonOn;
    public bool buttonActive;
    private AudioSource soundSource;
    public AudioClip soundEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonActive && !buttonOn && Input.GetKeyDown(KeyCode.E))
        {
            soundSource.PlayOneShot(soundEffect);
            buttonOn = true;   
            Debug.Log(soundEffect.name);
            this.GetComponentInParent<MoveObjectScript>().activeButton = this.transform.name;
        } else if (buttonActive && buttonOn && Input.GetKeyDown(KeyCode.E))
        {
            this.GetComponentInParent<MoveObjectScript>().activeButton = "None";
            buttonOn = false;
        }
        if (buttonOn)
        {
            this.GetComponentInParent<MoveObjectScript>().laserDirection = newDirection;
        }

        if (this.GetComponentInParent<MoveObjectScript>().activeButton == this.transform.name)
        {
            buttonOn = true;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
        } else
        {
            buttonOn = false;

            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        buttonActive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        buttonActive = false;
    }
}
