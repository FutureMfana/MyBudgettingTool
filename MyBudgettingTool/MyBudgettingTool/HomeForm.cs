using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBudgettingTool
{
    public partial class HomeForm : Form
    {
        BusinessClass bc = new BusinessClass();

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
                    throw new Exception(con);
                }
                showAllData();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Encounted an error");
            }
        }
        #endregion

        #region showAllData()
        private void showAllData()
        {
            //Exceptions handled at invoking methods
            DataSet ds = new DataSet();
            ds = bc.GetAll();
            if (ds.Tables.Count < 1)
            {
                throw new Exception("Data not found!");
            }
            dgvBudget.DataSource = ds.Tables[0];
        }
        #endregion

        #region showDataByMonth()
        private void showDataByMonth()
        {
            //Exceptions handled at invoking methods
            DataSet ds = new DataSet();
            ds = bc.GetBudgetByMonth(cboMonth.SelectedIndex + 1);
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
            ds = bc.GetDataByYear(Convert.ToInt16(cboYear.SelectedItem));
            if (ds.Tables.Count < 1)
            {
                throw new Exception("Data not found!");
            }
        }
        #endregion

        #region showDataByMonthAndYear()
        private void showDataByMonthAndYear()
        {
            //Exception handled at the calling method
            DataSet ds = new DataSet();
            /*MessageBox.Show(cboYear.SelectedItem.ToString());
            return;*/
            ds = bc.GetDataByYearAndMonth(Convert.ToInt16(cboYear.SelectedItem), cboMonth.SelectedIndex +1);
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
            do
            {
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
                MessageBox.Show(ex.ToString(), "Encounted an error");
            }
        }
        #endregion

        #region cboYear_SelectedIndexChanged
        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboMonth.SelectedIndex == 0)
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
                MessageBox.Show(ex.ToString(), "Encounted an error");
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
                    MessageBox.Show("Sorry... You cannot plan for the alredy months.", "Encounted an error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //Expenses, Costs, Month_, Year_
                string addToBudget = bc.AddToBudget(txtExpenditure.Text.ToString(), Convert.ToInt16(txtPrice.Text), month, Convert.ToInt16(year));
                if (addToBudget.ToLower().Equals("true")) {
                    MessageBox.Show("Data added successfuly!","Done",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else {
                    throw new Exception("Couldn't add data");
                }
                if (cboMonth.SelectedIndex >= 0)
                {
                    showDataByMonth();
                    if (cboYear.SelectedIndex >= 0)
                    {
                        showDataByMonthAndYear();
                    }
                }else if (cboYear.SelectedIndex >= 0) {
                    showDataByMonthAndYear();
                    if (cboMonth.SelectedIndex >= 0)
                    {
                        showDataByMonth();
                    }
                }
                dgvBudget.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Encounted an error");
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
                    MessageBox.Show("Please delete by Month or/and by Year ", "Encounted an error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
               if (cboIDRemove.SelectedIndex >= 0 && cboYearDelete.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete some of this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        delResult = bc.DeleteByYearAndMonth(Convert.ToInt16(cboYear.SelectedItem), cboIDRemove.SelectedIndex + 1);
                        if (delResult.ToLower().Equals("true"))
                        {
                            MessageBox.Show("Data deleted successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            throw new Exception("\nCouldn't delete data\n");
                        }
                    }
                    return;
                }
               if (cboIDRemove.SelectedIndex >= 0) {
                    if (MessageBox.Show("Are you sure you want to delete some of this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        delResult = bc.DeleteByMonth(cboIDRemove.SelectedIndex + 1);
                        if (delResult.ToLower().Equals("true"))
                        {
                            MessageBox.Show("Data deleted successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            throw new Exception("\nCouldn't delete data\n");
                        }
                    }
                    return;
                }
                if (cboYearDelete.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete some of this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        delResult = bc.DeleteByYear(Convert.ToInt16(cboYearDelete));
                        if (delResult.ToLower().Equals("true"))
                        {
                            MessageBox.Show("Data deleted successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            throw new Exception("\nCouldn't delete data\n");
                        }
                    }
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Encounted an error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        #region btnRemoveAll_Click()
        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            try {
                string delResults = bc.DeleteAll();
                if (MessageBox.Show("Are you sure you want to remove all data?", "Confimation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    if (delResults.ToLower().Equals("true"))
                    {
                        MessageBox.Show("Data Deleted successfuly", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        throw new Exception(delResults);
                    }
                    dgvBudget.Refresh();
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Encounted an error");
            }
        }
        #endregion
    }
}
