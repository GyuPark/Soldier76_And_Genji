using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gyu
{
    public class HealCounterUI : MonoBehaviour
    {
        float timer = 15f;
        public Image parentImage;
        TextMeshProUGUI tmpro;
        public PlayerHeal ph;

        private void OnEnable()
        {
            tmpro = GetComponent<TextMeshProUGUI>();
            parentImage.color = new Color(parentImage.color.r, parentImage.color.g, parentImage.color.b, 0.2f);
        }

        void Update()
        {
            timer -= Time.deltaTime;
            tmpro.text = ((int)timer).ToString();
            if (timer < 1)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            timer = 15f;
            parentImage.color = new Color(228/255f, 231/255f, 26/255f, 1f);
            ph.healState = PlayerHeal.HealState.Ready;
        }
    } 
}
