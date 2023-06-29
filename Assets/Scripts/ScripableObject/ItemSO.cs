using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemData/Item")]
public class ItemSO : ScriptableObject
{
    public int itemID;
    public string itemName;
    public ITEMRarities itemRarities = ITEMRarities.Common;
    public Sprite itemImg;
    [TextArea]
    public string itemDescription;

    [Space]
    public ITEM type;
    public GameObject itemPrefab;

    [ContextMenu(itemName: "Get item stat")]
    public void GetItemStat()
    {
        itemID = GetInstanceID();
        string originalText = itemPrefab.GetComponent<IItem>().GetStats();
        string[] lines = originalText.Split('\n');
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lines.Length; i++)
        {
            if (i > 0) sb.Append("\n"); // add a line break before each line except the first
            if(lines[i] != string.Empty)
                sb.Append("-" + lines[i]);
        }
        itemDescription = sb.ToString();
    }
}
