using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIScript : MonoBehaviour
{
    public void StartNew()
    {
        // TODO: check if player has name
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #endif

        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadScene(2);
    }
}
