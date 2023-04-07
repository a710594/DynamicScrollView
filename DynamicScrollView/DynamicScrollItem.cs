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
    public partial class DynamicScrollItem : FlowLayoutPanel
    {
        public static readonly int Height = 37;

        public Action<ScrollViewData> PanelOnClick;
        public Action OnFlavorDoubleClick;
        public Action OnCommentDoubleClick;

        public ScrollViewData Data;

        private bool isSelected = false;
        private int selectGrid = 1;
        private int cursorX1 = -1;
        private int cursorX2 = -1;
        public Color originalColor;
        private Color hightlightColor = Color.FromArgb(239, 134, 76);
        private Color doubleHightlightColor = Color.FromArgb(235, 101, 71);
        private Color blueColor = Color.FromArgb(55, 115, 188);
        private Dictionary<int, DataGridView> dataGridViewsDic = new Dictionary<int, DataGridView>(); //1:基本資料 2:口味 3:備註 4:折扣

        public DynamicScrollItem()
        {
            InitializeComponent();

            //dataGridViewsDic.Add(1, dataGridView1);
            //dataGridView1.Rows.Add("", "-", "", "+", "");
            //SetDataGridView(1, dataGridView1);

            //dataGridViewsDic.Add(2, dataGridView2);
            //dataGridView2.Rows.Add("");
            //dataGridView2.DoubleClick += new EventHandler(Flavor_DoubleClick);
            //SetDataGridView(2, dataGridView2);

            //dataGridViewsDic.Add(3, dataGridView3);
            //dataGridView3.Rows.Add("");
            //dataGridView3.DoubleClick += new EventHandler(Comment_DoubleClick);
            //SetDataGridView(3, dataGridView3);

            //dataGridViewsDic.Add(4, dataGridView4);
            //dataGridView4.Rows.Add("");
            //SetDataGridView(4, dataGridView4);
        }

        /*public void SetAmount(Order order)
        {
            dataGridView1.Rows[0].Cells[2].Value = order.Amount;
        }*/

        public void SetData(ScrollViewData data)
        {
            Data = data;
            originalColor = data.OriginalColor;

            SetSize();
        }

        public void ClearAllSelection()
        {
            foreach (KeyValuePair<int, DataGridView> pair in dataGridViewsDic)
            {
                pair.Value.ClearSelection();
            }
        }

        public void SetSize()
        {
            int width = dataGridViewsDic[1].Size.Width;
            int height = 0;

            foreach (KeyValuePair<int, DataGridView> pair in dataGridViewsDic)
            {
                if (pair.Value.Visible)
                {
                    pair.Value.Size = new Size(pair.Value.Columns.GetColumnsWidth(DataGridViewElementStates.Visible), pair.Value.Rows.GetRowsHeight(DataGridViewElementStates.Visible));
                    height += pair.Value.Size.Height;
                }
            }

            Size = new Size(width, height);
        }

        //public void SetFlavor(string flavor)
        //{
        //    dataGridViewsDic[2].Rows[0].Cells[0].Value = "   " + flavor;
        //    dataGridViewsDic[2].Visible = flavor != "";
        //}

        //public void SetComment(string comment)
        //{
        //    dataGridViewsDic[3].Rows[0].Cells[0].Value = "   " + comment;
        //    dataGridViewsDic[3].Visible = comment != "";
        //}

        //public void SetDiscount(Order order) 
        //{
        //    if (order.Discount != null)
        //    {
        //        string priceStr = Common.SetDecimaString(order.TotalPrice * order.Amount, 1);
        //        string discountStr = Common.SetDecimaString((order.TotalPrice - order.promoPrice) * order.Amount, 1);
        //        dataGridViewsDic[1].Rows[0].Cells[4].Value = priceStr;
        //        dataGridViewsDic[4].Rows[0].Cells[0].Value = "   " + order.Discount.Name;
        //        dataGridViewsDic[4].Rows[0].Cells[1].Value = "-" + discountStr;
        //        dataGridViewsDic[4].Visible = true;
        //    }
        //    else
        //    {
        //        dataGridViewsDic[4].Visible = false;
        //    }
        //}

        //public void ShowLine(string text) //使用口味那一欄顯示分隔線
        //{
        //    dataGridViewsDic[2].Rows[0].Cells[0].Value = "=====" + text + "=====";
        //    dataGridViewsDic[2].Visible = true;
        //    dataGridViewsDic[1].Visible = false;
        //    dataGridViewsDic[3].Visible = false;
        //    dataGridViewsDic[4].Visible = false;
        //}

        //public void SetPromoPrice(float defaultPrice, float promoPrice, string promoName)
        //{
        //    string priceStr = Common.SetDecimaString(defaultPrice * Data.Amount, 1);
        //    string discountStr = Common.SetDecimaString((defaultPrice - promoPrice) * Data.Amount, 1);
        //    dataGridViewsDic[4].Rows[0].Cells[0].Value = "   " + promoName;
        //    dataGridViewsDic[4].Rows[0].Cells[1].Value = "-" + discountStr;
        //    dataGridViewsDic[4].Visible = true;
        //    dataGridViewsDic[1].Rows[0].Cells[4].Value = priceStr;
        //}

        //public void SetNoDiscount()
        //{
        //    dataGridViewsDic[4].Visible = false;
        //}

        //public void SetTempAmount(string amount) //輸入數字鍵,按確認鍵之前,暫時顯示數量 
        //{
        //    dataGridViewsDic[1].Rows[0].Cells[2].Value = amount;
        //}

        //public void ShowRealAmount() //顯示真正的數量
        //{
        //    if (Data != null)
        //    {
        //        dataGridViewsDic[1].Rows[0].Cells[2].Value = Data.Amount;
        //    }
        //}

        public void SetEnable(bool enable)
        {
            foreach (KeyValuePair<int, DataGridView> pair in dataGridViewsDic)
            {
                pair.Value.Enabled = false;
            }
        }

        private void SetDataGridView(int index, DataGridView dataGridView)
        {
            dataGridView.ClearSelection();
            dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
        }

        //private void Minus()
        //{
        //    if (OnMinus != null)
        //    {
        //        OnMinus(Data.Amount, Data);
        //    }

        //    cursorX1 = -1;
        //}

        //private void Add()
        //{
        //    if (OnAdd != null)
        //    {
        //        OnAdd(Data.Amount, Data);
        //    }

        //    cursorX1 = -1;
        //}

        private int GetSelectedGridIndex(DataGridView dataGridView)
        {
            int index = 0;

            foreach (KeyValuePair<int, DataGridView> pair in dataGridViewsDic)
            {
                if (dataGridView.Equals(pair.Value))
                {
                    index = pair.Key;
                }
            }

            return index;
        }

        public void SetColor(bool isSelected) //isSelected:被點選的 order, isSet: 被點選的 order 的套餐品項, isDoubleHighlight:套餐中被選中的項目
        {
            this.isSelected = isSelected;

            //if (isSelected)
            //{
            //    foreach (KeyValuePair<int, DataGridView> pair in dataGridViewsDic)
            //    {
            //        if (OrderGroup.IsSetMealMain(Data))
            //        {
            //            pair.Value.DefaultCellStyle.BackColor = doubleHightlightColor;
            //            pair.Value.DefaultCellStyle.SelectionBackColor = doubleHightlightColor;
            //        }
            //        else
            //        {
            //            pair.Value.DefaultCellStyle.BackColor = hightlightColor;
            //            pair.Value.DefaultCellStyle.SelectionBackColor = hightlightColor;
            //        }

            //        if (pair.Key == 1)
            //        {
            //            pair.Value.DefaultCellStyle.ForeColor = Color.Black;
            //        }
            //        else
            //        {
            //            pair.Value.DefaultCellStyle.ForeColor = Color.White;
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (KeyValuePair<int, DataGridView> pair in dataGridViewsDic)
            //    {
            //        pair.Value.DefaultCellStyle.BackColor = originalColor;

            //        if (pair.Key == 1)
            //        {
            //            pair.Value.DefaultCellStyle.ForeColor = Color.Black;
            //        }
            //        else
            //        {
            //            pair.Value.DefaultCellStyle.ForeColor = blueColor;
            //        }
            //    }
            //    dataGridViewsDic[1].Columns[2].DefaultCellStyle.BackColor = originalColor;
            //}
            SetAddMinusVisible(isSelected);
        }

        public void SetAddMinusVisible(bool isVisible)
        {
            if (isVisible)
            {
                dataGridViewsDic[1].Columns[1].Visible = true;
                dataGridViewsDic[1].Columns[3].Visible = true;
                dataGridViewsDic[1].Columns[2].Width = 30;
                dataGridViewsDic[1].Columns[2].DefaultCellStyle.BackColor = Color.White;
                dataGridViewsDic[1].Columns[2].DefaultCellStyle.SelectionBackColor = Color.White;
            }
            else
            {
                dataGridViewsDic[1].Columns[1].Visible = false;
                dataGridViewsDic[1].Columns[3].Visible = false;
                dataGridViewsDic[1].Columns[2].Width = 80;
                if (isSelected)
                {
                    dataGridViewsDic[1].Columns[2].DefaultCellStyle.BackColor = dataGridViewsDic[1].DefaultCellStyle.SelectionBackColor;
                }
                else
                {
                    dataGridViewsDic[1].Columns[2].DefaultCellStyle.BackColor = dataGridViewsDic[1].DefaultCellStyle.BackColor;
                }
                dataGridViewsDic[1].Columns[2].DefaultCellStyle.SelectionBackColor = dataGridViewsDic[1].DefaultCellStyle.SelectionBackColor;
            }
            Refresh();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (PanelOnClick != null)
            {
                PanelOnClick(Data);
            }
            SetSize();
        }

        private void Flavor_DoubleClick(object sender, EventArgs e)
        {
            if (OnFlavorDoubleClick != null)
            {
                OnFlavorDoubleClick();
            }

            cursorX1 = -1;
        }

        private void Comment_DoubleClick(object sender, EventArgs e)
        {
            if (OnCommentDoubleClick != null)
            {
                OnCommentDoubleClick();
            }

            cursorX1 = -1;
        }

        /*#region 滑動刪除

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            cursorX1 = Cursor.Position.X;

            DataGridView dataGridView = (DataGridView)sender;
            selectGrid = GetSelectedGridIndex(dataGridView);
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            cursorX2 = Cursor.Position.X;
            if (cursorX1 != -1 && cursorX2 != -1 && (Math.Abs(cursorX2 - cursorX1) >= 100))
            {
                if (OnRemove != null)
                {
                    OnRemove(Data);
                }
            }
            cursorX1 = -1;
            cursorX2 = -1;
        }

        #endregion*/
    }
}
