using UnityEngine;
using UnityEngine.UI;

namespace Gyu
{
    public class DashUI : MonoBehaviour
    {
        Image image;
        Color defaultColor;
        PlayerState ps;

        private void Start()
        {
            ps = GetComponentInParent<PlayerState>();
            image = GetComponent<Image>();
            defaultColor = image.color;
        }

        private void Update()
        {
            if (ps.clientState == PlayerState.ClientState.Dash ||
                ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetKey(KeyCode.LeftShift)))
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }
            else
            {
                image.color = defaultColor;
            }
        }
    } 
}
