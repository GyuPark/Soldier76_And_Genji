using UnityEngine;
using Photon.Pun;


namespace Gyu
{
    //1. 달릴 때 zoom in 그렇지 않을 때 다시 zoom out
    //2. 공격 당할 때 연출
    public class CameraFieldOfView : MonoBehaviourPun
    {
        PlayerState ps;
        Camera cam;
        public float zoomSpeed;

        PhotonView _photonView;


        private void Start()
        {
            _photonView = GetComponentInParent<PhotonView>();


            if (!_photonView.IsMine)
                return;

            ps = GetComponentInParent<PlayerState>();
            cam = GetComponent<Camera>();
            cam.fieldOfView = 60f;
        }

        private void Update()
        {
            if (!_photonView.IsMine)
                return;

            if (ps.clientState == PlayerState.ClientState.Dash && Input.GetAxis("Vertical") > 0)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 0, Time.deltaTime* zoomSpeed);
            }
            else
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90, Time.deltaTime* zoomSpeed);
            }
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 50, 60);
        }
    } 
}
