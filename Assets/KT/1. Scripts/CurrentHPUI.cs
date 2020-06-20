using UnityEngine;
using TMPro;

namespace Gyu
{
    public class CurrentHPUI : MonoBehaviour
    {
        PlayerHP hp;

        TextMeshProUGUI currentHPUI;

        void Start()
        {
            hp = GetComponentInParent<PlayerHP>();
            currentHPUI = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            currentHPUI.text = hp.HP.ToString();
        }
    } 
}
