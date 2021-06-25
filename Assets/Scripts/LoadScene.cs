using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public LoadScene instance { get; set; }


    public const string key = "PlayerManager";

    public static string keyProperty
    {
        get
        {
            return key;
        }
        set
        {
            keyProperty = value;
        }
    }


    private void Start()
    {

    }
    public void LoadNextScene(Button btn)
    {
        Debug.Log("Hello");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetString(key, btn.transform.name);
    

    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

}
