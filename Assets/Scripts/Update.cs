using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Update : MonoBehaviour
{
    [SerializeField] Text UpdatePlayerNickName;
    [SerializeField] Text Errors;
    [SerializeField] PanelsManager PanelsManager;
    public void OnCLickUpdateNickName()
    {
        StartCoroutine(UpdatePlayer("http://localhost:8242/api/players", GameManager.instance.playerData.id));
    }

    IEnumerator UpdatePlayer(string url, int id)
    {
        string json = "{\"Id\":\"" + id + "\", \"NickName\":\"" + UpdatePlayerNickName.text + "\" }";
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
                    Errors.text = "El Usuario ya existe o es el valor es erroneo";
                    break;
                case UnityWebRequest.Result.Success:
                    print(webrequest.downloadHandler.text);
                    Player player = JsonUtility.FromJson<Player>(webrequest.downloadHandler.text);
                    GameManager.instance.playerData = player;
                    PanelsManager.onClickPanel(1);
                    break;
            };
        }
    }
}
