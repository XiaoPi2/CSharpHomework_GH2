﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderManagement;

namespace ch7Homework2_GH_week8
{
    public partial class MainForm : Form
    {
        public string SearchBy { get; set; }
        public string SearchValue { get; set; }
        public MainForm()
        {
            InitializeComponent();
            // 初始化
            Product product1 = new Product(1, "电脑", 100);
            Product product2 = new Product(2, "手机", 50);
            Client client1 = new Client(1, "张三");
            Client client2 = new Client(2, "李四");
            OrderItem newOrderItem1 = new OrderItem(1, 20, product1);
            OrderItem newOrderItem2 = new OrderItem(2, 50, product2);
            OrderItem newOrderItem3 = new OrderItem(3, 20, product2);
            OrderItem newOrderItem4 = new OrderItem(4, 15, product1);
            List<OrderItem> orderItems1 = new List<OrderItem>();
            List<OrderItem> orderItems2 = new List<OrderItem>();
            List<OrderItem> orderItems3 = new List<OrderItem>();
            List<OrderItem> orderItems4 = new List<OrderItem>();
            orderItems1.Add(newOrderItem1);            orderItems1.Add(newOrderItem2);
            orderItems2.Add(newOrderItem2);            orderItems2.Add(newOrderItem3);
            orderItems3.Add(newOrderItem3);            orderItems3.Add(newOrderItem4);
            orderItems4.Add(newOrderItem4);            orderItems4.Add(newOrderItem1);

            Order order1 = new Order(1, client1, orderItems1);
            Order order2 = new Order(2, client1, orderItems2);
            Order order3 = new Order(3, client2, orderItems3);
            Order order4 = new Order(4, client1, orderItems4);

            //测试创建功能
            OrderService.CreateOrder(order1);
            OrderService.CreateOrder(order3);
            OrderService.CreateOrder(order2);
            OrderService.CreateOrder(order2);
            OrderService.CreateOrder(order4);

            orderBindingSource.DataSource = OrderService.OrderList;

        }

   

        private void orderBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            List<Order> selectedList = new List<Order>();
            IEnumerable<Order> queryEnum = null;

            bool query = OrderService.QueryOrder(out queryEnum, SearchBy, SearchValue);
            if (query)
            {
                selectedList = queryEnum.ToList();
                orderBindingSource.DataSource = selectedList;
                orderBindingSource.ResetBindings(false);
                orderItemsBindingSource.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("查询失败，请重试");
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            orderBindingSource.DataSource = OrderService.OrderList;
            orderBindingSource.ResetBindings(false);
            orderItemsBindingSource.ResetBindings(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SelectedComboBox.DataBindings.Add("SelectedItem", this, "SearchBy");
            SearchText.DataBindings.Add("Text", this, "SearchValue");

        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            //            List<Order> newOrderList = new List<Order>(OrderService.OrderList.ToArray());
            List<Order> newOrderList = OrderService.OrderList;
            try
            {
                //orderBindingSource.DataSource = newOrderList;
                int index = dataGridView2.CurrentRow.Index;
                newOrderList.RemoveAt(index);
                orderBindingSource.ResetBindings(false);
            }
            catch
            {
                MessageBox.Show("删除错误，请检查后重试");
            }
        }

        private void AddandUpdateButton_Click(object sender, EventArgs e)
        {
            int index1 = dataGridView2.CurrentRow.Index;
            int index2 = dataGridView1.CurrentRow.Index;
            AddandUpdateForm addandUpdateForm = new AddandUpdateForm(index1,index2);
            addandUpdateForm.Show();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export";
            saveFileDialog.Filter = "XML File|*.xml";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = System.IO.Path.GetFullPath(saveFileDialog.FileName);
                OrderService.Export(path);
                MessageBox.Show("导出成功!");
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import";
            openFileDialog.Filter = "XML File|*.xml";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = System.IO.Path.GetFullPath(openFileDialog.FileName);
                OrderService.Import(path);
                orderBindingSource.ResetBindings(false);
                orderItemsBindingSource.ResetBindings(false);
            }
        }
    }
}
