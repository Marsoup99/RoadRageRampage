using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] private PlayerCtrl _carCtrl;
    [SerializeField] private IItem _weapon;
    [SerializeField] private IItem _defaultWeapon;

    [SerializeField] private IItem _frontArmor;
    [SerializeField] private Transform _frontArmorPosition;

    [SerializeField] private IItem _sideArmor;
    [SerializeField] private Transform _sideArmorPosition;

    [SerializeField] private IItem _nitroEngine;
    [SerializeField] private Transform _nitroEnginePosition;
    [SerializeField] private IItem[] _relics = new IItem[4];

    void Reset()
    {
        _carCtrl = GetComponentInParent<PlayerCtrl>();
        _carCtrl.playerItems = this;

        _frontArmorPosition = _carCtrl.carModel.Find("<FrontArmorPosition>");
        _sideArmorPosition = _carCtrl.carModel.Find("<SideArmorPosition>");
        _nitroEnginePosition = _carCtrl.carModel.Find("<NitroPosition>");
        
    }
    public void EquipNewItem(ITEM type, GameObject prefab)
    {
        if(type == ITEM.weapon) EquipWeapon(prefab);
        else if(type == ITEM.frontArmor) EquipFrontArmor(prefab);
        else if(type == ITEM.sideArmor) EquipSideArmor(prefab);
        else if(type == ITEM.nitroEngine) EquipNitroEngine(prefab);
    }
    

    private void EquipWeapon(GameObject prefab)
    {
        if(prefab == null) return;

        if(_weapon != null) _weapon.Unequip();

        prefab = Instantiate(prefab, _carCtrl.carWeapon.transform);
        _weapon = prefab.GetComponent<IItem>();
        _weapon.Equip();
    }
    private void EquipFrontArmor(GameObject prefab)
    {
        if(prefab == null) return;

        if(_frontArmor != null) _frontArmor.Unequip();

        prefab = Instantiate(prefab, _frontArmorPosition);
        _frontArmor = prefab.GetComponent<IItem>();
        _frontArmor.Equip();
    }
    private void EquipSideArmor(GameObject prefab)
    {
        if(prefab == null) return;

        if(_sideArmor != null) _sideArmor.Unequip();

        prefab = Instantiate(prefab, _sideArmorPosition);
        _sideArmor = prefab.GetComponent<IItem>();
        _sideArmor.Equip();
    }
    private void EquipNitroEngine(GameObject prefab)
    {
        if(prefab == null) return;

        if(_nitroEngine != null) _nitroEngine.Unequip();

        prefab = Instantiate(prefab, _sideArmorPosition);
        _sideArmor = prefab.GetComponent<IItem>();
        _sideArmor.Equip();
    }
    public void EquipNewRelic(int slot, GameObject prefab)
    {
        if(prefab == null) return;

        _relics[slot]?.Unequip();

        prefab = Instantiate(prefab, transform);
        _relics[slot] = prefab.GetComponent<IItem>();
        _relics[slot].Equip();
    }
}
