using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

//캐릭터를 선택하면 //PlayerPref에 저장하고
//선택된 캐릭터 정보를 저장하고 
//게임서버로 접속한다
public class CharacterSelectionManager : MonoBehaviourPun
{
    public enum Characters
    {
        None = 0,
        Soldier76 = 1,
        Genji = 2
    }
    // 플레이어 생성위치
    public Transform[] teamPos;

    [HideInInspector]public Characters character;

    int myCharacter = 0;

    // 솔져 버튼 누르면
    public void SelectSoldier76()
    {
        PlayerPrefs.SetInt("character", 1);
        DestroyUI();
    }

    // 겐지 버튼 누르면
    public void SelectGenji()
    {
        PlayerPrefs.SetInt("character", 2);
        DestroyUI();
    }
    public GameObject UIUI;
    // 게임서버 접속
    void DestroyUI()
    {
        // 팀설정
        int myTeamNumber = PhotonNetwork.PlayerList.Length % 2;
        // 생성될 위치
        Vector3 spawnPos = teamPos[myTeamNumber].position;

        if (PlayerPrefs.GetInt("character") == 1)
        {
            GameObject Player = PhotonNetwork.Instantiate(Path.Combine("Characters", "Soldier76"), spawnPos, Quaternion.identity);
            Destroy(UIUI);
        }
        if (PlayerPrefs.GetInt("character") == 2)
        {
            GameObject Player = PhotonNetwork.Instantiate(Path.Combine("Characters", "GenJi"), spawnPos, Quaternion.identity);
            Destroy(UIUI);

        }
    }

}
