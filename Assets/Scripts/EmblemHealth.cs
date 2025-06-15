using UnityEngine;
using UnityEngine.UI;

namespace YUCHEN
{
    public class EmblemHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        private int currentHealth;

        public Image healthImage;

        void Start()
        {
            currentHealth = maxHealth;
            UpdateHealthUI();
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;

            UpdateHealthUI();

            if (currentHealth == 0)
            {
                Debug.Log("Game Over：校徽已被摧毀！");
                // TODO: 顯示 Game Over 畫面
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
