规定受试者编号录入方式

//Get the DataPoint from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

            bool fire = false;

 

            // use Regex to define an entry format for Subject ID

            Regex myreg = new Regex(@"[a-zA-Z][a-zA-Z\-][a-zA-Z\-][a-zA-Z\-]");

 

            //Open query once the data is not matched with the myreg format

            if (!myreg.IsMatch(dp.Data.ToString()) && dp.Data != string.Empty) fire = true;

 

            //Open query use the PerformQueryAction Method

            CustomFunction.PerformQueryAction(27154, 1, false, false, dp, fire);

         return null;

 
