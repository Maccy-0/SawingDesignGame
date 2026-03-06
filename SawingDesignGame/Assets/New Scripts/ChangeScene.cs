using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public int SceneNumber;
    public Button yourself;
    private AudioSource soundSource;
    public AudioClip soundEffect;

    private void Start()
    {
        soundSource = GetComponent<AudioSource>();
        yourself.onClick.AddListener(changeScene);
    }

    private void changeScene()
    {
        soundSource.PlayOneShot(soundEffect);
        StartCoroutine(delayedSceneChange(0.35f));
        

    }

    IEnumerator delayedSceneChange(float maxDelay)
    {
        float delay = 0;
        while (delay < maxDelay)
        {
            delay += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneNumber);
    }
}
