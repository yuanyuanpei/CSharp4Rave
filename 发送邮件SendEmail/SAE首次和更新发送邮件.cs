
SAE首次和更新发送邮件

//Get AESER DataPoint passed in from Rave

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dpAESER = afp.ActionDataPoint;

 

 

            string subject, body, to;

            const string from = "noreply@mdsol.com";

            Subject currentSubject = dpAESER.Record.Subject;

 

            //Use string.Format to create a subject

            //with the Subject Name and Site Number

            //send email only the SAE is ticked with Yes

            if (dpAESER.Data == "1")

            {

                if (dpAESER.IsObjectChanged) //New

                {

                    subject = String.Format(@"Five Prime SAE - Concomitant Medications Added at Record {2}! (Site: {0}, Subject {1})", currentSubject.StudySite.Site.Name, currentSubject.Name, dpAESER.Record.RecordPosition);

                    body = BodyForNewAE(dpAESER);

                }

                else //Updated

                {

                    subject = String.Format(@"Five Prime SAE - Concomitant Medications Updated at Record {2}! (Site: {0}, Subject {1})", currentSubject.StudySite.Site.Name, currentSubject.Name, dpAESER.Record.RecordPosition);

                    body = BodyForUpdatedAE(dpAESER);

                }

 

 

                //Chose recipient based on current environment

                switch (currentSubject.StudySite.Study.Environment.ToUpper())

                {

                    case "DEV":

                        to = "kris.gao@macrostat.com";

                        break;

                    case "PROD":

                        to = "pvg@goldenzone.co.uk";

                        break;

                    case "UAT":

                        to = "mila.zhou@macrostat.com;";

                        break;

                    default:

                        to = "noreply@mdsol.com";

                        break;

 

                }

               //send email

               Message.SendEmail(to, from, subject, body);

               return null;

            }

        }

        //Composes the body of the email message for NEW serious AEs.

        //Lists all fields and their values of the current AE log line

        string BodyForNewAE(DataPoint dpAESER)

        {

            String body = string.Format("Serious Adverse Events - Concomitant Medications Page has been recorded by {0} {1} at {2}", dpAESER.Interaction.TrueUser.FirstName, dpAESER.Interaction.TrueUser.LastName, DateTime.Now.AddHours(8).ToString("yyyy/MM/dd HH:mm:ss"))

            + Environment.NewLine

            + Environment.NewLine;

 

            foreach (DataPoint dp in dpAESER.Record.DataPoints)

            {

                if (!dpAESER.IsObjectChanged) continue;

                body += string.Format("{0} : {1}", dp.Field.PreText, dp.UserValue()) + Environment.NewLine;

                body += Environment.NewLine;

            }

            return body;

        }

 

        //Composes the body of the email message for UPDATED serious AEs.

        //Lists only updated fields and their values of the current AE log line

        string BodyForUpdatedAE(DataPoint dpAESER)

        {

            string body = string.Format("Serious Adverse Events - Concomitant Medications Page has been updated by {0} {1} at {2}", dpAESER.Interaction.TrueUser.FirstName, dpAESER.Interaction.TrueUser.LastName, DateTime.Now.AddHours(8).ToString("yyyy/MM/dd HH:mm:ss"))

            + Environment.NewLine;

            body += "The following *(asterisk) fields have been updated."

            + Environment.NewLine

            + Environment.NewLine;

            foreach (DataPoint dp in dpAESER.Record.DataPoints)

            {

 

                if (dp.IsObjectChanged)

                {

                    body += string.Format("{0}*: {1}", dp.Field.PreText, dp.UserValue()) + Environment.NewLine;

                    body += Environment.NewLine;

                }

                else //changed

                {

                    body += string.Format("{0}: {1}", dp.Field.PreText, dp.UserValue()) + Environment.NewLine;

                    body += Environment.NewLine;

                }

            }

 return body;
