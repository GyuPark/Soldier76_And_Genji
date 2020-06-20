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
        /// geometry vector from a to b
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

        Vector3 WorldToLocal(Vector3 vector)
        {
            Vector3 newVector = Vector3.zero;
            newVector = player.InverseTransformVector(vector);
            return newVector.normalized;
        }

        Vector2 ConvertToVector2(Vector3 vector)
        {
            Vector2 v = Vector2.zero;
            float newX = vector.x;
            float newY = vector.y;
            v = new Vector2(newX, newY);
            return v;
        }

        float GetAngleFromVector2(Vector2 vector)
        {
            float angle = 0;
            angle = Mathf.Atan2(vector.y, vector.x) * 180f / Mathf.PI;
            return angle;
        }

        float ConvertAngleToZRot(float angle)
        {
            float zRot = 0f;
            zRot = angle - 90f;
            return zRot;
        }
    } 
}
