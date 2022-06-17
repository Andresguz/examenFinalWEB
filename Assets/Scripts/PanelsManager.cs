using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    public GameObject[] Panels;
  
    public void onClickPanel(int i) {
        foreach (var item in Panels)
        {
            item.SetActive(false);
        }
        Panels[i].SetActive(true);
    }
}
