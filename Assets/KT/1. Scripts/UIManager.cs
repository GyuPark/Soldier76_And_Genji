using UnityEngine;


namespace Gyu
{
    public class UIManager : MonoBehaviour
    {
        public GameObject skull;

        public Transform achievements;
        public GameObject[] eliminatedSignal;

        public Transform whoKilledWho;
        public GameObject[] xKilledY;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                skull.SetActive(true);

                GameObject eliminated = Instantiate(eliminatedSignal[Random.Range(0, 2)], achievements);
                eliminated.transform.SetSiblingIndex(0);
                Destroy(eliminated, 1.5f);

                GameObject whoKilled = Instantiate(xKilledY[Random.Range(0, 2)], whoKilledWho);
                whoKilled.transform.SetSiblingIndex(0);
                Destroy(whoKilled, 1.5f);
            }
        }
    } 
}
