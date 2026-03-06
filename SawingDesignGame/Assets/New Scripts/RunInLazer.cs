using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class RunInLazer : MonoBehaviour
{
    public Image dmgScreen;
    public GameObject Screen;
    public GameObject lazer;
    private Color fullAlpha;

    private AudioSource soundSource;
    public AudioClip soundEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgScreen = Screen.GetComponent<Image>();
        fullAlpha = dmgScreen.color;
        fullAlpha.a = 0.9f;

        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == lazer.transform.name)
        {
            dmgScreen.color = fullAlpha;
            soundSource.PlayOneShot(soundEffect);
            delaybytime(0.5f);
        }
        other = null;
    }
    
    IEnumerator delaybytime(float maxDelay)
    {
        float delay = 0;
        while (delay < maxDelay)
        {
            delay += Time.deltaTime;
            yield return null;
        }
    }
}
