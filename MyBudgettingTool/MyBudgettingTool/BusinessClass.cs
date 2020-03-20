using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Speech.Synthesis;
using System.Net;
using System.Net.Mail;

namespace MyBudgettingTool
{
    class BusinessClass
    {
        #region General Declarations
        private SqlDataAdapter da;
        private SqlCommand sqlCmd;
        private SqlConnection sqlCon;
        private string sql = "";

        //this array stores OTP and its hash value
        //OTP in first element and hash value in second element 
        private string otp;
        #endregion

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

        #region GetBudgetMonthAndByUserID()
        public DataSet GetBudgetByMonthAndByUserID(int month, int uid) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT * FROM Year2020.MyBudget WHERE Month = {month} AND UserID = {uid}";
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

        #region GetAllByUserID()
        public DataSet GetAllByUserID(int uid)
        {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT * FROM Year2020.MyBudget WHERE UserID = {uid}";
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

        #region GetDataByMonthAndUserID()
        public DataSet GetDataByMonth(int month, int uid) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT * FROM Year2020.MyBudget WHERE Month = {month} AND UserID = {uid}";
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

        #region GetDataByYearAndUserID()
        public DataSet GetDataByYearUserID(int year, int uid) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT* FROM Year2020.MyBudget WHERE Year = {year} AND UserID = {uid}";
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
        public DataSet GetDataByYearAndMonth(int year, int month, int uid) {
            DataSet ds = new DataSet();
            try
            {
                sql = $"SELECT * FROM Year2020.MyBudget WHERE Year = {year} AND Month = {month} AND UserID = {uid}";
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
        public string AddToBudget(string expense, int cost, int month, int year, int uid)
        {
            sql = $"INSERT INTO Year2020.MyBudget (Expenses, Costs, Month, Year, UserID) VALUES ( '{expense}', {cost}, {month}, {year}, {uid})";
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

        #region DeleteByYearAndMonthAndUserID()
        public string DeleteByYearAndMonth(int year, int month, int uid) {
            try
            {
                sql = $"DELETE FROM Year2020.MyBudget WHERE Year = {year} AND Month = {month} AND UserID = {uid}";
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

        #region DeleteByMonthAndUserID()
        public string DeleteByMonthAndUserID(int month, int uid)
        {
            try
            {
                sql = $"DELETE FROM Year2020.MyBudget WHERE Month = {month} AND UserID = {uid}";
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

        #region DeleteByYearAndUserID()
        public string DeleteByYear(int year, int uid)
        {
            try
            {
                sql = $"DELETE FROM Year2020.MyBudget WHERE Year = {year} AND UserID = {uid}";
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

        #region DeleteAllByUserID()
        public string DeleteAll(int uid)
        {
            try
            {
                sql = $"DELETE FROM Year2020.MyBudget WHERE UserID = {uid}";
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

        #region AddUser()
        public string AddUser(string UserName, string Password, int Salary, string WhatsAppNo = "Null", string EmailAddress = "Null") {
            try
            {
                //validate users' email address by mailing them
                //if the app couldn't mail them, there is possibility that the email is invalid
                string mailMsg = $"Dear {UserName}.<br><br>This is to notify you that you have successfully created account with MyBudgettingTool app.<br><br>Thank you for chosing<br><br>Regards, <br>215Devhelp Team<br><em>Kamvelihle Innocent<br>(+27)78 798 0344\nkamvelihleinnocent@gmai.com</em>";
                string mailRes = SendMail(EmailAddress, mailMsg, "Account created with My Budgetting Tool...");
                if (!mailRes.ToLower().Equals("true"))
                {
                    throw new Exception(mailRes);
                }

                sql = $"INSERT INTO Year2020.Users (UserName, Password, WhatsAppNo, EmailAddress, Salary) VALUES ('{UserName}', '{Password}', '{WhatsAppNo}', '{EmailAddress}', {Salary})";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                //the actual user registration (saving user information to the database)
                sqlCmd = new SqlCommand(sql, sqlCon);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
                
                return "true";
            } catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region EncryptPassword()
        public void EncryptPassword(string password,out string cypherOut) {
            
            try
            {
                using (SHA256 hash256SHA = SHA256.Create())
                {
                    byte[] cypherBytes = hash256SHA.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder cypher = new StringBuilder();

                    for (int i = 0; i < cypherBytes.Length; ++i)
                    {
                        cypher.Append(cypherBytes[i].ToString("x2"));
                    }
                    cypherOut = cypher.ToString();
                }
            }catch (Exception ex)
            {
                cypherOut = ex.Message.ToString();
            }
        }
        #endregion

        #region GetUserByEmailAddress()
        public DataSet GetUserByEmailAddress(string email, string username, string password)
        {
            DataSet ds = new DataSet();
            try
            {
                EncryptPassword(password, out password);
                if (password.Equals(""))
                {
                    throw new Exception(password);
                }
                sql = $"SELECT * FROM Year2020.Users WHERE EmailAddress = '{email}' AND Username = '{username}' AND Password = '{password}'";
                if (sqlCon.State != ConnectionState.Open)
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
            catch
            {  }
            return ds;
        }
        #endregion

        #region GetUserByWhatsAppNumber()
        public DataSet GetUserByWhatAppNumber(string whatsAppNumber, string username, string password)
        {
            DataSet ds = new DataSet();
            try
            {
                EncryptPassword(password, out password);
                sql = $"SELECT * FROM Year2020.Users WHERE Email = {whatsAppNumber} AND Username = {username} AND Password = '{password}'";
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }

                da = new SqlDataAdapter(sql, sqlCon);
                da.Fill(ds, "User");
                da.Dispose();

                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
            }
            catch
            { }
            return ds;
        }
        #endregion

        #region AuthenticateByWhatsApp()
        public string AuthenticateByWhatsApp(string whatsapp)
        {
            try
            {
                return "true";
            }catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion

        #region AuthenticateByEmail()
        public string AuthenticateByEmail(string email, string username)
        {
            try
            {
                generateOTP(out otp);
                if(this.otp.Equals("true")) //validates if there is OTP genereated for the user
                {
                    throw new Exception("\n\nCouldn't generate One Time Pin (OTP)!");
                }

                //generates hash value of the OTP for security reason before being stored to the DB
                string otpHash = "";
                EncryptPassword(otp, out otpHash);
                if (otpHash.Equals(""))
                {
                    throw new Exception("\n\nCouldn't ecrypt OTP. Something went wrong on the back end of the system.");
                }
                //we have to mail the OTP the user after it is generated before updating the table, just in case we couldn't email
                string msg = $"Dear {username}.\r\n\nYour OTP is: <h1>{otp}</h1>\r\n\nKind Regards, \n215Devhelp (Kamvelihle Innocent)\n(+27)787980344\nkamvelihleinnocent@gmail.com";
                string sub = "MyBudgettingTool OTP";
                string mailRes = SendMail(email, msg, sub);
                if (!mailRes.Equals("true")){
                    throw new Exception(mailRes);
                }

                //saves the OTP hash value to the database
                sql = $"UPDATE Year2020.Users SET OTP = '{otpHash}' WHERE EmailAddress = '{email}' AND Username = '{username}'";
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

        #region GenerateOTP()
        private void generateOTP(out string res)
        {
            StringBuilder pin = new StringBuilder();
            Random rnd = new Random();

            //this loop generates a five digit OTP
            for (int i = 0; i < 5; ++i)
            {
                pin.Append(rnd.Next(0, 9).ToString());
            }

            //keep the OTP in the first element of the result
            res = pin.ToString();
        }
        #endregion

        #region SendMail()
        public string SendMail(string email, string message, string subject)
        {
            try
            {
                MailMessage objMail = new MailMessage("215devhelp@gmail.com", email, subject, message);
                objMail.Priority = MailPriority.High;
                objMail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("215devhelp@gmail.com", "Kamv@215033787");
                smtp.EnableSsl = true;
                smtp.SendMailAsync(objMail);
                
                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion
    }
}