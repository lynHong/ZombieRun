using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string m_sceneName;
    [SerializeField] private int m_model;


    public void SwitchToNextScene()
    {
        // 切换场景前保存当前的model
        SaveModelData();
        SceneManager.LoadScene(m_sceneName);
    }

    public void QuitGame()
    {
        SaveModelData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SaveModelData()
    {
        PlayerPrefs.SetInt("Model", m_model);
        PlayerPrefs.Save();
    }
}
