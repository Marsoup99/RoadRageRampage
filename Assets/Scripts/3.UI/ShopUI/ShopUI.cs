using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopUI : ItemRewardUICtrl
{
    private ItemSO[] itemSOList = new ItemSO[8];
    public ItemSO[] repair;

    [Header("Buy Btn")]
    public TextMeshProUGUI tmpPlayerMoney;
    public TextMeshProUGUI tmpMoney;
    public Image buyButtonImage;
    [SerializeField] private int _priceCommon = 555;
    private int _price;
    public void OpenShop()
    {
        if(GameManager.Instance.InGameData.shopItems.Length == 4)
        {
            ShopRelics();
        }
        else ShopItems();

        tmpPlayerMoney.text = GameManager.Instance.InGameData.playerMoney.ToString();
    }
    public void ShopItems()
    {
        itemSOList = new ItemSO[8];

        ItemSO[] data = GameManager.Instance.InGameData.shopItems;

        for (int i = 0; i < data.Length; i++)
        {
            itemSOList[i] = data[i];
        }
        itemSOList[6] = repair[0];
        itemSOList[7] = repair[1];

        ShowItemRewardUI(itemSOList);
    }
    
    public void ShopRelics()
    {
        itemSOList = new ItemSO[4];

        ItemSO[] data = GameManager.Instance.InGameData.shopItems;
        for (int i = 0; i < data.Length; i++)
        {
            itemSOList[i] = data[i];
        }

        ShowItemRewardUI(itemSOList);
    }

    public override void Close()
    {
        inventoryUI.TransferChildren(inventoryUI.itemsGird, false);
        inventoryUI.TransferChildren(inventoryUI.itemsGird, true);
        
        inventoryUI.OnSelectRelic -= PlayerRelicsSelect;
        inventoryUI.OnSelectItems -= PlayerItemsSelect;
        
        holderGO.SetActive(false);

        GameManager.Instance.LastEnemyKilled();
        GameManager.Instance.DoneChosingReward();

        EnemySpawner.Instance.GetComponentInChildren<MovingShop>().ShopClosed();
    }

    public override void SelectIndex(int id)
    {
        if(_itemRewards[id] == null) 
        {
            buyButtonImage.color = Color.red;
            tmpMoney.text = string.Empty;
            return;
        }
        base.SelectIndex(id);

        //Set the price for item selected.
        _price = GlobalVariables.GetThePrices(_itemRewards[id].itemRarities, GameManager.Instance.LevelManager.currentLevel, GameManager.Instance.LevelManager.currentMap);
        
        if(GameManager.Instance.InGameData.playerMoney >= _price)
        {
            buyButtonImage.color = Color.green;
        }
        
        else buyButtonImage.color = Color.red;
        tmpMoney.text = _price.ToString();
    }
    public void BuyItem()
    {
        SoundManager.Instance?.PlayButtonPress();
        
        if(_itemRewards[selectedIndex] == null) return;

        if(GameManager.Instance.InGameData.playerMoney < _price) return;

        //Do some stuff with player money.
        GameManager.Instance.InGameData.MoneyChange(-_price);
        tmpPlayerMoney.text = GameManager.Instance.InGameData.playerMoney.ToString();

        if(selectedIndex == 6 | selectedIndex == 7)
        {
            BuyRepairPack();
            return;
        }   
        

        //Equip paid items or relic
        if(_itemRewards[selectedIndex].type == ITEM.relic) 
            GameManager.Instance.InGameData.EquipRelic(_relicReplaceSlotIndex, _itemRewards[selectedIndex]);
        else GameManager.Instance.InGameData.EquipItem(_itemRewards[selectedIndex]);
        

        _itemRewards[selectedIndex] = null;
        tmpMoney.text = string.Empty;
        itemsUI[selectedIndex].DeSelect();
        RefreshShop();
    }
    private void BuyRepairPack()
    {
        if(selectedIndex == 6)
        {
            PlayerCtrl.Instance.carStat.hpStat.Heal(PlayerCtrl.Instance.carStat.hpStat.maxValue / 5);
        }
        else if(selectedIndex == 7)
        {
            PlayerCtrl.Instance.carStat.hpStat.Heal(PlayerCtrl.Instance.carStat.hpStat.maxValue);
        }
    }

    private void RefreshShop()
    {
        if(_itemRewards[selectedIndex] == null)
        {
            itemsUI[selectedIndex].NoReplace();
            _playerItemUI.NoReplace();
        }
        for (int i = 0; i<_itemRewards.Length; i++)
        {
            itemsUI[i].SetItemUI(_itemRewards[i]);
            itemsUI[i].ShowText();
        }
        SelectIndex(selectedIndex);
    }
}
