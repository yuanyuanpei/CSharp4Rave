Lead Question为否，其他变量不为空

//This CF is prepared for the Form that can add logs and has more than one non-log variable.

            //The check for all log variables need to add a non-log variable in check action with Ispresent parameter.

            //Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint input_dp = afp.ActionDataPoint;

 

            //Define the query text

            string QUERY_TEXT = Localization.GetLocalDataString(123, "eng");

 

            //Get all the data point in current page

            DataPoints dps = input_dp.Record.DataPage.GetAllDataPoints();

            if (dps == null) return null;

            //checkbox=1 or YN=N, is log or non-log var

            bool openQuery = false;

 

            //excute when the lead question is check box or radio button with YN dictionary

            if ((input_dp.Field.ControlType == "CheckBox" && input_dp.Data == "1") || (input_dp.Field.ControlType != "CheckBox" && input_dp.Data == "2"))

            {

                //Loops all the data points

                foreach (DataPoint tmp_dp in dps)

                {

                    if (tmp_dp != null && tmp_dp.Active)

                    {

                        //avoid some var if need

                        if (tmp_dp.Field.OID == input_dp.Field.OID || tmp_dp.Field.DoesNotBreakSignature == true || tmp_dp.Field.DefaultValue != null || (tmp_dp.Field.IsLog == true && tmp_dp.Record.RecordPosition == 0) || (tmp_dp.Field.IsLog == false && tmp_dp.Record.RecordPosition > 0) || tmp_dp.IsVisible == false) continue;

                        //Open Query Once the data point is checked or entered

                        if ((tmp_dp.Field.ControlType == "CheckBox" && tmp_dp.Data == "1") || (tmp_dp.Field.ControlType != "CheckBox" && !CustomFunction.DataPointIsEmpty(tmp_dp)))

                        {

                            openQuery = true;

                            break;

                        }

                    }

                }

            }

            //Open query use the PerformQueryAction Method

            CustomFunction.PerformQueryAction(QUERY_TEXT, 1, false, false, input_dp, openQuery, afp.CheckID, afp.CheckHash);

return null;
