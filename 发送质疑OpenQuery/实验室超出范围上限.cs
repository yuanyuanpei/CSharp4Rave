实验室超出范围上限

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            //Get the current subject

            Subject sj = dp.Record.Subject;

 

            //Get the target data point

            DataPoint dp2 = dp.Record.DataPoints.FindByFieldOID("LBCRES02");

            DataPoint MS = sj.Instances.FindByFolderOID("SC").DataPages.FindByFormOID("TU").MasterRecord.DataPoints.FindByFieldOID("METSLOC4");

            DataPoint IE = sj.Instances.FindByFolderOID("SC").DataPages.FindByFormOID("IE").MasterRecord.DataPoints.FindByFieldOID("EXRES13");

 

            //Define query text

            string queryText = "Metastasis Site is not checked as Liver, and ALT/AST> 2.5*ULN in Screening Visit, however EC#13 is not checked, please verify.";

            bool openQuery = false;

 

            if (!CustomFunction.DataPointIsEmpty(dp) && !CustomFunction.DataPointIsEmpty(dp2))

            {

                //only excute when the lab has been added for this lab page

                if (dp.Record.DataPage.Lab != null)

                {

                    //Convert the data from string to double

                    double DP = Convert.ToDouble(dp.Data);

                    double DP2 = Convert.ToDouble(dp2.Data);

                    //get the High range of the dp's Analyterange

                    double high = dp.AnalyteRange.HighRange;

                    double high2 = dp2.AnalyteRange.HighRange;

 

                    //fire query once the dp value is not within normal range

                    if ((DP > 2.5 * high || DP2 > 2.5 * high) && MS.Data == "0" && IE.Data == "0")

                    {

                        openQuery = true;

                    }

                }

            }

            //fire query use the PerformQueryAction method

            CustomFunction.PerformQueryAction(queryText, 1, false, false, IE, openQuery);

 return null;
