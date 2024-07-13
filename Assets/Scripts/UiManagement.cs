using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UiManagement : MonoBehaviour
{
    public TMP_Text playerHPText;
    public TMP_Text playerMPText;
    public TMP_Text playerLevelText;
    public Slider HPBar;
    public Slider MPBar;

    public int stage;
    public GameObject itemInventoryUI; 

    public void ToggleItemInventory()
    {
        if (itemInventoryUI != null)
        {
            itemInventoryUI.SetActive(!itemInventoryUI.activeSelf);
        }
    }

    public void Level2Text(int level) {
        playerLevelText.text = level.ToString();
    }
    public void UpdateHP(int currentHP, int maxHP) {
        playerHPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        HPBar.maxValue = maxHP; 
        HPBar.value = currentHP;
    }

    public void UpdateMP(int currentMP, int maxMP) {
        playerMPText.text = currentMP.ToString() + "/" + maxMP.ToString();
        MPBar.maxValue = maxMP;
        MPBar.value = currentMP;
    }


}
