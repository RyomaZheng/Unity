using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    // 刷新背包
    public void RefreshUI()
    {
        for (int i = 0; i < slotHolders.Length; i++)
        {
            // 设置背包格子编号
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItem();
        }
    }
}
