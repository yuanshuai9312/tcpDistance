using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace tcp检测远程端口
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int outTime = 1000;
            double speed = new TcpClientWithTimeout(textBox1.Text, Convert.ToInt16(textBox2.Text), outTime).Connect();

            bool isTrue = Convert.ToInt32(speed) < outTime ? true : false;
            if (isTrue)
            {
                MessageBox.Show("开启");
            }
            else {
                MessageBox.Show("关闭");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
