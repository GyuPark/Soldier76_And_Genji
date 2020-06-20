using UnityEngine;
using Photon.Pun;

//목표 : weapon sway
namespace Gyu
{
    public class WeaponSway : MonoBehaviourPun
    {
        Vector3 initialPosition;
        public float amount; //.1
        public float maxAmount; //.1
        public float smoothAmount; //6

        PhotonView _photonView;


        void Start()
        {
            _photonView = GetComponentInParent<PhotonView>();

            if (!_photonView.IsMine)
                return;

            initialPosition = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_photonView.IsMine)
                return;

            float movementX = -Input.GetAxis("Mouse X") * amount;
            float movementY = -Input.GetAxis("Mouse Y") * amount;

            movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
            movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

            Vector3 finalPosition = new Vector3(movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + finalPosition, Time.deltaTime * smoothAmount);
        }

        public void SmoothAmount(float smoothAmount)
        {
            this.smoothAmount = smoothAmount;
        }
    } 
}
