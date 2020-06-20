using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gyu
{
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

        void Awake()
        {

        }

        void Update()
        {
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

        ScreenTarget[] TrackAllTargets(List<GameObject> targets)
        {
            ScreenTarget[] screenTargets = new ScreenTarget[targets.Count];

            for (int i = 0; i < targets.Count; i++)
            {
                screenTargets[i] = new ScreenTarget();
                screenTargets[i].targetObject = targets[i];
                screenTargets[i].screenPosition = firstPersonCamera.WorldToScreenPoint(targets[i].transform.position);
            }

            return screenTargets;
        }

        ScreenTarget[] GetValidTargets(ScreenTarget[] screenTargets)
        {
            List<ScreenTarget> validTargets = new List<ScreenTarget>();
            float distance = 0f;

            foreach  (ScreenTarget screenTarget in screenTargets)
            {
                distance = Vector2.Distance(screenTarget.screenPosition, new Vector2(firstPersonCamera.pixelWidth/2, firstPersonCamera.pixelHeight/2));
                screenTarget.distanceFromCenter = distance;

                if (distance < mActiveRadius * firstPersonCamera.pixelWidth)
                {
                    validTargets.Add(screenTarget);
                }
            }

            return validTargets.ToArray();
        }

        ScreenTarget GetClosestTarget(ScreenTarget[] screenTargets)
        {
            ScreenTarget closestTarget = null;
            float minDistance = float.MaxValue;

            foreach (ScreenTarget screenTarget in screenTargets)
            {
                if (screenTarget.distanceFromCenter < minDistance)
                {
                    minDistance = screenTarget.distanceFromCenter;
                    closestTarget = screenTarget;
                }
            }

            return closestTarget;
        }
    }

    public class ScreenTarget
    {
        public Vector2 screenPosition = Vector2.zero;
        public float distanceFromCenter = 0.0f;
        public GameObject targetObject = null;
    } 
}
