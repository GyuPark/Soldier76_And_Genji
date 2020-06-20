using UnityEngine;
using Photon.Pun;

/// <summary>
/// 마우스 커서 없애기
/// </summary>
namespace Gyu
{
    public class HideCursor : MonoBehaviourPun
    {
        void Start()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            /*else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }*/
        }
    } 
}
