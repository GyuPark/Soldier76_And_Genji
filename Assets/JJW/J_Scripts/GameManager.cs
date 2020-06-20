using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

// 선택창에서 고른 캐릭터를 생성한다.
public class GameManager : MonoBehaviour
{
    // 싱글톤 선언
    public static GameManager gm;
    
    // 해상도 설정
    private void Awake()
    {
        // 싱글턴
        if (gm == null)
        {
            gm = this;
        }

        // 해상도를 윈도우 모드로 960 x 640 크기로 설정한다.
        //Screen.SetResolution(960, 640, FullScreenMode.Windowed);
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        // 1. RPC 전송 빈도를 설정하기
        PhotonNetwork.SendRate = 30;

        // 2. SerializeView 함수 호출 빈도를 설정하기
        PhotonNetwork.SerializationRate = 30;

        // 3. 플레이어 생성
        //InstansiatePlayer();
    }

    // 플레이어 생성함수
    /*void InstansiatePlayer()
    {
        

    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
