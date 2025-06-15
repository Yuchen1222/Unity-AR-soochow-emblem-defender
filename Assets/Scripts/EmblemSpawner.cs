using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace YUCHEN
{
    public class EmblemSpawner : MonoBehaviour
    {
        public GameObject emblemPrefab;
        private GameObject placedEmblem;
        private ARRaycastManager raycastManager;
        private List<ARRaycastHit> hits = new();

        void Awake()
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }

        void Update()
        {
            if (placedEmblem != null) return;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 touchPos = Input.GetTouch(0).position;

                if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    placedEmblem = Instantiate(emblemPrefab, hitPose.position, Quaternion.identity);
                    placedEmblem.tag = "Emblem"; // 確保生成物有正確 Tag
                }
            }
        }
    }
}
