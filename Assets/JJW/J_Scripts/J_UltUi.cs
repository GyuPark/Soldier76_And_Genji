using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 궁게이지에 따라서 이미지의 색채움을 달리하고 싶다.
public class J_UltUi : MonoBehaviour
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Fill();
    }
    void Fill()
    {
        this.GetComponent<Image>().fillAmount = GetComponentInParent<Genji>().Ultpoint * 0.01f;
    }
}
