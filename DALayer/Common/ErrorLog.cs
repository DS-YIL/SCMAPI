﻿using DALayer.Emails;
using SCMModels.SCMModels;
using System;
using System.Data.SqlClient;
using System.Web;

namespace DALayer.Common
{
	public class ErrorLog
	{
		private IEmailTemplateDA emailTemplateDA = default(IEmailTemplateDA);
		public void ErrorMessage(string controllername, string methodname, string exception)
		{
			exception = exception.Replace("'", String.Empty);
			YSCMEntities DB = new YSCMEntities();
			string query = "insert into dbo.ApiErrorLog(ControllerName,MethodName,ExceptionMsg,OccuredDate,URL)values('" + controllername+"', '"+methodname+"', '"+exception+ "','"+ DateTime.Now + "','" + HttpContext.Current.Request.Url + "')";
			SqlConnection con = new SqlConnection(DB.Database.Connection.ConnectionString);
			SqlCommand cmd = new SqlCommand(query, con);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
			this.emailTemplateDA.sendErrorLog(controllername, methodname, exception, HttpContext.Current.Request.Url);

		}
	}
}