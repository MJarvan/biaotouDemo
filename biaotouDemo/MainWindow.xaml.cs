using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private DataTable bt3Datatable = new DataTable("Table3");

        public DataTable Bt3Datatable
        {
            get { return bt3Datatable; }
            set { bt3Datatable = value; }
        }

        private DataTable bt4Datatable = new DataTable("Table4");

        public DataTable Bt4Datatable
        {
            get { return bt4Datatable; }
            set { bt4Datatable = value; }
        }

        private DataTable dgDatatable = new DataTable("Tabledg");

        public DataTable DgDatatable
        {
            get { return dgDatatable; }
            set { dgDatatable = value; }
        }

        List<Double> listDoubleWidth = new List<double>();

        public static int intTotal = 0;

		private bool boolAddList = false;
        #endregion 属性

        public MainWindow()
        {
            InitializeComponent();
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			AddListen();
			InsertData();
            int intTableCount = DsDataSet.Tables.Count;
            for (int i = intTableCount - 1; i >= 0; i--)
            {
                if (i == intTableCount - 1)
                {
                    AddBottombiaotou(DsDataSet.Tables[i], i);
                }
                else
                {
                    AddToptbiaotou(DsDataSet.Tables[i], DsDataSet.Tables[i + 1], i);
                }
            }
            AddDataGrid();
            //AddTotalbiaotou();
            AddPage();
            InitComboBox();
        }

        /// <summary>
        /// 生成合计栏
        /// </summary>
        private void AddTotalbiaotou()
        {
			int intGridColumns = grid.ColumnDefinitions.Count;

			for(int i = 0; i < intGridColumns; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                GridLength width = new GridLength(datagrid.Columns[i].Width.Value);
                cd.Width = width;
                totalgrid.ColumnDefinitions.Add(cd);

                //合并第一二行
                if(i == 0)
                {
                    TextBlock textblock = new TextBlock();
                    textblock.ToolTip = i;
                    textblock.Text = "合计：";
                    textblock.Margin = new Thickness(10);
                    textblock.VerticalAlignment = VerticalAlignment.Center;
                    textblock.HorizontalAlignment = HorizontalAlignment.Center;

                    Border border = new Border();
                    border.BorderThickness = new Thickness(0, 0, 0.4, 0);
                    border.BorderBrush = Brushes.Black;
                    border.Child = textblock;

                    totalgrid.Children.Add(border);
                    border.SetValue(Grid.RowProperty, 0);
                    border.SetValue(Grid.ColumnProperty, i);
                    border.SetValue(Grid.ColumnSpanProperty, 2);
                }
            }

            //计算合计栏
            for (int m = 0; m < intGridColumns; m++)
            {
                string strName = datagrid.Columns[m].Header.ToString().Trim();
                if (strName.Contains("得分") == true)
                {
                    int intTotalPoint = 0;
                    for (int n = 0; n < DgDatatable.Rows.Count; n++)
                    {
                        intTotalPoint = intTotalPoint + Convert.ToInt32(DgDatatable.Rows[n][m].ToString().Trim());
                    }
                    TextBlock textblock = new TextBlock();
                    textblock.ToolTip = m;
                    textblock.Text = intTotalPoint.ToString();
                    textblock.Margin = new Thickness(10);
                    textblock.VerticalAlignment = VerticalAlignment.Center;
                    textblock.HorizontalAlignment = HorizontalAlignment.Center;

                    Border border = new Border();
                    border.BorderThickness = new Thickness(0.4, 0, 0.4, 0);
                    border.BorderBrush = Brushes.Black;
                    border.Child = textblock;

                    totalgrid.Children.Add(border);
                    border.SetValue(Grid.RowProperty, 0);
                    border.SetValue(Grid.ColumnProperty, m);
                }
            }
        }

        /// <summary>
        /// 增加datagrid数据
        /// </summary>
        private void AddDataGrid()
        {
            //刷新界面拿到grid每一列的宽
            int intGridColumns = grid.ColumnDefinitions.Count;
			for(int i = 0;i < intGridColumns;i++)
			{
				double doubleWidth = grid.ColumnDefinitions[i].ActualWidth;
				listDoubleWidth.Add(doubleWidth);
			}
			//添加tooltip和调整字体
			foreach(DataColumn dc in DgDatatable.Columns)
			{
				DataGridTemplateColumn column = new DataGridTemplateColumn() { Width = DataGridLength.Auto,Header = dc.ColumnName };
				DataTemplate temp = new DataTemplate();

				FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));
				FrameworkElementFactory textBox = new FrameworkElementFactory(typeof(TextBox));
				FrameworkElementFactory textBlock= new FrameworkElementFactory(typeof(TextBlock));
				Binding binding = new Binding(dc.ColumnName);
				binding.Mode = BindingMode.TwoWay;

				textBox.SetBinding(TextBox.TextProperty,binding);
				textBox.SetBinding(TextBox.ToolTipProperty,binding);
				textBox.SetValue(TextBox.HorizontalAlignmentProperty,HorizontalAlignment.Center);
				textBox.SetValue(TextBox.VerticalAlignmentProperty,VerticalAlignment.Center);
				textBox.SetValue(TextBox.VisibilityProperty,Visibility.Collapsed);
				//textBox.SetValue(TextBox.WidthProperty,double.NaN);

				textBlock.SetBinding(TextBlock.TextProperty,binding);
				textBlock.SetBinding(TextBlock.ToolTipProperty,binding);
				//textBlock.SetValue(TextBlock.HorizontalAlignmentProperty,HorizontalAlignment.Center);
				textBlock.SetValue(TextBlock.VerticalAlignmentProperty,VerticalAlignment.Center);

				grid.AppendChild(textBlock);
				grid.AppendChild(textBox);


				temp.VisualTree = grid;
				column.CellTemplate = temp;
				this.datagrid.Columns.Add(column);
			}

			this.datagrid.MouseDoubleClick += Datagrid_MouseDoubleClick;

			//计算累计得分
			for (int n = 0; n < DgDatatable.Rows.Count; n++)
            {
                int intTotalPoint = 0;
                for (int m = 0; m < intGridColumns; m++)
                {
                    string strName = datagrid.Columns[m].Header.ToString().Trim();
                    if (strName.Contains("得分") == true)
                    {

                        try
                        {
                            intTotalPoint = intTotalPoint + Convert.ToInt32(DgDatatable.Rows[n][m].ToString().Trim());
                        }
                        catch
                        {
                            DgDatatable.Rows[n][m] = intTotalPoint;
                        }

                    }
                }
            }
			datagrid.ItemsSource = DgDatatable.DefaultView;
		}

		private void Datagrid_MouseDoubleClick(object sender,MouseButtonEventArgs e)
		{
			Point aP = e.GetPosition(this.datagrid);
			IInputElement obj = this.datagrid.InputHitTest(aP);
			DependencyObject target = obj as DependencyObject;

			while(target != null)
			{
				if(target is Grid)
				{
					DependencyObject targetBlock = VisualTreeHelper.GetChild(target,0);
					DependencyObject targetBox = VisualTreeHelper.GetChild(target,1);
					double width = ((Grid)target).ActualWidth;
					targetBlock.SetValue(TextBlock.VisibilityProperty,Visibility.Collapsed);
					targetBox.SetValue(TextBox.VisibilityProperty,Visibility.Visible);
					targetBox.SetValue(TextBox.WidthProperty,width);

					break;
				}
				else if(target is TextBlock || target is TextBox)
				{
					target = VisualTreeHelper.GetParent(target);
				}
				else
				{
					try
					{
						target = VisualTreeHelper.GetChild(target,0);
					}
					catch
					{
						break;
					}
				}
			}
		}

		#region 表头

		/// <summary>
		/// 添加底层表头
		/// </summary>
		/// <param name="dt"></param>
		private void AddBottombiaotou(DataTable dt , int intGridRow)
        {
            int intTableCount = DsDataSet.Tables.Count;

            for (int j = 0; j < intTableCount; j++)
            {
                RowDefinition rd = new RowDefinition();
                if(j == intTableCount - 1)
                {
                    GridLength heigth = new GridLength(1, GridUnitType.Star);
                    rd.Height = heigth;
                }
                else
                {
                    GridLength heigth = new GridLength(1, GridUnitType.Auto);
                    rd.Height = heigth;
                }
                grid.RowDefinitions.Add(rd);
            }

            int rowsCount = dt.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                int intFatherID = Convert.ToInt32(dt.Rows[i]["FatherID"].ToString().Trim());
                string str = dt.Rows[i]["Name"].ToString().Trim();
                if (str == string.Empty)
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    GridLength width = new GridLength(100);
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
                textblock.ToolTip = intFatherID;
                textblock.Text = str;
                textblock.Margin = new Thickness(20);
                textblock.VerticalAlignment = VerticalAlignment.Center;
                textblock.HorizontalAlignment = HorizontalAlignment.Center;

                Border border = new Border();
                border.BorderThickness = new Thickness(0.4);
                border.BorderBrush = Brushes.Black;
                border.Name = "border" + str + i.ToString();
                border.ToolTip = 1;
                border.Tag = intFatherID;
                border.Child = textblock;

                grid.Children.Add(border);
                border.SetValue(Grid.RowProperty, intGridRow);
                border.SetValue(Grid.ColumnProperty, i);
            }
        }

        /// <summary>
        /// 添加上层表头
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        private void AddToptbiaotou(DataTable dt1, DataTable dt2, int intGridRow)
        {
            int intColumnSpan = 0;
            int rowsCount = dt1.Rows.Count;
            bool boolIsAdd = false;
            for (int i = 0; i < rowsCount; i++)
            {
                int intFatherID = Convert.ToInt32(dt1.Rows[i]["FatherID"].ToString().Trim());
                int intID = Convert.ToInt32(dt1.Rows[i]["ID"].ToString().Trim());
                string str = dt1.Rows[i]["Name"].ToString().Trim();

                TextBlock textblock = new TextBlock();
                textblock.ToolTip = intFatherID;
                textblock.Text = str;
                textblock.Margin = new Thickness(10);
                textblock.VerticalAlignment = VerticalAlignment.Center;
                textblock.HorizontalAlignment = HorizontalAlignment.Center;

                Border border = new Border();
                border.BorderThickness = new Thickness(0.4);
                border.BorderBrush = Brushes.Black;
                border.Name = "border" + str + i.ToString();
                border.Tag = intFatherID;
                border.Child = textblock;

                for(int j = 0; j < dt2.Rows.Count; j++)
                {
                    string strName = dt2.Rows[j]["Name"].ToString().Trim();
                    string strBorderName = "border" + strName + j.ToString();
                    Border element = GetChildObject<Border>(this.grid, strBorderName);
                    if (Convert.ToInt32(element.Tag) == intID && Convert.ToInt32(element.ToolTip) <=1)
                    {
                        intColumnSpan++;
                    }
                    else if (Convert.ToInt32(element.Tag) == intID && Convert.ToInt32(element.ToolTip) > 1)
                    {
                        intColumnSpan = intColumnSpan + Convert.ToInt32(element.ToolTip);
                    }
                }

                border.ToolTip = intColumnSpan;
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
                    border.ToolTip = intColumnSpan;
                    border.SetValue(Grid.RowProperty, intGridRow);
                    border.SetValue(Grid.ColumnProperty, intTotal);
                    border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);

                    //新建底下的border(要看底下有多少空的)

                    for (int k = intGridRow; k < DsDataSet.Tables.Count - 1; k++)
                    {
                        Border border2 = new Border();
                        border2.BorderThickness = new Thickness(0.4);
                        border2.ToolTip = 1;
                        border2.BorderBrush = Brushes.Black;
                        grid.Children.Add(border2);
                        border2.SetValue(Grid.RowProperty, k + 1);
                        border2.SetValue(Grid.ColumnProperty, intTotal);
                        border2.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                    }

                    //新建头顶的border(一个就够)
                    DataTable dtTest = DsDataSet.Tables[intGridRow - 1];
                    bool boolTest = false;
                    for (int k = 0; k < dtTest.Rows.Count; k++)
                    {
                        if ( intFatherID == Convert.ToInt32(dtTest.Rows[k]["ID"].ToString().Trim()))
                        {
                            boolTest = true;
                        }
                    }
                    if ( boolTest == false )
                    {
                        Border border3 = new Border();
                        border3.BorderThickness = new Thickness(0.25);
                        border3.ToolTip = 1;
                        border3.BorderBrush = Brushes.Black;
                        grid.Children.Add(border3);
                        border3.SetValue(Grid.RowProperty, intGridRow - 1);
                        border3.SetValue(Grid.ColumnProperty, intTotal);
                        border3.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                    }

                }
                else
                {
                if (intColumnSpan == 1 && boolIsAdd == false)
                    {
                        border.SetValue(Grid.RowProperty, intGridRow);
                        border.SetValue(Grid.ColumnProperty, i);
                        border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                    }
                    else if (intColumnSpan != 1 && boolIsAdd == false)
                    {
                        border.SetValue(Grid.RowProperty, intGridRow);
                        border.SetValue(Grid.ColumnProperty, i);
                        border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                        boolIsAdd = true;
                        intTotal = i + intColumnSpan;
                    }
                    else
                    {
                        border.SetValue(Grid.RowProperty, intGridRow);
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

            #region 表头测试

            Bt4Datatable.Columns.Add("FatherID");
            Bt4Datatable.Columns.Add("ID");
            Bt4Datatable.Columns.Add("Name");

            DataRow dr200 = Bt4Datatable.NewRow();
            dr200["FatherID"] = 0;
            dr200["ID"] = 0;
            dr200["Name"] = "MAX大变身";
            Bt4Datatable.Rows.Add(dr200);

            DataRow dr201 = Bt4Datatable.NewRow();
            dr201["FatherID"] = 0;
            dr201["ID"] = 1;
            dr201["Name"] = "HYPER大变身";
            Bt4Datatable.Rows.Add(dr201);

            #endregion 表头测试

            //-----------------------------------------

            #region 表头第一列
            Bt1Datatable.Columns.Add("FatherID");
            Bt1Datatable.Columns.Add("ID");
            Bt1Datatable.Columns.Add("Name");

            DataRow dr100 = Bt1Datatable.NewRow();
            dr100["FatherID"] = 0;
            dr100["ID"] = 0;
            dr100["Name"] = "增攀";
            Bt1Datatable.Rows.Add(dr100);

            DataRow dr101 = Bt1Datatable.NewRow();
            dr101["FatherID"] = 0;
            dr101["ID"] = 1;
            dr101["Name"] = "颖锋";
            Bt1Datatable.Rows.Add(dr101);

            DataRow dr102 = Bt1Datatable.NewRow();
            dr102["FatherID"] = 1;
            dr102["ID"] = 2;
            dr102["Name"] = "铭杰";
            Bt1Datatable.Rows.Add(dr102);

            DataRow dr103 = Bt1Datatable.NewRow();
            dr103["FatherID"] = 1;
            dr103["ID"] = 3;
            dr103["Name"] = "荷叶";
            Bt1Datatable.Rows.Add(dr103);

            #endregion 表头第一列

            //-----------------------------------------

            #region 表头第二列
            Bt2Datatable.Columns.Add("FatherID");
            Bt2Datatable.Columns.Add("ID");
            Bt2Datatable.Columns.Add("Name");

            DataRow dr0 = Bt2Datatable.NewRow();
            dr0["FatherID"] = 0;
            dr0["ID"] = 0;
            dr0["Name"] = "科室";
            Bt2Datatable.Rows.Add(dr0);

            DataRow dr1 = Bt2Datatable.NewRow();
            dr1["FatherID"] = 1;
            dr1["ID"] = 1;
            dr1["Name"] = string.Empty;
            Bt2Datatable.Rows.Add(dr1);

            DataRow dr2 = Bt2Datatable.NewRow();
            dr2["FatherID"] = 2;
            dr2["ID"] = 2;
            dr2["Name"] = "工作量20分";
            Bt2Datatable.Rows.Add(dr2);

            DataRow dr3 = Bt2Datatable.NewRow();
            dr3["FatherID"] = 2;
            dr3["ID"] = 3;
            dr3["Name"] = "经济指标30分";
            Bt2Datatable.Rows.Add(dr3);

            DataRow dr4 = Bt2Datatable.NewRow();
            dr4["FatherID"] = 2;
            dr4["ID"] = 4;
            dr4["Name"] = "药品比10分";
            Bt2Datatable.Rows.Add(dr4);

            DataRow dr50 = Bt2Datatable.NewRow();
            dr50["FatherID"] = 3;
            dr50["ID"] = 5;
            dr50["Name"] = "累计得分60分";
            Bt2Datatable.Rows.Add(dr50);

            #endregion 表头第二列

            //-----------------------------------------

            #region 表头第三列

            Bt3Datatable.Columns.Add("FatherID");
            Bt3Datatable.Columns.Add("ID");
            Bt3Datatable.Columns.Add("Name");

            DataRow dr5 = Bt3Datatable.NewRow();
            dr5["FatherID"] = 0;
            dr5["ID"] = 0;
            dr5["Name"] = string.Empty;
            Bt3Datatable.Rows.Add(dr5);

            DataRow dr6 = Bt3Datatable.NewRow();
            dr6["FatherID"] = 1;
            dr6["ID"] = 1;
            dr6["Name"] = "考核项目";
            Bt3Datatable.Rows.Add(dr6);

            DataRow dr7 = Bt3Datatable.NewRow();
            dr7["FatherID"] = 2;
            dr7["ID"] = 2;
            dr7["Name"] = "出科人数标准";
            Bt3Datatable.Rows.Add(dr7);

            DataRow dr8 = Bt3Datatable.NewRow();
            dr8["FatherID"] = 2;
            dr8["ID"] = 3;
            dr8["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr8);

            DataRow dr9 = Bt3Datatable.NewRow();
            dr9["FatherID"] = 2;
            dr9["ID"] = 4;
            dr9["Name"] = "手术费";
            Bt3Datatable.Rows.Add(dr9);

            DataRow dr10 = Bt3Datatable.NewRow();
            dr10["FatherID"] = 2;
            dr10["ID"] = 5;
            dr10["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr10);

            DataRow dr11 = Bt3Datatable.NewRow();
            dr11["FatherID"] = 2;
            dr11["ID"] = 6;
            dr11["Name"] = "科内手术室";
            Bt3Datatable.Rows.Add(dr11);

            DataRow dr12 = Bt3Datatable.NewRow();
            dr12["FatherID"] = 2;
            dr12["ID"] = 7;
            dr12["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr12);

            DataRow dr13 = Bt3Datatable.NewRow();
            dr13["FatherID"] = 2;
            dr13["ID"] = 8;
            dr13["Name"] = "使用病床率";
            Bt3Datatable.Rows.Add(dr13);

            DataRow dr14 = Bt3Datatable.NewRow();
            dr14["FatherID"] = 2;
            dr14["ID"] = 9;
            dr14["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr14);

            DataRow dr15 = Bt3Datatable.NewRow();
            dr15["FatherID"] = 3;
            dr15["ID"] = 10;
            dr15["Name"] = "月业务收入";
            Bt3Datatable.Rows.Add(dr15);

            DataRow dr16 = Bt3Datatable.NewRow();
            dr16["FatherID"] = 3;
            dr16["ID"] = 11;
            dr16["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr16);

            DataRow dr17 = Bt3Datatable.NewRow();
            dr17["FatherID"] = 3;
            dr17["ID"] = 12;
            dr17["Name"] = "结余额";
            Bt3Datatable.Rows.Add(dr17);

            DataRow dr18 = Bt3Datatable.NewRow();
            dr18["FatherID"] = 3;
            dr18["ID"] = 13;
            dr18["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr18);

            DataRow dr19 = Bt3Datatable.NewRow();
            dr19["FatherID"] = 3;
            dr19["ID"] = 14;
            dr19["Name"] = "费用成本率";
            Bt3Datatable.Rows.Add(dr19);

            DataRow dr20 = Bt3Datatable.NewRow();
            dr20["FatherID"] = 3;
            dr20["ID"] = 15;
            dr20["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr20);

            DataRow dr21 = Bt3Datatable.NewRow();
            dr21["FatherID"] = 4;
            dr21["ID"] = 16;
            dr21["Name"] = "得分";
            Bt3Datatable.Rows.Add(dr21);

            #endregion 表头第三列

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
            DgDatatable.Columns.Add("累计得分", typeof(int));

            DataRow myDr = DgDatatable.NewRow();
            myDr["科室"] = "儿科";
            myDr["考核项目"] = "bugster切除手术";
            myDr["出科人数标准"] = 4;
            myDr["出科人数标准得分"] = 4;
            myDr["手术费"] = 4396;
            myDr["手术费得分"] = 4;
            myDr["科内手术室"] = "手术室5";
            myDr["科内手术室得分"] = 2;
            myDr["病床使用率"] = "99.9%";
            myDr["病床使用率得分"] = 5;
            myDr["月业务收入"] = 85000;
            myDr["月业务收入得分"] = 5;
            myDr["结余额"] = 85000;
            myDr["结余额得分"] = 8;
            myDr["费用成本率"] = "85%";
            myDr["费用成本率得分"] = 5;
            myDr["药品比得分"] = 7;
            DgDatatable.Rows.Add(myDr);

            DataRow myDr2 = DgDatatable.NewRow();
            myDr2["科室"] = "心脏内科";
            myDr2["考核项目"] = "通波仔";
            myDr2["出科人数标准"] = 10;
            myDr2["出科人数标准得分"] = 5;
            myDr2["手术费"] = 25764;
            myDr2["手术费得分"] = 5;
            myDr2["科内手术室"] = "手术室6";
            myDr2["科内手术室得分"] = 3;
            myDr2["病床使用率"] = "99.9%";
            myDr2["病床使用率得分"] = 4;
            myDr2["月业务收入"] = 200000;
            myDr2["月业务收入得分"] = 8;
            myDr2["结余额"] = 78554;
            myDr2["结余额得分"] = 6;
            myDr2["费用成本率"] = "85%";
            myDr2["费用成本率得分"] = 8;
            myDr2["药品比得分"] = 6;
            DgDatatable.Rows.Add(myDr2);

            DataRow myDr3 = DgDatatable.NewRow();
            myDr3["科室"] = "内科";
            myDr3["考核项目"] = "伤风";
            myDr3["出科人数标准"] = 10;
            myDr3["出科人数标准得分"] = 5;
            myDr3["手术费"] = 75372;
            myDr3["手术费得分"] = 5;
            myDr3["科内手术室"] = "手术室9";
            myDr3["科内手术室得分"] = 3;
            myDr3["病床使用率"] = "99.9%";
            myDr3["病床使用率得分"] = 4;
            myDr3["月业务收入"] = 7535273;
            myDr3["月业务收入得分"] = 4;
            myDr3["结余额"] = 2453;
            myDr3["结余额得分"] = 4;
            myDr3["费用成本率"] = "85%";
            myDr3["费用成本率得分"] = 4;
            myDr3["药品比得分"] = 10;
            DgDatatable.Rows.Add(myDr3);

            DataRow myDr4 = DgDatatable.NewRow();
            myDr4["科室"] = "外科";
            myDr4["考核项目"] = "骨折";
            myDr4["出科人数标准"] = 10;
            myDr4["出科人数标准得分"] = 1;
            myDr4["手术费"] = 78575;
            myDr4["手术费得分"] = 2;
            myDr4["科内手术室"] = "手术室66";
            myDr4["科内手术室得分"] = 3;
            myDr4["病床使用率"] = "99.9%";
            myDr4["病床使用率得分"] = 5;
            myDr4["月业务收入"] = 4534452;
            myDr4["月业务收入得分"] = 8;
            myDr4["结余额"] = 78676;
            myDr4["结余额得分"] = 9;
            myDr4["费用成本率"] = "85%";
            myDr4["费用成本率得分"] = 4;
            myDr4["药品比得分"] = 8;
            DgDatatable.Rows.Add(myDr4);

            DataRow myDr5 = DgDatatable.NewRow();
            myDr5["科室"] = "神经内科";
            myDr5["考核项目"] = "老人痴呆";
            myDr5["出科人数标准"] = 10;
            myDr5["出科人数标准得分"] = 5;
            myDr5["手术费"] = 78572;
            myDr5["手术费得分"] = 5;
            myDr5["科内手术室"] = "手术室9";
            myDr5["科内手术室得分"] = 3;
            myDr5["病床使用率"] = "99.9%";
            myDr5["病床使用率得分"] = 4;
            myDr5["月业务收入"] = 78567;
            myDr5["月业务收入得分"] = 4;
            myDr5["结余额"] = 445;
            myDr5["结余额得分"] = 4;
            myDr5["费用成本率"] = "85%";
            myDr5["费用成本率得分"] = 4;
            myDr5["药品比得分"] = 1;
            DgDatatable.Rows.Add(myDr5);

            DataRow myDr6 = DgDatatable.NewRow();
            myDr6["科室"] = "内科";
            myDr6["考核项目"] = "发热";
            myDr6["出科人数标准"] = 10;
            myDr6["出科人数标准得分"] = 5;
            myDr6["手术费"] = 25764;
            myDr6["手术费得分"] = 5;
            myDr6["科内手术室"] = "手术室9";
            myDr6["科内手术室得分"] = 3;
            myDr6["病床使用率"] = "99.9%";
            myDr6["病床使用率得分"] = 4;
            myDr6["月业务收入"] = 200000;
            myDr6["月业务收入得分"] = 4;
            myDr6["结余额"] = 78554;
            myDr6["结余额得分"] = 8;
            myDr6["费用成本率"] = "85%";
            myDr6["费用成本率得分"] = 8;
            myDr6["药品比得分"] = 8;
            DgDatatable.Rows.Add(myDr6);

            DataRow myDr7 = DgDatatable.NewRow();
            myDr7["科室"] = "传染科";
            myDr7["考核项目"] = "H5N1";
            myDr7["出科人数标准"] = 10;
            myDr7["出科人数标准得分"] = 5;
            myDr7["手术费"] = 4356;
            myDr7["手术费得分"] = 5;
            myDr7["科内手术室"] = "手术室9";
            myDr7["科内手术室得分"] = 3;
            myDr7["病床使用率"] = "99.9%";
            myDr7["病床使用率得分"] = 4;
            myDr7["月业务收入"] = 235;
            myDr7["月业务收入得分"] = 4;
            myDr7["结余额"] = 456;
            myDr7["结余额得分"] = 7;
            myDr7["费用成本率"] = "85%";
            myDr7["费用成本率得分"] = 4;
            myDr7["药品比得分"] = 7;
            DgDatatable.Rows.Add(myDr7);

            DataRow myDr8 = DgDatatable.NewRow();
            myDr8["科室"] = "妇科";
            myDr8["考核项目"] = "分娩";
            myDr8["出科人数标准"] = 10;
            myDr8["出科人数标准得分"] = 2;
            myDr8["手术费"] = 123;
            myDr8["手术费得分"] = 5;
            myDr8["科内手术室"] = "手术室9";
            myDr8["科内手术室得分"] = 3;
            myDr8["病床使用率"] = "99.9%";
            myDr8["病床使用率得分"] = 4;
            myDr8["月业务收入"] = 456;
            myDr8["月业务收入得分"] = 4;
            myDr8["结余额"] = 789;
            myDr8["结余额得分"] = 8;
            myDr8["费用成本率"] = "85%";
            myDr8["费用成本率得分"] = 4;
            myDr8["药品比得分"] =2;
            DgDatatable.Rows.Add(myDr8);

            DataRow myDr9 = DgDatatable.NewRow();
            myDr9["科室"] = "儿科";
            myDr9["考核项目"] = "先天性缺陷";
            myDr9["出科人数标准"] = 10;
            myDr9["出科人数标准得分"] = 5;
            myDr9["手术费"] = 15689;
            myDr9["手术费得分"] = 5;
            myDr9["科内手术室"] = "手术室9";
            myDr9["科内手术室得分"] = 3;
            myDr9["病床使用率"] = "99.9%";
            myDr9["病床使用率得分"] = 4;
            myDr9["月业务收入"] = 46489;
            myDr9["月业务收入得分"] = 5;
            myDr9["结余额"] = 16548;
            myDr9["结余额得分"] = 5;
            myDr9["费用成本率"] = "85%";
            myDr9["费用成本率得分"] = 5;
            myDr9["药品比得分"] = 0;
            DgDatatable.Rows.Add(myDr9);

            DataRow myDr10 = DgDatatable.NewRow();
            myDr10["科室"] = "CR";
            myDr10["考核项目"] = "手术等级LV100";
            myDr10["出科人数标准"] = 1;
            myDr10["出科人数标准得分"] = 5;
            myDr10["手术费"] = 0;
            myDr10["手术费得分"] = 5;
            myDr10["科内手术室"] = "手术室9";
            myDr10["科内手术室得分"] = 5;
            myDr10["病床使用率"] = "99.9%";
            myDr10["病床使用率得分"] = 5;
            myDr10["月业务收入"] = 0;
            myDr10["月业务收入得分"] = 5;
            myDr10["结余额"] = 0;
            myDr10["结余额得分"] = 5;
            myDr10["费用成本率"] = "85%";
            myDr10["费用成本率得分"] = 10;
            myDr10["药品比得分"] = 10;
            DgDatatable.Rows.Add(myDr10);

            DataRow myDr11 = DgDatatable.NewRow();
            myDr11["科室"] = "无证";
            myDr11["考核项目"] = "第50战术";
            myDr11["出科人数标准"] = 10;
            myDr11["出科人数标准得分"] = 5;
            myDr11["手术费"] = 25764;
            myDr11["手术费得分"] = 5;
            myDr11["科内手术室"] = "手术室9";
            myDr11["科内手术室得分"] = 3;
            myDr11["病床使用率"] = "99.9%";
            myDr11["病床使用率得分"] = 4;
            myDr11["月业务收入"] = 200000;
            myDr11["月业务收入得分"] = 10;
            myDr11["结余额"] = 78554;
            myDr11["结余额得分"] = 10;
            myDr11["费用成本率"] = "85%";
            myDr11["费用成本率得分"] = 10;
            myDr11["药品比得分"] = 10;
            DgDatatable.Rows.Add(myDr11);

            #endregion datagrid内容

            DsDataSet.Tables.Add(Bt4Datatable);
            DsDataSet.Tables.Add(Bt1Datatable);
            DsDataSet.Tables.Add(Bt2Datatable);
            DsDataSet.Tables.Add(Bt3Datatable);

        }

        #endregion 增加数据

        #region 辅助函数
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
		/// 添加监听
		/// </summary>
		public void AddListen()
		{
			DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualWidthProperty,typeof(ColumnDefinition));
			descriptor.AddValueChanged(this.grid,delegate
			{
				for(int i = 0;i < this.grid.ColumnDefinitions.Count;i++)
				{
					if(listDoubleWidth[i] == 0)
					{
						listDoubleWidth[i] = this.grid.ColumnDefinitions[i].ActualWidth;
					}
					else
					{
						return;
					}
				}
				AddList();
			});
		}

		private void AddList()
		{
			if(boolAddList == false)
			{
				//调整列宽度
				int intGridColumns = grid.ColumnDefinitions.Count;

				for(int k = 0;k < intGridColumns;k++)
				{
					datagrid.Columns[k].Width = listDoubleWidth[k];
				}

				boolAddList = true;
			}
			else
			{
				return;
			}
		}

		//// Dependency Property Declaration
		//private static DependencyPropertyKey ElementActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("ElementActualWidth",typeof(double),typeof(double),new PropertyMetadata());
		//public static DependencyProperty ElementActualWidthProperty = ElementActualWidthPropertyKey.DependencyProperty;
		//public double ElementActualWidth
		//{
		//	get
		//	{
		//		return (double)GetValue(ElementActualWidthProperty);
		//	}
		//}
		//private void SetActualWidth(double value)
		//{
		//	SetValue(ElementActualWidthPropertyKey,value);
		//}

		// Dependency Property Callback
		// Called when this.MyElement.ActualWidth is changed
		//private void OnActualWidthChanged(object sender,Eventargs e)
		//{
		//	this.SetActualWidth(this.MyElement.ActualWidth);
		//}


		#endregion 辅助函数

		#region 分页

		#region 添加分页控件

		/// <summary>
		/// 添加分页控件
		/// </summary>
		private void AddPage()
        {

            ComboBox combobox = new ComboBox();
            combobox.Name = "comboBoxPageNumber";
            combobox.Width = double.NaN;
            combobox.VerticalAlignment = VerticalAlignment.Center;
            combobox.SelectionChanged += new SelectionChangedEventHandler(comboBoxPageNumber_SelectionChanged);
            stackpanel.Children.Add(combobox);

            Button button1 = new Button();
            button1.Name = "buttonHome";
            button1.Content = "首页";
            button1.VerticalAlignment = VerticalAlignment.Center;
            button1.Margin = new Thickness(3, 0, 3, 0);
            button1.Click += new RoutedEventHandler(buttonHome_Click);
            stackpanel.Children.Add(button1);

            Button button2 = new Button();
            button2.Name = "buttonUp";
            button2.Content = "上一页";
            button2.VerticalAlignment = VerticalAlignment.Center;
            button2.Margin = new Thickness(3, 0, 3, 0);
            button2.Click += new RoutedEventHandler(buttonUp_Click);
            stackpanel.Children.Add(button2);

            Button button3 = new Button();
            button3.Name = "buttonNext";
            button3.Content = "下一页";
            button3.VerticalAlignment = VerticalAlignment.Center;
            button3.Margin = new Thickness(3, 0, 3, 0);
            button3.Click += new RoutedEventHandler(buttonNext_Click);
            stackpanel.Children.Add(button3);

            Button button4 = new Button();
            button4.Name = "buttonEnd";
            button4.Content = "尾页";
            button4.VerticalAlignment = VerticalAlignment.Center;
            button4.Margin = new Thickness(3, 0, 3, 0);
            button4.Click += new RoutedEventHandler(buttonEnd_Click);
            stackpanel.Children.Add(button4);

            TextBlock textblock1 = new TextBlock();
            textblock1.Text = "共";
            textblock1.VerticalAlignment = VerticalAlignment.Center;
            stackpanel.Children.Add(textblock1);

            TextBlock textblock2 = new TextBlock();
            textblock2.Text = "0";
            textblock2.Name = "textBlockTotal";
            textblock2.VerticalAlignment = VerticalAlignment.Center;
            stackpanel.Children.Add(textblock2);

            TextBlock textblock3 = new TextBlock();
            textblock3.Text = "条";
            textblock3.VerticalAlignment = VerticalAlignment.Center;
            textblock3.Margin = new Thickness(3, 0, 3, 0);
            stackpanel.Children.Add(textblock3);

            TextBlock textblock4 = new TextBlock();
            textblock4.Text = "第";
            textblock4.VerticalAlignment = VerticalAlignment.Center;
            stackpanel.Children.Add(textblock4);

            TextBlock textblock5 = new TextBlock();
            textblock5.Text = "0";
            textblock5.VerticalAlignment = VerticalAlignment.Center;
            textblock5.Name = "textBlockPage";
            stackpanel.Children.Add(textblock5);

            TextBlock textblock6 = new TextBlock();
            textblock6.Text = "页";
            textblock6.VerticalAlignment = VerticalAlignment.Center;
            textblock6.Margin = new Thickness(3, 0, 3, 0);
            stackpanel.Children.Add(textblock6);

            TextBlock textblock7 = new TextBlock();
            textblock7.Text = "/";
            textblock7.VerticalAlignment = VerticalAlignment.Center;
            textblock7.Margin = new Thickness(3, 0, 3, 0);
            stackpanel.Children.Add(textblock7);

            TextBlock textblock8 = new TextBlock();
            textblock8.Text = "共";
            textblock8.VerticalAlignment = VerticalAlignment.Center;
            stackpanel.Children.Add(textblock8);

            TextBlock textblock9 = new TextBlock();
            textblock9.Text = "0";
            textblock9.Name = "textBlockTotalPage";
            textblock9.VerticalAlignment = VerticalAlignment.Center;
            stackpanel.Children.Add(textblock9);

            TextBlock textblock30 = new TextBlock();
            textblock30.Text = "页";
            textblock30.VerticalAlignment = VerticalAlignment.Center;
            textblock30.Margin = new Thickness(3, 0, 3, 0);
            stackpanel.Children.Add(textblock30);

            TextBlock textblock10 = new TextBlock();
            textblock10.Text = "转到";
            textblock10.VerticalAlignment = VerticalAlignment.Center;
            stackpanel.Children.Add(textblock10);

            TextBox textbox = new TextBox();
            textbox.Name = "textBoxPageNumber";
            textbox.TextAlignment = TextAlignment.Center;
            textbox.VerticalAlignment = VerticalAlignment.Center;
            textbox.Width = 70;
            textbox.TextChanged += new TextChangedEventHandler(textBoxPageNumber_TextChanged);
            stackpanel.Children.Add(textbox);

            TextBlock textblock20 = new TextBlock();
            textblock20.Text = "页";
            textblock20.VerticalAlignment = VerticalAlignment.Center;
            stackpanel.Children.Add(textblock20);

            Button button5 = new Button();
            button5.Name = "buttonOK";
            button5.Content = "GO";
            button5.VerticalAlignment = VerticalAlignment.Center;
            button5.Margin = new Thickness(3, 0, 3, 0);
            button5.Click += new RoutedEventHandler(buttonOK_Click);
            stackpanel.Children.Add(button5);
        }

        #endregion 添加分页控件

        private void buttonHome_Click(object sender, RoutedEventArgs e)
        {
            //int currentPage = int.Parse(textBlockPage.Text);
            //int totalPage = int.Parse(textBlockTotalPage.Text);
            if (DgDatatable != null)
            {
                //if (currentPage < totalPage)
                {
                    ComboBox comboBoxPageNumber = GetChildObject<ComboBox>(this.stackpanel, "comboBoxPageNumber");

                    Page(DgDatatable, (int)comboBoxPageNumber.SelectedValue, 1);
                }
            }
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textBlockPage = GetChildObject<TextBlock>(this.stackpanel, "textBlockPage");
            int currentPage = int.Parse(textBlockPage.Text);
            if (DgDatatable != null)
            {
                if (currentPage > 1)
                {
                    ComboBox comboBoxPageNumber = GetChildObject<ComboBox>(this.stackpanel, "comboBoxPageNumber");
                    Page(DgDatatable, (int)comboBoxPageNumber.SelectedValue, currentPage - 1);
                }
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textBlockPage = GetChildObject<TextBlock>(this.stackpanel, "textBlockPage");
            int currentPage = int.Parse(textBlockPage.Text);
            TextBlock textBlockTotalPage = GetChildObject<TextBlock>(this.stackpanel, "textBlockTotalPage");
            int totalPage = int.Parse(textBlockTotalPage.Text);
            if (DgDatatable != null)
            {
                if (currentPage < totalPage)
                {
                    ComboBox comboBoxPageNumber = GetChildObject<ComboBox>(this.stackpanel, "comboBoxPageNumber");
                    Page(DgDatatable, (int)comboBoxPageNumber.SelectedValue, currentPage + 1);
                }
            }
        }

        private void buttonEnd_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textBlockTotalPage = GetChildObject<TextBlock>(this.stackpanel, "textBlockTotalPage");
            int totalPage = int.Parse(textBlockTotalPage.Text);
            if (DgDatatable != null)
            {
                //if (currentPage < totalPage)
                {
                    ComboBox comboBoxPageNumber = GetChildObject<ComboBox>(this.stackpanel, "comboBoxPageNumber");
                    Page(DgDatatable, (int)comboBoxPageNumber.SelectedValue, totalPage);
                }
            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (DgDatatable != null)
            {
                ComboBox comboBoxPageNumber = GetChildObject<ComboBox>(this.stackpanel, "comboBoxPageNumber");
                TextBox textBoxPageNumber = GetChildObject<TextBox>(this.stackpanel, "textBoxPageNumber");
                Page(DgDatatable, (int)comboBoxPageNumber.SelectedValue, int.Parse(textBoxPageNumber.Text));
            }
        }

        /// <summary>
        /// 显示页面条数
        /// </summary>
        /// <param name="pageNumber">当前页显示的条数</param>
        /// <param name="currentPage">当前页</param>
        private DataTable Page(DataTable dt, int pageNumber, int currentPage)
        {
            DataTable dataTablePage = new DataTable();
            dataTablePage = dt.Clone();
            int total = dt.Rows.Count;

            int totalPage = 0;//总页数
            if (total % pageNumber == 0)
            {
                totalPage = total / pageNumber;
            }
            else
            {
                totalPage = total / pageNumber + 1;
            }

            int first = pageNumber * (currentPage - 1);//当前记录是多少条
            first = (first > 0) ? first : 0;
            //如果总数量大于每页显示数量  
            if (total >= pageNumber * currentPage)
            {
                for (int i = first; i < pageNumber * currentPage; i++)
                    dataTablePage.ImportRow(dt.Rows[i]);
            }
            else
            {
                for (int i = first; i < dt.Rows.Count; i++)
                    dataTablePage.ImportRow(dt.Rows[i]);
            }

            this.datagrid.ItemsSource = dataTablePage.DefaultView;
            //	tmpTable.Dispose();
            TextBlock textBlockTotal = GetChildObject<TextBlock>(this.stackpanel, "textBlockTotal");
            TextBlock textBlockTotalPage = GetChildObject<TextBlock>(this.stackpanel, "textBlockTotalPage");
            TextBlock textBlockPage = GetChildObject<TextBlock>(this.stackpanel, "textBlockPage");

            textBlockTotal.Text = total.ToString();
            textBlockTotalPage.Text = totalPage.ToString();
            textBlockPage.Text = currentPage.ToString();

            ButonStatus(currentPage, totalPage);
            return dataTablePage;
        }

        /// <summary>
        /// 按钮状态
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="totalPage">总页数</param>
        private void ButonStatus(int currentPage, int totalPage)
        {
            Button buttonHome = GetChildObject<Button>(this.stackpanel, "buttonHome");
            Button buttonUp = GetChildObject<Button>(this.stackpanel, "buttonUp");
            Button buttonEnd = GetChildObject<Button>(this.stackpanel, "buttonEnd");
            Button buttonNext = GetChildObject<Button>(this.stackpanel, "buttonNext");

            if (currentPage == 1)
            {
                buttonHome.IsEnabled = false;
                buttonUp.IsEnabled = false;
            }
            else
            {
                buttonHome.IsEnabled = true;
                buttonUp.IsEnabled = true;
            }

            if (currentPage == totalPage)
            {
                buttonEnd.IsEnabled = false;
                buttonNext.IsEnabled = false;
            }
            else
            {
                buttonEnd.IsEnabled = true;
                buttonNext.IsEnabled = true;
            }
        }

        private void comboBoxPageNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgDatatable != null)
            {
                ComboBox comboBoxPageNumber = GetChildObject<ComboBox>(this.stackpanel, "comboBoxPageNumber");
                TextBox textBoxPageNumber = GetChildObject<TextBox>(this.stackpanel, "textBoxPageNumber");
                Page(DgDatatable, (int)comboBoxPageNumber.SelectedValue, 1);
                textBoxPageNumber.Text = "";
            }
        }

        private void InitComboBox()
        {
            ComboBox comboBoxPageNumber = GetChildObject<ComboBox>(this.stackpanel, "comboBoxPageNumber");

            Dictionary<int, string> dicComboBox = new Dictionary<int, string>()
            {
                {5,"每页显示5条"},
                {10,"每页显示10条"},
                {20,"每页显示20条"},
                {50,"每页显示50条"}
            };

            comboBoxPageNumber.ItemsSource = null;
            comboBoxPageNumber.SelectedValuePath = "Key";
            comboBoxPageNumber.DisplayMemberPath = "Value";
            comboBoxPageNumber.ItemsSource = dicComboBox;

            comboBoxPageNumber.SelectedIndex = 0;
        }

        private void textBoxPageNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strNumber = string.Empty;
            TextBox textBoxPageNumber = GetChildObject<TextBox>(this.stackpanel, "textBoxPageNumber");
            TextBlock textBlockTotalPage = GetChildObject<TextBlock>(this.stackpanel, "textBlockTotalPage");

            foreach (char charText in textBoxPageNumber.Text.Trim())
            {
                int intOut = 0;
                if (Int32.TryParse(charText.ToString(), out intOut))
                {
                    strNumber = strNumber + charText.ToString();
                }
            }

            if (strNumber != string.Empty)
            {
                if (Convert.ToDecimal(strNumber) > Convert.ToDecimal(textBlockTotalPage.Text))
                {
                    strNumber = textBlockTotalPage.Text;
                }
                if (Convert.ToDecimal(strNumber) < 1)
                {
                    strNumber = "1";
                }
            }
            else
            {
                strNumber = "1";
            }

            textBoxPageNumber.Text = strNumber;
        }

        #endregion 分页
    }
}
