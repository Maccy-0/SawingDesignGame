using Unity.VisualScripting;
using UnityEngine;

public class spawnerButton : MonoBehaviour
{
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
