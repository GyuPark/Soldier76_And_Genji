using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LoginManager : MonoBehaviourPunCallbacks
{
    // 접속할 서버 버전
    public string serverVersion = "0.0.1";

    // 아이디 입력 필드
    public InputField id;

    void Start()
    {
        // 해상도를 윈도우 모드로 960 x 640 크기로 설정한다.
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
    }

    void Update()
    {
        
    }

    // 접속하기 버튼을 클릭했을 때 실행되는 함수
    public void OnClickCreateButton()
    {
        // 서버 버전을 설정한다.
        PhotonNetwork.GameVersion = serverVersion;

        // 아이디를 서버에서 사용할 닉네임으로 설정한다.
        PhotonNetwork.NickName = id.text;

        // 씬 데이터를 자동으로 동기화하도록 설정한다.
        //PhotonNetwork.AutomaticallySyncScene = true;

        // 오프라인 모드를 끈다.
        PhotonNetwork.OfflineMode = false;

        // 그 밖의 설정은 환경 설정 파일대로 서버에 접속을 시작한다.
        PhotonNetwork.ConnectUsingSettings();
    }

    // 네임 서버 접속 성공 콜백
    public override void OnConnected()
    {
        print("지나가겠습니다~");
        PhotonNetwork.KeepAliveInBackground = 600;
    }

    // 마스터 서버 접속 성공 콜백
    public override void OnConnectedToMaster()
    {
        print("마스터 서버 접속 성공!");

        // 로비에 접속한다.
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        //PhotonNetwork.JoinLobby(new TypedLobby("Wonseok channel", LobbyType.Default));
    }

    // 로비에 접속 성공 콜백
    public override void OnJoinedLobby()
    {
        print("로비 접속 성공!");

        // 로비 씬으로 전환한다.
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
