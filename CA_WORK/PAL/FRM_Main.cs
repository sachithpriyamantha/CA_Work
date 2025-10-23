using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CA_WORK
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        BLL.BLL_CS_CaWork b1 = new BLL.BLL_CS_CaWork();
        private bool isTyping = false;
        public static string val = "";
        double DsRate;
        private bool substs;

        public Main()
        {
            InitializeComponent();
            
        }

        private void gvJobReg_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        

        {
            if (e.Column.FieldName == "sub")
            {
                string currentSub = gvJobReg.GetRowCellValue(e.RowHandle, "sub")?.ToString();

                if (!string.IsNullOrWhiteSpace(currentSub))
                {
                    int duplicateCount = 0;

                    for (int i = 0; i < gvJobReg.RowCount; i++)
                    {
                        if (i == e.RowHandle) continue;

                        string subValue = gvJobReg.GetRowCellValue(i, "sub")?.ToString();

                        if (!string.IsNullOrWhiteSpace(subValue) && subValue == currentSub)
                        {
                            duplicateCount++;
                        }
                    }

                    if (duplicateCount > 0)
                    {
                        MessageBox.Show("Duplicate Sub value detected. Please enter a unique value.", "Duplicate Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gvJobReg.SetRowCellValue(e.RowHandle, "sub", "");
                        return;
                    }
                }
            }


            
            int emptyDescCount = 0;
            for (int i = 0; i < gvJobReg.RowCount; i++)
            {
                if (gvJobReg.GetRowCellValue(i, "description")?.ToString() == "")
                {
                    emptyDescCount++;
                }
            }

            if (emptyDescCount == 0)
            {
                b1.addnewJobLine(txtProjno.Text);
                gvJobReg.SetFocusedRowCellValue("Sub", "001"); 
            }
        }

        //private void gvJobReg_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    // Check if the 'description' column value is updated
        //    if (e.Column.FieldName == "description" && !string.IsNullOrEmpty(e.Value?.ToString()))
        //    {
        //        int cunt = 0;

        //        // Count rows with empty "description"
        //        for (int i = 0; i < gvJobReg.RowCount; i++)
        //        {
        //            if (string.IsNullOrEmpty(gvJobReg.GetRowCellValue(i, "description")?.ToString()))
        //            {
        //                cunt++;
        //            }
        //        }

        //        // If no empty rows, proceed to add a new job line
        //        if (cunt == 0)
        //        {
        //            b1.addnewJobLine(txtProjno.Text);

        //            // Find the maximum value in the "Sub" column
        //            int maxSub = 0;
        //            for (int i = 0; i < gvJobReg.RowCount; i++)
        //            {
        //                string subValue = gvJobReg.GetRowCellValue(i, "Sub")?.ToString();
        //                if (int.TryParse(subValue, out int subNumber) && subNumber > maxSub)
        //                {
        //                    maxSub = subNumber;
        //                }
        //            }

        //            // Increment maxSub and format as "001"
        //            int newSubValue = maxSub + 1;
        //            string formattedSubValue = newSubValue.ToString("D3");

        //            // Set the new "Sub" value for the focused row
        //            gvJobReg.SetFocusedRowCellValue("Sub", formattedSubValue);

        //            // Refresh the GridView to display the updated value
        //            gvJobReg.RefreshRow(gvJobReg.FocusedRowHandle);
        //        }
        //    }
        //}



        private void Main_Load(object sender, EventArgs e)
        {
            pcAllAccounts.Visible = false;
            pcMainJob.Visible = false;
            pcJobCategory.Visible = false;
            gcJobReg.DataSource = b1.loadSubjobDetails("","");

        }

        private void cmbJobstatus_Properties_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbJobstatus.Text == "OPENED")
            {
                cmbJobstatus.BackColor = Color.White;
                cmbJobstatus.ForeColor = Color.Black;
            }
            else if (cmbJobstatus.Text == "CLOSED")
            {
                cmbJobstatus.BackColor = Color.Red;
                cmbJobstatus.ForeColor = Color.White;
            }
            else if (cmbJobstatus.Text == "CONFIRMED")
            {
                cmbJobstatus.BackColor = Color.Yellow;
                cmbJobstatus.ForeColor = Color.Black;
            }
            else if (cmbJobstatus.Text == "PENDING")
            {
                cmbJobstatus.BackColor = Color.Green;
                cmbJobstatus.ForeColor = Color.White;
            }
           
            else if (cmbJobstatus.Text == "CANCELLED")
            {
                cmbJobstatus.BackColor = Color.Black;
                cmbJobstatus.ForeColor = Color.White;
            }
        }

        private void lbl_BnameClose_Click(object sender, EventArgs e)
        {
            pcAllAccounts.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            pcMainJob.Visible = false;
        }

        private void txtAccoutno_Properties_DoubleClick(object sender, EventArgs e)
        {
            pcAllAccounts.Visible = true;
            pcAllAccounts.Location = new Point(
            this.ClientSize.Width / 2 - pcAllAccounts.Size.Width / 2,
            this.ClientSize.Height / 2 - pcAllAccounts.Size.Height / 2);
            pcAllAccounts.Anchor = AnchorStyles.None;
            gcAllAccounts.DataSource = b1.loadAllAccounts();
        }

        private void txtProjno_Properties_DoubleClick(object sender, EventArgs e)
        {
            pcMainJob.Visible = true;
            pcMainJob.Location = new Point(
            this.ClientSize.Width / 2 - pcMainJob.Size.Width / 2,
            this.ClientSize.Height / 2 - pcMainJob.Size.Height / 2);
            pcMainJob.Anchor = AnchorStyles.None;
            string ca = txtCategory.Text;
            if (ca != " ")
            {
                gcMainJobDetails.DataSource = b1.loadAllMainJobs(ca);
            }
        }

        private void gvAllAccounts_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                txtAccoutno.Text = gvAllAccounts.GetFocusedRowCellValue("code").ToString();
                pcAllAccounts.Visible = false;
            }
        }

        private void gvMainJobDetails_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                string jobno;
                string jcat;
                jobno = gvMainJobDetails.GetFocusedRowCellValue("Jno").ToString();
                txtProjno.Text = jobno;
                txtProjname.Text = gvMainJobDetails.GetFocusedRowCellValue("description").ToString();
                txtDspRate.Text = gvMainJobDetails.GetFocusedRowCellValue("depRate").ToString();

                dteStartdate.Text = gvMainJobDetails.GetFocusedRowCellValue("sdate").ToString();
                dteEnddate.Text = gvMainJobDetails.GetFocusedRowCellValue("edate").ToString();
                txtAccoutno.Text = gvMainJobDetails.GetFocusedRowCellValue("accNo").ToString();
                jcat = gvMainJobDetails.GetFocusedRowCellValue("jcat").ToString();
                txtCategory.Text = gvMainJobDetails.GetFocusedRowCellValue("jcat").ToString();
                cmbJobstatus.Text = gvMainJobDetails.GetFocusedRowCellValue("status").ToString();



                pcMainJob.Visible = false;
                gcJobReg.DataSource = b1.loadSubjobDetails(jobno, jcat); 

                try
                {
                    if (dteStartdate.Text != "" && dteEnddate.Text != "")
                    {
                        colsd.OptionsColumn.AllowEdit = true;
                        coled.OptionsColumn.AllowEdit = true;

                        rdeSdate.MaxValue = Convert.ToDateTime(dteEnddate.Text);
                        rdeSdate.MinValue = Convert.ToDateTime(dteStartdate.Text);

                        rdeEdate.MaxValue = Convert.ToDateTime(dteEnddate.Text);
                        rdeEdate.MinValue = Convert.ToDateTime(dteStartdate.Text);
                    }
                    else
                    {
                        colsd.OptionsColumn.AllowEdit = false;
                        coled.OptionsColumn.AllowEdit = false;
                    }
                }
                catch
                { }


            }
        }

        private void gvMainJobDetails_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView View = gvMainJobDetails;
            if (e.Column.FieldName == "status")
            {
                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["status"]);

                if (category == "OPENED")
                {
                    e.Appearance.BackColor = Color.Green;
                    e.Appearance.ForeColor = Color.White;
                }

                if (category == "Confirmed")
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void gvJobReg_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = gvJobReg;
            if (e.Column.FieldName == "status")
            {
                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["status"]);

                if (category == "Pending")
                {
                    e.Appearance.BackColor = Color.Green;
                    e.Appearance.ForeColor = Color.White;
                }

                if (category == "Confirmed")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.White;
                }
            }
        }

        private void bntNew_Click(object sender, EventArgs e)
        {
            txtProjno.Text = "";
            txtAccoutno.Text = "";
            txtProjname.Text = "";
            txtCategory.Text = "";
            txtDspRate.Text = "";
            cmbJobstatus.Text = "OPENED";
            dteStartdate.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            dteEnddate.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            gcJobReg.DataSource = b1.loadSubjobDetails("","");
      
        }




        //private void btnSave_Click(object sender, EventArgs e)

        //{


        //    if (txtProjname.Text == "")
        //        DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Enter Project Name", "Missing data");

        //    else if (txtCategory.Text == "")
        //        DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select Job Category", "Missing data");

        //    //else if (txtAccoutno.Text == "")
        //    //    DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select Account no", "Missing data");

        //    else if (dteStartdate.Text == "")
        //        DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select start date", "Missing data");

        //    else if (dteEnddate.Text == "")
        //        DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select end date", "Missing data");

        //    else if (txtCategory.Text == "CA")
        //    {
        //        if (txtDspRate.Text == "")
        //            DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Despreation Rate cannot be null", "Missing data");

        //        else if (Convert.ToDouble(txtDspRate.Text) >= 100)
        //            DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Despreation Rate cannot be grater than to 100", "Missing data");
        //    }
        //    else
        //    {

        //        Boolean mainsts = false;
        //        Boolean substs = false;


        //        string jcat = txtCategory.Text;
        //        string jmain = txtProjno.Text;

        //        mainsts = b1.saveCAMainJobRegister(txtDspRate.Text, jcat, jmain, txtProjname.Text, dteStartdate.Text, dteEnddate.Text, cmbJobstatus.Text, txtAccoutno.Text);

        //        if (!mainsts)
        //        {
        //            DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Job already exists with the same Category and Job No", "Duplicate Job", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        //string jcat =  ;
        //        b1.getNewMainJobNo(txtCategory.Text);
        //        mainsts = b1.saveCAMainJobRegister(txtDspRate.Text, txtCategory.Text, txtProjno.Text, txtProjname.Text, dteStartdate.Text, dteEnddate.Text, cmbJobstatus.Text, txtAccoutno.Text);

        //        for (int i = 0; i < gvJobReg.RowCount; i++)
        //        {
        //            string sub = gvJobReg.GetRowCellValue(i, "sub").ToString();
        //            string spec = gvJobReg.GetRowCellValue(i, "spec").ToString();
        //            string extc = gvJobReg.GetRowCellValue(i, "extc").ToString();
        //            string desc = gvJobReg.GetRowCellValue(i, "description").ToString();
        //            string start = gvJobReg.GetRowCellDisplayText(i, "sd").ToString();
        //            string end = gvJobReg.GetRowCellDisplayText(i, "ed").ToString();
        //            string sts = gvJobReg.GetRowCellValue(i, "status").ToString();


        //            if (gvJobReg.GetRowCellValue(i, "description").ToString() != "")
        //            {

        //                substs = b1.saveCASubJobRegister(txtCategory.Text, val, sub, spec, extc, desc, start, end, sts);
        //            }
        //        }

        //        if (mainsts)

        //            txtProjno.Text = val;
        //        gcJobReg.DataSource = b1.loadSubjobDetails(val, txtCategory.Text);
        //        DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Job Saved Successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);


        //    }
        //}


        private void btnSave_Click(object sender, EventArgs e)
        {


            if (txtProjname.Text == "")
                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Enter Project Name", "Missing data");

            else if (txtCategory.Text == "")
                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select Job Category", "Missing data");

            //else if (txtAccoutno.Text == "")
            //    DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select Account no", "Missing data");

            else if (dteStartdate.Text == "")
                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select start date", "Missing data");

            else if (dteEnddate.Text == "")
                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please Select end date", "Missing data");

            else if (txtCategory.Text == "CA")
            {
                if (txtDspRate.Text == "")
                    DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Despreation Rate cannot be null", "Missing data");

                else if (Convert.ToDouble(txtDspRate.Text) >= 100)
                    DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Despreation Rate cannot be grater than to 100", "Missing data");
            }
            else
            {
                Boolean mainsts = false;
                Boolean substs = false;

                string jcat = txtCategory.Text;
                string jmain = txtProjno.Text;

                
                mainsts = b1.saveCAMainJobRegister(txtDspRate.Text, jcat, jmain, txtProjname.Text, dteStartdate.Text, dteEnddate.Text, cmbJobstatus.Text, txtAccoutno.Text);


                if (!mainsts)
                //{

                //    b1.getNewMainJobNo(txtCategory.Text);
                //    mainsts = b1.saveCAMainJobRegister(txtDspRate.Text, txtCategory.Text, txtProjno.Text, txtProjname.Text, dteStartdate.Text, dteEnddate.Text, cmbJobstatus.Text, txtAccoutno.Text);
                //}
                {
                     
                    gcJobReg.DataSource = b1.loadSubjobDetails(jmain, jcat);
                    DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel,
                        "Job saved/updated successfully", "Save",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                for (int i = 0; i < gvJobReg.RowCount; i++)
                {
                    string sub = gvJobReg.GetRowCellValue(i, "sub").ToString();
                    string spec = gvJobReg.GetRowCellValue(i, "spec").ToString();
                    string extc = gvJobReg.GetRowCellValue(i, "extc").ToString();
                    string desc = gvJobReg.GetRowCellValue(i, "description").ToString();
                    string start = gvJobReg.GetRowCellDisplayText(i, "sd").ToString();
                    string end = gvJobReg.GetRowCellDisplayText(i, "ed").ToString();
                    string sts = gvJobReg.GetRowCellValue(i, "status").ToString();

                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        substs = b1.saveCASubJobRegister(txtCategory.Text, txtProjno.Text, sub, spec, extc, desc, start, end, sts);
               
                    }
                }

                 
                gcJobReg.DataSource = b1.loadSubjobDetails(txtProjno.Text, txtCategory.Text);
                //dteEnddate.Text = gvJobReg.GetFocusedRowCellValue("ed").ToString();

                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Job Saved Successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }




        public void loadmainjobDetails()
        {
            DataTable dt = b1.loadMainJob("OR", val);
            foreach (DataRow dr in dt.Rows)
            {
                txtProjno.Text = dr["pmd_jmain"].ToString();
                txtProjname.Text = dr["pmd_description"].ToString();
                txtAccoutno.Text = dr["pmd_asset_account"].ToString();
                cmbJobstatus.EditValue = dr["pmd_status"].ToString();
                dteStartdate.EditValue = dr["pmd_start_date"].ToString();
                dteEnddate.EditValue = dr["pmd_end_date"].ToString();
   
            }
            gcJobReg.DataSource = b1.loadSubjobDetails(val,"");
        }

        private void rtxtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void gcJobReg_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridView view = gvJobReg;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo hitInfo = view.CalcHitInfo(pt);
            String colname = "";
            colname = hitInfo.Column.FieldName.ToString();

            if (txtProjno.Text == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Please select a project Number", "Missing value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (gvJobReg.GetFocusedRowCellValue("sub").ToString() == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Invalid subjob number", "Missing value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (gvJobReg.GetFocusedRowCellValue("description").ToString() == "")
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel, "Invalid Description", "Missing value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (colname == "sub" || colname == "spec" || colname == "extc")
                {
                    //CA_WORK.Classes.Midval.Spcat = "CA";
                    //CA_WORK.Classes.Midval.Spmain = txt_projno.Text;
                    //CA_WORK.Classes.Midval.Spsub = gridView1.GetFocusedRowCellValue("sub").ToString();
                    //CA_WORK.Classes.Midval.Spspec = gridView1.GetFocusedRowCellValue("spec").ToString();
                    //CA_WORK.Classes.Midval.Spextc = gridView1.GetFocusedRowCellValue("extc").ToString();
                    //CA_WORK.Classes.Midval.Spdesc = gridView1.GetFocusedRowCellValue("description").ToString();
                    //this.Hide();
                }
            }
        }

        private void dteStartdate_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
         
                try
                {
                    if (dteStartdate.Text != "" && dteEnddate.Text != "")
                    {
                        colsd.OptionsColumn.AllowEdit = true;
                        coled.OptionsColumn.AllowEdit = true;

                    rdeSdate.MaxValue = Convert.ToDateTime(dteEnddate.Text);
                    rdeSdate.MinValue = Convert.ToDateTime(dteStartdate.Text);

                    rdeEdate.MaxValue = Convert.ToDateTime(dteEnddate.Text);
                    rdeEdate.MinValue = Convert.ToDateTime(dteStartdate.Text);
                    }
                    else
                    {
                        colsd.OptionsColumn.AllowEdit = false;
                        coled.OptionsColumn.AllowEdit = false;
                    }
                }
                catch
                { }
            }

        private void dteEnddate_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                if (dteStartdate.Text != "" && dteEnddate.Text != "")
                {
                    colsd.OptionsColumn.AllowEdit = true;
                    coled.OptionsColumn.AllowEdit = true;

                    rdeSdate.MaxValue = Convert.ToDateTime(dteEnddate.Text);
                    rdeSdate.MinValue = Convert.ToDateTime(dteStartdate.Text);

                    rdeEdate.MaxValue = Convert.ToDateTime(dteEnddate.Text);
                    rdeEdate.MinValue = Convert.ToDateTime(dteStartdate.Text);
                }
                else
                {
                    colsd.OptionsColumn.AllowEdit = false;
                    coled.OptionsColumn.AllowEdit = false;
                }
            }
            catch
            { }
        }


        private void btnExcell_Click(object sender, EventArgs e)
        {
            if (gvJobReg.RowCount < 2)
            {
                XtraMessageBox.Show(this.LookAndFeel, "Job Register Grid is Empty. Please add some data before printing.", "Job Register Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    gvJobReg.ExportToXlsx("C:\\EXE\\Job Register.Xlsx");
                    Process.Start("C:\\EXE\\Job Register.Xlsx");
                }
                catch (Exception)
                {
                    XtraMessageBox.Show(this.LookAndFeel, "Excel Sheet is already open, Close it and export a new Excel Sheet.", "Job Register Module", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }



        private void txtProjname_Properties_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtCategory_DoubleClick(object sender, EventArgs e)
        {
            //pcJobCategory.Location = new Point(
            //this.ClientSize.Width / 2 - pcJobCategory.Size.Width / 2,
            //this.ClientSize.Height / 2 - pcJobCategory.Size.Height / 2);
            //pcJobCategory.Anchor = AnchorStyles.None;
            pcJobCategory.Visible = true;
            gcJobCategory.DataSource = b1.loadJCat();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pcJobCategory.Visible = false;
        }

        private void gvJobCategory_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                txtCategory.Text = gvJobCategory.GetFocusedRowCellValue("jCode").ToString();
                pcJobCategory.Visible = false;

            }
        }

        private void txtDspRate_Properties_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pcJobCategory_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtProjno_EditValueChanged(object sender, EventArgs e)
        //{

        //    TextEdit txt = sender as TextEdit;
        //    if (txt != null)
        //    {
        //        string currentText = txt.Text;
        //        string numericText = new string(currentText.Where(char.IsDigit).ToArray());

        //        if (txt.Text != numericText)
        //        {
        //            txt.Text = numericText;
        //            txt.SelectionStart = txt.Text.Length;
        //        }


        //        string jcat = txtCategory.Text.Trim();
        //        string jmain = txtProjno.Text.Trim();

        //        if (!string.IsNullOrEmpty(jcat) && !string.IsNullOrEmpty(jmain))
        //        {
        //            bool exists = b1.IsMainJobExists(jcat, jmain);

        //            if (exists)
        //            {
        //                DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel,
        //                    "A job with this Category and Job No already exists.",
        //                    "Duplicate Job",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Warning);

        //                //txtProjno.Text = "";
        //                //txtProjname.Text = "";
        //            }
        //        }

        //    }
        //}


        {
            TextEdit txt = sender as TextEdit;
            if (txt != null)
            {
                string currentText = txt.Text;
                string numericText = new string(currentText.Where(char.IsDigit).ToArray());

                if (txt.Text != numericText)
                {
                    txt.Text = numericText;
                    txt.SelectionStart = txt.Text.Length;
                }

                
                if (isTyping)
                {
                    string jcat = txtCategory.Text.Trim();
                    string jmain = txtProjno.Text.Trim();

                    if (!string.IsNullOrEmpty(jcat) && !string.IsNullOrEmpty(jmain))
                    {
                        bool exists = b1.IsMainJobExists(jcat, jmain);

                        if (exists)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel,
                                "A job with this Category and Job No already exists.",
                                "Duplicate Job",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                           
                            // txtProjno.Text = "";
                            // txtProjname.Text = "";
                        }
                    }

                    isTyping = false;
                }
            }
        }


        private void gcJobReg_Click(object sender, EventArgs e)
        {

        }

        private void txtProjno_Leave(object sender, EventArgs e)
        {
            //string jcat = txtCategory.Text.Trim();
            //string jmain = txtProjno.Text.Trim();

            //if (!string.IsNullOrEmpty(jcat) && !string.IsNullOrEmpty(jmain))
            //{
            //    bool exists = b1.IsMainJobExists(jcat, jmain);

            //    if (exists)
            //    {
            //        DevExpress.XtraEditors.XtraMessageBox.Show(this.LookAndFeel,
            //            "A job with this Category and Job No already exists.",
            //            "Duplicate Job",
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);

            //        txtProjno.Text = "";
            //        txtProjname.Text = "";

            //    }
            //}
        }

        private void txtAccoutno_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtProjno_KeyPress(object sender, KeyPressEventArgs e)
        {

           
                pcJobCategory.Visible = false;
                isTyping = true;
        }

        private void txtProjno_KeyDown(object sender, KeyEventArgs e)
       {
            
                pcJobCategory.Visible = false;  
            

        }
        

        private void txtProjname_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dteEnddate_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void dteStartdate_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gvJobReg_RowCellClick(object sender, RowCellClickEventArgs e)
        {
           

        }
    }
}
