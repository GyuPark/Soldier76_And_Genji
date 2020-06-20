using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 버튼 변수
    public Button btn_Create;
    public Button btn_Join;

    // 입력 필드 변수
    public InputField field_RoomName;
    public InputField field_maxplayers;

    // 대기 인원 텍스트 변수
    public Text txt_waitPlayers;

    // 방 목록 버튼 변수
    public GameObject roomInfo;

    // 스크롤 뷰의 콘텐트 게임 오브젝트
    public GameObject content;

    // 방 목록 저장용 변수
    Dictionary<string, RoomInfo> cachedRoomlist = new Dictionary<string, RoomInfo>();



    public override void OnEnable()
    {
        // 콜백 인터페이스 사용하기
        base.OnEnable();

    }


    void Start()
    {
        // 해상도를 윈도우 모드로 960 x 640 크기로 설정한다.
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
    }

    void Update()
    {
        // 대기 인원수 표시하기
        txt_waitPlayers.text = "대기중인 인원: " + PhotonNetwork.CountOfPlayersOnMaster.ToString() + "명";
    }

    // 방 목록 업데이트 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 방 목록을 받아서 프리팹을 생성한다.

        // 1. 기존에 있던 방 리스트를 다 삭제한다.
        foreach (Transform cont in content.transform)
        {
            Destroy(cont.gameObject);
        }

        // 2. 방 목록을 받아온다(저장).
        foreach (RoomInfo ri in roomList)
        {
            // 만일, 같은 방 이름이 존재한다면 그 방의 정보를 갱신한다.
            if (cachedRoomlist.ContainsKey(ri.Name))
            {
                // 만일 그 방이 폭파된 방이면...
                if (ri.RemovedFromList)
                {
                    // 저장된 방 딕셔너리에서 제거한다.
                    cachedRoomlist.Remove(ri.Name);
                }
                else
                {
                    // 현재 참여 인원 수 등이 변경된 경우라면 딕셔너리의 값(value)을 덮어씌운다.
                    cachedRoomlist[ri.Name] = ri;
                }
            }
            // 저장한 적이 없는 방이라면 새로 생성된 방이므로...
            else
            {
                // 딕셔너리에 새로 추가한다.
                cachedRoomlist.Add(ri.Name, ri);
            }
        }

        // 3. 방 목록만큼 프리팹을 생성해서 content의 자식 오브젝트로 등록한다.
        MakeRoomList();
    }

    // 방 목록 프리팹 생성 함수
    void MakeRoomList()
    {
        // 만일, 저장된 방이 없다면...
        if (cachedRoomlist.Count <= 0)
        {
            // 함수를 종료시킨다.
            return;
        }

        foreach (RoomInfo ri in cachedRoomlist.Values)
        {
            // 1. 프리팹을 생성한다.
            GameObject go = Instantiate(roomInfo);
            go.transform.SetParent(content.transform);

            // 2. 프리팹에 방 정보에 대한 텍스트를 추가한다.
            Text roomText = go.GetComponentInChildren<Text>();
            roomText.text = ri.Name + "(" + ri.PlayerCount.ToString() + "/" + ri.MaxPlayers.ToString() + ")";

            // 3. 이벤트 트리거를 생성해서 이벤트 함수로 바인딩한다.

            // 3-1. 이벤트 트리거 컴포넌트를 추가한다.
            EventTrigger trigger = go.AddComponent<EventTrigger>();

            // 3-2. 선택 엔트리를 만든다.
            EventTrigger.Entry myEntry = new EventTrigger.Entry();
            myEntry.eventID = EventTriggerType.Select;

            // 3-3. 엔트리에 함수를 바인딩한다.
            //myEntry.callback.AddListener(msmsms);
            myEntry.callback.AddListener((data) => { OnSelectRoom(data); });

            // 3-4. 이벤트 트리거에 선택 엔트리를 등록한다.
            trigger.triggers.Add(myEntry);
        }
    }

    //void msmsms(BaseEventData data)
    //{
    //    OnSelectRoom(data);
    //}

    // 선택했을 때에 실행될 내용을 구현한 함수

    void OnSelectRoom(BaseEventData eventData)
    {
        // 방 목록 입력 필드에 방 이름을 적는다.
        string roomName = eventData.selectedObject.GetComponentInChildren<Text>().text;

        string[] splitedName = roomName.Split('(');
        //[0]: "방 이름" , [1]: "현재 인원수 /최대 인원수)"
        field_RoomName.text = splitedName[0];
        splitedName = splitedName[1].Split('/');
        // [0]: "현재 인원수", [1]: "최대 인원수)"
        field_maxplayers.text = splitedName[1].Substring(0, splitedName[1].Length - 1);

        // 입장하기 버튼을 활성화시킨다.
        btn_Join.interactable = true;
    }

    // 방 생성하기 버튼 함수
    public void OnClickedCreateButton()
    {
        // 룸의 옵션 설정하기
        RoomOptions myRoom = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = byte.Parse(field_maxplayers.text)
        };

        PhotonNetwork.CreateRoom(field_RoomName.text, myRoom, TypedLobby.Default);
    }

    // 방을 생성하는데 성공했을 때의 콜백 함수
    public override void OnCreatedRoom()
    {
        print("방 생성 성공!");
    }

    // 생성된 방에 들어갔을 때의 콜백 함수
    public override void OnJoinedRoom()
    {
        print("방에 들어갔어요!");
        PhotonNetwork.LoadLevel("GameScene");
    }

    // 방 생성에 실패했을 때의 콜백 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방 생성 실패 - " + message);
        
        //OnClickedJoinButton();
    }

    // 방에 입장이 실패했을 때의 콜백 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방 입장 실패 -" + message);
    }

    // 방에 입장하기 버튼 함수
    public void OnClickedJoinButton()
    {
        // 방 이름 인풋 필드에 해당하는 방으로 들어가겠다.
        PhotonNetwork.JoinRoom(field_RoomName.text);
    }
}
