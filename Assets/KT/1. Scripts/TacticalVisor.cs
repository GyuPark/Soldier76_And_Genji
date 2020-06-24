using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gyu
{
    public class ScreenTarget
    {
        //필요한 속성들을 이렇게 나열해 놓는다...
        public Vector2 screenPosition = Vector2.zero; //화면상의 좌표
        public float distanceFromCenter = 0.0f; //중심으로부터의 거리
        public GameObject targetObject = null; //게임오브젝트
    }
    
    public class TacticalVisor : MonoBehaviour
    {
        //list of all the targets
        public List<GameObject> mTargets = null;
        //list of all the reticlces
        public List<Transform> mReticles = null;
        public Camera firstPersonCamera;

        public Transform closestReticle = null;
        public ScreenTarget closestTarget = null;

        float mActiveRadius = .2f;

        #region Main
        void Awake()
        {

        }

        void Update()
        {
            //매 프레임마다 
            ScreenTarget[] screenTargets = TrackAllTargets(mTargets);

            screenTargets = GetValidTargets(screenTargets);

            for (int i = 0; i < mReticles.Count; i++)
            {
                if (i < screenTargets.Length)
                {
                    mReticles[i].position = screenTargets[i].screenPosition;
                    mReticles[i].gameObject.SetActive(true);
                }
                else
                {
                    mReticles[i].gameObject.SetActive(false);
                    mReticles[i].position = Vector3.zero;
                }
            }

            closestTarget = GetClosestTarget(screenTargets);

            Vector2 centerReticlePosition = closestTarget != null ? closestTarget.screenPosition : new Vector2(Screen.width / 2, Screen.height / 2);
            closestReticle.transform.position = centerReticlePosition;
        }
        #endregion

        #region Helper
        //target object들을 모두 ScreenTarget 형식으로 저장하는 함수
        ScreenTarget[] TrackAllTargets(List<GameObject> targets)
        {
            //ScreenTarget을 담을 배열을 생성
            ScreenTarget[] screenTargets = new ScreenTarget[targets.Count];

            for (int i = 0; i < targets.Count; i++)
            {
                //새로운 ScreenTarget을 생성
                screenTargets[i] = new ScreenTarget();
                //새로운 ScreenTarget의 targetObject를 target 오브젝트로 설정
                screenTargets[i].targetObject = targets[i];
                //새로운 ScreenTarget의 화면상 위치는 target의 world에서 screen공간으로 변환된 위치
                screenTargets[i].screenPosition = firstPersonCamera.WorldToScreenPoint(targets[i].transform.position);
            }

            return screenTargets;
        }

        /// <summary>
        /// 유효한 타겟 배열에 모으기
        /// </summary>
        /// <param name="screenTargets"></param>
        /// <returns></returns>
        ScreenTarget[] GetValidTargets(ScreenTarget[] screenTargets)
        {
            List<ScreenTarget> validTargets = new List<ScreenTarget>();
            float distance = 0f;

            foreach (ScreenTarget screenTarget in screenTargets)
            {
                //화면 중앙에서 ScreenTarget의 화면상 위치까지 거리를 구한다.
                distance = Vector2.Distance(screenTarget.screenPosition, new Vector2(firstPersonCamera.pixelWidth / 2, firstPersonCamera.pixelHeight / 2));
                //ScreenTarget의 distanceFromCenter에 할당해준다.
                screenTarget.distanceFromCenter = distance;

                //screen space 상 거리가 범위 안에 들어온다면, 
                if (distance < mActiveRadius * firstPersonCamera.pixelWidth)
                {
                    //유효 타겟 리스트에 넣는다.
                    validTargets.Add(screenTarget);
                }
            }

            //리스트를 배열에 변환하여 반환한다.
            return validTargets.ToArray();
        }

        /// <summary>
        /// 가장 근점한 타겟 찾기
        /// </summary>
        /// <param name="screenTargets"></param>
        /// <returns></returns>
        ScreenTarget GetClosestTarget(ScreenTarget[] screenTargets)
        {
            ScreenTarget closestTarget = null;

            float minDistance = float.MaxValue;

            foreach (ScreenTarget screenTarget in screenTargets)
            {
                if (screenTarget.distanceFromCenter < minDistance)
                {
                    //가장 짧은 거리를 갱신하고,
                    minDistance = screenTarget.distanceFromCenter;
                    //가장 가까운 ScreenTarget을 갱신한다.
                    closestTarget = screenTarget;
                }
            }

            //가장 근접한 타겟을 반환한다.
            return closestTarget;
        } 
        #endregion
    }

}
