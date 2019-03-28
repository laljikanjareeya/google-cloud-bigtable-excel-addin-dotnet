namespace GoogleBigTableAddIn
{
    partial class CBTRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public CBTRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CBTRibbon));
            this.CBTTab = this.Factory.CreateRibbonTab();
            this.grp1 = this.Factory.CreateRibbonGroup();
            this.btnTestAddIn = this.Factory.CreateRibbonButton();
            this.grp2 = this.Factory.CreateRibbonGroup();
            this.eb_ProjectId = this.Factory.CreateRibbonEditBox();
            this.eb_instanceId = this.Factory.CreateRibbonEditBox();
            this.eb_crePath = this.Factory.CreateRibbonEditBox();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.eb_tableName = this.Factory.CreateRibbonEditBox();
            this.eb_columnFamily = this.Factory.CreateRibbonEditBox();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.btnAdd = this.Factory.CreateRibbonButton();
            this.btnDelete = this.Factory.CreateRibbonButton();
            this.button1 = this.Factory.CreateRibbonButton();
            this.btnInsertTestData = this.Factory.CreateRibbonButton();
            this.button2 = this.Factory.CreateRibbonButton();
            this.CBTTab.SuspendLayout();
            this.grp1.SuspendLayout();
            this.grp2.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBTTab
            // 
            this.CBTTab.Groups.Add(this.grp1);
            this.CBTTab.Groups.Add(this.grp2);
            this.CBTTab.Groups.Add(this.group1);
            this.CBTTab.Groups.Add(this.group2);
            this.CBTTab.Label = "Google CBT";
            this.CBTTab.Name = "CBTTab";
            // 
            // grp1
            // 
            this.grp1.Items.Add(this.btnTestAddIn);
            this.grp1.Label = "Big Table";
            this.grp1.Name = "grp1";
            // 
            // btnTestAddIn
            // 
            this.btnTestAddIn.Image = global::GoogleBigTableAddIn.Properties.Resources.test_addin;
            this.btnTestAddIn.Label = "Test AddIn";
            this.btnTestAddIn.Name = "btnTestAddIn";
            this.btnTestAddIn.ScreenTip = "Test this AddIn is working fine or not.";
            this.btnTestAddIn.ShowImage = true;
            this.btnTestAddIn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSayHelloWorld_Click);
            // 
            // grp2
            // 
            this.grp2.Items.Add(this.eb_ProjectId);
            this.grp2.Items.Add(this.eb_instanceId);
            this.grp2.Items.Add(this.eb_crePath);
            this.grp2.Label = "Project Details";
            this.grp2.Name = "grp2";
            // 
            // eb_ProjectId
            // 
            this.eb_ProjectId.Label = "         Project Id";
            this.eb_ProjectId.Name = "eb_ProjectId";
            this.eb_ProjectId.ScreenTip = "Project Id of Google Bigtable";
            this.eb_ProjectId.SizeString = "Project Id of Google Bigtable";
            this.eb_ProjectId.Text = "grass-clump-479";
            // 
            // eb_instanceId
            // 
            this.eb_instanceId.Label = "       Instance Id";
            this.eb_instanceId.Name = "eb_instanceId";
            this.eb_instanceId.ScreenTip = "Instance Id of Google Bigtable";
            this.eb_instanceId.SizeString = "Project Id of Google Bigtable";
            this.eb_instanceId.Text = "dotnet-perf-2";
            // 
            // eb_crePath
            // 
            this.eb_crePath.Label = "Credential Path";
            this.eb_crePath.Name = "eb_crePath";
            this.eb_crePath.ScreenTip = "Credential Path of Google Bigtable";
            this.eb_crePath.SizeString = "Project Id of Google Bigtable";
            this.eb_crePath.Text = "C:\\google_client_credential.json";
            // 
            // group1
            // 
            this.group1.Items.Add(this.eb_tableName);
            this.group1.Items.Add(this.eb_columnFamily);
            this.group1.Label = "Table Details";
            this.group1.Name = "group1";
            // 
            // eb_tableName
            // 
            this.eb_tableName.Label = "     Table Name ";
            this.eb_tableName.Name = "eb_tableName";
            this.eb_tableName.ScreenTip = "Table Name of Google Bigtable";
            this.eb_tableName.SizeString = "Project Id of Google Bigtable";
            this.eb_tableName.Text = "table-name";
            // 
            // eb_columnFamily
            // 
            this.eb_columnFamily.Label = "Column Family";
            this.eb_columnFamily.Name = "eb_columnFamily";
            this.eb_columnFamily.ScreenTip = "Column Family";
            this.eb_columnFamily.SizeString = "Project Id of Google Bigtable";
            this.eb_columnFamily.Text = "TRD";
            // 
            // group2
            // 
            this.group2.Items.Add(this.btnAdd);
            this.group2.Items.Add(this.btnDelete);
            this.group2.Items.Add(this.button1);
            this.group2.Items.Add(this.btnInsertTestData);
            this.group2.Items.Add(this.button2);
            this.group2.Label = "Table Methods";
            this.group2.Name = "group2";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::GoogleBigTableAddIn.Properties.Resources.add_table;
            this.btnAdd.Label = "Add Table";
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ScreenTip = "Add New Table in Google Bigtable";
            this.btnAdd.ShowImage = true;
            this.btnAdd.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::GoogleBigTableAddIn.Properties.Resources.delete_table;
            this.btnDelete.Label = "Delete Table";
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ScreenTip = "Delete Existing Table from Google Bigtable";
            this.btnDelete.ShowImage = true;
            this.btnDelete.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDelete_Click);
            // 
            // button1
            // 
            this.button1.Image = global::GoogleBigTableAddIn.Properties.Resources.check_table;
            this.button1.Label = "Check Table";
            this.button1.Name = "button1";
            this.button1.ScreenTip = "Check Table is Exists or not in Google Bigtable";
            this.button1.ShowImage = true;
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCheck_Click);
            // 
            // btnInsertTestData
            // 
            this.btnInsertTestData.Image = ((System.Drawing.Image)(resources.GetObject("btnInsertTestData.Image")));
            this.btnInsertTestData.Label = "Insert Test Data";
            this.btnInsertTestData.Name = "btnInsertTestData";
            this.btnInsertTestData.ScreenTip = "Insert Test Data into Google Bigtable";
            this.btnInsertTestData.ShowImage = true;
            this.btnInsertTestData.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnInsertTestData_Click);
            // 
            // button2
            // 
            this.button2.Image = global::GoogleBigTableAddIn.Properties.Resources.display_table;
            this.button2.Label = "Display Table Data";
            this.button2.Name = "button2";
            this.button2.ScreenTip = "Display Table Data from Google Bigtable";
            this.button2.ShowImage = true;
            this.button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDispalyData_Click);
            // 
            // CBTRibbon
            // 
            this.Name = "CBTRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.CBTTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.CBTRibbon_Load);
            this.CBTTab.ResumeLayout(false);
            this.CBTTab.PerformLayout();
            this.grp1.ResumeLayout(false);
            this.grp1.PerformLayout();
            this.grp2.ResumeLayout(false);
            this.grp2.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab CBTTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grp1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnTestAddIn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAdd;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grp2;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox eb_ProjectId;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox eb_tableName;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox eb_instanceId;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDelete;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnInsertTestData;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox eb_crePath;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox eb_columnFamily;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button2;
    }

    partial class ThisRibbonCollection
    {
        internal CBTRibbon CBTRibbon
        {
            get { return this.GetRibbon<CBTRibbon>(); }
        }
    }
}
