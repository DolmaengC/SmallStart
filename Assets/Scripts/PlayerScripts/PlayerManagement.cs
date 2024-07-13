using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerManagement : MonoBehaviour
{
    public UiManagement UI;
    public Camera inventoryCamera; // 인벤토리 카메라 참조
    public CinemachineFreeLook cinemachineCamera; // Cinemachine 가상 카메라 참조
    private Rigidbody rb;
    public int playerLevel;
    public int playermaxHP;
    public int playerCurrentHP;
    public int playermaxMP;
    public int playerCurrentMP;

    private bool isInventoryOpen = false; // 인벤토리 창 상태 추적
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initializePlayerStatus();
        UI.Level2Text(playerLevel);
        UI.UpdateHP(playerCurrentHP, playermaxHP);
        UI.UpdateMP(playerCurrentMP, playermaxMP);

        if (inventoryCamera != null)
        {
            inventoryCamera.gameObject.SetActive(false); // 시작 시 인벤토리 카메라 비활성화
        }

        if (cinemachineCamera != null)
        {
            cinemachineCamera.gameObject.SetActive(true); // 시작 시 Cinemachine 카메라 활성화
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("SkillBook"))
        // {   
        //     Destroy(other.gameObject);
        // }
        // else if (other.gameObject.CompareTag("Item"))
        // {
        //     Destroy(other.gameObject);
        // }
        
        if (other.gameObject.name == "Portal_for_nextStage")
        {
            SceneManager.LoadScene(UI.stage + 1);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Monster"))
        {
            playerCurrentHP -= 1;
            UI.UpdateHP(playerCurrentHP, playermaxHP);
        }
    }

    private void initializePlayerStatus()
    {
        playerLevel = 1;
        playermaxHP = 10;
        playermaxMP = 10;
        playerCurrentHP = playermaxHP;
        playerCurrentMP = playermaxMP;
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        UI.ToggleItemInventory();

        if (inventoryCamera != null)
        {
            inventoryCamera.gameObject.SetActive(isInventoryOpen);
        }

        if (cinemachineCamera != null)
        {
            cinemachineCamera.gameObject.SetActive(!isInventoryOpen);
        }
    }
}
