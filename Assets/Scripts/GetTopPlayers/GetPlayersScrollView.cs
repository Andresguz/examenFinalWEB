using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = System.Random;

/// <summary>
/// LevelsScrollViewController - generate scrollview items
/// handle all things those required for scrollview controller
/// </summary>
public class GetPlayersScrollView : MonoBehaviour
{
    [SerializeField] Text playerNameText;
    [SerializeField] GameObject BtnPref;
    [SerializeField] Transform BtnParent;
    [SerializeField] Text Error;


    private void Start()
    {
        StartCoroutine(GetRequestPlayers("http://localhost:8242/api/players"));
    }

    private void LoadPlayersButtons(Player[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameObject levelBtnObj = Instantiate(BtnPref, BtnParent) as GameObject;
            levelBtnObj.GetComponent<PlayersListButtonItem>().id = players[i].id; 
            levelBtnObj.GetComponent<PlayersListButtonItem>().player = players[i].nickName;
            levelBtnObj.GetComponent<PlayersListButtonItem>().getPlayersScrollView = this;
        }
    }

    IEnumerator GetRequestPlayers(string url)
    {
        using (UnityWebRequest webrequest = UnityWebRequest.Get(url))
        {
            yield return webrequest.SendWebRequest();

            switch (webrequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    print("error");
                    break;
                case UnityWebRequest.Result.Success:
                    print(webrequest.downloadHandler.text);
                    Players players = JsonUtility.FromJson<Players>("{ \"players\":" + webrequest.downloadHandler.text + "}");
                    LoadPlayersButtons(players.players);
                    break;

            };
        }
    }

    public void OnLevelButtonClick(string name)
    {
        playerNameText.text = "Player: " + (name);
    }
}
