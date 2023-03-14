遍历整个Site找重复的受试者筛选号

//Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dpEC = afp.ActionDataPoint;

 

            bool flag = false;

 

            //Get subject ID

            int ID = dpEC.Record.Subject.ID;

 

            //Define a query text

            string QUERY_MESSAGE = "2-Digit screening # is duplicated. Please verify.";

 

            if (!CustomFunction.DataPointIsEmpty(dpEC))

            {

                //Try **** Catch means find the exceptions, when no exception, excute the try codes, if any exception, excute the catch codes

                try

                {

                    // find all subject screening number within all study sites and exclude the current subject

                    string[] allSEQNO = dpEC.Record.Subject.StudySite.GetAllValuesForOIDPath(null, "SUBJECT", "SUBJER", ID);

 

                    //Loops all the SEQNO data points

                    for (int i = 0; i < allSEQNO.Length; i++)

                    {

                        // if any other value equal to the current value, fire query.

                        if (allSEQNO[i] == dpEC.Data.ToString()) flag = true;

                    }

                }

                //return null if any exception happens

                catch

                {

                    return null;

                }

            }

            //Open query use the PerformQueryAction Method

            CustomFunction.PerformQueryAction(QUERY_MESSAGE, 1, true, true, dpEC, flag);

 return null;
