using UnityEngine;

//cc.isGrounded가 아니라면 일정 높이까지 이동해서 멈춘다.
//cc.isGrounded 시점에 충격만큼 흔들리며 떨어진다..

//0.5 ~ 1.0 
namespace Gyu
{
    public class BottomSectionUI : MonoBehaviour
    {
        PlayerMove pm;
        PlayerState ps;
        CharacterController cc;

        Vector3 startPos;

        void Start()
        {
            cc = GetComponentInParent<CharacterController>();
            pm = GetComponentInParent<PlayerMove>();
            ps = GetComponentInParent<PlayerState>();
            startPos = transform.localPosition;
        }

        void Update()
        {
            if (!pm.contact || pm.yVelocity < -1f) //공중에 떠 있을 때
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, startPos + new Vector3(0f, 50f, 0f), Time.deltaTime * 10f);
            }
            else if (ps.clientState == PlayerState.ClientState.Dash) // 달릴 때
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, startPos + new Vector3(0f, 20f, 0f), Time.deltaTime * 20f);
            }
            else
            {
                if (Vector3.Distance(transform.localPosition, startPos) > 2f)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 20f);
                }
            }
        }
    } 
}
