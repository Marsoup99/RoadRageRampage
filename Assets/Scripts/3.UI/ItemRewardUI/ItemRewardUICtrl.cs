using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemRewardUICtrl : MonoBehaviour
{
    [SerializeField] protected InventoryUI inventoryUI;
    [SerializeField] protected GameObject holderGO;
    public ItemUI[] itemsUI = new ItemUI[2];
    protected ItemUI _playerItemUI;
    protected ItemSO[] _itemRewards;
    private bool _isRelicsHaveSlot = true;
    protected int _relicReplaceSlotIndex = 0;
    [SerializeField] protected int selectedIndex = 0;
    [Header("Holders and others")]
    [SerializeField] private Transform playerItems;
    [SerializeField] private GameObject playerItemInfoGO;
    [SerializeField] private TextMeshProUGUI playerItemName;
    [SerializeField] private TextMeshProUGUI playerItemStat;
    [SerializeField] private Image playerItemImage;
    public void ShowItemRewardUI(ItemSO[] items)
    {   
        //Because we are using the Items gameObject from InventoryUI so we can listen to when user click
        //on items from InventoryUI.
        inventoryUI.OnSelectRelic += PlayerRelicsSelect;
        inventoryUI.OnSelectItems += PlayerItemsSelect;

        _itemRewards = items;
        int itemRewardsLength = _itemRewards.Length;
        for (int i = 0; i<itemsUI.Length; i++)
        {
            if(i < itemRewardsLength)
            {
                itemsUI[i].gameObject.SetActive(true);
                itemsUI[i].SetItemUI(items[i]);
                itemsUI[i].ShowText();
            }
            else itemsUI[i].gameObject.SetActive(false);
            
            
        }
        if(items[0].type == ITEM.relic)
        {
            _isRelicsHaveSlot = true;
            _relicReplaceSlotIndex = 0;
            //transfer the Item gameObject to this.
            inventoryUI.TransferChildren(playerItems, true);
        }
        else inventoryUI.TransferChildren(playerItems, false);

        //TurnOn the UI.
        holderGO.SetActive(true);
        
        SelectIndex(0);
    }
    public virtual void Close()
    {   
        if(_itemRewards[0].type == ITEM.relic)
            inventoryUI.TransferChildren(inventoryUI.relicsGrid, true);
        else
            inventoryUI.TransferChildren(inventoryUI.itemsGird, false);
        
        inventoryUI.OnSelectRelic -= PlayerRelicsSelect;
        inventoryUI.OnSelectItems -= PlayerItemsSelect;
        GameManager.Instance.DoneChosingReward();
        holderGO.SetActive(false);

        SoundManager.Instance?.PlayButtonPress();
    }

    public virtual void SelectIndex(int id)
    {
        if(_itemRewards[id] == null) return;

        for (int i = 0; i<itemsUI.Length; i++)
        {
            if(i == id)
            {
                itemsUI[i].Select();
                selectedIndex = i;
            } 
            else itemsUI[i].DeSelect();
        }
        MarkReplaceSlot();

        SoundManager.Instance?.PlayButtonPress();
    }
    public void Equip()
    {
        //Call from button "EQUIP"
        if(_itemRewards[selectedIndex].type == ITEM.relic) 
            GameManager.Instance.InGameData.EquipRelic(_relicReplaceSlotIndex, _itemRewards[selectedIndex]);
        else GameManager.Instance.InGameData.EquipItem(_itemRewards[selectedIndex]);
        Close();

        SoundManager.Instance?.PlayButtonPress();
    }

    private void MarkReplaceSlot()
    {
        //If item in slot match the type of reward item then it will have a red frame.
        _playerItemUI?.NoReplace();//Reset the frame to white
        if(_itemRewards[selectedIndex] == null) return;
        switch (_itemRewards[selectedIndex].type)
        {
            case ITEM.weapon:
                inventoryUI.weaponSlot.Replace();
                inventoryUI.SelectItems(0);
                _playerItemUI = inventoryUI.weaponSlot;
                break;
            case ITEM.nitroEngine:
                inventoryUI.nitroSlot.Replace();
                inventoryUI.SelectItems(1);
                _playerItemUI = inventoryUI.nitroSlot;
                break;
            case ITEM.frontArmor:
                inventoryUI.frontSlot.Replace();
                inventoryUI.SelectItems(2);
                _playerItemUI = inventoryUI.frontSlot;
                break;
            case ITEM.sideArmor:
                inventoryUI.sideSlot.Replace();
                inventoryUI.SelectItems(3);
                _playerItemUI = inventoryUI.sideSlot;
                break;
            case ITEM.relic:
            {
                ItemUI[] relics = inventoryUI.relicsSlot;
                for(int i = 0; i<relics.Length; i++)
                {
                    if(relics[i].hadItem == false) 
                    {
                        relics[i].Replace();
                        inventoryUI.SelectRelics(i);
                        _playerItemUI = relics[i];
                        _relicReplaceSlotIndex = i;
                        return;
                    }
                }
                _isRelicsHaveSlot = false;
                _playerItemUI = relics[_relicReplaceSlotIndex];
                _playerItemUI.Replace();
                inventoryUI.SelectRelics(_relicReplaceSlotIndex);
                
                break;
            }
        }
    }

    protected void PlayerItemsSelect(int index)
    {
        ItemUI item = inventoryUI.items[index];
        if(item.hadItem == false)
        {
            playerItemInfoGO.SetActive(false);
            return;
        } 
        playerItemInfoGO.SetActive(true);
        playerItemImage.sprite = item.itemImg.sprite;
        playerItemName.text = item.nameString;
        playerItemStat.text = item.statString;
    }
    
    protected void PlayerRelicsSelect(int index)
    {
        ItemUI item = inventoryUI.relicsSlot[index];
        if(!_isRelicsHaveSlot) 
        {
            _playerItemUI.NoReplace();
            _playerItemUI = item;
            _playerItemUI.Replace();
            _relicReplaceSlotIndex = index;
        }

        if(item.hadItem == false)
        {
            playerItemInfoGO.SetActive(false);
            return;
        } 

        playerItemInfoGO.SetActive(true);
        playerItemImage.sprite = item.itemImg.sprite;
        playerItemName.text = item.nameString;
        playerItemStat.text = item.statString;
    }
}
