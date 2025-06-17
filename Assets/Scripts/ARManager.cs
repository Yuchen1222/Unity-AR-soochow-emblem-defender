using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

namespace YUCHEN
{
    /// <summary>
    /// 擴增實境管理器
    /// </summary>
    public class ARManager : MonoBehaviour
    {
        [SerializeField, Header("塔防物件")]
        private GameObject goTD;

        private bool isPlaced;
        private ARRaycastManager arRay;

        private void Awake()
        {
            arRay = GetComponent<ARRaycastManager>();
        }

        private void Update()
        {
            // 如果點擊過就跳出
            if (isPlaced) return;

            // 如果按下左鍵（等於手機觸控）
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // 獲得滑鼠座標（觸控座標）
                Vector3 mousePosition = Input.mousePosition;
                //Debug.Log($"<color=#ff3>點擊座標：{mousePosition}</color>");

                //AR 射線擊中物件清單
                List<ARRaycastHit> hits = new List<ARRaycastHit>();

                // 如果 AR 射線 打到東西 就
                if (arRay.Raycast(mousePosition, hits, TrackableType.Planes))
                {
                    // 生成塔防遊戲物件
                    Instantiate(goTD, hits[0].pose.position, Quaternion.identity);
                    // 已經點擊過
                    isPlaced = true;
                }
            }
        }
    }
}
