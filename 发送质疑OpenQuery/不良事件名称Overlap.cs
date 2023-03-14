不良事件名称Overlap

//Define a query text 

            string QUERY_TEXT = "The same AE term is reported more than once. Please check if any overlap on AE duration.";

 

            //Get the target fields OID

            const string fieldOID = "AETERM";

            const string fieldOID_STDAT = "AESTDAT";

            const string fieldOID_ENDAT = "AEENDAT";

            //optional fields OID

            const string fieldOID_STTIM = "AESTTIM";

            const string fieldOID_ENTIM = "AEENTIM";

 

            //Get the data point from edit Check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint input_dp = afp.ActionDataPoint;

 

            //Get all records of the current data page

            Records allRecords = input_dp.Record.DataPage.Records;

 

            //Sort all the records according to the record position

            allRecords = GetSortedRecords(allRecords);

 

            bool openQuery = false;

            DataPoint i_dp = null;

            DataPoint j_dp = null;

            DateTime i_dp_STDTC, i_dp_ENDTC, j_dp_ENDTC, j_dp_STDTC;

 

            //Loops all the records in current data page

            for (int i = allRecords.Count - 1; i > 1; i--)

            {

                if (allRecords[i].Active)

                {

                    //Get the current date and time fields

                    i_dp = allRecords[i].DataPoints.FindByFieldOID(fieldOID);

                    //Get the current record AE start date and time

                    i_dp_STDTC = getDateTime(allRecords[i].DataPoints.FindByFieldOID(fieldOID_STDAT), allRecords[i].DataPoints.FindByFieldOID(fieldOID_STTIM));

                    //Get the current record AE End date and time

                    i_dp_ENDTC = getDateTime(allRecords[i].DataPoints.FindByFieldOID(fieldOID_ENDAT), allRecords[i].DataPoints.FindByFieldOID(fieldOID_ENTIM));

 

                    openQuery = false;

                    if (i_dp_STDTC != DateTime.MaxValue)

                    {

                        //Loops agian for all the records in current data page

                        for (int j = i - 1; j > 0; j--)

                        {

                            if (allRecords[j].Active)

                            {

                                //Get the previous date and time fields

                                j_dp = allRecords[j].DataPoints.FindByFieldOID(fieldOID);

                                //Get the previous record AE start date and time

                                j_dp_STDTC = getDateTime(allRecords[j].DataPoints.FindByFieldOID(fieldOID_STDAT), allRecords[j].DataPoints.FindByFieldOID(fieldOID_STTIM));

                                //Get the previous record AE End date and time

                                j_dp_ENDTC = getDateTime(allRecords[j].DataPoints.FindByFieldOID(fieldOID_ENDAT), allRecords[j].DataPoints.FindByFieldOID(fieldOID_ENTIM));

                                //Compare the current record datetime and the previous record datetime

                                if (string.Compare(i_dp.Data, j_dp.Data, true) == 0 && i_dp_STDTC != DateTime.MaxValue && j_dp_STDTC != DateTime.MaxValue && i_dp_STDTC <= j_dp_ENDTC && i_dp_ENDTC >= j_dp_STDTC)

                                {

                                    openQuery = true;

                                    break;

                                }

                            }

                        }

                    }

                    //Open query use the PerformQueryAction Method

                    CustomFunction.PerformQueryAction(QUERY_TEXT, 1, false, false, i_dp, openQuery, afp.CheckID, afp.CheckHash);

                }

            }

            return null;

        }

        //The below function is use to sort the records 

        Records GetSortedRecords(Records unSortedRecords)

        {

            //Use the record position to sort the collected records

            Record[] tmpRecords = new Record[unSortedRecords.Count];

            for (int i = 0; i < unSortedRecords.Count; i++)

            {

                Record record = unSortedRecords[i];

                tmpRecords[record.RecordPosition] = record;

            }

 

            //return a sorted records

            Records sortedRecords = new Records();

            for (int i = 0; i < tmpRecords.Length; i++)

            {

                sortedRecords.Add(tmpRecords[i]);

            }

            return sortedRecords;

        }

        //The below function is use to combine the AE date and time

        DateTime getDateTime(DataPoint DAT, DataPoint TIM)

        {

            //Combine the date and time

            DateTime tmp_DTC = DateTime.MaxValue;

            if (DAT != null && DAT.StandardValue() is DateTime)

            {

                if (TIM != null && TIM.Data != string.Empty && TIM.StandardValue() is TimeSpan)

                {

                    tmp_DTC = Convert.ToDateTime(DAT.StandardValue()).Add((TimeSpan)(TIM.StandardValue()));

                }

                else

                {

                    tmp_DTC = DateTime.Parse(DAT.StandardValue().ToString());

                }

            }

 return tmp_DTC;
