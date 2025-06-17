using UnityEngine;
using UnityEngine.SceneManagement;

namespace YUCHEN
{
    public class RestartGame : MonoBehaviour
    {
        // 此方法會在 Retry 按鈕被點擊時呼叫
        public void RestartScene()
        {
            // 取得目前的場景名稱，並重新載入
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
