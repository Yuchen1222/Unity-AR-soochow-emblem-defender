using UnityEngine;
using TMPro;

namespace YUCHEN
{
    public class TapToDestroy : MonoBehaviour
    {
        public GameObject explosionEffectPrefab;
        public TextMeshProUGUI scoreText; // 用 TMP 顯示分數
        private int score = 0;

        void Start()
        {
            UpdateScoreUI();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        if (explosionEffectPrefab != null)
                        {
                            GameObject fx = Instantiate(explosionEffectPrefab, hit.transform.position, Quaternion.identity);
                            Destroy(fx, 2f);
                        }

                        Destroy(hit.collider.gameObject);
                        score += 1;
                        UpdateScoreUI();
                    }
                }
            }
        }

        void UpdateScoreUI()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }
    }
}
