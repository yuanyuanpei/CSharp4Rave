                      日期时间结合比较（日期和时间组合在一起进行大小的比较）

          //Get the target fields OID

            const string DATE1 = "FACSDAT";

            const string TIME1 = "FACSTIM";

            const string TFOLDER = "SDA";

            const string TPAGE = "EX";

            const string DATE2 = "EXDAT";

            const string TIME2 = "EXTIM";

            const string EXCYC = "D_CYCLE";

            //Define query text

            const string QueryText = "\"Time of Sample Collection\" is not prior to Study drug administration time at the current visit, please check and update as appropriate.";

 

            //Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint aPoint = afp.ActionDataPoint;

            bool OpenQuery = false;

 

            //Compare the two date time use the GetTheDifference Method

            double valueDiff = getTheDifference(aPoint, TFOLDER, TPAGE, EXCYC, DATE1, TIME1, DATE2, TIME2);

 

            //Open Query once the valueDiff is greater than or equal to 0

            if (valueDiff >= 0)

                OpenQuery = true;

 

            ////Open query use the PerformQueryAction Method

            CustomFunction.PerformQueryAction(QueryText, 1, false, false, aPoint, OpenQuery, afp.CheckID, afp.CheckHash);

            return null;

        }

 

        private double getTheDifference(DataPoint aPoint, string TFOLDER, string TPAGE, string EXCYC, string DATE1, string TIME1, string DATE2, string TIME2)

        {

            //Get the current record

            Record currRec = aPoint.Record;

 

            //Get the current subject and current folder

            Subject currSubj = currRec.Subject;

            string currFold = currRec.Instance.Folder.OID;

 

            //Get the target instance and data page,all records

            Instance targInst = currSubj.Instances.FindByFolderOID(TFOLDER);

            DataPage tarPage = targInst.DataPages.FindByFormOID(TPAGE);

            Records allRecs = tarPage.Records;

 

            double combEX = 0;

            double combOTH = 0;

            bool OpenQuery = false;

 

            //Loops all target records

            for (int i = 0; i < allRecs.Count; i++)

            {

                if (allRecs[i].Active)

                {

                    DataPoints dPointsEX = allRecs[i].DataPoints;

                    DataPoint dpCYC = dPointsEX.FindByFieldOID(EXCYC);

 

                    if (dpCYC.Data != string.Empty && dpCYC.Data == currFold)

                    {

                        //Get the first date value and  time value

                        DataPoint dpDATEX = dPointsEX.FindByFieldOID(DATE2);

                        DataPoint dpTIMEX = dPointsEX.FindByFieldOID(TIME2);

 

                        if (dpDATEX.Data != string.Empty && dpTIMEX.Data != string.Empty)

                        {

                            //combine the first date value and time value, convert to a double value

                            combEX = getDateTimeValue(dpDATEX, dpTIMEX);

 

                            //Get the second date value and  time value

                            DataPoint dpDATOTH = currRec.DataPoints.FindByFieldOID(DATE1);

                            DataPoint dpTIMOTH = currRec.DataPoints.FindByFieldOID(TIME1);

 

                            if (dpDATOTH.Data != string.Empty && dpDATOTH.Active && dpTIMOTH.Data != string.Empty && dpTIMOTH.Active)

                            {

                                //combine the second date value and time value, convert to a double value

                                combOTH = getDateTimeValue(dpDATOTH, dpTIMOTH);

                                break;

                            }

                        }

                    }

                }

            }

            //return the result of the two double value

            return combOTH - combEX;

        }

 

        private double getDateTimeValue(DataPoint dpDAT, DataPoint dpTIM)

        {

            double vDAT = 0;

            double vTIM = 0;

 

            if (dpDAT.Data != string.Empty && dpTIM.Data != string.Empty && dpDAT.StandardValue() is DateTime && dpTIM.StandardValue() is TimeSpan)

            {

                //Get the date and time value

                DateTime dtDate1 = (DateTime)dpDAT.StandardValue();

                TimeSpan dtTime1 = (TimeSpan)dpTIM.StandardValue();

 

                //Convert the date and time value to a double value

                vDAT = (dtDate1.ToFileTime() / 10000000) / 60;

                vTIM = dtTime1.TotalMinutes;

            }

            //return the total double value

            return vDAT + vTIM;

 
