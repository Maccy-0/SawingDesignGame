using UnityEngine;
using UnityEngine.UIElements;

public class Fade : MonoBehaviour
{

    public GameObject Screen;
    public Image dmgScreen;
    public float speed;
    public Color screenTint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgScreen = Screen.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        screenTint = dmgScreen.tintColor;
        screenTint.a -= Time.deltaTime * speed;
        dmgScreen.tintColor = screenTint;
    }
}
