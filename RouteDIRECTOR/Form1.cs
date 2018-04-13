using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RouteDIRECTOR.RouteDirectorControl;

namespace RouteDIRECTOR
{
	
	public partial class Form1 : Form
	{
		public RouteDirectControl routeDirect = new RouteDirectControl();
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void btnStartConnect_Click(object sender, EventArgs e)
		{
			int res = routeDirect.EstablishConnection(txtIp.Text, txtPort.Text);
			if (res == 0)
				lblConnectStatus.Text = "连接成功";
			else
				lblConnectStatus.Text = "连接失败";
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			routeDirect.StopConnection();
			lblConnectStatus.Text = "已断开连接";
		}
	}
}
