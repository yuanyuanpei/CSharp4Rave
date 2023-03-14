质疑文字中嵌入特定变量的值

//Get the DataPoint from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            bool openQuery = false;

            //Define a query text use [fieldOID]

            string queryText = @"This is a test CustomFunction,[VISDAT]";

 

            if (dp.Data != string.Empty && dp.EntryStatus != EntryStatusEnum.NonConformant)

            {

                openQuery = true;

            }

            //use string.Replace() function to replace the data point value

            queryText = queryText.Replace("[VISDAT]", "<core:link text='datapoint.uservalue' formoid='SV' folderoid='C1' fieldoid='VISDAT'/>");

            

            //open query once the condition is true

            CustomFunction.PerformQueryAction(queryText, 1, false, false, dp, openQuery);

            return null;
