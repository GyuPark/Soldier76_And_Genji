using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    public class PlayerCameraMode : MonoBehaviourPun
    {
        public enum CameraMode
        {
            First = 0,
            Third = 1
        }
        public CameraMode cameraMode;

        public Camera firstPersonCamera;
        public Camera thirdPersonCamera;

        public GameObject soldier76UI;

        public GameObject[] hiddenBodyParts;

        


        private void Start()
        {
            if (photonView.IsMine) //내꺼라면
            {
                cameraMode = CameraMode.First;
                //head, backpack, body 꺼준다.
                foreach (var item in hiddenBodyParts)
                {
                    item.SetActive(false);
                }
            }
            
            else //내 것이 아니라면
            {
                //모든 카메라를 꺼준다.
                firstPersonCamera.enabled = false;
                thirdPersonCamera.gameObject.SetActive(false);
                //UI도 꺼준다.
                soldier76UI.SetActive(false);
            }
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            switch (cameraMode)
            {
                case CameraMode.First:
                    firstPersonCamera.enabled = true;
                    thirdPersonCamera.enabled = false;
                    foreach (var item in hiddenBodyParts)
                    {
                        item.SetActive(false);
                    }
                    break;
                case CameraMode.Third:
                    firstPersonCamera.enabled = false;
                    thirdPersonCamera.enabled = true;
                    foreach (var item in hiddenBodyParts)
                    {
                        item.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

            if (Input.GetMouseButtonDown(2))
            {
                if (cameraMode == CameraMode.Third)
                {
                    cameraMode = 0;
                }
                else
                {
                    cameraMode += 1;
                }
            }
        }
    } 
}
