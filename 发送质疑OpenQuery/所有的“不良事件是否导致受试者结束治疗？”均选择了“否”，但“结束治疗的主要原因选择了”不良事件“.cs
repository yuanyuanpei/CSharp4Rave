所有的“不良事件是否导致受试者结束治疗？”均选择了“否”，但“结束治疗的主要原因选择了”不良事件“

//Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            //Get current subject

            Subject subj = dp.Record.Subject;

            bool OpenQuery = false;

 

            //Get all AEDIS data points

            DataPoints AEDISs = CustomFunction.FetchAllDataPointsForOIDPath("AEDIS", "AE", "AE", subj);

 

            //Excute when the input dp is entered with AE

            if (dp.Data == "1")

            {

                //fire query once AE is chosen

                OpenQuery = true;

 

                //Loops all AEDIS data points

                foreach (DataPoint AEDIS in AEDISs)

                {

                    if (!AEDIS.Active) continue;

                    if (AEDIS.Data == string.Empty || AEDIS.Data == "1")

                    {

                        //close query once any one data point ticked.

                        OpenQuery = false;

                        break;

                    }

                }

            }

            //Open query use the PerformQueryAction Method

            CustomFunction.PerformQueryAction(6397, 1, false, false, dp, OpenQuery);

 return null;
