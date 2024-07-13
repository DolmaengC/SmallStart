using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line to use UI elements

public class PlayerItemManagement : MonoBehaviour
{
    public GameObject weapon;
    public GameObject NullSpace;
    public GameObject[] inventory; // 배열 선언 수정
    public GameObject[] inventory_spaces; // 배열 선언 수정
    public GameObject BlankItemSpace;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = new GameObject[36];
        
        // for (int i = 0; i < inventory.Length; i++)
        // {
        //     inventory_spaces[i] = new GameObject BlankItemSpace;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkillBook") || other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("WeaponItem"))
        {
            int i = findInventoryBlank();
            if (i != -1)
            {
                inventory[i] = other.gameObject;
                other.gameObject.SetActive(false);
                UpdateInventoryUI(i, other.gameObject.GetComponent<ItemData>());
            }
        }
    }

    private int findInventoryBlank()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private void UpdateInventoryUI(int slotIndex, ItemData itemData)
    {
        // Assuming your inventory_spaces have Image components to display item icons
        Image slotImage = inventory_spaces[slotIndex].GetComponent<Image>();
        if (itemData != null && itemData.itemIcon != null)
        {
            slotImage.sprite = itemData.itemIcon;
            slotImage.enabled = true; // Enable the Image component to show the icon
        }
    }
}
