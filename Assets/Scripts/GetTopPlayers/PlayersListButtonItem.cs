using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersListButtonItem : MonoBehaviour
{
    public int id;
    public string player;
    public GetPlayersScrollView getPlayersScrollView;
    public Button button;
    [SerializeField] Text PlayersButtonText;


    private void Start()
    {
        PlayersButtonText.text = (player).ToString();
    }

    // click event of level button
    public void OnPlayerButtonClick()
    {
        getPlayersScrollView.OnLevelButtonClick(player);
    }
}
