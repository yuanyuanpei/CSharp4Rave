重复加SAE表格并命名

//Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dpSERIOUS = afp.ActionDataPoint;

 

            //Get the current instance and subject

            Instance current_Inst = dpSERIOUS.Record.DataPage.Instance;

            Subject subj = dpSERIOUS.Record.Subject;

            //Get the MatrixID and SAE form

            int subjectMatrixID = dpSERIOUS.Record.SubjectMatrixID;

            Form SAE = subj.CRFVersion.Forms.FindByOID("SAE");

 

            try

            {

                //only excute when the dpSeriours is ticked

                if (dpSERIOUS.Data.ToString() == "1")

                {

                    //Add SAE form

                    AddSAEForm(dpSERIOUS, current_Inst, SAE, subjectMatrixID);

                }

            }

            catch

            {

            }

 

            return null;

        }

 

        private string NewSAEFormName(DataPage dpg, DataPoint dp)

        {

            return "Serious Adverse Event" + "_Line " + dp.Record.RecordPosition.ToString();

        }

 

        private void AddSAEForm(DataPoint dp, Instance Inst, Form Form, int MatrixID)

        {

            string SAEFormName = "Serious Adverse Event" + "_Line " + dp.Record.RecordPosition.ToString();

 

            //if the current page has been added, return null

            if (FindDatapageByName(Inst, SAEFormName) != null)

            {

                return;

            }

            //if current page is not exsit, add the SAE page

            Inst.AddCRF(Form, MatrixID);

            DataPage dpgNew = FindDatapageByName(Inst, Form.Name);

 

            //set a name for the new SAE page

            dpgNew.Name = NewSAEFormName(dpgNew, dp);

            //assign value for the fields of the new SAE page

            dpgNew.MasterRecord.DataPoints.FindByFieldOID("AENUM").Enter(dp.Record.RecordPosition.ToString(), null, 0);

            dpgNew.MasterRecord.DataPoints.FindByFieldOID("AETERM").Enter(dp.Record.DataPoints.FindByFieldOID("AETERM").Data.ToString(), null, 0);

            dpgNew.MasterRecord.DataPoints.FindByFieldOID("AESTDAT").Enter(dp.Record.DataPoints.FindByFieldOID("AESTDAT").Data.ToString(), null, 0);

        }

 

        private DataPage FindDatapageByName(Instance instance, string name)

        {

            foreach (DataPage dpg in instance.DataPages)

            {

                if (dpg.Name == name)

                {

                    return dpg;

                }

            }

 return null;
