using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Text;

public class GetSkinsScrollView : MonoBehaviour
{
    [SerializeField] Text levelNumberText;
    [SerializeField] GameObject BtnPref;
    [SerializeField] Transform BtnParent;
    [SerializeField] Text Error;
    List<GameObject> availableSkinsBtnObjets = new List<GameObject>();

    public Text Money2;
    // public int buyAm;
    public SHOP tienda;
    public int monto;
    public int monto2;
    public int c1;
    public int c2;
    public int c3;
    private bool puedeComprar;
    private int idC;
    public Text mensaje;

    [SerializeField] GameObject playerSkinsObject;
    private void Start()
    {
      
        if (GameManager.instance.playerData.id == -1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GetRequestSkins("http://localhost:8242/api/skins"));
        }
        monto =GameManager.instance.playerData.money;
       
    }
    private void Update()
    {
        
       Money2.text = GameManager.instance.playerData.money.ToString();

       monto = GameManager.instance.playerData.money;
 



    }
    public void LoadSkinButtons()
    {
        Skin[] skins = GameManager.instance.availableSkins;
        for (int i = 0; i < skins.Length; i++)
        {
            GameObject availableSkinBtnObj = Instantiate(BtnPref, BtnParent) as GameObject;
            availableSkinBtnObj.GetComponent<SkinButtonItem>().skinName = skins[i].name;
            availableSkinBtnObj.GetComponent<SkinButtonItem>().costo = skins[i].cost;
            availableSkinBtnObj.GetComponent<SkinButtonItem>().id = skins[i].id;
            foreach (var item in GameManager.instance.playerData.playerSkins)
            {
                if (item.skinId == skins[i].id)
                {
                    availableSkinBtnObj.GetComponent<SkinButtonItem>().button.interactable = false;
                }
            }
            availableSkinBtnObj.GetComponent<SkinButtonItem>().skinsScrollView = this;
            availableSkinsBtnObjets.Add(availableSkinBtnObj);
        }
    }

    public void ClearAvailableSkinsButtons()
    {
        foreach (var item in availableSkinsBtnObjets)
        {
            Destroy(item);
        }
        availableSkinsBtnObjets.Clear();
    }

    IEnumerator GetRequestSkins(string url)
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
                    Skins skins = JsonUtility.FromJson<Skins>("{ \"skins\":" + webrequest.downloadHandler.text + "}");
                    GameManager.instance.availableSkins = skins.skins;
                    LoadSkinButtons();
                    c1=skins.skins[0].cost;
                    c2=skins.skins[1].cost;
                    c3=skins.skins[2].cost;
                  
                    break;

            };
        }
    }



    public void OnAvailableSkinButtonClick(int id)
    {
     
       if(monto>= GameManager.instance.availableSkins[id-1].cost)
        {
            levelNumberText.text = "SkinId: " + (id);

            StartCoroutine(BuySkin("http://localhost:8242/api/playerSkins", id));
            Diamont2(GameManager.instance.availableSkins[id-1].cost);
           
            mensaje.text = "Compra Exitosa";
        }

        else
        {
            mensaje.text = "Recargue dinero";
        }
       
    }

    public void OnButtonClickRefresh()
    {
        StartCoroutine(Refresh("http://localhost:8242/api/players/" + GameManager.instance.playerData.id));
    }

    IEnumerator BuySkin(string url, int id)
    {
        Random rd = new System.Random();
        WWWForm form = new WWWForm();
        form.AddField("Id", rd.Next());
        form.AddField("PlayerId", GameManager.instance.playerData.id);
        form.AddField("SkinId", id);
        form.AddField("Date", DateTime.Now.ToString());
        using (UnityWebRequest webrequest = UnityWebRequest.Post(url, form))
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

    public void Diamont2(int mon)
    {
        
     monto2= monto - mon;
        StartCoroutine(DiamontRequest("http://localhost:8242/api/players", GameManager.instance.playerData.id));
    }

    IEnumerator DiamontRequest(string url, int id)
    {
        string json = "{\"Id\":\"" + id + "\", \"Nickname\":\"" + GameManager.instance.playerData.nickName + "\", \"Money\":\"" + monto2 + "\" }";
        byte[] body = Encoding.UTF8.GetBytes(json);
        using (UnityWebRequest webrequest = UnityWebRequest.Put(url + "/" + id, body))
        {
            webrequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
            webrequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webrequest.SetRequestHeader("Content-Type", "application/json");
            yield return webrequest.SendWebRequest();

            switch (webrequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                //    Errors.text = "Erro de transaccion";
                    break;
                case UnityWebRequest.Result.Success:
                    print(webrequest.downloadHandler.text);
                    OnButtonClickRefresh();
                
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
                        GameManager.instance.playerData = JsonUtility.FromJson<Player>(webrequest.downloadHandler.text);
                    
                        monto = GameManager.instance.playerData.money;
                        ClearAvailableSkinsButtons();
                        LoadSkinButtons();

                        playerSkinsObject.GetComponent<PlayerSkinsScrollView>().ClearPlayerSkinsButtons();
                        playerSkinsObject.GetComponent<PlayerSkinsScrollView>().LoadPlayerSkinsButtons();

                    }
                    break;

            };
        }
    }
}