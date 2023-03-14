实验室范围特定单位时不在正常范围中

//Get data point from Check Step         

            DataPoint dp = (DataPoint)ThisObject;

            double i = 0;

            //only excute when the lab has been added for this lab page and the entered data is a numeric value

            if (dp.Record.DataPage.Lab != null && Double.TryParse(dp.Data.ToString(), out i))

            {

                //fire query once the dp value is not within the normal range and the Lab unit is equal to a specify unit

                if ((i > 0 && i < 3.4 && dp.AnalyteRange.EnteredLabUnit.Name.ToString() == "mmol/L") || (i > 0 && i < 300 && dp.AnalyteRange.EnteredLabUnit.Name.ToString() == "mg/dL"))

                {

                    return true;

                }

            }

 return false;
