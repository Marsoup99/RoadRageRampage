
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image coverBG;
    [SerializeField] public Image itemImg;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Canvas canvas;
    public string nameString, statString;
    private float firstLineSize = 28f;
    private float sencondLineSize = 18f;
    public bool hadItem = false;
    void Reset()
    {
        coverBG = GetComponent<Image>();
        itemImg = transform.Find("itemImage").GetComponent<Image>();
        TextMeshProUGUI[] arr = GetComponentsInChildren<TextMeshProUGUI>();
        if(arr.Length > 1)
        {
            nameText = arr[0];
            statText = arr[1];
        } 
    }
    public void SetItemUI(ItemSO item)
    {
        // itemImg = item.image;
        if(item == null) 
        {
            Unequip();
            return;
        }
        hadItem = true;
        // Add rich text tags to set different font sizes for each line
        nameString = "<size=" + firstLineSize + ">" + item.itemName +"</size>\n<size=" 
                    + sencondLineSize + ">" + ItemType(item.type) + "</size>";
        statString = item.itemDescription;
        itemImg.sprite = item.itemImg;
        itemImg.enabled = true;
    }
    public void SetTMP(TextMeshProUGUI nameTMP, TextMeshProUGUI statTMP, Image itemImage)
    {    
        this.nameText = nameTMP;
        this.statText = statTMP;
    }

    public Sprite ShowText()
    {
        //Only use when not having its own TMP
        nameText.text = nameString;
        statText.text = statString;
        if(hadItem)
            itemImg.enabled = true;
        return itemImg.sprite;
    }

    public void Unequip()
    {
        hadItem = false;
        itemImg.sprite = null;
        nameText.text = null;
        statText.text = null;
        nameString = string.Empty;
        statString = string.Empty;
        itemImg.enabled = false;
    }
    public void Select()
    {
        if(canvas != null)
            canvas.sortingOrder = 21;
        transform.localScale = Vector3.one;
        // transform.DOScale(Vector3.one, 0.2f);
        if(coverBG.color == Color.red) return;
        coverBG.color = Color.green;
    }

    public void DeSelect()
    {
        if(canvas != null)
            canvas.sortingOrder = 20;
        transform.localScale = Vector3.one * 0.8f;
        if(coverBG.color == Color.red) return;
        coverBG.color = Color.gray;
    }

    public void Replace()
    {
        coverBG.color = Color.red;
    }
    public void NoReplace()
    {
        coverBG.color = Color.gray;
    }
    void OnDisable()
    {
        NoReplace();
    }

    private string ItemType(ITEM type)
    {
        switch (type)
        {
            case ITEM.weapon:
                return "(WEAPON)";
            case ITEM.nitroEngine:
                return "(NITRO ENGINE)";
            case ITEM.frontArmor:
                return "(FRONT ARMOR)";
            case ITEM.sideArmor:
                return "(SIDE ARMOR)";
            case ITEM.relic:
                return "(RELIC)";
        }
        return null;
    }
}
