using UnityEngine;
using UnityEngine.UI;

namespace Gyu
{
    public class DamageIndicatorUI : MonoBehaviour
    {
        public Transform player;
        public Vector3 enemy;
        float timer;
        float lifeCycle = 4f;

        float newAlpha;
        Image image;

        public AnimationCurve alphaCurve;

        private void OnEnable()
        {
            timer = 0f;
            image = GetComponent<Image>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > lifeCycle)
            {
                Destroy(gameObject);
            }

            newAlpha = alphaCurve.Evaluate(timer);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);

            Vector3 v = GetDirVector3(player.position, enemy);
            Vector3 v2 = WorldToLocal(v);
            Vector2 v3 = ConvertToVector2(v2);
            float angle = GetAngleFromVector2(v3);
            float zRot = ConvertAngleToZRot(angle);

            transform.localEulerAngles = new Vector3(0f, 0f, zRot);
        }

        /// <summary>
        /// 방향 벡터를 구하는 함수
        /// </summary>
        /// <param name="a">origin</param>
        /// <param name="b">destination</param>
        /// <returns></returns>
        Vector3 GetDirVector3(Vector3 a, Vector3 b)
        {
            Vector3 vector = Vector3.zero;
            vector = b - a;
            return vector;
        }

        /// <summary>
        /// 월드공간 벡터를 로컬공간 벡터로 치환하는 함수
        /// </summary>
        /// <param name="vector">기하 벡터</param>
        /// <returns></returns>
        Vector3 WorldToLocal(Vector3 vector)
        {
            Vector3 newVector = Vector3.zero;
            newVector = player.InverseTransformVector(vector);
            return newVector.normalized;
        }

        /// <summary>
        /// 3차원 벡터를 화면에 표시하기 위해 2차원 벡터로 치환하는 함수
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        Vector2 ConvertToVector2(Vector3 vector)
        {
            Vector2 v = Vector2.zero;
            float newX = vector.x;
            float newY = vector.y;
            v = new Vector2(newX, newY);
            return v;
        }

        /// <summary>
        /// 2차원 백터를 degrees 각도로 치환하는 함수
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        float GetAngleFromVector2(Vector2 vector)
        {
            float angle = 0;
            //radian → degree로 치환
            angle = Mathf.Atan2(vector.y, vector.x) * 180f / Mathf.PI;
            return angle;
        }

        /// <summary>
        /// degree 각도를 Z축 회전값으로 치환
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        float ConvertAngleToZRot(float angle)
        {
            float zRot = 0f;
            zRot = angle - 90f;
            return zRot;
        }
    } 
}
