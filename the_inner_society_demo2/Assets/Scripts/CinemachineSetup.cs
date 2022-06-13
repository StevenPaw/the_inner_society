using System;
using Cinemachine;
using farmingsim.Utils;
using UnityEngine;

namespace farmingsim
{
    public class CinemachineSetup : MonoBehaviour
    {
        private CinemachineVirtualCamera cameraSystem;
        private Transform playerTransform;

        private void Start()
        {
            cameraSystem = GetComponent<CinemachineVirtualCamera>();
            playerTransform = GameObject.FindWithTag(GameTags.PLAYER).transform;

            cameraSystem.Follow = playerTransform;
        }
    }
}