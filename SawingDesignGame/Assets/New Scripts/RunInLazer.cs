using UnityEngine;
using UnityEngine.UIElements;

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
        fullAlpha = dmgScreen.tintColor;
        fullAlpha.a = 255;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name == lazer.transform.name)
        {
            dmgScreen.tintColor = fullAlpha;
        }
    }
}
