using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    public class PlayerRot : MonoBehaviourPun
    {

        #region Field
        public Transform player;
        public float mouseSensitivity; //350
        float _newX, _newY;

        PhotonView _photonView;


        #endregion

        #region Main 
        private void Start()
        {
            mouseSensitivity = 350f;
            _photonView = GetComponentInParent<PhotonView>();

        }

        private void Update()
        {
            if (!_photonView.IsMine)
                return;

            //newX newY 계산
            Rotate(Vector2MouseXY().x, Vector2MouseXY().y, mouseSensitivity, ref _newX, ref _newY);

            //newY 범위 적용
            ClampCameraY(ref _newY, 60.0f);

            //player를 y축 회전한다
            player.transform.eulerAngles = new Vector3(0, _newX, 0);

            //camera x축 회전한다
            transform.localEulerAngles = new Vector3(-_newY, 0, 0);
        }
        #endregion

        #region Helper
        /// <summary>
        /// 마우스 XY input
        /// </summary>
        /// <returns></returns>
        Vector2 Vector2MouseXY()
        {
            Vector2 v = Vector2.zero;
            v.x = Input.GetAxis("Mouse X");
            v.y = Input.GetAxis("Mouse Y");
            return v;
        }

        void ClampCameraY(ref float y, float clampAngle)
        {
            y = Mathf.Clamp(y, -clampAngle, clampAngle);
        }

        void Rotate(float mouseX, float mouseY, float sensitivity, ref float newX, ref float newY)
        {
            newX += mouseX * sensitivity * Time.deltaTime;
            newY += mouseY * sensitivity * Time.deltaTime;
        }
        #endregion
    } 
}
