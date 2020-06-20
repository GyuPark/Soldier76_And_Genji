using UnityEngine;

namespace Gyu
{
    public class ClientAnimation : MonoBehaviour
    {

        #region Field
        Animator anim;
        #endregion

        #region Main
        private void Start()
        {
            Init();
        }
        #endregion

        #region Helper
        /// <summary>
        /// 변수 초기화
        /// </summary>
        void Init()
        {
            anim = GetComponent<Animator>();
        }
        #endregion
    } 
}
