using UnityEngine;
using UnityEngine.UI;

namespace YUCHEN
{
    public class EmblemHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        private int currentHealth;

        public Image healthImage;               // 血條
        public GameObject explosionEffect;      // 爆炸特效
        public GameObject gameOverUI;           // Game Over 畫面
        public AudioClip destroySound;          // 播放音效
        private AudioSource audioSource;        // 播放來源

        void Start()
        {
            currentHealth = maxHealth;
            audioSource = gameObject.AddComponent<AudioSource>(); // 自動加 AudioSource
            UpdateHealthUI();

            if (healthImage == null)
            {
                GameObject found = GameObject.Find("圖片_血條");
                if (found != null) healthImage = found.GetComponent<Image>();
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;

            UpdateHealthUI();

            if (currentHealth == 0)
            {
                // 🔊 播放音效
                if (destroySound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(destroySound);
                }

                // 💥 播放爆炸特效
                if (explosionEffect != null)
                {
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                }

                // 📺 顯示 Game Over 畫面
                if (gameOverUI != null)
                {
                    gameOverUI.SetActive(true);
                }

                Debug.Log("Game Over：校徽已被摧毀！");
                Destroy(gameObject); // 校徽物件本身刪除（或你可以延遲一點點時間再 Destroy）
            }
        }

        void UpdateHealthUI()
        {
            if (healthImage != null)
            {
                float fill = (float)currentHealth / maxHealth;
                healthImage.fillAmount = fill;
            }
        }
    }
}
