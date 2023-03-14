通过筛选期肿瘤评估信息Derive常规访视肿瘤评估信息

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

 

            //Get the current subject

            Subject subj = afp.ActionDataPoint.Record.Subject;

 

            //Get the source folder OID, form OID, field OID

            string scrfolder = "SCREEN";

            string scrform = "TL_SCR";

            string strTLYN = "TLYN";

 

            //Get the target folder OID, form OID, field OID

            string strTLLES = "TLLES";

            string strTLSIT = "TLSIT";

            string strTLSITSP = "TLSITSP";

            string strTLLOC = "TLLOC";

 

            //Get the current visit datapage

            DataPage dpgTA1 = afp.ActionDataPoint.Record.DataPage;

 

            //Get the screen visit datapage

            Records rdsTA1 = subj.Instances.FindByFolderOID(scrfolder).DataPages.FindByFormOID(scrform).Records;

 

            DataPoint dpSYN = subj.Instances.FindByFolderOID(scrfolder).DataPages.FindByFormOID(scrform).MasterRecord.DataPoints.FindByFieldOID(strTLYN);

 

            DataPoint dpYN = afp.ActionDataPoint.Record.DataPoints.FindByFieldOID(strTLYN);

 

            //Add Log record according to the logs number source data page

            if (dpSYN.Data != "1" || dpgTA1.Active == false || (dpYN.Data != "1" && dpYN.ChangeCount < 2)) return null;

            while (rdsTA1.Count > dpgTA1.Records.Count) dpgTA1.AddLogRecord();

 

            //Loops of the source records

            for (int i = 1; i < rdsTA1.Count; i++)

            {

                //Inactive the target log if the source log is inactived

                if (rdsTA1.FindByRecordPosition(i).Active == false)

                {

                    dpgTA1.Records.FindByRecordPosition(i).Active = false;

                }

                else

                {

                    //Get the source data point and target corresponding data point

                    dpgTA1.Records.FindByRecordPosition(i).Active = true;

                    DataPoint dpSTLLES = rdsTA1.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLLES);

                    DataPoint dpTLLES = dpgTA1.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLLES);

 

                    DataPoint dpSTLSIT = rdsTA1.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLSIT);

                    DataPoint dpTLSIT = dpgTA1.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLSIT);

 

                    DataPoint dpSTLSITSP = rdsTA1.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLSITSP);

                    DataPoint dpTLSITSP = dpgTA1.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLSITSP);

 

                    DataPoint dpSTLLOC = rdsTA1.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLLOC);

                    DataPoint dpTLLOC = dpgTA1.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID(strTLLOC);

 

                    //Derive the data in source data page to the target data page

                    if (dpYN.Data == "1")

                    {

                        dpTLLES.Enter(dpSTLLES.Data, null, 0);

                        dpTLSIT.Enter(dpSTLSIT.Data, null, 0);

                        dpTLSITSP.Enter(dpSTLSITSP.Data, null, 0);

                        dpTLLOC.Enter(dpSTLLOC.Data, null, 0);

                    }

                    else

                    {

                        dpTLLES.Enter("", null, 0);

                        dpTLSIT.Enter("", null, 0);

                        dpTLSITSP.Enter("", null, 0);

                        dpTLLOC.Enter("", null, 0);

                    }

                }

            }

 return null;
