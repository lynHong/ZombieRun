using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToSettlementScene()
    {
        // 切换到场景2 (Settlement)
        SceneManager.LoadScene("Settlement");
    }
}
