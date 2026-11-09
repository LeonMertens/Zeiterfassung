namespace Zeiterfassung_NEU
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            mainTable = new DataGridView();
            Check = new DataGridViewCheckBoxColumn();
            Datum = new DataGridViewTextBoxColumn();
            Anfang = new DataGridViewTextBoxColumn();
            Ende = new DataGridViewTextBoxColumn();
            btnDelete = new Button();
            lbStatus = new Label();
            tbDatum = new TextBox();
            tbAnfang = new TextBox();
            tbEnde = new TextBox();
            btnSave = new Button();
            label1 = new Label();
            lbRowNr = new Label();
            lbAnkunft = new Label();
            lbEnde = new Label();
            trayMenu = new ContextMenuStrip(components);
            menuZeitSetzen = new ToolStripMenuItem();
            menuGehen = new ToolStripMenuItem();
            menuEinstellungen = new ToolStripMenuItem();
            menuOeffnen = new ToolStripMenuItem();
            menuBeenden = new ToolStripMenuItem();
            trayIcon = new NotifyIcon(components);
            btnSetTime = new Button();
            btnSettings = new Button();
            lbSeitenInfo = new Label();
            btnSeiteZurueck = new Button();
            btnSeiteWeiter = new Button();
            ((System.ComponentModel.ISupportInitialize)mainTable).BeginInit();
            trayMenu.SuspendLayout();
            SuspendLayout();
            // 
            // mainTable
            // 
            mainTable.AllowUserToAddRows = false;
            mainTable.AllowUserToDeleteRows = false;
            mainTable.AllowUserToOrderColumns = true;
            mainTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            mainTable.Columns.AddRange(new DataGridViewColumn[] { Check, Datum, Anfang, Ende });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            mainTable.DefaultCellStyle = dataGridViewCellStyle1;
            mainTable.Location = new Point(12, 42);
            mainTable.Name = "mainTable";
            mainTable.ReadOnly = true;
            mainTable.RowHeadersVisible = false;
            mainTable.Size = new Size(403, 488);
            mainTable.TabIndex = 0;
            mainTable.CellClick += mainTable_CellClick;
            mainTable.CellMouseUp += mainTable_CellMouseUp;
            // 
            // Check
            // 
            Check.Frozen = true;
            Check.HeaderText = "Check";
            Check.MinimumWidth = 25;
            Check.Name = "Check";
            Check.ReadOnly = true;
            Check.Width = 50;
            // 
            // Datum
            // 
            Datum.DataPropertyName = "Datum";
            Datum.Frozen = true;
            Datum.HeaderText = "Datum";
            Datum.MinimumWidth = 100;
            Datum.Name = "Datum";
            Datum.ReadOnly = true;
            Datum.Width = 150;
            // 
            // Anfang
            // 
            Anfang.DataPropertyName = "Ankunft";
            Anfang.Frozen = true;
            Anfang.HeaderText = "Anfang";
            Anfang.MinimumWidth = 50;
            Anfang.Name = "Anfang";
            Anfang.ReadOnly = true;
            // 
            // Ende
            // 
            Ende.DataPropertyName = "Ende";
            Ende.Frozen = true;
            Ende.HeaderText = "Ende";
            Ende.MinimumWidth = 50;
            Ende.Name = "Ende";
            Ende.ReadOnly = true;
            // 
            // btnDelete
            // 
            btnDelete.FlatAppearance.MouseDownBackColor = Color.IndianRed;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.IndianRed;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btnDelete.Location = new Point(421, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(155, 50);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Löschen";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += bntDelete_Click;
            // 
            // lbStatus
            // 
            lbStatus.AutoSize = true;
            lbStatus.Location = new Point(12, 533);
            lbStatus.Name = "lbStatus";
            lbStatus.Size = new Size(42, 15);
            lbStatus.TabIndex = 3;
            lbStatus.Text = "";
            // 
            // tbDatum
            // 
            tbDatum.Font = new Font("Segoe UI", 15.75F);
            tbDatum.Location = new Point(421, 299);
            tbDatum.Name = "tbDatum";
            tbDatum.ReadOnly = true;
            tbDatum.Size = new Size(201, 35);
            tbDatum.TabIndex = 4;
            tbDatum.Text = "Datum";
            tbDatum.Visible = false;
            // 
            // tbAnfang
            // 
            tbAnfang.Font = new Font("Segoe UI", 15.75F);
            tbAnfang.Location = new Point(522, 348);
            tbAnfang.Name = "tbAnfang";
            tbAnfang.Size = new Size(100, 35);
            tbAnfang.TabIndex = 5;
            tbAnfang.Visible = false;
            // 
            // tbEnde
            // 
            tbEnde.Font = new Font("Segoe UI", 15.75F);
            tbEnde.Location = new Point(522, 393);
            tbEnde.Name = "tbEnde";
            tbEnde.Size = new Size(100, 35);
            tbEnde.TabIndex = 6;
            tbEnde.Visible = false;
            // 
            // btnSave
            // 
            btnSave.FlatAppearance.MouseDownBackColor = Color.LightGreen;
            btnSave.FlatAppearance.MouseOverBackColor = Color.LightGreen;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(421, 463);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(126, 67);
            btnSave.TabIndex = 8;
            btnSave.Text = "Speichern";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Visible = false;
            btnSave.Click += btnSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(421, 271);
            label1.Name = "label1";
            label1.Size = new Size(46, 21);
            label1.TabIndex = 9;
            label1.Text = "Zeile:";
            label1.Visible = false;
            // 
            // lbRowNr
            // 
            lbRowNr.AutoSize = true;
            lbRowNr.Font = new Font("Segoe UI", 12F);
            lbRowNr.Location = new Point(473, 271);
            lbRowNr.Name = "lbRowNr";
            lbRowNr.Size = new Size(0, 21);
            lbRowNr.TabIndex = 10;
            lbRowNr.Visible = false;
            // 
            // lbAnkunft
            // 
            lbAnkunft.AutoSize = true;
            lbAnkunft.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            lbAnkunft.Location = new Point(421, 348);
            lbAnkunft.Name = "lbAnkunft";
            lbAnkunft.Size = new Size(95, 30);
            lbAnkunft.TabIndex = 11;
            lbAnkunft.Text = "Ankunft";
            lbAnkunft.Visible = false;
            // 
            // lbEnde
            // 
            lbEnde.AutoSize = true;
            lbEnde.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            lbEnde.Location = new Point(421, 393);
            lbEnde.Name = "lbEnde";
            lbEnde.Size = new Size(61, 30);
            lbEnde.TabIndex = 12;
            lbEnde.Text = "Ende";
            lbEnde.Visible = false;
            // 
            // trayMenu
            // 
            trayMenu.Items.AddRange(new ToolStripItem[] { menuZeitSetzen, menuGehen, menuEinstellungen, menuOeffnen, menuBeenden });
            trayMenu.Name = "trayMenu";
            trayMenu.Size = new Size(146, 114);
            // 
            // menuZeitSetzen
            // 
            menuZeitSetzen.Name = "menuZeitSetzen";
            menuZeitSetzen.Size = new Size(145, 22);
            menuZeitSetzen.Text = "Zeit setzen";
            menuZeitSetzen.Click += menuZeitSetzen_Click;
            // 
            // menuGehen
            // 
            menuGehen.Name = "menuGehen";
            menuGehen.Size = new Size(145, 22);
            menuGehen.Text = "Gehen";
            menuGehen.Click += menuGehen_Click;
            // 
            // menuEinstellungen
            // 
            menuEinstellungen.Name = "menuEinstellungen";
            menuEinstellungen.Size = new Size(145, 22);
            menuEinstellungen.Text = "Einstellungen";
            menuEinstellungen.Click += menuEinstellungen_Click;
            // 
            // menuOeffnen
            // 
            menuOeffnen.Name = "menuOeffnen";
            menuOeffnen.Size = new Size(145, 22);
            menuOeffnen.Text = "Öffnen";
            menuOeffnen.Click += menuOeffnen_Click;
            // 
            // menuBeenden
            // 
            menuBeenden.Name = "menuBeenden";
            menuBeenden.Size = new Size(145, 22);
            menuBeenden.Text = "Beenden";
            menuBeenden.Click += menuBeenden_Click;
            // 
            // trayIcon
            // 
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Text = "Zeiterfassung";
            trayIcon.Visible = true;
            trayIcon.DoubleClick += trayIcon_DoubleClick;
            // 
            // btnSetTime
            // 
            btnSetTime.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btnSetTime.Location = new Point(421, 68);
            btnSetTime.Name = "btnSetTime";
            btnSetTime.Size = new Size(155, 50);
            btnSetTime.TabIndex = 13;
            btnSetTime.Text = "Zeit Setzen";
            btnSetTime.UseVisualStyleBackColor = true;
            btnSetTime.Click += menuZeitSetzen_Click;
            // 
            // btnSettings
            // 
            btnSettings.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btnSettings.Location = new Point(421, 124);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(155, 50);
            btnSettings.TabIndex = 14;
            btnSettings.Text = "Einstellungen";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += menuEinstellungen_Click;
            // 
            // lbSeitenInfo
            // 
            lbSeitenInfo.AutoSize = true;
            lbSeitenInfo.Font = new Font("Segoe UI", 9F);
            lbSeitenInfo.Location = new Point(12, 17);
            lbSeitenInfo.Name = "lbSeitenInfo";
            lbSeitenInfo.Size = new Size(100, 15);
            lbSeitenInfo.TabIndex = 15;
            lbSeitenInfo.Text = "Zeige 1-50 von 50";
            // 
            // btnSeiteZurueck
            // 
            btnSeiteZurueck.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSeiteZurueck.Location = new Point(325, 10);
            btnSeiteZurueck.Name = "btnSeiteZurueck";
            btnSeiteZurueck.Size = new Size(40, 25);
            btnSeiteZurueck.TabIndex = 16;
            btnSeiteZurueck.Text = "<";
            btnSeiteZurueck.UseVisualStyleBackColor = true;
            btnSeiteZurueck.Click += btnSeiteZurueck_Click;
            // 
            // btnSeiteWeiter
            // 
            btnSeiteWeiter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSeiteWeiter.Location = new Point(375, 10);
            btnSeiteWeiter.Name = "btnSeiteWeiter";
            btnSeiteWeiter.Size = new Size(40, 25);
            btnSeiteWeiter.TabIndex = 17;
            btnSeiteWeiter.Text = ">";
            btnSeiteWeiter.UseVisualStyleBackColor = true;
            btnSeiteWeiter.Click += btnSeiteWeiter_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(634, 556);
            Controls.Add(btnSeiteWeiter);
            Controls.Add(btnSeiteZurueck);
            Controls.Add(lbSeitenInfo);
            Controls.Add(btnSettings);
            Controls.Add(btnSetTime);
            Controls.Add(lbEnde);
            Controls.Add(lbAnkunft);
            Controls.Add(lbRowNr);
            Controls.Add(label1);
            Controls.Add(btnSave);
            Controls.Add(tbEnde);
            Controls.Add(tbAnfang);
            Controls.Add(tbDatum);
            Controls.Add(lbStatus);
            Controls.Add(btnDelete);
            Controls.Add(mainTable);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Zeiterfassung";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)mainTable).EndInit();
            trayMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView mainTable;
        private Button btnDelete;
        private Label lbStatus;
        private DataGridViewCheckBoxColumn Check;
        private DataGridViewTextBoxColumn Datum;
        private DataGridViewTextBoxColumn Anfang;
        private DataGridViewTextBoxColumn Ende;
        private TextBox tbDatum;
        private TextBox tbAnfang;
        private TextBox tbEnde;
        private Button btnSave;
        private Label label1;
        private Label lbRowNr;
        private Label lbAnkunft;
        private Label lbEnde;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem menuZeitSetzen;
        private ToolStripMenuItem menuGehen;
        private ToolStripMenuItem menuEinstellungen;
        private ToolStripMenuItem menuOeffnen;
        private ToolStripMenuItem menuBeenden;
        private NotifyIcon trayIcon;
        private Button btnSetTime;
        private Button btnSettings;
        private Label lbSeitenInfo;
        private Button btnSeiteZurueck;
        private Button btnSeiteWeiter;
    }
}