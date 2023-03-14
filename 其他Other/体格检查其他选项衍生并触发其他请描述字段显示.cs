                        体格检查其他选项衍生并触发其他请描述字段显示

 

        //get the target fields OID

            const string TESTPOINT = "PETEST";

            const string OTHERPOINT = "PEOTH";

            const string OTHERVALUE = "99";

            // Number of log lines before Other,Coded value of PETEST

            const int LOGLINES = 13;

 

            //Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint aPoint = afp.ActionDataPoint;

 

            //derive data to the new log record and set the OTHER data point visible

            addOther(aPoint, TESTPOINT, OTHERPOINT, OTHERVALUE, LOGLINES);

 

            return null;

        }

 

        private void addOther(DataPoint aPoint, string TESTPOINT, string OTHERPOINT, string OTHERVALUE, int LOGLINES)

        {

            int j = LOGLINES;

            Records logRecs = aPoint.Record.DataPage.Records;

            //Loops the current data page records

            for (int i = j; i < logRecs.Count; i++)

            {

                //get the new log record fields

                DataPoints dPoints = logRecs[i].DataPoints;

                DataPoint dpTEST = dPoints.FindByFieldOID(TESTPOINT);

 

                //derive data to the new log record and set dpOTHER visible

                if (dpTEST.Data == string.Empty)

                {

                    dpTEST.Enter(OTHERVALUE, string.Empty, 0);

                    DataPoint dpOTHER = dPoints.FindByFieldOID(OTHERPOINT);

                    dpOTHER.IsVisible = true;

                }

            }
