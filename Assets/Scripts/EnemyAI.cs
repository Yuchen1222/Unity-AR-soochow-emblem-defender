using UnityEngine;

namespace YUCHEN
{
    public class EnemyAI : MonoBehaviour
    {
        public float speed = 1.5f;
        private Transform targetEmblem;
        private Animator animator;

        void Start()
        {
            GameObject emblemObj = GameObject.FindWithTag("Emblem");
            if (emblemObj != null)
            {
                targetEmblem = emblemObj.transform;
            }

            animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("移動", true); // 播放動畫
            }
        }

        void Update()
        {
            if (targetEmblem != null)
            {
                Vector3 dir = (targetEmblem.position - transform.position).normalized;
                dir.y = 0; // 防止飛起來或鑽地板
                transform.position += dir * speed * Time.deltaTime;
                transform.LookAt(targetEmblem);
            }
        }

        // ✅ 改為使用 Trigger 感應扣血
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Emblem"))
            {
                var health = other.GetComponent<EmblemHealth>();
                if (health != null)
                {
                    health.TakeDamage(10);
                }

                Destroy(gameObject); // 扣血後自爆
            }
        }
    }
}
