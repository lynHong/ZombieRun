using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Awake()
    {
        // 确保在场景切换时不销毁 TimerManager
        if (FindObjectsOfType<TimerManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 从 PlayerPrefs 中读取先前的时间（如果有）
        elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);
        isRunning = true;

        // 订阅场景切换事件
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("Elapsed time: " + elapsedTime);
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        // 在场景卸载时保存时间到 PlayerPrefs
        SaveElapsedTime();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 场景加载时重新读取时间
        elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        SaveElapsedTime();
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    private void SaveElapsedTime()
    {
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // 取消订阅场景切换事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
