
namespace Kitchen
{
    partial class KitcheForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbOrders = new System.Windows.Forms.GroupBox();
            this.lstW_Orders = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.gbOrders.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOrders
            // 
            this.gbOrders.Controls.Add(this.lstW_Orders);
            this.gbOrders.Location = new System.Drawing.Point(30, 29);
            this.gbOrders.Name = "gbOrders";
            this.gbOrders.Size = new System.Drawing.Size(1151, 529);
            this.gbOrders.TabIndex = 0;
            this.gbOrders.TabStop = false;
            this.gbOrders.Text = "Orders";
            // 
            // lstW_Orders
            // 
            this.lstW_Orders.GridLines = true;
            this.lstW_Orders.HideSelection = false;
            this.lstW_Orders.Location = new System.Drawing.Point(15, 21);
            this.lstW_Orders.Name = "lstW_Orders";
            this.lstW_Orders.Size = new System.Drawing.Size(1113, 491);
            this.lstW_Orders.TabIndex = 0;
            this.lstW_Orders.UseCompatibleStateImageBehavior = false;
            this.lstW_Orders.View = System.Windows.Forms.View.Details;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkGreen;
            this.button1.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.DarkSalmon;
            this.button1.Location = new System.Drawing.Point(319, 579);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(221, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "is Ready";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkGreen;
            this.button2.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.DarkSalmon;
            this.button2.Location = new System.Drawing.Point(619, 579);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(221, 34);
            this.button2.TabIndex = 2;
            this.button2.Text = "All is Ready";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // KitcheForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(1221, 771);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbOrders);
            this.Name = "KitcheForm";
            this.Text = "Kitchen";
            this.gbOrders.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOrders;
        private System.Windows.Forms.ListView lstW_Orders;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

