using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = System.Random;
using UnityEngine.SceneManagement;

/// <summary>
/// LevelsScrollViewController - generate scrollview items
/// handle all things those required for scrollview controller
/// </summary>
public class PlayerSkinsScrollView : MonoBehaviour
{

    [SerializeField] Text levelNumberText;
    [SerializeField] GameObject BtnPref;
    [SerializeField] Transform BtnParent;
    [SerializeField] Text Error;
    List<GameObject> skinPlayerBtnObjets = new List<GameObject>();
    [SerializeField] GameObject skinsObject;

    private void Start()
    {
        if (GameManager.instance.playerData.id == -1)
        {
            SceneManager.LoadScene(0);
        }
        else {
            ClearPlayerSkinsButtons();
            LoadPlayerSkinsButtons();
        }        
    }

    public void ClearPlayerSkinsButtons()
    {
        foreach (var item in skinPlayerBtnObjets)
        {
            Destroy(item);
        }
        skinPlayerBtnObjets.Clear();
    }

    public void GetSkins(Skin[] skins)
    {
        for (int i = 0; i < skins.Length; i++)
        {
            GameObject skinBtnObj = Instantiate(BtnPref, BtnParent) as GameObject;
            skinBtnObj.GetComponent<PlayerSkinsButtonItem>().skinName = skins[i].code;
            skinBtnObj.GetComponent<PlayerSkinsButtonItem>().id = GameManager.instance.playerData.playerSkins[i].id;
            skinBtnObj.GetComponent<PlayerSkinsButtonItem>().playerSkinsScrollView = this;
            skinPlayerBtnObjets.Add(skinBtnObj);
        }
    }


    public void LoadPlayerSkinsButtons()
    {
        Player player = GameManager.instance.playerData;
        Skin[] aux = new Skin[player.playerSkins.Length];
        for (int i = 0; i < player.playerSkins.Length; i++)
        {
            aux[i] = player.playerSkins[i].skin;
        }
        GetSkins(aux);
    }

    public void OnLevelButtonClickDelete(int id) {
        StartCoroutine(DeleteSkin("http://localhost:8242/api/playerSkins", id));
    }

    public void OnButtonClickRefresh()
    {
        StartCoroutine(Refresh("http://localhost:8242/api/players/" + GameManager.instance.playerData.id));
    }

    IEnumerator DeleteSkin(string url, int id)
    {
        using (UnityWebRequest webrequest = UnityWebRequest.Delete(url + '/' + id))
        {
            yield return webrequest.SendWebRequest();

            switch (webrequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Error.text = "Error en el servidor";
                    break;
                case UnityWebRequest.Result.Success:
                    OnButtonClickRefresh();
                    print("success");
                    break;

            };
        }
    }

    IEnumerator Refresh(string url)
    {
        using (UnityWebRequest webrequest = UnityWebRequest.Get(url))
        {
            yield return webrequest.SendWebRequest();

            switch (webrequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Error.text = "Error en el servidor";
                    break;
                case UnityWebRequest.Result.Success:
                    if (webrequest.downloadHandler.text == "")
                    {
                        Error.text = "El Player no existe";
                    }
                    else
                    {
                        Player player = JsonUtility.FromJson<Player>(webrequest.downloadHandler.text);
                        GameManager.instance.playerData = player;
                        ClearPlayerSkinsButtons();
                        LoadPlayerSkinsButtons();

                        skinsObject.GetComponent<GetSkinsScrollView>().ClearAvailableSkinsButtons();
                        skinsObject.GetComponent<GetSkinsScrollView>().LoadSkinButtons();
                    }
                    break;

            };
        }
    }
}
