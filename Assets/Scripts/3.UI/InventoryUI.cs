using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour
{
    [Header("Grid Holder")]
    public GameObject inventoryUI;
    public Transform itemsGird;
    public Transform relicsGrid;
    public Transform bottomTextArea;
    public TextMeshProUGUI nameTMP;
    public TextMeshProUGUI statTMP;
    public Image itemImage;

    [Header("ItemUI")]
    public ItemUI[] items;
    public ItemUI weaponSlot;
    public ItemUI nitroSlot;
    public ItemUI frontSlot;
    public ItemUI sideSlot;
    public ItemUI[] relicsSlot = new ItemUI[4];
    public Action<int> OnSelectRelic;
    public Action<int> OnSelectItems;

    public void OnOpenInventory()
    {
        TimeEffectControl.DoSlowMotion(0);
        CheckIfHaveItem();
        SelectItems(0);
        inventoryUI.SetActive(true);

        TransferChildren(itemsGird, false);
        TransferChildren(relicsGrid, true);

        SoundManager.Instance?.PlayButtonPress();
    }

    public void OnCloseInventory()
    {
        inventoryUI.SetActive(false);
        TimeEffectControl.DoNormalTime();

        SoundManager.Instance?.PlayButtonPress();
    }

    public void TransferChildren(Transform target, bool isRelic)
    {
        //0 mean normal item, 1 mean relic
        if(isRelic)
        {
            foreach(ItemUI item in relicsSlot)
            {
                item.transform.SetParent(target, false);
            }
        }
        else 
        {
            foreach(ItemUI item in items)
            {
                item.transform.SetParent(target, false);
            }
        }    
    }
    void Start()
    {
        items = new ItemUI[4]{weaponSlot, nitroSlot, frontSlot, sideSlot};

        //set the TMP to the same big one in the UI.
        foreach(ItemUI item in items)
        {
            item.SetTMP(nameTMP, statTMP, itemImage);
        }
        foreach(ItemUI item in relicsSlot)
        {
            item.SetTMP(nameTMP, statTMP, itemImage);
        }
        nameTMP.text = null;
    }
    public void EquipItem(ItemSO item)
    {
        switch (item.type)
        {
            case ITEM.weapon:
                weaponSlot.SetItemUI(item);
                break;
            case ITEM.nitroEngine:
                nitroSlot.SetItemUI(item);
                break;
            case ITEM.frontArmor:
                frontSlot.SetItemUI(item);
                break;
            case ITEM.sideArmor:
                sideSlot.SetItemUI(item);
                break;
        }
    }

    public void EquipRelics(int slot, ItemSO item)
    {
        relicsSlot[slot].SetItemUI(item);
    }

    public void UnequipItem(ItemSO item)
    {
         switch (item.type)
        {
            case ITEM.weapon:
                weaponSlot.Unequip();
                break;
            case ITEM.nitroEngine:
                nitroSlot.Unequip();
                break;
            case ITEM.frontArmor:
                frontSlot.Unequip();
                break;
            case ITEM.sideArmor:
                sideSlot.Unequip();
                break;
        }
        CheckIfHaveItem();
    }
    public void UnequipRelics(int slot, ItemSO item)
    {
        relicsSlot[slot].Unequip();
        CheckIfHaveItem();
    }

    public void SelectItems(int index)
    {
        //The select one will have green frame;
        for(int i = 0; i < items.Length; i++)
        {
            if(i == index)
            {
                items[i].Select();
                itemImage.sprite = items[i].ShowText();
            }
            else items[i].DeSelect();
            relicsSlot[i].DeSelect();
        }
        CheckIfHaveItem();
        OnSelectItems?.Invoke(index);

        SoundManager.Instance?.PlayButtonPress();
    }
    public void SelectRelics(int index)
    {
        //The select one will have green frame;
        for(int i = 0; i < relicsSlot.Length; i++)
        {
            if(i == index)
            {
                relicsSlot[i].Select();
                itemImage.sprite = relicsSlot[i].ShowText();
            }
            else relicsSlot[i].DeSelect();
            items[i].DeSelect();
        }
        CheckIfHaveItem();
        OnSelectRelic?.Invoke(index);

        SoundManager.Instance?.PlayButtonPress();
    }

    private void CheckIfHaveItem()
    {
        // Debug.Log(nameTMP.text);
        if(nameTMP.text == null | nameTMP.text == string.Empty)
             bottomTextArea.gameObject.SetActive(false);
        else bottomTextArea.gameObject.SetActive(true);
    }
}
