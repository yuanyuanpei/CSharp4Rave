设置AEMH动态搜索列表

//define AE and MH fileds OID

            const string CMINDC_FIELDOID = "CMINDC";

            const string AESTART_FIELDOID = "AESTDAT";

            const string AETERM_FIELDOID = "AETERM";

            const string AE_FORMOID = "AE";

            const string AE_FOLDEROID = "VIF";

 

            const string MHSTART_FIELDOID = "MHSTDAT";

            const string MHTERM_FIELDOID = "MHTERM";

            const string MH_FORMOID = "MH";

            const string MH_FOLDEROID = "VIF";

            const int DSL_FIELD_LENGTH = 90;

 

            //Get data point from edit check

            DataPoint dpDsl = ((Medidata.Core.Objects.DynamicSearchParams)ThisObject).DataPoint;

            Subject sub = dpDsl.Record.Subject;

 

            //Define a new KeyValueCollection varaible

            Medidata.Utilities.KeyValueCollection kvcList = new Medidata.Utilities.KeyValueCollection();

 

            //Get the indication data point

            DataPoint dpCmindc = dpDsl.Record.DataPoints.FindByFieldOID(CMINDC_FIELDOID);

 

            //if indication is Adverse Events

            if (dpCmindc.Data.ToString() == "1")

            {

                //Get the AE data page 

                DataPoints dpsAE = CustomFunction.FetchAllDataPointsForOIDPath(AESTART_FIELDOID, AE_FORMOID, AE_FOLDEROID, sub);

                DataPage dpgAE = dpsAE[0].Record.DataPage;

 

                //Loops the records in AE data page

                for (int i = 1; i < dpgAE.Records.Count; i++)

                {

                    Record recAE = dpgAE.Records.FindByRecordPosition(i);

                    if (recAE.Active)

                    {

                        DataPoint dpAeTerm = recAE.DataPoints.FindByFieldOID(AETERM_FIELDOID);

                        if (!CustomFunction.DataPointIsEmpty(dpAeTerm))

                        {

                            DataPoint dpAeStartDate = recAE.DataPoints.FindByFieldOID(AESTART_FIELDOID);

 

                            //use the BuildEntry function to generate the AE value, then collect in kvAE list

                            string entry = BuildEntry(recAE.RecordPosition, dpAeTerm.Data, dpAeStartDate.Data, DSL_FIELD_LENGTH);

                            Medidata.Utilities.KeyValue kvAE = new Medidata.Utilities.KeyValue(entry, entry);

                            kvcList.Add(kvAE);

                        }

                    }

                }

            }

            //if indication is Medical History

            else if (dpCmindc.Data.ToString() == "2")

            {

                //Get the MH data page

                DataPage dpgMH = sub.Instances.FindByFolderOID(MH_FOLDEROID).DataPages.FindByFormOID(MH_FORMOID);

 

                //Loops the records in MH data page

                for (int j = 1; j < dpgMH.Records.Count; j++)

                {

                    Record recMH = dpgMH.Records.FindByRecordPosition(j);

                    if (recMH.Active)

                    {

                        DataPoint dpMhTerm = recMH.DataPoints.FindByFieldOID(MHTERM_FIELDOID);

                        if (!CustomFunction.DataPointIsEmpty(dpMhTerm))

                        {

                            DataPoint dpMhStartDate = recMH.DataPoints.FindByFieldOID(MHSTART_FIELDOID);

 

                            //use the BuildEntry function to generate the AE value, then collect in kvMH list

                            string entry = BuildEntry(recMH.RecordPosition, dpMhTerm.Data, dpMhStartDate.Data, DSL_FIELD_LENGTH);

                            Medidata.Utilities.KeyValue kvMH = new Medidata.Utilities.KeyValue(entry, entry);

                            kvcList.Add(kvMH);

                        }

                    }

                }

            }

            return kvcList;

        }

        // this function is use to generate the AE or MH value

        private string BuildEntry(int recPos, string term, string date, int targetFieldLength)

        {

            string myRecPos = recPos.ToString();

            string myTerm = term.Trim();

            int delimterLength = 6;

            //6 is length of twice " - "

 

            int remainingLength = targetFieldLength - (myRecPos.Length + date.Length + delimterLength);

            if (myTerm.Length > remainingLength)

            {

                myTerm = myTerm.Substring(0, remainingLength - 3) + "...";

                //3 is length of "..."

            }

            string entry = recPos.ToString() + " - " + myTerm;

            if (date != string.Empty)

            {

                entry += (" - " + date);

            }

 return entry;
