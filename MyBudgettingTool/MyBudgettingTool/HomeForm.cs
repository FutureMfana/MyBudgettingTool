using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBudgettingTool
{
    public partial class HomeForm : Form
    {
        BusinessClass bc = new BusinessClass();
        int salary = 0;

        #region FormInitialization
        public HomeForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Form_Load()
        private void HomeForm_Load(object sender, EventArgs e)
        {
           try
            {
                PopulateYearCbo();

                string con = bc.GetConnection();
                if (!con.ToLower().Equals("true"))
                {
                    ErrorMessage(con);
                }
                txtUsername.Text = Users.userName;
                txtBalance.Text = Users.salary.ToString();
                salary = Users.salary;
                showAllData();
            }
            catch (Exception ex) {
                ErrorMessage(ex.Message.ToString(), "Encounted an error");
            }
        }
        #endregion

        #region showAllData()
        private void showAllData()
        {
            //Exceptions handled at invoking methods
            DataSet ds = new DataSet();
            ds = bc.GetAllByUserID(Users.userID);
            if (ds.Tables.Count < 1)
            {
                OperationSucceeded($"No data found for this user.", "Sorry,");
                return;
            }
            dgvBudget.DataSource = ds.Tables[0];
            int salary = this.salary;
            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
            {
                salary -= Convert.ToInt16(ds.Tables[0].Rows[i]["Costs"]);
            }
            Users.salary = salary; 
            txtBalance.Text = salary.ToString();
        }
        #endregion

        #region showDataByMonth()
        private void showDataByMonth()
        {
            //Exceptions handled at invoking methods
            DataSet ds = new DataSet();
            ds = bc.GetBudgetByMonthAndByUserID(cboMonth.SelectedIndex + 1, Users.userID);
            if (ds.Tables.Count < 1)
            {
                throw new Exception("Data not found!");
            }
            dgvBudget.DataSource = ds.Tables[0];
            dgvBudget.Refresh();
        }
        #endregion

        #region showDataByYear()
        private void showDataByYear()
        {
            DataSet ds = new DataSet();
            ds = bc.GetDataByYearUserID(Convert.ToInt16(cboYear.SelectedItem), Users.userID);
            if (ds.Tables.Count < 1)
            {
                throw new Exception("Data not found!");
            }
            dgvBudget.DataSource = ds.Tables[0];
            dgvBudget.Refresh();
        }
        #endregion

        #region showDataByMonthAndYear()
        private void showDataByMonthAndYear()
        {
            //Exception handled at the calling method
            DataSet ds = new DataSet();
            /*MessageBox.Show(cboYear.SelectedItem.ToString());
            return;*/
            ds = bc.GetDataByYearAndMonth(Convert.ToInt16(cboYear.SelectedItem), cboMonth.SelectedIndex +1, Users.userID);
            if (ds.Tables.Count < 1)
            {
                throw new Exception("Data not found!");
            }
            dgvBudget.DataSource = ds.Tables[0];
            dgvBudget.Refresh();
        }
        #endregion

        #region PopulateYearCbo()
        public void PopulateYearCbo()
        {
            //Exceptions handled by the calling method
            var year = DateTime.Now.Year;
            cboReportYear.Items.Add((year + 1).ToString());
            cboYearDelete.Items.Add((year + 1).ToString());
            cboYear.Items.Add((year + 1).ToString());
            do
            {
                cboReportYear.Items.Add((year).ToString());
                cboYearDelete.Items.Add(year.ToString());
                cboYear.Items.Add(year--.ToString());
            } while (year > 2020);
        }
        #endregion

        #region cboMonth_SelectedIndexChanged()
        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboYear.SelectedIndex < 0)
                {
                    showDataByMonth();
                }
                else
                {
                    showDataByMonthAndYear();
                }

            }
            catch (Exception ex)
            {
                ErrorMessage(ex.ToString(), "Encounted an error");
                return;
            }
        }
        #endregion

        #region cboYear_SelectedIndexChanged
        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboMonth.SelectedIndex < 0)
                {
                    showDataByYear();
                }
                else
                {
                    showDataByMonthAndYear();
                }
            }
            catch(Exception ex)
            {
                ErrorMessage(ex.ToString(), "Encounted an error");
                return;
            }
        }
        #endregion

        #region btnAdd_Click()
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var year = DateTime.Now.Year;
                if (chkNextYear.Checked)
                {
                    year++;
                }
                var month = cboMonthAdd.SelectedIndex + 1;
                if (month < DateTime.Now.Month && year <= DateTime.Now.Year)
                {
                    OperationSucceeded("Sorry, you cannot plan for the already months. Please try again");
                    return;
                }

                int userID = Users.userID;
                //to ensure user doen't spend over income salary
                if (chkNextYear.CheckState == CheckState.Unchecked) //this validations happens on the current year
                {
                    if ((Convert.ToInt16(txtBalance.Text) - Convert.ToInt16(txtPrice.Text)) < 0)
                    {
                        OperationSucceeded("Sorry, you're exceeding your remaing balance please try again");
                        return;
                    }
                    else
                    {
                        txtBalance.Text = (Convert.ToInt16(txtBalance.Text) - Convert.ToInt16(txtPrice.Text)).ToString();
                        Users.salary -= (Convert.ToInt16(txtBalance.Text) - Convert.ToInt16(txtPrice.Text));
                    }
                }
                //writes to DB and return results
                string addToBudget = bc.AddToBudget(txtExpenditure.Text.ToString(), Convert.ToInt16(txtPrice.Text), month, Convert.ToInt16(year), userID);
                if (addToBudget.ToLower().Equals("true")) {
                    OperationSucceeded("We've successfuly added a new record");
                }
                else {
                    ErrorMessage(addToBudget);
                    return;
                }

                //cleans textboxes for the next input
                txtExpenditure.Text = "";
                txtPrice.Text = "";

                //to get user's data to refresh dgv
                DataSet ds = new DataSet();
                ds = bc.GetDataByYearAndMonth(year, month, userID);
                dgvBudget.DataSource = ds;
                dgvBudget.Refresh();

                if (cboMonth.SelectedIndex >= 0 && cboYear.SelectedIndex >= 0)
                {
                    showDataByMonthAndYear();
                    return;
                }

                if (cboMonth.SelectedIndex >= 0)
                {
                    showDataByMonth();
                    return;
                }

                if (cboYear.SelectedIndex >= 0)
                {
                    showDataByYear();
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.ToString(), "Encounted an error");
                return;
            }
        }
        #endregion

        #region btnRemoveItem_Click()
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            try
            {
                string delResult;
                if (cboIDRemove.SelectedIndex < 0 && cboYearDelete.SelectedIndex < 0)
                {
                    OperationSucceeded("You can delete by Month or/and by Year ", "Sorry, operation failed");
                    return;
                }
               if (cboIDRemove.SelectedIndex >= 0 && cboYearDelete.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete some of this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        delResult = bc.DeleteByYearAndMonth(Convert.ToInt16(cboYear.SelectedItem), cboIDRemove.SelectedIndex + 1, Users.userID);
                        if (delResult.ToLower().Equals("true"))
                        {
                            OperationSucceeded("We've successfully deleted data", "Congratulations, your operation succeeded.");
                            txtBalance.Text = Users.salary.ToString();
                        }
                        else
                        {
                            OperationSucceeded("Couldn't delete data.", "Sorry, your operation failed");
                            return;
                        }
                    }
                }
               else if (cboIDRemove.SelectedIndex >= 0) {
                    if (MessageBox.Show("Are you sure you want to delete some of this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        delResult = bc.DeleteByMonthAndUserID(cboIDRemove.SelectedIndex + 1, Users.userID);
                        if (delResult.ToLower().Equals("true"))
                        {
                            OperationSucceeded("We've successfully deleted data", "Congratulations, your operation succeeded.");
                            txtBalance.Text = Users.salary.ToString();
                        }
                        else
                        {
                            OperationSucceeded("Couldn't delete data", "Sorry, your operation falied");
                        }
                    }
                }
                else if (cboYearDelete.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete some of this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        delResult = bc.DeleteByYear(Convert.ToInt16(cboYearDelete.SelectedItem), Users.userID);
                        if (delResult.ToLower().Equals("true"))
                        {
                            OperationSucceeded("We've successfully deleted data", "Congratulations, your operation succeeded.");
                            txtBalance.Text = Users.salary.ToString();
                        }
                        else
                        {
                            OperationSucceeded("Couldn't delete data","Sorry, operation failed.");
                        }
                    }
                }
                #region just to refresh dgv
                if (cboMonth.SelectedIndex > -1 && cboYear.SelectedIndex > -1)
                {
                    showDataByMonthAndYear();
                }
                else if (cboMonth.SelectedIndex > -1)
                {
                    showDataByMonth();
                }
                else if (cboYear.SelectedIndex > -1)
                {
                    showDataByYear();
                }
                else
                {
                    showAllData();
                }
                #endregion

            }
            catch (Exception ex)
            {
                ErrorMessage(ex.ToString(), "Encounted an error");
            }
        }
        #endregion

        #region btnRemoveAll_Click()
        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            try {
                string delResults = bc.DeleteAll(Users.userID);
                if (MessageBox.Show("Are you sure you want to remove all data?", "Confimation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    if (delResults.ToLower().Equals("true"))
                    {
                        OperationSucceeded("We've successfully deleted data", "Congratulations, your operation succeeded.");
                        txtBalance.Text = Users.salary.ToString();
                    }
                    else
                    {
                        ErrorMessage(delResults);
                        return;
                    }
                    showAllData();
                }
            }catch (Exception ex)
            {
                ErrorMessage(ex.ToString(), "Encounted an error");
                return;
            }
        }
        #endregion

        #region btnReport_Click()
        private void btnReport_Click(object sender, EventArgs e)
        {
            if (cboReportMon.SelectedIndex == -1)
            {
                ErrorMessage("You're required to provide a month for your report","Required field");
                cboReportMon.Focus();
                return;
            }else if (cboReportYear.SelectedIndex == -1) {
                ErrorMessage("You're required to provide a year for your report", "Required field");
                cboReportYear.Focus();
                return;
            }
            Users.salary = Convert.ToInt16(txtBalance.Text);
            Report report = new Report();
            report.yr = Convert.ToInt16(cboReportYear.SelectedItem);
            report.mon = cboReportMon.SelectedIndex + 1;
            report.Show();
        }
        #endregion

        #region UpdateSalary()
        private void UpdateSalary(DataSet ds, int expense)
        {
            int salary = Users.salary;
            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
            {
                salary -= Convert.ToInt32(ds.Tables[0].Rows[i]["Costs"]);
            }
            if (salary < 0)
            {
                throw new Exception("Sorry, you are exceeding your salary...");
            }
            this.salary = salary;
        }
        #endregion

        #region ErrorMessage();
        private void ErrorMessage(string msg, string subj = "Encounted an error")
        {
            MessageBox.Show(msg, subj, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region OperationSucceeded()
        public void OperationSucceeded(string details, string headline = "")
        {
            try
            {
                SpeechSynthesizer speaker = new SpeechSynthesizer();
                speaker.SelectVoice("Microsoft Zira Desktop");
                speaker.SpeakAsync(headline);
                speaker.SpeakAsync(details);
            }
            catch (Exception ex)
            {
                SpeechSynthesizer speaker = new SpeechSynthesizer();
                speaker.SpeakAsync("Sorry! The program encountered an error!");
                MessageBox.Show(ex.Message.ToString(), "Encounted an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                speaker.SpeakAsync(ex.Message.ToString());
            }
        }
        #endregion
    }
}
