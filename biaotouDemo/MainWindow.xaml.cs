﻿using System;
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

namespace biaotouDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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

        public static DependencyProperty ColumnProperty = Grid.ColumnProperty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InsertData();
            AddSecondbiaotou();
            AddFirstbiaotou();
        }

        private void AddSecondbiaotou()
        {
            RowDefinition rd1 = new RowDefinition();
            GridLength heigth1 = new GridLength(grid.ActualHeight / 2);
            rd1.Height = heigth1;
            grid.RowDefinitions.Add(rd1);

            RowDefinition rd2 = new RowDefinition();
            GridLength heigth2 = new GridLength(1, GridUnitType.Star);
            rd2.Height = heigth2;
            grid.RowDefinitions.Add(rd2);

            int rowsCount = Bt2Datatable.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                int no = Convert.ToInt32(Bt2Datatable.Rows[i]["No"].ToString().Trim());
                string str = Bt2Datatable.Rows[i]["Name"].ToString().Trim();
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
                textblock.Margin = new Thickness(10);
                textblock.VerticalAlignment = VerticalAlignment.Center;
                textblock.HorizontalAlignment = HorizontalAlignment.Center;

                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = Brushes.Black;
                border.Name = "border" + str + i.ToString();
                border.Tag = no;
                border.Child = textblock;

                grid.Children.Add(border);
                border.SetValue(Grid.RowProperty, 1);
                border.SetValue(Grid.ColumnProperty, i);
            }
        }
        public static int intTotal = 0;
        private void AddFirstbiaotou()
        {
            int intColumnSpan = 0;
            int rowsCount = Bt1Datatable.Rows.Count;
            bool boolIsAdd = false;
            for (int i = 0; i < rowsCount; i++)
            {
                int no = Convert.ToInt32(Bt1Datatable.Rows[i]["No"].ToString().Trim());
                string str = Bt1Datatable.Rows[i]["Name"].ToString().Trim();
                TextBlock textblock = new TextBlock();
                textblock.Tag = no;
                textblock.Text = str;
                textblock.Margin = new Thickness(10);
                textblock.VerticalAlignment = VerticalAlignment.Center;
                textblock.HorizontalAlignment = HorizontalAlignment.Center;
                textblock.TextWrapping = TextWrapping.Wrap;
                

                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = Brushes.Black;
                border.Child = textblock;

                for(int j = 0; j < Bt2Datatable.Rows.Count; j++)
                {
                    string strName = Bt2Datatable.Rows[j]["Name"].ToString().Trim();
                    string strBorderName = "border" + strName + j.ToString();
                    Border element = GetChildObject<Border>(this.grid, strBorderName);
                    if (Convert.ToInt32(element.Tag) == no)
                    {
                        intColumnSpan++;
                    }
                }

                grid.Children.Add(border);

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
                else /*if (intColumnSpan != 1 && boolIsAdd == true)*/
                {
                    border.SetValue(Grid.RowProperty, 0);
                    border.SetValue(Grid.ColumnProperty, intTotal);
                    border.SetValue(Grid.ColumnSpanProperty, intColumnSpan);
                    intTotal = intTotal + intColumnSpan;
                }

                intColumnSpan = 0;
            }
        }

        #region 增加数据

        private void InsertData()
        {
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

            //----------------------------------------------

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
        }

        #endregion 增加数据


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

    }
}