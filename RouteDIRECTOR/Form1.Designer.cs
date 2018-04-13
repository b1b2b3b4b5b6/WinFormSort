namespace RouteDIRECTOR
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStartConnect = new System.Windows.Forms.Button();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.txtIp = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblConnectStatus = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnStartConnect
			// 
			this.btnStartConnect.Location = new System.Drawing.Point(247, 212);
			this.btnStartConnect.Name = "btnStartConnect";
			this.btnStartConnect.Size = new System.Drawing.Size(75, 23);
			this.btnStartConnect.TabIndex = 0;
			this.btnStartConnect.Text = "连接";
			this.btnStartConnect.UseVisualStyleBackColor = true;
			this.btnStartConnect.Click += new System.EventHandler(this.btnStartConnect_Click);
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Location = new System.Drawing.Point(349, 212);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
			this.btnDisconnect.TabIndex = 1;
			this.btnDisconnect.Text = "断开连接";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// txtIp
			// 
			this.txtIp.Location = new System.Drawing.Point(247, 120);
			this.txtIp.Name = "txtIp";
			this.txtIp.Size = new System.Drawing.Size(177, 21);
			this.txtIp.TabIndex = 2;
			this.txtIp.Text = "172.16.18.171";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(247, 160);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(177, 21);
			this.txtPort.TabIndex = 3;
			this.txtPort.Text = "3000";
			// 
			// lblConnectStatus
			// 
			this.lblConnectStatus.AutoSize = true;
			this.lblConnectStatus.Location = new System.Drawing.Point(453, 222);
			this.lblConnectStatus.Name = "lblConnectStatus";
			this.lblConnectStatus.Size = new System.Drawing.Size(41, 12);
			this.lblConnectStatus.TabIndex = 4;
			this.lblConnectStatus.Text = "label1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.lblConnectStatus);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.txtIp);
			this.Controls.Add(this.btnDisconnect);
			this.Controls.Add(this.btnStartConnect);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStartConnect;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.TextBox txtIp;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label lblConnectStatus;
	}
}

