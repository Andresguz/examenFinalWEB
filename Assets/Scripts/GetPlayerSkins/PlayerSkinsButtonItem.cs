using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// LevelButtonItem - attached to level button
/// handle specific button related actions
/// </summary>
public class PlayerSkinsButtonItem : MonoBehaviour
{

    public int id;
    public string skinName;
    public PlayerSkinsScrollView playerSkinsScrollView;
    public Button button;
    [SerializeField] Text playerSkinsButtonText;


    private void Start()
    {
        playerSkinsButtonText.text = skinName;
    }

    public void OnLevelButtonClickDelete()
    {
        playerSkinsScrollView.OnLevelButtonClickDelete(id);
    }


}
