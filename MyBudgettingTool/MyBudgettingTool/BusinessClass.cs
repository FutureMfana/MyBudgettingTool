using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MyBudgettingTool
{
    class BusinessClass
    {
        private SqlDataAdapter da;
        private SqlCommand sqlCmd;
        private SqlConnection sqlCon;
        private string sql = "";

        #region GetConnection()
        public string GetConnection()
        {
            try
            {
                sqlCon = new SqlConnection(@"Data Source=DESKTOP-G1E2CG2;Integrated Security=SSPI;Initial Catalog=Budget");
                sqlCon.Open();
                sqlCon.Close();
                return "true";
            }catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region GetBudgetMonth()
        public DataSet GetBudgetByMonth(int month) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT * FROM Year2020.My2020Budget WHERE Month = {month}";
                if(sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                da = new SqlDataAdapter(sql, sqlCon);
                da.Fill(ds, "Bugdet");
                da.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                //pass
            }
            return ds;
        }
        #endregion

        #region GetAll()
        public DataSet GetAll()
        {
            DataSet ds = new DataSet();
            try
            {
                sql = "SELECT * FROM Year2020.My2020Budget";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                da = new SqlDataAdapter(sql, sqlCon);
                da.Fill(ds);
                da.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }

            }
            catch(Exception ex)
            {
                //pass
            }
            return ds;
        }
        #endregion

        #region GetDataByMonth()
        public DataSet GetDataByMonth(int month) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT * FROM Year2020.My2020Budget WHERE Month = {month}";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                da = new SqlDataAdapter(sql, sqlCon);
                da.Fill(ds, "DataByMonth");
                da.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }

            }
            catch (Exception ex)
            {
                //pass
            }
            return ds;
        }
        #endregion

        #region GetDataByYear()
        public DataSet GetDataByYear(int year) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT* FROM Year2020.My2020Budget WHERE Year = {year}";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                da = new SqlDataAdapter(sql, sqlCon);
                da.Fill(ds, "DataByYear");
                da.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }

            }
            catch (Exception ex)
            {
                //pass
            }
            return ds;
        }
        #endregion

        #region GetDataByYearAndMonth()
        public DataSet GetDataByYearAndMonth(int year, int month) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT * FROM Year2020.My2020Budget WHERE Year = {year} AND Month = {month}";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                da = new SqlDataAdapter(sql, sqlCon);
                da.Fill(ds, "DataByYearAndMont");
                da.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }

            }
            catch (Exception ex)
            {
                //pass
            }
            return ds;
        }
        #endregion

        #region AddToBudget()
        public string AddToBudget(string expense, int cost, int month, int year)
        {
            sql = $"INSERT INTO Year2020.MyBudget (Expenses, Costs, Month_, Year_) VALUES ( '{expense}', {cost}, {month}, {year})";
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }

                return "true";
            }catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region DeleteByYearAndMonth()
        public string DeleteByYearAndMonth(int year, int month) {
            try
            {
                sql = $"DELETE FROM Year2020.MyBudget WHERE Year = {year} AND Month_ = {month}";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                return "true";
            }catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region DeleteByMonth()
        public string DeleteByMonth(int month)
        {
            try
            {
                sql = $"DELETE FROM Year2020.MyBudget WHERE Month_ = {month}";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region DeleteByYear()
        public string DeleteByYear(int year)
        {
            try
            {
                sql = $"DELETE FROM Year2020.MyBudget WHERE Year_ = {year}";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region DeleteAll()
        public string DeleteAll()
        {
            try
            {
                sql = "TRUNCATE TABLE Year2020.MyBudget";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();

                if (sqlCon.State == ConnectionState.Open) {
                    sqlCon.Close(); 
                }

                return "true";
            }catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

    }
}
