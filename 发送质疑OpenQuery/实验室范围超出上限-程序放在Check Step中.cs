实验室范围超出上限-程序放在Check Step中

//Get data point from Check Step

            DataPoint dp = (DataPoint)ThisObject;

 

            double i = 0;

            //only excute when the lab has been added for this lab page and the entered data is a numeric value

            if (dp.Record.DataPage.Lab != null && Double.TryParse(dp.Data.ToString(), out i))

            {

                //Get the Lab High Range

                double high = dp.AnalyteRange.HighRange;

                //fire query once the dp value is not within normal range

                if (i > 2 * high)

                {

                    return true;

                }

            }

            return false;
