using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class SHOP : MonoBehaviour
{
    
   
    public Text Money;
  
    public Text comprado;
    public Text Errors;

    public int buyAmount;
    public int buyAmount2;
    public int buyAmount3;
    void Start()
    {
      
       buyAmount = GameManager.instance.playerData.money;
       //GameManager.instance.Dinero = buyAmount;
    }


    void Update()
    {
       
       
       if (GameManager.instance != null && GameManager.instance.playerData.idNavigation != null)
        {
          
            Money.text = GameManager.instance.playerData.money.ToString();
        
        }
       
    }
   
    public void Diamont1(int monto)
    {
   
        buyAmount2 = buyAmount + monto;
        
        StartCoroutine(DiamontRequest("http://localhost:8242/api/players", GameManager.instance.playerData.id));
       
    }
    public void OnButtonClickRefresh()
    {
        StartCoroutine(Refresh("http://localhost:8242/api/players/" + GameManager.instance.playerData.id));
    }

    IEnumerator DiamontRequest(string url, int id)
    {
        string json = "{\"Id\":\"" + id + "\", \"Nickname\":\"" + GameManager.instance.playerData.nickName + "\", \"Money\":\"" + buyAmount2 + "\" }";
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
                    Errors.text = "Error de transaccion";
                    break;
                case UnityWebRequest.Result.Success:
                    print(webrequest.downloadHandler.text);
                    comprado.text = "Compra exitosa";
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
                  //  Error.text = "Error en el servidor";
                    break;
                case UnityWebRequest.Result.Success:
                    if (webrequest.downloadHandler.text == "")
                    {
                     Debug.Log("El Player no existe");
                    }
                    else
                    {
                        GameManager.instance.playerData = JsonUtility.FromJson<Player>(webrequest.downloadHandler.text);
                      //  Debug.Log("El Player ");
                        buyAmount = GameManager.instance.playerData.money;
                    }
                    break;

            };
        }
    }

}
