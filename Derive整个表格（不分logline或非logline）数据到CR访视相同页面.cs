               Derive整个表格（不分logline或非logline）数据到CR访视相同页面

           //Get data point form edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dpInput = afp.ActionDataPoint;

 

            //Get the current subject and record

            Subject subj = dpInput.Record.Subject;

            Records rdsBL = dpInput.Record.DataPage.Records;

 

            //Get the target folder OID and form OID

            string folderOID = "CR";

            string formOID = "CR_" + dpInput.Record.DataPage.Form.OID;

 

            //Get the target data page

            DataPage dpgCR = subj.Instances.FindByFolderOID(folderOID).DataPages.FindByFormOID(formOID);

 

            //Add the same number of Loglines to the target form

            while (rdsBL.Count > dpgCR.Records.Count) dpgCR.AddLogRecord();

 

            //Active and Inactive records in the target form

            for (int i = 0; i < rdsBL.Count; i++)

            {

                if (rdsBL.FindByRecordPosition(i).Active == false)

                {

                    dpgCR.Records.FindByRecordPosition(i).Active = false;

                }

                else

                {

                    dpgCR.Records.FindByRecordPosition(i).Active = true;

                    //Derive data to the target form

                    dpEnterCR(dpInput, rdsBL, dpgCR, i);

                }

            }

 

 

            return null;

        }

 

        private static void dpEnterCR(DataPoint dpInput, Records rdsBL, DataPage dpgCR, int i)

        {

            //Loops all the data point in source data page

            foreach (DataPoint dp in dpInput.Record.DataPoints)

            {

                string fieldOID = dp.Field.OID;

                DataPoint dpNew = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID(fieldOID);

 

                if (dpNew.Active || dpNew.IsObjectChanged)

                {

                    //Derive the data to the corresponding data point in the target data page

                    DataPoint dpCR = dpgCR.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID(fieldOID);

                    dpCR.Enter(dpNew.Data, null, 0);

                }

                ;

            }
