using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicScrollView
{
    public partial class DynamicScrollViewGrid : FlowLayoutPanel
    {
        public Action<ScrollViewData> PanelClickHandler;

        private ScrollViewData selectedData = null;
        public List<DynamicScrollItem> itemList = new List<DynamicScrollItem>();

        public DynamicScrollViewGrid()
        {
            InitializeComponent();
        }

        public void Init(int itemAmount, int width)
        {
            DynamicScrollItem item;
            Size = new Size(width, DynamicScrollItem.Height * itemAmount);
            Controls.Clear();
            itemList.Clear();
            for (int i = 0; i < itemAmount; i++)
            {
                item = new DynamicScrollItem();
                itemList.Add(item);
                Controls.Add(item);

                item.PanelOnClick += PanelOnClick;
            }
        }

        public void RefreshData(int startIndex, List<ScrollViewData> list)
        {
            int count = 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (startIndex + count < list.Count)
                {
                    itemList[i].Visible = true;
                    itemList[i].SetData(list[startIndex + count]);
                }
                else
                {
                    itemList[i].Visible = false;
                }
                count++;
            }
        }

        /*public void SetData(OrderPanelGroup.OrderPanelData data)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Order == data.Order)
                {
                    itemList[i].SetData(data);
                }
            }
        }

        public void SetSelectedOrder(Order order)
        {
            selectedData = order;
        }

        public void SetAddMinusVisible()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Order != null && itemList[i].Order.Equals(selectedData))
                {
                    itemList[i].SetAddMinusVisible(true);
                }
                else
                {
                    itemList[i].SetAddMinusVisible(false);
                }
            }
        }*/

        public virtual void SetPanelColor()
        {
        }

        /*public void SetEnable(bool isEnable)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].Enabled = isEnable;
            }
        }*/

        public void ClearAllSelection()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].ClearAllSelection();
            }
        }

        /*public void SetOrderPanelSize()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].SetSize();
            }
        }

        public void SetAmount(Order order)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Order == order)
                {
                    itemList[i].SetAmount(order);
                }
            }
        }

        public void SetTempAmount(string amount) //輸入數字鍵,按確認鍵之前,暫時顯示數量 
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Order == selectedData)
                {
                    itemList[i].SetAmount(selectedData);
                }
            }
        }*/

        private void PanelOnClick(ScrollViewData newData)
        {
            ClearAllSelection();

            selectedData = newData;

            if (PanelClickHandler != null)
            {
                PanelClickHandler(newData);
            }
        }

        /*private void AddOnClick(int amount, Order order)
        {
            amount++;
            if (SetAmountHandler != null)
            {
                SetAmountHandler(amount, order);
            }
        }

        private void Remove(Order order)
        {
            if (SetAmountHandler != null)
            {
                SetAmountHandler(0, order);
            }
        }

        private void MinusOnClick(int amount, Order order)
        {
            if (amount > 0)
            {
                amount--;
                if (SetAmountHandler != null)
                {
                    SetAmountHandler(amount, order);
                }
            }
        }

        private void FlavorDoubleClik()
        {
            if (OpenFlavorHandler != null)
            {
                OpenFlavorHandler();
            }
        }

        private void CommentDoubleClick()
        {
            if (OpenCommentHandler != null)
            {
                OpenCommentHandler();
            }
        }*/
    }
}
