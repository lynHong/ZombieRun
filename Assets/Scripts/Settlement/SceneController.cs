using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string m_sceneName; // 设置目标场景名称
    [SerializeField] private int m_model;


    public void SwitchToNextScene()
    {
        // 切换场景前保存当前的model
        SaveModelData();
        SceneManager.LoadScene(m_sceneName);
    }

    public void QuitGame()
    {
        SaveModelData(); // 可以选择在退出游戏时保存数据
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SaveModelData()
    {
        PlayerPrefs.SetInt("Model", m_model); // 直接保存model值
        PlayerPrefs.Save(); // 确保数据立即写入
    }
}
