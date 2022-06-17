using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    public Text NickName;
    public Text FirstName;
    public Text LastName;
    public Text Email;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.playerData.idNavigation != null) {
            NickName.text = GameManager.instance.playerData.nickName;
            FirstName.text = GameManager.instance.playerData.idNavigation.firstName;
            LastName.text = GameManager.instance.playerData.idNavigation.lastName;
            Email.text = GameManager.instance.playerData.idNavigation.email;
        }
    }
}
