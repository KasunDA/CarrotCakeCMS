﻿using System;
using System.Net.Mail;
using System.Web;
using System.Xml;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	public class EMailSettings {
		public EMailSettings() {
			this.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
			this.MailDomainName = String.Empty;
			this.MailUserName = String.Empty;
			this.MailPassword = String.Empty;
			this.ReturnAddress = String.Empty;
		}

		public SmtpDeliveryMethod DeliveryMethod { get; set; }
		public string MailDomainName { get; set; }
		public string MailUserName { get; set; }
		public string MailPassword { get; set; }
		public string ReturnAddress { get; set; }

		public static EMailSettings GetEMailSettings() {
			HttpContext context = HttpContext.Current;

			EMailSettings mailSettings = new EMailSettings();

			//parse web.config as XML because of medium trust issues
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(context.Server.MapPath("~/Web.config"));

			XmlElement xmlMailSettings = xDoc.SelectSingleNode("//system.net/mailSettings/smtp") as XmlElement;

			if (xmlMailSettings != null) {
				if (xmlMailSettings.Attributes["from"] != null) {
					mailSettings.ReturnAddress = xmlMailSettings.Attributes["from"].Value;
				}
				if (xmlMailSettings.Attributes["deliveryMethod"] != null && xmlMailSettings.Attributes["deliveryMethod"].Value.ToLowerInvariant() == "network") {
					mailSettings.DeliveryMethod = SmtpDeliveryMethod.Network;
					if (xmlMailSettings.HasChildNodes) {
						XmlNode xmlNetSettings = xmlMailSettings.SelectSingleNode("//system.net/mailSettings/smtp/network");
						if (xmlNetSettings != null && xmlNetSettings.Attributes["password"] != null) {
							mailSettings.MailUserName = xmlNetSettings.Attributes["userName"].Value;
							mailSettings.MailPassword = xmlNetSettings.Attributes["password"].Value;
							mailSettings.MailDomainName = xmlNetSettings.Attributes["host"].Value;
						}
					}
				}
			}

			if (String.IsNullOrEmpty(mailSettings.MailDomainName)) {
				mailSettings.MailDomainName = context.Request.ServerVariables["SERVER_NAME"];
			}

			if (String.IsNullOrEmpty(mailSettings.ReturnAddress)) {
				mailSettings.ReturnAddress = "no-reply@" + mailSettings.MailDomainName;
			}

			return mailSettings;
		}
	}
}