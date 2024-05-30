using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;

    [Header("Select Item")]

    public GameObject useButton;
    private PlayerController controller;
    private PlayerCondition condition;
    public Transform dropPosition;

    ItemData selectedItem;
    int curEquipIndex;
    int selectedItemIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        CharacterManager.Instance.Player.addItem += AddItem;
        slots = new ItemSlot[slotPanel.childCount];
        controller.inventory += OnUse;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    
    
    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantitiy++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantitiy = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantitiy < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }
    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }
    public bool SelectItem(int index)
    {
        if (slots[index].item == null) return false;
        
        selectedItem = slots[index].item;
        selectedItemIndex = index;
        return true;
    }
    public void OnUse(int index)
    {
        if (SelectItem(index - 1))
        {
            if (selectedItem.type == ItempType.Consumable)
            {
                for (int i = 0; i < selectedItem.consumables.Length; i++)
                {
                    switch (selectedItem.consumables[i].type)
                    {
                        case ConsumableType.Health:
                            condition.Heal(selectedItem.consumables[i].value);
                            break;
                        case ConsumableType.Stamina:
                            condition.StaminaPlus(selectedItem.consumables[i].value);
                            break;
                    }
                }
                RemoveSecletedItem();
            }
        }
    }
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSecletedItem();
    }
    void RemoveSecletedItem()
    {
        slots[selectedItemIndex].quantitiy--;
        if (slots[selectedItemIndex].quantitiy <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
        }
        UpdateUI();
    }
    public void OnEqipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();
        SelectItem(selectedItemIndex);
    }
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();
        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }

    }
    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
