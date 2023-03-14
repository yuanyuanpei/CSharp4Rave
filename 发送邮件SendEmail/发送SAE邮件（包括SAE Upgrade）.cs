                            发送SAE邮件（包括SAE Upgrade）

 

        //Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dpSerious = afp.ActionDataPoint;

 

            //Get the current Subject

            Subject currentSubject = dpSerious.Record.Subject;

 

            // Per Site and Role send email

            string email = "";

 

            //Define the EDC role name which need to receive the email

            Users a = currentSubject.StudySite.Users;

            foreach (User b in a)

            {

                if (b.Active && b.IsUserAssociatedWithStudySite(currentSubject.StudySite))

                {

                    Role brl = b.UsersRoleInStudy(currentSubject.StudySite.Study);

                    string rname = (brl != null) ? brl.RoleName : string.Empty;

                    if (rname == "Clinical Research Associate" || rname == "Clinical Research Coordinator")

                    {

                        email += b.Email.ToString() + ";";

                    }

                }

            }

            //Define the email subject, from, body, to variable

            string subject = string.Empty, body = string.Empty, to = string.Empty;

            string from = "noreply@mdsol.com";

            int iChanged = 0;

 

            string lineno = dpSerious.Record.RecordPosition.ToString();

            int j = 0;

            DateTime DT_aut = DateTime.MinValue;

            foreach (DataPoint dp in dpSerious.Record.DataPoints)

            {

                if (dp.IsObjectChanged)

                {

                    iChanged = iChanged + 1;

                }

            }

            if (iChanged == 0)

            {

                return null;

            }

            //Define the to email list according to different environment

            switch (currentSubject.StudySite.Study.Environment.ToUpper())

            {

                case "DEV":

                    to = "";

                    break;

                case "UAT":

                    to = "";

                    break;

                case "PROD":

                    to = email;

                    break;

                default:

                    to = "";

                    break;

            }

            //Get the audits of the data point

            Audits audits = Audits.Load(dpSerious);

            for (int i = 0; i < audits.Count; i++)

            {

                if (audits[i].Category.Name == "Data" && audits[i].AuditTime < DateTime.Now && audits[i].AuditTime > DT_aut)

                {

                    DT_aut = audits[i].AuditTime;

                }

            }

            if (dpSerious.Data == "01")

            {

                if (dpSerious.IsObjectChanged)

                {

                    if (DT_aut == DateTime.MinValue) //new SAE

                    {

                        subject = String.Format("SAE Added! Site: {0}, Subject: {1}, AE Line #: {2}", currentSubject.StudySite.Site.Number, currentSubject.Name, lineno);

                        body = BodyForNewAE(dpSerious);

                    }

                    else if (DT_aut > DateTime.MinValue)

                    {

                        for (int i = 0; i < audits.Count; i++)

                        {

                            if (audits[i].Category.Name == "Data" && audits[i].AuditTime == DT_aut)

                            {

                                if (audits[i].Readable.Contains("Yes (01)")) //updated SAE

                                {

                                    subject = String.Format("SAE updated! Site: {0}, Subject: {1}, AE Line #: {2}", currentSubject.StudySite.Site.Number, currentSubject.Name, lineno);

                                    body = BodyForUpdatedAE(dpSerious);

                                }

                                else //upgraded SAE

                                {

                                    subject = String.Format("SAE Added! Site: {0}, Subject: {1}, AE Line #: {2}", currentSubject.StudySite.Site.Number, currentSubject.Name, lineno);

                                    body = BodyForNewAE(dpSerious);

                                }

                            }

                        }

 

                    }

 

                }

 

                else if (!dpSerious.IsObjectChanged) //updated SAE

                {

                    subject = String.Format("SAE updated! Site: {0}, Subject: {1}, AE Line #: {2}", currentSubject.StudySite.Site.Number, currentSubject.Name, lineno);

                    body = BodyForUpdatedAE(dpSerious);

                }

 

                Message.SendEmail(to, from, subject, body);

 

            }

            else if (dpSerious.Data == "02")

            {

                for (int i = 0; i < audits.Count; i++)

                {

                    if (audits[i].Category.Name == "Data" && audits[i].AuditTime == DT_aut)

                    {

                        if (audits[i].Readable.Contains("Yes (01)")) //downgraded SAE

                        {

                            subject = String.Format("SAE changed to regular AE! Site: {0}, Subject: {1}, AE Line #: {2}", currentSubject.StudySite.Site.Number, currentSubject.Name, lineno);

                            body = BodyForDownAE(dpSerious);

                            Message.SendEmail(to, from, subject, body);

                        }

                    }

                }

 

            }

            return null;

        }

 

        string BodyForNewAE(DataPoint dpSerious)

        {

            string body = "A new serious AE has been recorded by " + dpSerious.Interaction.TrueUser.Login + Environment.NewLine + Environment.NewLine;

 

            foreach (DataPoint dp in dpSerious.Record.DataPoints)

            {

                if (dp.Field.OID == "AESER") continue;

                body += string.Format("{0}: {1}", dp.Field.PreText, dp.UserValue());

                body += Environment.NewLine;

                body += Environment.NewLine;

            }

 

            return body;

        }

 

        string BodyForUpdatedAE(DataPoint dpSerious)

        {

            DateTime DT_aut2;

            string body = "An existing serious AE has been updated by " + dpSerious.Interaction.TrueUser.Login + Environment.NewLine + Environment.NewLine;

            body += "The following fields have been updated:" + Environment.NewLine;

 

            foreach (DataPoint dp in dpSerious.Record.DataPoints)

            {

                DT_aut2 = DateTime.MinValue;

                if (dp.IsObjectChanged)

                {

                    Audits audits2 = Audits.Load(dp);

                    for (int i = 0; i < audits2.Count; i++)

                    {

                        if (audits2[i].Category.Name == "Data" && audits2[i].AuditTime < DateTime.Now && audits2[i].AuditTime > DT_aut2)

                        {

                            DT_aut2 = audits2[i].AuditTime;

                        }

                    }

                    for (int i = 0; i < audits2.Count; i++)

                    {

                        if (audits2[i].Category.Name == "Data" && audits2[i].AuditTime == DT_aut2 && !audits2[i].Readable.Contains(dp.Data))

                        {

                            body += string.Format("{0}: {1}", dp.Field.PreText, dp.UserValue());

                            body += Environment.NewLine;

                            body += Environment.NewLine;

                        }

                        if (audits2[i].Category.Name == "Data" && audits2[i].AuditTime == DT_aut2 && !audits2[i].Readable.Contains("entered empty") && dp.Data == "")

                        {

                            body += string.Format("{0}: {1}", dp.Field.PreText, dp.UserValue());

                            body += Environment.NewLine;

                            body += Environment.NewLine;

                        }

                    }

                }

            }

            return body;

        }

 

        string BodyForDownAE(DataPoint dpSerious)

        {

            string body = "A serious AE has been downgraded to Non-serious AE by " + dpSerious.Interaction.TrueUser.Login + Environment.NewLine + Environment.NewLine;

 

            foreach (DataPoint dp in dpSerious.Record.DataPoints)

            {

                if (dp.Field.OID == "AESER") continue;

                body += string.Format("{0}: {1}", dp.Field.PreText, dp.UserValue());

                body += Environment.NewLine;

                body += Environment.NewLine;

            }

 

            return body;
