using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolToggle : MonoBehaviour
{
    [SerializeField]
    UIController controller;

    [SerializeField]
    Button addPlantButton;
    [SerializeField]
    Button removePlantButton;
    [SerializeField]
    Button toggleFire;
    [SerializeField]
    Button selectPlant;


    void Start()
    {
        addPlantButton.onClick.AddListener(AddPlantClicked);
        removePlantButton.onClick.AddListener(RemovePlantClicked);
        toggleFire.onClick.AddListener(ToggleFireClicked);
        selectPlant.onClick.AddListener(SelectPlantClicked);
    }

    void AddPlantClicked()
    {
        controller.ToggleTool(UIController.LMBTools.ADD_PLANT);

        addPlantButton.interactable = false;
        removePlantButton.interactable = true;
        toggleFire.interactable = true;
        selectPlant.interactable = true;
    }

    void RemovePlantClicked()
    {
        controller.ToggleTool(UIController.LMBTools.REMOVE_PLANT);

        addPlantButton.interactable = true;
        removePlantButton.interactable = false;
        toggleFire.interactable = true;
        selectPlant.interactable = true;
    }

    void ToggleFireClicked()
    {
        controller.ToggleTool(UIController.LMBTools.TOGGLE_FIRE);

        addPlantButton.interactable = true;
        removePlantButton.interactable = true;
        toggleFire.interactable = false;
        selectPlant.interactable = true;
    }

    void SelectPlantClicked()
    {
        controller.ToggleTool(UIController.LMBTools.SELECT_PLANT);

        addPlantButton.interactable = true;
        removePlantButton.interactable = true;
        toggleFire.interactable = true;
        selectPlant.interactable = false;
    }
}
