using UnityEngine;
using UnityEngine.SceneManagement;

public class GoScene : MonoBehaviour
{
    public string scenename;

    public void GoSceneTo()
    {
        SceneManager.LoadScene(scenename);
    }
}
