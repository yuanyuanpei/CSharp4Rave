发送SAE时带上更新之前的数据

//Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint; 

            //Get current subject

            Subject subj = dp.Record.Subject;

 

            //Get the information need to be sent

            string folderOID = dp.Record.DataPage.Instance.Folder.OID;

            DataPoint dpAETERM = dp.Record.DataPoints.FindByFieldOID("AETERM");

            string strStudy = dp.Record.Subject.StudySite.Study.Name;

            string strCountry = dp.Record.Subject.StudySite.Site.SiteGroup.Name;

            string strSiteNum = dp.Record.Subject.StudySite.Site.Number; 

            string strSUBJID = dp.Record.Subject.PrimaryName;

 

            //Get the user name who enter the SAE data

            string strCreatedOrUpdatedBy = "Created by: " + dp.Interaction.TrueUser.FirstName + " " + dp.Interaction.TrueUser.LastName + " [" + dp.Interaction.TrueUser.Login + "]";

            DataPoints dps = dpAETERM.Record.DataPoints; bool blnAddFirst = true; bool blnSendEmail = false; string NewLine = Environment.NewLine;

            string strStatusSAE1 = "new AE/SAE"; string strStatusSAE2 = "entered"; StringBuilder strUpdatedItems = new StringBuilder("");

 

            //Define the email from, to , subject, contents

            Subject currentSubject = dp.Record.Subject; string from = "codeitol@mdsol.com"; string to = "";

            StringBuilder strSubject = new StringBuilder(""); StringBuilder strMessage = new StringBuilder("");

 

            //Define email address according to EDC role name

            Users a = currentSubject.StudySite.Users;

            foreach (User b in a)

            {

                if (b.Active)

                {

                    Role brl = b.UsersRoleInStudy(currentSubject.StudySite.Study); string rname = (brl != null) ? brl.RoleName : string.Empty;

                    if (rname == "Site Manager") to += b.Email.ToString() + ";";

                }

            }

            ////Define email address according to study environment

            switch (subj.StudySite.Study.Environment.ToString().ToUpper())

            {

                case "DEV":

                    strSubject.Append("**TEST AE/SAE Email** " + "AE/SAE email notification (Study " + strStudy + " / Country " + strCountry + " / Site " + strSiteNum + " / Patient " + strSUBJID + ") - " + strStatusSAE1);

                    strMessage.Append("ATTENTION! A AE/SAE has been " + strStatusSAE2 + ". Please review ASAP at https://" + "www.imedidata.com/.");

                    to += ""; break;

                case "PROD":

                    strSubject.Append("AE/SAE email notification (Study " + strStudy + " / Country " + strCountry + " / Site " + strSiteNum + " / Patient " + strSUBJID + ") - " + strStatusSAE1);

                    strMessage.Append("ATTENTION! A AE/SAE has been " + strStatusSAE2 + ". Please review ASAP at https://" + "www.imedidata.com/.");

                    to += ""; break;

                default:

                    to += ""; break;

            }

            strMessage.Append(NewLine + NewLine + "Study: " + strStudy + NewLine);

            strMessage.Append(NewLine + "Country: " + strCountry + NewLine);

            strMessage.Append(NewLine + "Site number: " + strSiteNum + NewLine);

            strMessage.Append(NewLine + "Subject number: " + strSUBJID + NewLine);

            strMessage.Append(NewLine + "AE/SAE log line number: " + dpAETERM.Record.RecordPosition.ToString() + NewLine);

            if (dpAETERM.Data != string.Empty)

            {

                blnSendEmail = true;

            }

            //Loops all the data points

            for (int i = 0; i < dps.Count; i++)

            {

                if (dps[i].IsVisible && dps[i].Data.Length > 0)

                {

                    strMessage.Append(NewLine + dps[i].Field.PreText + ": " + dps[i].UserValue().ToString() + NewLine);

                }

                if ((dps[i].Name == "AETERM" && dps[i].IsObjectChanged && dps[i].ChangeCount > 1) || (dps[i].Name != "AETERM" && dps[i].IsObjectChanged && dps[i].ChangeCount > 1 && dps[i].IsVisible))

                {

                    //Get all the Audits and Get the previous data

                    string strDpPrevUserValue = string.Empty; Audits at = Audits.Load(dps[i]);

                    for (int k = 0; k < at.Count; k++)

                    {

                        if (at[k] != null)

                        {

 

                            if (at[k].SubCategory == AuditSubCategory.EnteredEmpty || at[k].SubCategory == AuditSubCategory.EnteredEmptyWithChangeCode) break;

 

                            else if (at[k].SubCategory == AuditSubCategory.Entered || at[k].SubCategory == AuditSubCategory.EnteredWithChangeCode || at[k].SubCategory == AuditSubCategory.EnteredNonConformant || at[k].SubCategory == AuditSubCategory.EnteredInForeignLocale || at[k].SubCategory == AuditSubCategory.EnteredInForeignLocaleWithChangeCode)

                            {

                                int startIndex = at[k].Readable.IndexOf("'"); int endIndex = at[k].Readable.LastIndexOf("'");

                                if (startIndex > -1 && endIndex > 0 && endIndex > startIndex)

                                    strDpPrevUserValue = at[k].Readable.Substring(startIndex + 1, ((endIndex - startIndex) - 1)).Trim(); break;

                            }

                        }

                    }

                    if (strDpPrevUserValue.Length > 0 && dps[i].Variable.DataDictionaryID > 0)

                    {

                        int startIndex = strDpPrevUserValue.IndexOf("(");

                        if (startIndex > 0) strDpPrevUserValue = strDpPrevUserValue.Substring(0, startIndex - 1);

                    }

                    string strDpCurUserValue = string.Empty;

                    if (dps[i].Data.Length > 0)

                    { strDpCurUserValue = dps[i].UserValue().ToString(); }

                    if (strDpPrevUserValue.ToUpper() != strDpCurUserValue.ToUpper())

                    {

                        if (blnAddFirst)

                        {

                            strStatusSAE1 = "updated SAE"; strStatusSAE2 = "updated";

                            strCreatedOrUpdatedBy = "Updated by: " + dpAETERM.Interaction.TrueUser.FirstName + " " + dpAESER.Interaction.TrueUser.LastName + " [" + dpAESER.Interaction.TrueUser.Login + "]";

                            strUpdatedItems.Append(NewLine + "Updated items:" + NewLine); blnAddFirst = false;

                        }

                        if (dps[i].ID == dpAETERM.ID && strDpPrevUserValue.ToUpper() == "YES") blnSendEmail = true;

                        else if (dps[i].ID == dpAETERM.ID)

                        { strStatusSAE1 = "new SAE"; strStatusSAE2 = "entered"; }

                        strUpdatedItems.Append(dps[i].Field.PreText + " has been changed to '" + strDpCurUserValue + "'." + NewLine);

                    }

                }

            }

            

            strMessage.Append(NewLine + strCreatedOrUpdatedBy + NewLine); strMessage.Append(NewLine + strUpdatedItems.ToString() + NewLine);

            strMessage.Append(NewLine + NewLine + NewLine + "*** This message was generated by an automated system, please DO NOT reply to this message. ***");

            strMessage.Replace("<b>", ""); strMessage.Replace("</b>", ""); strMessage.Replace("<B>", ""); strMessage.Replace("</B>", "");

            strMessage.Replace("<i>", ""); strMessage.Replace("</i>", ""); strMessage.Replace("<I>", "");

            strMessage.Replace("</I>", ""); strMessage.Replace("<br />", " "); strMessage.Replace("<BR />", " ");

 

            //send email once the condition is true

            if (blnSendEmail) Message.SendEmail(to, from, strSubject.ToString(), strMessage.ToString());

return null;
