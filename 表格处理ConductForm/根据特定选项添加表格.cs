根据特定选项添加表格

//Get data point from edit check

             ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            DataPages dpgs = dp.Record.Instance.DataPages;

            bool addLBURIN = true;

 

            //Loop all datapages, add form condition equal to false  if LBURIN form is already present

            foreach (DataPage dpg in dpgs)

            {

                if (dpg.Form.OID == "LBURIN") addLBURIN = false;

            }

            int i = 0;

            if (int.TryParse(dp.Data, out i))

            {

                //Add form according to the dp's data

                if (i % 6 == 1)

                {

                    if (addLBURIN == false) dp.Record.Instance.DataPages.FindByFormOID("LBURIN").Active = true;

                    if (addLBURIN == true) dp.Record.Instance.AddCRF(Form.FetchByOID("LBURIN", dp.Record.Subject.CRFVersionID), dp.Record.SubjectMatrixID);

                }

                //Inactive form if the condition is false

                if (i % 6 != 1 && addLBURIN == false) dp.Record.Instance.DataPages.FindByFormOID("LBURIN").Active = false;

            }

 return null;
