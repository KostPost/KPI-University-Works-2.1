namespace Laba3;

using System.ComponentModel;
using System.ComponentModel;
using System.Windows.Forms;

partial class MainForm
{
    private IContainer components = null;
    private TextBox txtKey;
    private TextBox txtData;
    private Button btnSearch;
    private Button btnAdd;
    private Button btnUpdate;
    private Button btnDelete;
    private Button btnGenerateData;
    private DataGridView dataGridView;
    private Label lblKey;
    private Label lblData;
    private Label lblStatus;
    private GroupBox groupBoxOperations;
    private GroupBox groupBoxData;
    private Button btnMeasure;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.txtKey = new System.Windows.Forms.TextBox();
        this.txtData = new System.Windows.Forms.TextBox();
        this.btnSearch = new System.Windows.Forms.Button();
        this.btnAdd = new System.Windows.Forms.Button();
        this.btnUpdate = new System.Windows.Forms.Button();
        this.btnDelete = new System.Windows.Forms.Button();
        this.btnGenerateData = new System.Windows.Forms.Button();
        this.dataGridView = new System.Windows.Forms.DataGridView();
        this.lblKey = new System.Windows.Forms.Label();
        this.lblData = new System.Windows.Forms.Label();
        this.lblStatus = new System.Windows.Forms.Label();
        this.groupBoxOperations = new System.Windows.Forms.GroupBox();
        this.groupBoxData = new System.Windows.Forms.GroupBox();
        this.btnMeasure = new System.Windows.Forms.Button();
        
        // 
        // Form setup
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Dense Index Database";
        
        // 
        // GroupBox Operations
        // 
        this.groupBoxOperations.Location = new System.Drawing.Point(12, 12);
        this.groupBoxOperations.Size = new System.Drawing.Size(300, 150);
        this.groupBoxOperations.Text = "Operations";
        
        // 
        // Labels
        // 
        this.lblKey.Location = new System.Drawing.Point(20, 25);
        this.lblKey.Size = new System.Drawing.Size(70, 23);
        this.lblKey.Text = "Key:";
        
        this.lblData.Location = new System.Drawing.Point(20, 55);
        this.lblData.Size = new System.Drawing.Size(70, 23);
        this.lblData.Text = "Data:";
        
        // 
        // TextBoxes
        // 
        this.txtKey.Location = new System.Drawing.Point(100, 25);
        this.txtKey.Size = new System.Drawing.Size(180, 23);
        
        this.txtData.Location = new System.Drawing.Point(100, 55);
        this.txtData.Size = new System.Drawing.Size(180, 23);
        
        // 
        // Buttons
        // 
        this.btnSearch.Location = new System.Drawing.Point(20, 90);
        this.btnSearch.Size = new System.Drawing.Size(80, 23);
        this.btnSearch.Text = "Search";
        
        this.btnAdd.Location = new System.Drawing.Point(110, 90);
        this.btnAdd.Size = new System.Drawing.Size(80, 23);
        this.btnAdd.Text = "Add";
        
        this.btnUpdate.Location = new System.Drawing.Point(200, 90);
        this.btnUpdate.Size = new System.Drawing.Size(80, 23);
        this.btnUpdate.Text = "Update";
        
        this.btnDelete.Location = new System.Drawing.Point(20, 120);
        this.btnDelete.Size = new System.Drawing.Size(80, 23);
        this.btnDelete.Text = "Delete";
        
        this.btnGenerateData.Location = new System.Drawing.Point(110, 120);
        this.btnGenerateData.Size = new System.Drawing.Size(170, 23);
        this.btnGenerateData.Text = "Generate Random Data";
        
        // 
        // DataGridView
        // 
        this.dataGridView.Location = new System.Drawing.Point(12, 170);
        this.dataGridView.Size = new System.Drawing.Size(776, 230);
        this.dataGridView.AllowUserToAddRows = false;
        this.dataGridView.AllowUserToDeleteRows = false;
        this.dataGridView.ReadOnly = true;
        this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        
        // 
        // Status Label
        // 
        this.lblStatus.Location = new System.Drawing.Point(12, 410);
        this.lblStatus.Size = new System.Drawing.Size(776, 23);
        this.lblStatus.Text = "Ready";
        
        // 
        // Measure Button
        // 
        this.btnMeasure.Location = new System.Drawing.Point(320, 12);
        this.btnMeasure.Size = new System.Drawing.Size(150, 23);
        this.btnMeasure.Text = "Measure Performance";
        
        // 
        // Add controls to form
        // 
        this.groupBoxOperations.Controls.Add(this.lblKey);
        this.groupBoxOperations.Controls.Add(this.lblData);
        this.groupBoxOperations.Controls.Add(this.txtKey);
        this.groupBoxOperations.Controls.Add(this.txtData);
        this.groupBoxOperations.Controls.Add(this.btnSearch);
        this.groupBoxOperations.Controls.Add(this.btnAdd);
        this.groupBoxOperations.Controls.Add(this.btnUpdate);
        this.groupBoxOperations.Controls.Add(this.btnDelete);
        this.groupBoxOperations.Controls.Add(this.btnGenerateData);
        
        this.Controls.Add(this.groupBoxOperations);
        this.Controls.Add(this.dataGridView);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.btnMeasure);
        
        // Wire up event handlers
        this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
        this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
        this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
        this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
        this.btnGenerateData.Click += new System.EventHandler(this.BtnGenerateData_Click);
        this.btnMeasure.Click += new System.EventHandler(this.BtnMeasure_Click);
    }
}