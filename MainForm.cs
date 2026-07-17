using SmartInventory.Data;
using SmartInventory.Forms;
using SmartInventory.Models;
using SmartInventory.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;


namespace SmartInventory
{
    public partial class MainForm : Form
    {
        // 畫面（Designer）已備好下列控件，直接照投影片使用即可：
        //   dgv（清單）、txtSearch、cmbCategory、txtName/txtCategory/txtQuantity/txtPrice、
        //   btnAdd/btnUpdate/btnDelete、btnCheck、lblTotal
        //
        // TODO（13-1）：宣告全部商品清單
        private List<Product> all = new List<Product>();

        // 綁定畫面用
        private BindingList<Product> view = new BindingList<Product>();

        public MainForm()
        {
            InitializeComponent();       
            dgv.DataSource = view;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;


            //設定ComboBox
            cmbCategory.Items.Add("全部");
            cmbCategory.Items.AddRange(ProductService.Categories);
            cmbCategory.SelectedIndex = 0;

            cmbInputCategory.Items.AddRange(ProductService.Categories);
            cmbInputCategory.SelectedIndex = 0;


            DbHelper.InitDb();
            all = DbHelper.GetAllProducts();
            RefreshView();
        }

        private void InitializeChartInfrastructure()
        {
            chart.ChartAreas.Clear();
            chart.Legends.Clear();

            // --- 1. 左邊：數量直條圖區域 ---
            var areaQty = new ChartArea("AreaQuantity");
            areaQty.AxisX.MajorGrid.Enabled = false;
            areaQty.AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas.Add(areaQty);

            // --- 2. 右邊：金額環形圖區域 ---
            var areaValue = new ChartArea("AreaValue");
            chart.ChartAreas.Add(areaValue);

            // 圖例統一設定於下方置中
            var legend = new Legend("L")
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            };
            chart.Legends.Add(legend);          
          
        }

        public void RefreshView()
        {
            //  篩選機制
            var filtered = ProductService.Search(all, txtSearch.Text.Trim(), cmbCategory.Text);
            view.Clear();
            foreach (var p in filtered)
            {
                view.Add(p);
            }

            var (total, qty) = ProductService.GetTotalValue(all);
            lblTotal.Text = $"總庫存價值：$ {total}    數量為: {qty}";

            InitializeChartInfrastructure();
            var stat = ProductService.Statistics(all); // 統計各分類
            ChartServices.DrawChart(chart,stat); // 繪製圖表
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ReadInput(out Product p)) return;

            //插入資料庫
            DbHelper.InsertProduct(p);
            all = DbHelper.GetAllProducts();
            //更新畫面                
            RefreshView();
            ClearInput();
        }

        private void ClearInput()
        {
            TextBox[] boxs = { txtName, txtPrice, txtQuantity };
            //cmbInputCategory.SelectedIndex = 0;
            foreach (var b in boxs) b.Text = string.Empty;
        }


        private bool ReadInput(out Product product)
        {
            product = new Product();
            if (txtName.Text.Trim() == "" || cmbInputCategory.Text == "")
            {
                MessageBox.Show("商品名稱或分類不能為空!");
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int q) || q <= 0)
            {
                MessageBox.Show("數量輸入不正確!");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal p) || p <= 0)
            {
                MessageBox.Show("金額輸入不正確!");
                return false;
            }

            product.Name = txtName.Text;
            product.Category = cmbInputCategory.Text;
            product.Quantity = q;
            product.Price = p;

            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= view.Count) return;
            var p = view[e.RowIndex];
            txtName.Text = p.Name;
            cmbInputCategory.Text = p.Category;
            txtQuantity.Text = p.Quantity.ToString();
            txtPrice.Text = p.Price.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;

            int index = dgv.CurrentRow.Index;
            var p = view[index];

            if (MessageBox.Show($"是否刪除Id:{p.Id}-{p.Name}", "確認",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            DbHelper.DeleteProduct(p);
            all = DbHelper.GetAllProducts();
            RefreshView();

            if (view.Count > 0)
            {
                if (index >= view.Count) index = view.Count - 1;
            }

            //維持當下位置
            dgv.Rows[index].Selected = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;

            if (!ReadInput(out Product p)) return;

            int index = dgv.CurrentRow.Index;
            //取得對應商品的實際Id
            p.Id = view[index].Id;

            if (MessageBox.Show($"是否更新Id:{p.Id}-{p.Name}", "確認",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            //更新商品
            DbHelper.UpdateProduct(p);
            all = DbHelper.GetAllProducts();
            RefreshView();

            //維持當下位置
            dgv.Rows[index].Selected = true;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshView();
        }


        private bool isCheckMode = false;
        private void btnCheck_Click(object sender, EventArgs e)
        {



            isCheckMode = !isCheckMode;

            if (isCheckMode)
            {
                btnCheck.Text = "恢復";
                //  篩選機制
                var filtered = ProductService.GetLowStack(all);
                view.Clear();
                foreach (var p in filtered)
                {
                    view.Add(p);
                }
            }
            else
            {
                btnCheck.Text = "庫存警示";
                cmbCategory.SelectedIndex = 0;
                RefreshView();
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            //var calcTotal = ProductService.Statistics(all);

            //foreach (var kv in calcTotal)
            //{
            //    Debug.WriteLine($"{kv.Key} => {kv.Value[0]} {kv.Value[1]}");
            //}

            new ChartForm(all).ShowDialog();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (all.Count == 0) { return; }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV檔案|*.csv";
            var now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            // 取得程式執行的當下目錄，並設為預設存檔路徑
            dialog.InitialDirectory = Application.StartupPath;
            dialog.FileName = $"SmartInventory{now}.csv";

            if (dialog.ShowDialog() != DialogResult.OK) return;

            //寫入檔案
            using (StreamWriter sw = new StreamWriter(
                dialog.FileName, false, Encoding.Unicode))
            {
                sw.WriteLine("編號,分類,產品名稱,數量,單價,總計");

                foreach (Product p in all)
                {
                    string line = string.Format(
                        "{0},{1},{2},{3},{4},{5}",
                        p.Id,
                        p.Category,
                        p.Name,
                        p.Quantity,
                        p.Price,
                        p.TotalValue
                        );

                    sw.WriteLine(line);
                }
            }
        }


        // ───── 以下方法 13-2 才會寫（按鈕事件可在 Designer 雙擊自動產生）─────
        // 13-2：RefreshView()             刷新清單與總價值（用 ProductService.Search 過濾）
        // 13-2：ReadInput(out Product p)  讀輸入＋TryParse 驗證
        // 13-2：ClearInput()              清空輸入框
        // 13-2：btnAdd_Click             新增 → InsertProduct → 重讀 → RefreshView → ClearInput
        // 13-2：dgv_CellClick            點選列帶回輸入欄
        // 13-2：btnUpdate_Click          修改（p.Id 沿用主鍵）→ UpdateProduct → 重讀 → RefreshView
        // 13-2：btnDelete_Click          確認後 DeleteProduct → 重讀 → RefreshView
        // 13-2：btnCheck_Click           ProductService.GetLowStock → MessageBox 列出
    }
}
