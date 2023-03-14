Derive整个表格数据至另一个表（原表格为Log表）

//Get data point form edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            //Define two DataPoints Class

            DataPoints new_dps = new DataPoints();

            string total = null;

            DataPoints new_dps1 = new DataPoints();

            string total1 = null;

            

            //get the source data page and source fields

            DataPoint CMYN = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("CMYN");

            DataPoints CMTRTs = CustomFunction.FetchAllDataPointsForOIDPath("CMTRT", "CM", "COM", dp.Record.Subject);

            int recordposition = 0;

 

            //get the target data page

            DataPage CR_CM = dp.Record.Subject.DataPages.FindByFormOID("CR_CM");

            if (CR_CM == null) return null;

            DataPoint CR_CMYN = CR_CM.MasterRecord.DataPoints.FindByFieldOID("CMYN");

            //Add log record according to the logs number in the source data page

            for (int i = 0; i < dp.Record.DataPage.Records.Count; i++)

            {

                if (dp.Record.DataPage.Records.Count == CR_CM.Records.Count)

                {

                    break;

                }

                CR_CM.AddLogRecord();

            }

 

            //Derive data to the same log in target page

            for (int i = 0; i < CMTRTs.Count; i++)

            {

                total = null;

                total1 = null;

                new_dps = new DataPoints();

                new_dps1 = new DataPoints();

                recordposition = CMTRTs[i].Record.RecordPosition;

 

                //find the source DataPoints

                DataPoint CMTRT = CMTRTs[i];

                DataPoint CMDSTXT = CMTRT.Record.DataPoints.FindByFieldOID("CMDSTXT");

                DataPoint CMDOSU = CMTRT.Record.DataPoints.FindByFieldOID("CMDOSU");

                DataPoint CMDOSUO = CMTRT.Record.DataPoints.FindByFieldOID("CMDOSUO");

                DataPoint CMDOSFRQ = CMTRT.Record.DataPoints.FindByFieldOID("CMDOSFRQ");

                DataPoint CMDOSFO = CMTRT.Record.DataPoints.FindByFieldOID("CMDOSFO");

                DataPoint CMSTDAT = CMTRT.Record.DataPoints.FindByFieldOID("CMSTDAT");

                DataPoint CMENDAT = CMTRT.Record.DataPoints.FindByFieldOID("CMENDAT");

                DataPoint CMONGO = CMTRT.Record.DataPoints.FindByFieldOID("CMONGO");

                DataPoint CMTREAS = CMTRT.Record.DataPoints.FindByFieldOID("CMTREAS");

                DataPoint CMTREASO = CMTRT.Record.DataPoints.FindByFieldOID("CMTREASO");

 

                DataPoints rec_dps = CMTRT.Record.DataPoints;

                //collect the source AENOs datapoint 

                foreach (DataPoint dp1 in rec_dps)

                {

                    if (dp1.Field.OID.Contains("CMAENO") && dp1.Data != string.Empty)

                    {

                        new_dps.Add(dp1);

                    }

                }

                //combine the AENOs data to a string variable

                for (int j = 0; j < new_dps.Count; j++)

                {

                    total = total + new_dps[j].Data;

                    if (j < new_dps.Count - 1)

                    {

                        total = total + "/";

                    }

                }

                //collect the source MHNOs datapoint 

                foreach (DataPoint dp1 in rec_dps)

                {

                    if (dp1.Field.OID.Contains("CMMHNO") && dp1.Data != string.Empty)

                    {

                        new_dps1.Add(dp1);

                    }

                }

                //combine the MHNOs data to a string variable

                for (int z = 0; z < new_dps1.Count; z++)

                {

                    total1 = total1 + new_dps1[z].Data;

                    if (z < new_dps1.Count - 1)

                    {

                        total1 = total1 + "/";

                    }

                }

                //find the target fields

                DataPoint CR_CMSPID = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMSPID");

                DataPoint CR_CMTRT = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMTRT");

                DataPoint CR_CMDSTXT = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMDSTXT");

                DataPoint CR_CMDOSU = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMDOSU");

                DataPoint CR_CMDOSUO = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMDOSUO");

                DataPoint CR_CMDOSFRQ = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMDOSFRQ");

                DataPoint CR_CMDOSFO = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMDOSFO");

                DataPoint CR_CMSTDAT = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMSTDAT");

                DataPoint CR_CMENDAT = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMENDAT");

                DataPoint CR_CMONGO = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMONGO");

                DataPoint CR_CMTREAS = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMTREAS");

                DataPoint CR_CMTREASO = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMTREASO");

                DataPoint CR_CMAENO = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMAENO");

                DataPoint CR_CMMHNO = CR_CM.Records.FindByRecordPosition(recordposition).DataPoints.FindByFieldOID("CMMHNO");

 

                //for the speicfy fields, derive data to the target datapage

                CR_CM.Records.FindByRecordPosition(recordposition).Freeze();

                CR_CMSPID.UnFreeze();

                CR_CMSPID.Enter(recordposition.ToString(), string.Empty, 0);

                CR_CMSPID.Freeze();

                CR_CMAENO.UnFreeze();

                CR_CMAENO.Enter(total, string.Empty, 0);

                CR_CMAENO.Freeze();

                CR_CMMHNO.UnFreeze();

                CR_CMMHNO.Enter(total1, string.Empty, 0);

                CR_CMMHNO.Freeze();

                //Derive the source datapage data to the target datapage

                derive(CMTRT, CR_CMTRT);

                derive(CMDSTXT, CR_CMDSTXT);

                derive(CMDOSU, CR_CMDOSU);

                derive(CMDOSUO, CR_CMDOSUO);

                derive(CMDOSFRQ, CR_CMDOSFRQ);

                derive(CMDOSFO, CR_CMDOSFO);

                derive(CMSTDAT, CR_CMSTDAT);

                derive(CMENDAT, CR_CMENDAT);

                derive(CMONGO, CR_CMONGO);

                derive(CMTREAS, CR_CMTREAS);

                derive(CMTREASO, CR_CMTREASO);

 

                //Inactive the target log if the source log is inactived

                if (CMTRT.Record.Active == false) CR_CMTRT.Record.Active = false;

                else CR_CMTRT.Record.Active = true;

            }

            derive(CMYN, CR_CMYN);

            return null;

        }

        public void derive(DataPoint DP1, DataPoint CR_DP)

        {

//Unfreeze the target data point ,and derive the source data to the target data point, then freeze

            if (DP1.Data != string.Empty)

            {

                CR_DP.UnFreeze();

                CR_DP.Enter(DP1.Data.ToString(), string.Empty, 0);

                CR_DP.Freeze();

            }

//if the source data is empty,derive string.empty to the target data point

            else

            {

                CR_DP.UnFreeze();

                CR_DP.Enter(string.Empty, string.Empty, 0);

                CR_DP.Freeze();

            }
