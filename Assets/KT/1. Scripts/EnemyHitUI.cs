using UnityEngine;
using UnityEngine.UI;

/*
Enemy가 나한테 한 발 맞을 때마다 animation이 시작된다.
- object가 disable 상태라면 enable 해주고,
- 이미 enable 상태라면 timer 를 0으로 다시 세팅한다.

맞는 시간이 길어질 수록 hitcrosshair가 커진다.??
*/
namespace Gyu
{
    public class EnemyHitUI : MonoBehaviour
    {
        [HideInInspector] public float timer;
        float lifeCycle = 0.5f;
        float newAlpha;
        Image image;

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
                Destroy(gameObject);
            }

            newAlpha = alphaCurve.Evaluate(timer);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
        }
    } 
}
