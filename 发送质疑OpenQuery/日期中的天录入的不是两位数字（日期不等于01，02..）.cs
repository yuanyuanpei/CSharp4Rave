日期中的天录入的不是两位数字（日期不等于01，02..）

//Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint input_dp = afp.ActionDataPoint;

 

            bool OpenQuery = false;

            //Define query text

            string QueryText = "The day is not in the correct format. Please provide double digit value for day (e.g., 03).";

 

            //only excute when the data is a DateTime value

            if (input_dp != null && input_dp.Active && !CustomFunction.DataPointIsEmpty(input_dp) && input_dp.StandardValue() is DateTime)

            {

                string dayString = input_dp.Data;

                //use the string.Split() method to split the data point value,and get the day part

                string[] dayValues = dayString.Split('/');

                if (dayValues.Length > 0)

                {

                    //when the lengh is equal to 1 and the value in with 0-9, fire query

                    if (dayValues[0].Length == 1)

                    {

                        //use Regex to compare the data format

                        if (Regex.IsMatch(dayValues[0], "^[0-9]{1}$"))

                        {

                            OpenQuery = true;

                        }

 

                    }

                }

            }

            //Open query use the PerformQueryAction Method

            CustomFunction.PerformQueryAction(QueryText, 1, false, false, input_dp, OpenQuery, afp.CheckID, afp.CheckHash);

 return null;
