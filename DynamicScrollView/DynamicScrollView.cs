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
    public partial class DynamicScrollView : UserControl
    {
        public Action<ScrollViewData> PanelClickHandler;

        private static readonly Color color1 = Color.FromArgb(255, 243, 226);
        private static readonly Color color2 = Color.FromArgb(232, 234, 238);

        private List<ScrollViewData> dataList = new List<ScrollViewData>();

        private int _startIndex = 0;
        private int _itemAmount;
        private Point subGridOriginalPoint;
        private Point subGridCurrentPoint;

        public DynamicScrollView()
        {
            InitializeComponent();
        }

        private void DynamicScrollView_Load(object sender, EventArgs e)
        {
            subGridOriginalPoint = gird.PointToScreen(Point.Empty);
            panel1.Size = new Size(Width, Height - dataGridView1.Height);

            gird.PanelClickHandler += PanelOnClick;
        }

        /*public void SetOrderPanel(OrderGroup orderGroup)
        {
            List<OrderGroup> orderGroupList = new List<OrderGroup>();
            orderGroupList.Add(orderGroup);
            SetOrderPanel(orderGroupList);
        }

        public void SetOrderPanel(List<OrderGroup> orderGroupList)
        {
            int colorIndex = 0;
            Color color;
            Order order;
            OrderGroup orderGroup;

            dataList.Clear();
            for (int i = 0; i < orderGroupList.Count; i++)
            {
                orderGroup = orderGroupList[i];
                for (int j = 0; j < orderGroup.List.Count; j++)
                {
                    order = orderGroup.List[j];

                    if (!orderGroup.IsSetMealSub(order))
                    {
                        colorIndex++;
                    }
                    if (colorIndex % 2 == 1)
                    {
                        color = color1;
                    }
                    else
                    {
                        color = color2;
                    }
                    dataList.Add(new OrderPanelData(orderGroup, order, null, color));

                    //已送單
                    if (order.HasSend && (j == orderGroupList[i].List.Count - 1 || (j < orderGroupList[i].List.Count - 1 && !orderGroupList[i].List[j + 1].HasSend)))
                    {
                        colorIndex++;
                        if (colorIndex % 2 == 1)
                        {
                            color = color1;
                        }
                        else
                        {
                            color = color2;
                        }
                        dataList.Add(new OrderPanelData(null, null, "已送單", color));
                    }
                }

                if (orderGroupList[i].HasCheckout) //分隔線
                {
                    colorIndex++;
                    if (colorIndex % 2 == 1)
                    {
                        color = color1;
                    }
                    else
                    {
                        color = color2;
                    }
                    dataList.Add(new OrderPanelData(null, null, "已結帳", color));
                }
            }

            int height = 0;
            for (int i = 0; i < dataList.Count; i++)
            {
                height += dataList[i].GetHeight();
            }
            panel2.Size = new Size(panel2.Width, height);

            _itemAmount = (int)Math.Ceiling(panel1.Height / (float)OrderPanel.Height) + 1;
            gird.Init(_itemAmount, panel2.Width);
            gird.RefreshData(0, dataList);
            ClearAllSelection();
            SetOrderPanelSize();
        }

        private void SetAmount(int amount, Order order)
        {
            if (SetAmountHandler != null)
            {
                SetAmountHandler(amount, order);
            }
        }

        public void ScrollToFirst()
        {
            if (dataList.Count > 0)
            {
                SetSelectedOrder(dataList.First().Order);
            }
        }

        public void ScrollToLast()
        {
            if (dataList.Count > 0)
            {
                SetSelectedOrder(dataList.Last().Order);
            }
        }

        public void SetSelectedOrder(Order order)
        {
            int index = GetIndex(order);
            if (index != -1)
            {
                _startIndex = index;

                int height1 = 0;
                for (int i = 0; i < dataList.Count; i++)
                {
                    height1 += dataList[i].GetHeight();
                    if (dataList[i].Order == order)
                    {
                        break;
                    }
                }

                int height2 = dataList[_startIndex].GetHeight();
                while (_startIndex > 0 && panel1.Height > height2)
                {
                    _startIndex--;
                    height2 += dataList[_startIndex].GetHeight();
                }

                int height3 = 0;
                for (int i = 0; i < _startIndex; i++)
                {
                    height3 += dataList[i].GetHeight();
                }

                gird.Location = new Point(0, height3);
                gird.RefreshData(_startIndex, dataList);
                gird.SetSelectedOrder(order); ;
                gird.SetPanelColor();
                if (height1 > panel1.Height)
                {
                    panel1.VerticalScroll.Value = height1 - panel1.Height;
                }
                else
                {
                    panel1.VerticalScroll.Value = 0;
                }
                panel1.PerformLayout();
            }
        }

        private int GetIndex(Order order)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Order == order)
                {
                    return i;
                }
            }
            return -1;
        }

        public void SetAddMinusVisible(bool isVisible)
        {
            gird.SetAddMinusVisible();
        }

        public void SetPanelColor()
        {
            gird.SetPanelColor();
        }

        public void SetAmount(Order order)
        {
            gird.SetAmount(order);
        }

        public void SetTempAmount(string amount) //輸入數字鍵,按確認鍵之前,暫時顯示數量 
        {
            gird.SetTempAmount(amount);
        }

        public void SetSize(int width, int height, List<OrderGroup> orderGroupList)
        {
            panel1.Size = new Size(width, height);
            Size = new Size(width, height + dataGridView1.Size.Height);
            SetOrderPanel(orderGroupList);
            ScrollToFirst();
        }

        public void SetEnable(bool isEnable)
        {
            gird.SetEnable(isEnable);
        }

        public void SetData(Order order)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Order == order)
                {
                    gird.SetData(dataList[i]);
                }
            }
        }

        public void ClearAllSelection()
        {
            gird.ClearAllSelection();
        }

        public void SetOrderPanelSize()
        {
            gird.SetOrderPanelSize();
        }

        private OrderGroup GetOrderGroup(List<OrderGroup> orderGroupList, Order order)
        {
            for (int i = 0; i < orderGroupList.Count; i++)
            {
                if (orderGroupList[i].List.Contains(order))
                {
                    return orderGroupList[i];
                }
            }
            return null;
        }*/

        private void PanelOnClick(ScrollViewData data)
        {
            if (PanelClickHandler != null)
            {
                PanelClickHandler(data);
            }

            this.ActiveControl = null;
        }

        /*private void FlavorDoubleClik()
        {
            if (OpenFlavorOnClick != null)
            {
                OpenFlavorOnClick();
            }
        }

        private void CommentDoubleClick()
        {
            if (OpenCommentOnClick != null)
            {
                OpenCommentOnClick();
            }
        }*/

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshSub();
        }

        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            RefreshSub();
        }

        private void RefreshSub()
        {
            int height = 0;
            Point point;

            if (dataList.Count > 0)
            {
                subGridCurrentPoint = gird.PointToScreen(Point.Empty);

                if (subGridOriginalPoint.Y - subGridCurrentPoint.Y > dataList[_startIndex].GetHeight())
                {
                    while (_startIndex + 1 < dataList.Count && height + dataList[_startIndex].GetHeight() < subGridOriginalPoint.Y - subGridCurrentPoint.Y)
                    {
                        height += dataList[_startIndex].GetHeight();
                        _startIndex++;
                    }
                    point = new Point(subGridCurrentPoint.X, subGridCurrentPoint.Y + height);
                    gird.Location = new Point(gird.Location.X, gird.Parent.PointToClient(point).Y);                   
                    gird.RefreshData(_startIndex, dataList);
                    gird.SetPanelColor();
                }
                else if (subGridOriginalPoint.Y - subGridCurrentPoint.Y < -0.1f && _startIndex > 0)
                {
                    _startIndex--;
                    height += dataList[_startIndex].GetHeight();
                    while (_startIndex > 0 && subGridOriginalPoint.Y - (subGridCurrentPoint.Y - height) < -0.1f)
                    {
                        height += dataList[_startIndex].GetHeight();
                        _startIndex--;
                    }
                    point = new Point(subGridCurrentPoint.X, subGridCurrentPoint.Y - height);
                    gird.Location = new Point(gird.Location.X, gird.Parent.PointToClient(point).Y);
                    gird.RefreshData(_startIndex, dataList);
                    gird.SetPanelColor();
                }
            }
        }
    }
}
