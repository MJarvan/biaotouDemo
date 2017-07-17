using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace biaotouDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 属性

        private DataSet dsDataSet = new DataSet("DataSet1");

        public DataSet DsDataSet
        {
            get { return dsDataSet; }
            set { dsDataSet = value; }
        }

        private DataTable bt1Datatable = new DataTable("Table1");

        public DataTable Bt1Datatable
        {
            get { return bt1Datatable; }
            set { bt1Datatable = value; }
        }

        private DataTable bt2Datatable = new DataTable("Table2");

        public DataTable Bt2Datatable
        {
            get { return bt2Datatable; }
            set { bt2Datatable = value; }
        }

        private DataTable dgDatatable = new DataTable("Table3");

        public DataTable DgDatatable
        {
            get { return dgDatatable; }
            set { dgDatatable = value; }
        }

        List<Double> listDoubleWidth = new List<double>();

        public static int intTotal = 0;

        private static DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback(ExitFrame);

        #endregion 属性

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InsertData();
            int intTableCount = DsDataSet.Tables.Count;
            for (int i = intTableCount - 1; i >= 0; i--)
            {
                if (i == intTableCount - 1)
                {
                    AddBottombiaotou(DsDataSet.Tables[i]);
                }
                else
                {
                    AddToptbiaotou(DsDataSet.Tables[i], DsDataSet.Tables[i + 1]);
                }
            }
            AddDataGrid();
        }

        /// <summary>
        /// 增加datagrid数据
        /// </summary>
        private void AddDataGrid()
        {
            DoEvents();
            for (int i = 0; i < Bt2Datatable.Rows.Count; i++)
            {
                double doubleWidth = grid.ColumnDefinitions[i].ActualWidth;
                listDoubleWidth.Add(doubleWidth);
            }

            for (int j = 0; j < DgDatatable.Columns.Count; j++)
            {
                string strName = DgDatatable.Columns[j].ColumnName;
                DataGridTextColumn col = new DataGridTextColumn() { Header = strName, Binding = new Binding() { Path = new PropertyPath((strName ?? "").ToString().Trim()), } };
                col.ElementStyle = FindResource("wrapCellStyle") as Style;
                this.datagrid.Columns.Add(col);
            }
            datagrid.ItemsSource = DgDatatable.DefaultView;
            for (int i = 0; i < listDoubleWidth.Count; i++)
            {
                datagrid.Columns[i].Width = listDoubleWidth[i];
            }
        }

        #region 表头

        private void AddBottombiaotou(DataTable dt)
        {
            RowDefinition rd1 = new RowDefinition();
            GridLength heigth1 = new GridLength(1, GridUnitType.Auto);
            rd1.Height = heigth1;
            grid.RowDefinitions.Add(rd1);

            RowDefinition rd2 = new RowDefinition();
            GridLength heigth2 = new GridLength(1, GridUnitType.Star);
            rd2.Height = heigth2;
            grid.RowDefinitions.Add(rd2);

            int rowsCount = dt.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                int no = Convert.ToInt32(dt.Rows[i]["No"].ToString().Trim());
                string str = dt.Rows[i]["Name"].ToString().Trim();
                if (str == string.Empty)
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    GridLength width = new GridLength(35);
                    cd.Width = width;
                    grid.ColumnDefinitions.Add(cd);
                }
                else if ( i + 1  == rowsCount)
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    GridLength width = new GridLength(1, GridUnitType.Star);
                    cd.Width = width;
                    grid.ColumnDefinitions.Add(cd);
                }
                else
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    GridLength width = new GridLength(1, GridUnitType.Auto);
                    cd.Width = width;
                    grid.ColumnDefinitions.Add(cd);
                }

                TextBlock textblock = new TextBlock();
                textblock.Text = str;
                textblock.Margin = new Thickness(20);
                textblock.VerticalAlignment = VerticalAlignment.Center;
                textblock.HorizontalAlignment = HorizontalAlignment.Center;

                Border border = new Border();
                border.BorderThickness = new Thickness(0.5);
                border.BorderBrush = Brushes.Black;
                border.Name = "border" + str + i.ToString();
                border.Tag = no;
                border.Child = textblock;

                grid.Children.Add(border);
                border.SetValue(Grid.RowProperty, 1);
                border.SetValue(Grid.ColumnProperty, i);
            }
        }

        private void AddToptbiaotou(DataTable dt1, DataTable dt2)
        {
            int intColumnSpan = 0;
            int rowsCount = dt1.Rows.Count;
            bool boolIsAdd = false;
            for (int i = 0; i < rowsCount; i++)
            {
                int no = Convert.ToInt32(dt1.Rows[i]["No"].ToString().Trim());
                string str = dt1.Rows[i]["Name"].ToString().Trim();
                TextBlock textblock = new TextBlock();
                textblock.Tag = no;
                textblock.Text = str;
                textblock.Margin = new Thickness(10);
                textblock.VerticalAlignment = VerticalAlignment.Center;
                textblock.HorizontalAlignment = HorizontalAlignment.Center;
                textblock.TextWrapping = TextWrapping.Wrap;

                Border border = new Border();
                border.BorderThickness = new Thickness(0.5);
                border.BorderBrush = Brushes.Black;
                border.Child = textblock;

                for(int j = 0; j < dt2.Rows.Count; j++)
                {
                    string strName = dt2.Rows[j]["Name"].ToString().Trim();
                    string strBorderName = "border" + strName + j.ToString();
                    Border element = GetChildObject<Border>(this.grid, strBorderName);
                    if (Convert.ToInt32(element.Tag) == no)
                    {
                        intColumnSpan++;
                    }
                }

                grid.Children.Add(border);

                if (intColumnSpan == 0)
                {
                    //新建一列
                    ColumnDefinition cd = new ColumnDefinition();
                    GridLength width = new GridLength(1, GridUnitType.Auto);
                    cd.Width = width;
                    grid.ColumnDefinitions.Add(cd);

                    intTotal = intTotal + 1;
                    intColumnSpan = 1;
                    border.SetValue(Grid.RowProperty, 0);
                    border.SetValue(Grid.ColumnProperty, intTotal);
                    border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);

                    //新建第二列的border
                    Border border2 = new Border();
                    border2.BorderThickness = new Thickness(0.5);
                    border2.BorderBrush = Brushes.Black;
                    grid.Children.Add(border2);
                    border2.SetValue(Grid.RowProperty, 1);
                    border2.SetValue(Grid.ColumnProperty, intTotal);
                    border2.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                }
                else
                {
                if (intColumnSpan == 1 && boolIsAdd == false)
                    {
                        border.SetValue(Grid.RowProperty, 0);
                        border.SetValue(Grid.ColumnProperty, i);
                        border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                    }
                    else if (intColumnSpan != 1 && boolIsAdd == false)
                    {
                        border.SetValue(Grid.RowProperty, 0);
                        border.SetValue(Grid.ColumnProperty, i);
                        border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                        boolIsAdd = true;
                        intTotal = i + intColumnSpan;
                    }
                    else
                    {
                        border.SetValue(Grid.RowProperty, 0);
                        border.SetValue(Grid.ColumnProperty, intTotal);
                        border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                        intTotal = intTotal + intColumnSpan;
                    }
                }

                intColumnSpan = 0;
            }
        }

        #endregion 表头

        #region 增加数据

        private void InsertData()
        {
            #region 表头第一列

            //表头第一列

            Bt1Datatable.Columns.Add("No");
            Bt1Datatable.Columns.Add("Name");

            DataRow dr0 = Bt1Datatable.NewRow();
            dr0["No"] = 0;
            dr0["Name"] = "科室";
            Bt1Datatable.Rows.Add(dr0);

            DataRow dr1 = Bt1Datatable.NewRow();
            dr1["No"] = 1;
            dr1["Name"] = string.Empty;
            Bt1Datatable.Rows.Add(dr1);

            DataRow dr2 = Bt1Datatable.NewRow();
            dr2["No"] = 2;
            dr2["Name"] = "工作量20分";
            Bt1Datatable.Rows.Add(dr2);

            DataRow dr3 = Bt1Datatable.NewRow();
            dr3["No"] = 3;
            dr3["Name"] = "经济指标25分";
            Bt1Datatable.Rows.Add(dr3);

            DataRow dr4 = Bt1Datatable.NewRow();
            dr4["No"] = 4;
            dr4["Name"] = "药品比10分";
            Bt1Datatable.Rows.Add(dr4);

            DataRow dr50 = Bt1Datatable.NewRow();
            dr50["No"] = 5;
            dr50["Name"] = "累计得分";
            Bt1Datatable.Rows.Add(dr50);

            #endregion 表头第一列

            //-----------------------------------------

            #region 表头第二列

            //表头第二列

            Bt2Datatable.Columns.Add("No");
            Bt2Datatable.Columns.Add("Name");

            DataRow dr5 = Bt2Datatable.NewRow();
            dr5["No"] = 0;
            dr5["Name"] = string.Empty;
            Bt2Datatable.Rows.Add(dr5);

            DataRow dr6 = Bt2Datatable.NewRow();
            dr6["No"] = 1;
            dr6["Name"] = "考核项目";
            Bt2Datatable.Rows.Add(dr6);

            DataRow dr7 = Bt2Datatable.NewRow();
            dr7["No"] = 2;
            dr7["Name"] = "出科人数标准";
            Bt2Datatable.Rows.Add(dr7);

            DataRow dr8 = Bt2Datatable.NewRow();
            dr8["No"] = 2;
            dr8["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr8);

            DataRow dr9 = Bt2Datatable.NewRow();
            dr9["No"] = 2;
            dr9["Name"] = "手术费";
            Bt2Datatable.Rows.Add(dr9);

            DataRow dr10 = Bt2Datatable.NewRow();
            dr10["No"] = 2;
            dr10["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr10);

            DataRow dr11 = Bt2Datatable.NewRow();
            dr11["No"] = 2;
            dr11["Name"] = "科内手术室";
            Bt2Datatable.Rows.Add(dr11);

            DataRow dr12 = Bt2Datatable.NewRow();
            dr12["No"] = 2;
            dr12["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr12);

            DataRow dr13 = Bt2Datatable.NewRow();
            dr13["No"] = 2;
            dr13["Name"] = "使用病床率";
            Bt2Datatable.Rows.Add(dr13);

            DataRow dr14 = Bt2Datatable.NewRow();
            dr14["No"] = 2;
            dr14["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr14);

            DataRow dr15 = Bt2Datatable.NewRow();
            dr15["No"] = 3;
            dr15["Name"] = "月业务收入";
            Bt2Datatable.Rows.Add(dr15);

            DataRow dr16 = Bt2Datatable.NewRow();
            dr16["No"] = 3;
            dr16["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr16);

            DataRow dr17 = Bt2Datatable.NewRow();
            dr17["No"] = 3;
            dr17["Name"] = "结余额";
            Bt2Datatable.Rows.Add(dr17);

            DataRow dr18 = Bt2Datatable.NewRow();
            dr18["No"] = 3;
            dr18["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr18);

            DataRow dr19 = Bt2Datatable.NewRow();
            dr19["No"] = 3;
            dr19["Name"] = "费用成本率";
            Bt2Datatable.Rows.Add(dr19);

            DataRow dr20 = Bt2Datatable.NewRow();
            dr20["No"] = 3;
            dr20["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr20);

            DataRow dr21 = Bt2Datatable.NewRow();
            dr21["No"] = 4;
            dr21["Name"] = "得分";
            Bt2Datatable.Rows.Add(dr21);

            #endregion 表头第二列

            //-----------------------------------------

            #region datagrid内容

            //datagrid内容

            DgDatatable.Columns.Add("科室",typeof(string));
            DgDatatable.Columns.Add("考核项目",typeof(string));
            DgDatatable.Columns.Add("出科人数标准", typeof(int));
            DgDatatable.Columns.Add("出科人数标准得分", typeof(int));
            DgDatatable.Columns.Add("手术费", typeof(decimal));
            DgDatatable.Columns.Add("手术费得分", typeof(int));
            DgDatatable.Columns.Add("科内手术室", typeof(string));
            DgDatatable.Columns.Add("科内手术室得分", typeof(int));
            DgDatatable.Columns.Add("病床使用率", typeof(string));
            DgDatatable.Columns.Add("病床使用率得分", typeof(int));
            DgDatatable.Columns.Add("月业务收入", typeof(decimal));
            DgDatatable.Columns.Add("月业务收入得分", typeof(int));
            DgDatatable.Columns.Add("结余额", typeof(decimal));
            DgDatatable.Columns.Add("结余额得分", typeof(int));
            DgDatatable.Columns.Add("费用成本率", typeof(string));
            DgDatatable.Columns.Add("费用成本率得分", typeof(int));
            DgDatatable.Columns.Add("药品比得分", typeof(int));

            DataRow myDr = DgDatatable.NewRow();
            myDr["科室"] = "儿科";
            myDr["考核项目"] = "bugster切除手术";
            myDr["出科人数标准"] = 4;
            myDr["出科人数标准得分"] = 84;
            myDr["手术费"] = 4396;
            myDr["手术费得分"] = 88;
            myDr["科内手术室"] = "手术室5";
            myDr["科内手术室得分"] = 90;
            myDr["病床使用率"] = "99.9%";
            myDr["病床使用率得分"] = 88;
            myDr["月业务收入"] = 85000;
            myDr["月业务收入得分"] = 85;
            myDr["结余额"] = 85000;
            myDr["结余额得分"] = 85;
            myDr["费用成本率"] = 85000;
            myDr["费用成本率得分"] = 85;
            myDr["药品比得分"] = 75;
            DgDatatable.Rows.Add(myDr);

            #endregion datagrid内容

            DsDataSet.Tables.Add(Bt1Datatable);
            DsDataSet.Tables.Add(Bt2Datatable);

        }

        #endregion 增加数据

        /// <summary>
        /// 根据控件名获取控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public static void DoEvents()
        {
            DispatcherFrame nestedFrame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke
            (DispatcherPriority.Background,exitFrameCallback, nestedFrame);
            Dispatcher.PushFrame(nestedFrame);
            if (exitOperation.Status != DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }
        private static Object ExitFrame(Object state)
        {
            DispatcherFrame frame = state as DispatcherFrame;
            frame.Continue = false;
            return null;
        }
    }
}
