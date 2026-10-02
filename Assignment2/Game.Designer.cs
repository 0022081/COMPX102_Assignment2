namespace Assignment2
{
    partial class Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxBoard = new System.Windows.Forms.PictureBox();
            this.pictureBoxPlayer1Hand = new System.Windows.Forms.PictureBox();
            this.pictureBoxPlayer2Hand = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.player1label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTurn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer1Hand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer2Hand)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxBoard
            // 
            this.pictureBoxBoard.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBoxBoard.Location = new System.Drawing.Point(13, 61);
            this.pictureBoxBoard.Name = "pictureBoxBoard";
            this.pictureBoxBoard.Size = new System.Drawing.Size(1421, 719);
            this.pictureBoxBoard.TabIndex = 0;
            this.pictureBoxBoard.TabStop = false;
            // 
            // pictureBoxPlayer1Hand
            // 
            this.pictureBoxPlayer1Hand.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBoxPlayer1Hand.Location = new System.Drawing.Point(16, 814);
            this.pictureBoxPlayer1Hand.Name = "pictureBoxPlayer1Hand";
            this.pictureBoxPlayer1Hand.Size = new System.Drawing.Size(425, 140);
            this.pictureBoxPlayer1Hand.TabIndex = 1;
            this.pictureBoxPlayer1Hand.TabStop = false;
            // 
            // pictureBoxPlayer2Hand
            // 
            this.pictureBoxPlayer2Hand.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBoxPlayer2Hand.Location = new System.Drawing.Point(804, 814);
            this.pictureBoxPlayer2Hand.Name = "pictureBoxPlayer2Hand";
            this.pictureBoxPlayer2Hand.Size = new System.Drawing.Size(425, 140);
            this.pictureBoxPlayer2Hand.TabIndex = 2;
            this.pictureBoxPlayer2Hand.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1447, 33);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(73, 29);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(141, 34);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // player1label
            // 
            this.player1label.AutoSize = true;
            this.player1label.Location = new System.Drawing.Point(16, 787);
            this.player1label.Name = "player1label";
            this.player1label.Size = new System.Drawing.Size(69, 20);
            this.player1label.TabIndex = 4;
            this.player1label.Text = "Player 1:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(800, 787);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Player 2:";
            // 
            // lblTurn
            // 
            this.lblTurn.AutoSize = true;
            this.lblTurn.Location = new System.Drawing.Point(556, 787);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(99, 40);
            this.lblTurn.TabIndex = 6;
            this.lblTurn.Text = "Player\'s Turn\r\n\r\n";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1447, 958);
            this.Controls.Add(this.lblTurn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.player1label);
            this.Controls.Add(this.pictureBoxPlayer2Hand);
            this.Controls.Add(this.pictureBoxPlayer1Hand);
            this.Controls.Add(this.pictureBoxBoard);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Game";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer1Hand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer2Hand)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxBoard;
        private System.Windows.Forms.PictureBox pictureBoxPlayer1Hand;
        private System.Windows.Forms.PictureBox pictureBoxPlayer2Hand;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label player1label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTurn;
    }
}

