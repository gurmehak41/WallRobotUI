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
		int xHome;
		int yHome;
		byte[,] trajectory;

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

		private void button6_Click(object sender, EventArgs e)
		{
			xHome = Convert.ToInt32(textBoxXHome.Text);
			yHome = Convert.ToInt32(textBoxYHome.Text);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			int[,] trajectoryMECH = new int[,]
				{
					{0,-20,-10},
					{1,-18,10},
					{1,-16,6 },
					{1,-14,10 },
					{1,-12,-10 },
					{0,-4,-10 },
					{1,-8,-10 },
					{1,-8,10 },
					{1,-4,10 },
					{0,-4,0 },
					{1,-8,0 },
					{0,8,-10 },
					{1,4,-10 },
					{1,4,10 },
					{1,8,10 },
					{0,16,-10 },
					{1,16,10 },
					{0,12,-10 },
					{1,12,10 },
					{0,16,0 },
					{1,12,0 },
					{0,0,-20 }
				};

			int numCommands = trajectoryMECH.GetLength(0);
			bool trajLoaded;

			try
			{
				//Convert from relative to absolute coordinates
				for (int i = 0; i < numCommands; i++)
				{
					trajectory[i, 0] = Convert.ToByte(trajectoryMECH[i, 0]);
					trajectory[i, 1] = Convert.ToByte(xHome + trajectoryMECH[i, 1]);
					trajectory[i, 2] = Convert.ToByte(yHome + trajectoryMECH[i, 2]);
				}

				trajLoaded = loadTrajectory(trajectory);

				if (!trajLoaded)
					throw new Exception();
			}
			catch {
				MessageBox.Show("Trajectory not loaded. Please enter valid home coordinates");
			}
		}

		public bool loadTrajectory(byte[,] traj)
		{
			if (traj.GetLength(1) != 3)
				return false;

			int numCommands = traj.GetLength(0);

			for (int i = 0; i < numCommands; i++)
			{
				message[0] = traj[i,0];
				message[1] = traj[i,1];
				message[2] = traj[i,2];

				if (serialPort1.IsOpen)
					serialPort1.Write(message, 0, 3);
				else
					return false;

				textBox1.AppendText(message[0].ToString() + ',' + message[1].ToString()
					+ ',' + message[2].ToString() + "\r\n");
			}
			
			return true;
		}
	}
}
