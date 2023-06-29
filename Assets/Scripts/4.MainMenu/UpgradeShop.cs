using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UpgradeShop : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public RectTransform bottomButons;
    public RectTransform shopGO;
    public UpgradeItemUI[] upgradeItems = new UpgradeItemUI[4];

    private MainGameDataSO _mainGameDataSO;

    private bool _isDirty = false;

    public void ShopStart()
    {
        _mainGameDataSO = SaveLoadManager.Instance.mainGameDataSO;
        moneyText.text = _mainGameDataSO.money.ToString();
    }
    public void OpenUpgradeShop()
    {
        _isDirty = false;

        upgradeItems[0].SetUI(_mainGameDataSO.healthText, _mainGameDataSO.hpProcess, _mainGameDataSO.prices[_mainGameDataSO.hpProcess]);
        upgradeItems[1].SetUI(_mainGameDataSO.nitroText, _mainGameDataSO.nitroProcess, _mainGameDataSO.prices[_mainGameDataSO.nitroProcess]);
        upgradeItems[2].SetUI(_mainGameDataSO.collideText, _mainGameDataSO.collideProcess, _mainGameDataSO.prices[_mainGameDataSO.collideProcess]);
        upgradeItems[3].SetUI(_mainGameDataSO.nitroInvincibleText, _mainGameDataSO.nitroInvincibleProcess? 1:0, _mainGameDataSO.nitroInvincibleProcess? _mainGameDataSO.prices[4]:_mainGameDataSO.prices[3]);
        
        moneyText.text = _mainGameDataSO.money.ToString();
        shopGO.anchoredPosition = Vector2.left * 1500;
        shopGO.gameObject.SetActive(true);
        
        shopGO.DOAnchorPosX(0, 1);
        bottomButons.DOAnchorPosY(-500, 1);
    }

    public void CloseShop()
    {
        if(_isDirty)
            SaveLoadManager.Instance.SaveMainGame();

        shopGO.DOAnchorPosX(-1500, 1);
        bottomButons.DOAnchorPosY(0, 1).OnComplete(() => shopGO.gameObject.SetActive(false));

        //PlaySound
        SoundManager.Instance?.PlayButtonPress();
    }
    public void Upgrade(int id)
    {
        //Todo: check money.
        if (id == 0)
        {
            if(_mainGameDataSO.hpProcess < _mainGameDataSO.hpValue.Length - 1)
            {
                if(_mainGameDataSO.money < _mainGameDataSO.prices[_mainGameDataSO.hpProcess]) return;
                
                _mainGameDataSO.money -= _mainGameDataSO.prices[_mainGameDataSO.hpProcess];
                _mainGameDataSO.hpProcess++;

                _isDirty = true;
                _mainGameDataSO.UpdateString();
                upgradeItems[id].SetUI(_mainGameDataSO.healthText, _mainGameDataSO.hpProcess,
                                      _mainGameDataSO.prices[_mainGameDataSO.hpProcess]);
            }
        }
        else if (id == 1)
        {
            if(_mainGameDataSO.nitroProcess < _mainGameDataSO.nitroValue.Length - 1)
            {
                if(_mainGameDataSO.money < _mainGameDataSO.prices[_mainGameDataSO.nitroProcess]) return;

                _mainGameDataSO.money -= _mainGameDataSO.prices[_mainGameDataSO.nitroProcess];
                _mainGameDataSO.nitroProcess++;

                _isDirty = true;
                _mainGameDataSO.UpdateString();
                upgradeItems[id].SetUI(_mainGameDataSO.nitroText, _mainGameDataSO.nitroProcess,
                                      _mainGameDataSO.prices[_mainGameDataSO.nitroProcess]);
            }
        }
        else if (id == 2)
        {
            if(_mainGameDataSO.collideProcess < _mainGameDataSO.collideValue.Length - 1)
            {
                if(_mainGameDataSO.money < _mainGameDataSO.prices[_mainGameDataSO.collideProcess]) return;

                _mainGameDataSO.money -= _mainGameDataSO.prices[_mainGameDataSO.collideProcess];
                _mainGameDataSO.collideProcess++;

                _isDirty = true;
                _mainGameDataSO.UpdateString();
                upgradeItems[id].SetUI(_mainGameDataSO.collideText, _mainGameDataSO.collideProcess,
                                      _mainGameDataSO.prices[_mainGameDataSO.collideProcess]);
            }
        }
        else if (id == 3)
        {
            if(!_mainGameDataSO.nitroInvincibleProcess)
            {
                if(_mainGameDataSO.nitroInvincibleProcess) return;
                if(_mainGameDataSO.money < _mainGameDataSO.prices[3]) return;

                _mainGameDataSO.money -= _mainGameDataSO.prices[3];
                _mainGameDataSO.nitroInvincibleProcess = true;

                _isDirty = true;
                _mainGameDataSO.UpdateString();
                upgradeItems[id].SetUI(_mainGameDataSO.nitroInvincibleText,
                                _mainGameDataSO.nitroInvincibleProcess? 1:0,
                                _mainGameDataSO.nitroInvincibleProcess? _mainGameDataSO.prices[4]:_mainGameDataSO.prices[3]);
            }
        }  
        moneyText.text = _mainGameDataSO.money.ToString();

        //PlaySound
        SoundManager.Instance?.PlayButtonPress();
    }

    public void Reset()
    {
        for (int i = 0; i < _mainGameDataSO.prices.Length - 1; i++)
        {
            if(_mainGameDataSO.hpProcess > i) _mainGameDataSO.money += _mainGameDataSO.prices[i];
            if(_mainGameDataSO.nitroProcess > i) _mainGameDataSO.money += _mainGameDataSO.prices[i];
            if(_mainGameDataSO.collideProcess > i) _mainGameDataSO.money += _mainGameDataSO.prices[i];
        }
        if(_mainGameDataSO.nitroInvincibleProcess) _mainGameDataSO.money += _mainGameDataSO.prices[3];
        
        _mainGameDataSO.ResetProcess();

        upgradeItems[0].SetUI(_mainGameDataSO.healthText, _mainGameDataSO.hpProcess, _mainGameDataSO.prices[_mainGameDataSO.hpProcess]);
        upgradeItems[1].SetUI(_mainGameDataSO.nitroText, _mainGameDataSO.nitroProcess, _mainGameDataSO.prices[_mainGameDataSO.nitroProcess]);
        upgradeItems[2].SetUI(_mainGameDataSO.collideText, _mainGameDataSO.collideProcess, _mainGameDataSO.prices[_mainGameDataSO.collideProcess]);
        upgradeItems[3].SetUI(_mainGameDataSO.nitroInvincibleText, _mainGameDataSO.nitroInvincibleProcess? 1:0, _mainGameDataSO.nitroInvincibleProcess? _mainGameDataSO.prices[4]:_mainGameDataSO.prices[3]);

        moneyText.text = _mainGameDataSO.money.ToString();
        SaveLoadManager.Instance.SaveMainGame();

        //PlaySound
        SoundManager.Instance?.PlayButtonPress();
    }
}
