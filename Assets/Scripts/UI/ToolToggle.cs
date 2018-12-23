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

    bool interactionsEnabled = true;

    public void EnableInteractions(bool enable)
    {
        interactionsEnabled = enable;

        ToggleTools();
    }

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

        ToggleTools();
    }

    void RemovePlantClicked()
    {
        controller.ToggleTool(UIController.LMBTools.REMOVE_PLANT);

        ToggleTools();
    }

    void ToggleFireClicked()
    {
        controller.ToggleTool(UIController.LMBTools.TOGGLE_FIRE);

        ToggleTools();
    }

    void SelectPlantClicked()
    {
        controller.ToggleTool(UIController.LMBTools.SELECT_PLANT);

        ToggleTools();
    }

    void ToggleTools()
    {
        addPlantButton.interactable = (controller.currentTool != UIController.LMBTools.ADD_PLANT) && interactionsEnabled;
        removePlantButton.interactable = (controller.currentTool != UIController.LMBTools.REMOVE_PLANT) && interactionsEnabled;
        toggleFire.interactable = (controller.currentTool != UIController.LMBTools.TOGGLE_FIRE) && interactionsEnabled;
        selectPlant.interactable = (controller.currentTool != UIController.LMBTools.SELECT_PLANT);
    }
}
