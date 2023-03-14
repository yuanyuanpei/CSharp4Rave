时间允许录入未知UN（时间录入UN:UN不报质疑标记和质疑文字）

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            //Define an Open Query Boolean value and Query text

            bool openQuery = false;

            string QueryText = "Data entered is non-conformant. Please review.";

 

            //when the data point is not empty and the value is Nonconformant

            if (dp.Active && dp.Data != string.Empty && dp.EntryStatus == EntryStatusEnum.NonConformant)

            {

                //when the value is "UN:UN",set the data point status as conformant

                if (dp.Data.ToString().ToUpper() == "UN:UN")

                    dp.SetNonConformant(false, false);

                // else open query and set data point as Nonconformant

                else

                {

                    openQuery = true;

                    dp.SetNonConformant(true);

                }

            }

            //Open query use the PerformQueryAction Method

            CustomFunction.PerformQueryAction(QueryText, 1, false, false, dp, openQuery, afp.CheckID, afp.CheckHash);

 

 return null;
