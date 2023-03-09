using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Application.Model;
using Application.DB;
using System.Configuration;

namespace Application
{
    public sealed class GeneralUtilities
    {
        // All methods are static, so this can be private
        private GeneralUtilities()
        { }

        static string appMode = ConfigurationManager.AppSettings["App_Mode"].ToString().Trim();

        /// <summary>
        /// Create random password for registered students in case forgot password
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public static string CreateRandomPassword(string emailId)
        {
            string pass = "";
            using (AUGSDGraduateEntities objEntities = new AUGSDGraduateEntities())
            {
                StudentRegistration objStudent = objEntities.StudentRegistrations.Where(s => s.EmailId == emailId).SingleOrDefault();
                if (objStudent != null)
                {
                    Random r = new Random();
                    pass = objStudent.StudentId.Substring(0, 1).Trim().ToLower() + r.Next(10, 99).ToString() + objStudent.StudentName.Substring(0, 1).Trim().ToLower() + objStudent.EmailId.Substring(0, 1).Trim().ToLower() + r.Next(10, 99).ToString() + objStudent.DegreeName.Substring(0, 1).Trim().ToLower();
                }
            }
            return pass;
        }

        /// <summary>
        /// Create random password for a students at the registration time
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentName"></param>
        /// <param name="emailId"></param>
        /// <param name="courseName"></param>
        /// <returns></returns>
        public static string CreateRandomPassword(string studentId, string studentName, string emailId, string courseName)
        {
            Random r = new Random();
            return studentId.Substring(0, 1).Trim().ToLower() + r.Next(10, 99).ToString() + studentName.Substring(0, 1).Trim().ToLower() + emailId.Substring(0, 1).Trim().ToLower() + r.Next(10, 99).ToString() + courseName.Substring(0, 1).Trim().ToLower();
        }

        /// <summary>
        /// Encode the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        /// <summary>
        /// Decode the encrypted password
        /// </summary>
        /// <param name="encodedData"></param>
        /// <returns></returns>
        public static string DecodePasswordFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        /// <summary>
        /// Genearte dyanmic menu for Admin user to manage the dynamic access panel
        /// </summary>
        /// <param name="adminUserName"></param>
        /// <returns></returns>
        public static string CreateDynamicMenuForAdmin(string adminUserName)
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append(" <ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>");

            if (!string.IsNullOrEmpty(adminUserName))
            {
                List<AdminUserRight> objList = AdminDB.GetAllRightsByAdminUserName(adminUserName);
                if (objList.Count > 0)
                {
                    foreach (AdminUserRight right in objList)
                    {
                        Int32 menuid;
                        Int32.TryParse(right.MenuId.ToString().Trim(), out menuid);
                        if (menuid > 0)
                        {
                            AdminMenu objMenu = AdminMenuDB.GetMenuDetailById(menuid);
                            if (objMenu != null)
                                sbMenu.Append("<li class='nav-item'><a class='nav-link' href='" + objMenu.Url.Trim() + "'><i class='far fa-circle nav-icon'></i><p>" + objMenu.DisplayText.Trim() + "</p></a></li>");
                        }
                    }

                }
            }

            if (appMode == "Test")
                sbMenu.Append("<li class='nav-item'><a class='nav-link' href='/Admin/ChangePassword.aspx' style='border-top:1px solid #4f5962;'><i class='far fa-circle nav-icon'></i><p>Change Password</p></a></li>");
            else
                sbMenu.Append("<li class='nav-item'><a class='nav-link' href='../Admin/ChangePassword.aspx' style='border-top:1px solid #4f5962;'><i class='far fa-circle nav-icon'></i><p>Change Password</p></a></li>");

            sbMenu.Append("</ul>");
            return sbMenu.ToString();
        }


        /// <summary>
        /// Genearte dyanmic menu for Registrar user to manage the dynamic access panel
        /// </summary>
        /// <param name="adminUserName"></param>
        /// <returns></returns>
        public static string CreateDynamicMenuForRegistrar(string adminUserName)
        {
            StringBuilder sbMenu = new StringBuilder();
            sbMenu.Append(" <ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>");

            if (!string.IsNullOrEmpty(adminUserName))
            {
                List<AdminUserRight> objList = AdminDB.GetAllRightsByAdminUserName(adminUserName);
                if (objList.Count > 0)
                {
                    foreach (AdminUserRight right in objList)
                    {
                        Int32 menuid;
                        Int32.TryParse(right.MenuId.ToString().Trim(), out menuid);
                        if (menuid > 0)
                        {
                            AdminMenu objMenu = AdminMenuDB.GetMenuDetailById(menuid);
                            if (objMenu != null)
                                sbMenu.Append("<li class='nav-item'><a class='nav-link' href='" + objMenu.Url.Trim() + "'><i class='far fa-circle nav-icon'></i><p>" + objMenu.DisplayText.Trim() + "</p></a></li>");
                        }
                    }

                }
            }

            if (appMode == "Test")
                sbMenu.Append("<li class='nav-item'><a class='nav-link' href='/Registrar/ChangePassword.aspx' style='border-top:1px solid #4f5962;'><i class='far fa-circle nav-icon'></i><p>Change Password</p></a></li>");
            else
                sbMenu.Append("<li class='nav-item'><a class='nav-link' href='../Registrar/ChangePassword.aspx' style='border-top:1px solid #4f5962;'><i class='far fa-circle nav-icon'></i><p>Change Password</p></a></li>");

            sbMenu.Append("</ul>");
            return sbMenu.ToString();
        }



        /// <summary>
        /// Get the user public IP address
        /// </summary>
        /// <returns></returns>
        public static string GetUserIPAddress()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Trim();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress.Trim();
            }
            return VisitorsIPAddr;
        }
    }
}