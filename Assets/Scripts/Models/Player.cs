using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    public int id;
    public string nickName;
    public int money;
    public Rank[] ranks;
    public PlayerSkin[] playerSkins;
    public User idNavigation;
}
