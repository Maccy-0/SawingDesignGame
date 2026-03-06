using UnityEngine;
using UnityEngine.UI;

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
        screenTint = dmgScreen.color;
        screenTint.a -= Time.deltaTime * speed;
        dmgScreen.color = screenTint;
    }
}
