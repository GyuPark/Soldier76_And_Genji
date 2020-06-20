using UnityEngine;
using UnityEngine.UI;

/*
Skull은 Enemy를 죽였을 때 Enable 된다.
Skull의 LifeTime은 1.2초다. (1.2초가 경과하면 알아서 꺼진다)
*/
namespace Gyu
{
    public class SkullUI : MonoBehaviour
    {
        float timer;
        float lifeCycle = 0.5f;
        float newAlpha;
        Image image;

        public AnimationCurve scaleCurve;
        public AnimationCurve alphaCurve;

        void OnEnable()
        {
            image = GetComponent<Image>();
            timer = 0f;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > lifeCycle)
            {
                gameObject.SetActive(false);
            }

            newAlpha = alphaCurve.Evaluate(timer);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            transform.localScale = Vector3.one * scaleCurve.Evaluate(timer);
        }

        private void OnDisable()
        {
            timer = 0f;
        }
    } 
}
