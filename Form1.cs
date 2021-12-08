using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_menu
{
	public partial class Form1 : Form
	{
		byte[] message = new byte[17];
		byte[] start = new byte[1];
		int offset = 0;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
			if (comboBox1.Items.Count == 0)
			{
				comboBox1.Text = "No COM ports!";
			}
			else
			{
				comboBox1.SelectedIndex = 0;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{

			if (serialPort1.IsOpen)
			{
				serialPort1.Close();
				button_connectSerial.Text = "Connect Serial";
			}
			else
			{
				serialPort1.PortName = comboBox1.Text;
				serialPort1.Open();
				button_connectSerial.Text = "Disconnect Serial";
			}
		}
		private void button5_Click(object sender, EventArgs e)
		{
			message[0] = Convert.ToByte(textBox3.Text);
			message[1] = Convert.ToByte(textBox4.Text);
			message[2] = Convert.ToByte(textBox5.Text);

			serialPort1.Write(message, 0, 3);

			textBox1.AppendText(message[0].ToString() + ',' + message[1].ToString()
				+ ',' + message[2].ToString() + "\r\n");

		}

        private void button1_Click_1(object sender, EventArgs e)
        {
			start[0] = 200; //start value
			serialPort1.Write(start ,0, 1); //start moving
			textBox1.Clear();
        }
    }
}
