using UnityEngine;
using UnityEngine.UI;

public class RunInLazer : MonoBehaviour
{
    public Image dmgScreen;
    public GameObject Screen;
    public GameObject lazer;
    private Color fullAlpha;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgScreen = Screen.GetComponent<Image>();
        fullAlpha = dmgScreen.color;
        fullAlpha.a = 100;
        
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
        }
        other = null;
    }
}
