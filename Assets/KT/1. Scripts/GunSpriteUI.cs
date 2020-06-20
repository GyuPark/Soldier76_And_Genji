using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gyu
{
    public class GunSpriteUI : MonoBehaviour
    {
        Image image;
        Color defaultColor;
        PlayerState ps;
        public PlayerWeapon pw;

        void Start()
        {
            ps = GetComponentInParent<PlayerState>();
            image = GetComponent<Image>();
            defaultColor = image.color; //255,255,255,83
        }

        void Update()
        {
            //정상적으로 사격할 때
            if (ps.clientState == PlayerState.ClientState.Rifle && pw.currentBullets != 0)
            {
                image.color = new Color(0f, 80/255f, 224/225f, .65f);
            }
            else if (pw.currentBullets == 0) //currentBullets총알이 없을 때
            {
                //StartCoroutine("ReloadWarning");
            }
            else
            {
                image.color = defaultColor;
            }
        }

        //IEnumerator ReloadWarning()
        //{
        //    image.color = new Color(246 / 255f, 48 / 255f, 48 / 255f, .65f);
        //    yield return new WaitForSeconds(1f);
        //    image.color = defaultColor;
        //    yield return new WaitForSeconds(1f);
        //}
    } 
}
