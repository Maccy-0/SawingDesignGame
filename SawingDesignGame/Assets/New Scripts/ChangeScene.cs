using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public int SceneNumber;
    public Button yourself;


    private void Start()
    {
        yourself.onClick.AddListener(changeScene);
    }

    private void changeScene()
    {
        SceneManager.LoadScene(SceneNumber);
    }
}
