using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gyu
{
    public class GrenadeCounterUI : MonoBehaviour
    {
        float timer = 6f;
        public Image parentImage;
        TextMeshProUGUI tmpro;
        public PlayerWeapon pw;


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
            timer = 6f;

            parentImage.color = new Color(18/255f, 243/255f, 250/255f, 1f);

            pw.grenadeState = PlayerWeapon.GrenadeState.Ready;
        }
    } 
}
