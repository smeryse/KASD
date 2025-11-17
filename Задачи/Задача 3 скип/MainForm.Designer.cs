//using System.Windows.Forms;

//partial class MainForm
//{
//    private System.ComponentModel.IContainer components = null;
//    private ComboBox comboAlgorithms;
//    private NumericUpDown numericSize;
//    private Button btnRun;
//    private ListBox listResults;
//    private Label lblAlg;
//    private Label lblSize;

//    protected override void Dispose(bool disposing)
//    {
//        if (disposing && (components != null))
//        {
//            components.Dispose();
//        }
//        base.Dispose(disposing);
//    }

//    private void InitializeComponent()
//    {
//        this.comboAlgorithms = new ComboBox();
//        this.numericSize = new NumericUpDown();
//        this.btnRun = new Button();
//        this.listResults = new ListBox();
//        this.lblAlg = new Label();
//        this.lblSize = new Label();
//        ((System.ComponentModel.ISupportInitialize)(this.numericSize)).BeginInit();
//        this.SuspendLayout();

//        // comboAlgorithms
//        this.comboAlgorithms.DropDownStyle = ComboBoxStyle.DropDownList;
//        this.comboAlgorithms.Items.AddRange(new object[] {
//        "Bubble Sort",
//        "Insertion Sort",
//        "Quick Sort"});
//        this.comboAlgorithms.Location = new System.Drawing.Point(20, 45);
//        this.comboAlgorithms.Name = "comboAlgorithms";
//        this.comboAlgorithms.Size = new System.Drawing.Size(180, 23);

//        // numericSize
//        this.numericSize.Location = new System.Drawing.Point(220, 45);
//        this.numericSize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
//        this.numericSize.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
//        this.numericSize.Value = 10000;

//        // btnRun
//        this.btnRun.Location = new System.Drawing.Point(350, 45);
//        this.btnRun.Name = "btnRun";
//        this.btnRun.Size = new System.Drawing.Size(120, 23);
//        this.btnRun.Text = "Сравнить";
//        this.btnRun.Click += new System.EventHandler(this.btnRun_Click);

//        // listResults
//        this.listResults.Location = new System.Drawing.Point(20, 90);
//        this.listResults.Size = new System.Drawing.Size(450, 260);

//        // labels
//        this.lblAlg.Text = "Алгоритм:";
//        this.lblAlg.Location = new System.Drawing.Point(20, 20);

//        this.lblSize.Text = "Размер:";
//        this.lblSize.Location = new System.Drawing.Point(220, 20);

//        // Form1
//        this.ClientSize = new System.Drawing.Size(500, 370);
//        this.Controls.Add(this.comboAlgorithms);
//        this.Controls.Add(this.numericSize);
//        this.Controls.Add(this.btnRun);
//        this.Controls.Add(this.listResults);
//        this.Controls.Add(this.lblAlg);
//        this.Controls.Add(this.lblSize);
//        this.Text = "Sorting Comparison";
//        ((System.ComponentModel.ISupportInitialize)(this.numericSize)).EndInit();
//        this.ResumeLayout(false);
//    }
//}