using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;

    public Text amount;

    public InventoryData_SO Bag { get; set; }

    public int Index { get; set; } = -1;

    public void SetUpItemUI(ItemData_SO item, int itemAmount)
    {
        if (itemAmount == 0)
        {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }
        if (item != null)
        {
            // 设置图标
            icon.sprite = item.itemIcon;
            // 设置数量
            amount.text = itemAmount.ToString();

            icon.gameObject.SetActive(true);

        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    public ItemData_SO getItem()
    {
        return Bag.items[Index].itemData;
    }
}
