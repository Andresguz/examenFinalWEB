using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Email;
    public Text PassWord;
    public Text Error;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCLickLogin() {
        StartCoroutine(PostLogin("http://localhost:8242/api/users/authenticate"));
    }
    IEnumerator PostLogin(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("Email", Email.text);
        form.AddField("Password", PassWord.text);
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
                    if (webrequest.downloadHandler.text == "")
                    {
                        Error.text = "El usuario o la contraseña son incorrectas";
                    }
                    else {
                        print(webrequest.downloadHandler.text);
                        Player player = JsonUtility.FromJson<Player>(webrequest.downloadHandler.text);
                        GameManager.instance.playerData = player;
                        SceneManager.LoadScene(1);
                    }
                    break;

            };
        }
    }
}
