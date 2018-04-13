namespace WinFormSort
{
    partial class MainForm
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
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.lbconstatus = new System.Windows.Forms.Label();
			this.txtHeartMsg = new System.Windows.Forms.RichTextBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.txtHost = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblmsg = new System.Windows.Forms.Label();
			this.rtxDivertMsg = new System.Windows.Forms.RichTextBox();
			this.btnDataAnalyze = new System.Windows.Forms.Button();
			this.btnStartHeart = new System.Windows.Forms.Button();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Transparent;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.btnDisconnect);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.lbconstatus);
			this.panel2.Controls.Add(this.txtHeartMsg);
			this.panel2.Controls.Add(this.btnConnect);
			this.panel2.Controls.Add(this.txtPort);
			this.panel2.Controls.Add(this.txtHost);
			this.panel2.Location = new System.Drawing.Point(21, 28);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(526, 468);
			this.panel2.TabIndex = 22;
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Location = new System.Drawing.Point(341, 19);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
			this.btnDisconnect.TabIndex = 25;
			this.btnDisconnect.Text = "断开";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(17, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 12);
			this.label2.TabIndex = 24;
			this.label2.Text = "心跳消息:";
			// 
			// lbconstatus
			// 
			this.lbconstatus.AutoSize = true;
			this.lbconstatus.ForeColor = System.Drawing.Color.Red;
			this.lbconstatus.Location = new System.Drawing.Point(422, 26);
			this.lbconstatus.Name = "lbconstatus";
			this.lbconstatus.Size = new System.Drawing.Size(71, 12);
			this.lbconstatus.TabIndex = 23;
			this.lbconstatus.Text = "连接状态...";
			// 
			// txtHeartMsg
			// 
			this.txtHeartMsg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtHeartMsg.Location = new System.Drawing.Point(19, 98);
			this.txtHeartMsg.Name = "txtHeartMsg";
			this.txtHeartMsg.Size = new System.Drawing.Size(480, 353);
			this.txtHeartMsg.TabIndex = 20;
			this.txtHeartMsg.Text = "";
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(260, 19);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 23);
			this.btnConnect.TabIndex = 19;
			this.btnConnect.Text = "连接";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(154, 19);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(50, 21);
			this.txtPort.TabIndex = 18;
			this.txtPort.Text = "3000";
			// 
			// txtHost
			// 
			this.txtHost.Location = new System.Drawing.Point(19, 19);
			this.txtHost.Name = "txtHost";
			this.txtHost.Size = new System.Drawing.Size(100, 21);
			this.txtHost.TabIndex = 17;
			this.txtHost.Text = "172.16.18.171";
			this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lblmsg);
			this.panel1.Controls.Add(this.rtxDivertMsg);
			this.panel1.Location = new System.Drawing.Point(572, 28);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(412, 468);
			this.panel1.TabIndex = 21;
			// 
			// lblmsg
			// 
			this.lblmsg.AutoSize = true;
			this.lblmsg.Location = new System.Drawing.Point(13, 30);
			this.lblmsg.Name = "lblmsg";
			this.lblmsg.Size = new System.Drawing.Size(65, 12);
			this.lblmsg.TabIndex = 25;
			this.lblmsg.Text = "分拣信息：";
			// 
			// rtxDivertMsg
			// 
			this.rtxDivertMsg.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.rtxDivertMsg.Location = new System.Drawing.Point(15, 61);
			this.rtxDivertMsg.Name = "rtxDivertMsg";
			this.rtxDivertMsg.Size = new System.Drawing.Size(380, 390);
			this.rtxDivertMsg.TabIndex = 24;
			this.rtxDivertMsg.Text = "";
			// 
			// btnDataAnalyze
			// 
			this.btnDataAnalyze.Location = new System.Drawing.Point(588, 502);
			this.btnDataAnalyze.Name = "btnDataAnalyze";
			this.btnDataAnalyze.Size = new System.Drawing.Size(90, 23);
			this.btnDataAnalyze.TabIndex = 27;
			this.btnDataAnalyze.Text = "测试数据解析";
			this.btnDataAnalyze.UseVisualStyleBackColor = true;
			this.btnDataAnalyze.Click += new System.EventHandler(this.btnDataAnalyze_Click);
			// 
			// btnStartHeart
			// 
			this.btnStartHeart.Location = new System.Drawing.Point(701, 502);
			this.btnStartHeart.Name = "btnStartHeart";
			this.btnStartHeart.Size = new System.Drawing.Size(90, 23);
			this.btnStartHeart.TabIndex = 28;
			this.btnStartHeart.Text = "手动启动心跳";
			this.btnStartHeart.UseVisualStyleBackColor = true;
			this.btnStartHeart.Click += new System.EventHandler(this.btnheart_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1013, 532);
			this.Controls.Add(this.btnStartHeart);
			this.Controls.Add(this.btnDataAnalyze);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbconstatus;
        public System.Windows.Forms.RichTextBox txtHeartMsg;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.RichTextBox rtxDivertMsg;
        private System.Windows.Forms.Label lblmsg;
        private System.Windows.Forms.Button btnDataAnalyze;
        private System.Windows.Forms.Button btnStartHeart;
    }
}

