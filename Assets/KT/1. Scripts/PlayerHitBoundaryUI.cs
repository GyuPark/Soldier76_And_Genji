using UnityEngine;
using UnityEngine.UI;

namespace Gyu
{
    public class PlayerHitBoundaryUI : MonoBehaviour
    {
        float timer;
        float lifeCycle = 4f;

        float newAlpha;
        Image image;

        public AnimationCurve alphaCurve;

        // Start is called before the first frame update
        void Start()
        {
            timer = 0f;
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
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
