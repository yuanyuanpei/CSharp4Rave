实验室临床意义评价为空

//Get the DataPoint from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

            bool openQuery = false;

 

            //use the translation workbench to get a numeric ID for Chinese quert text

            string querytext = Localization.GetLocalDataString(4706, "eng");

 

            //Loop all datapoints on the current datapage

            DataPoints dps = dp.Record.DataPage.GetAllDataPoints();

            foreach (DataPoint dp1 in dps)

            {

                openQuery = false;

                //if ClinicalSigificane != null means the clinical sigificane dictionary is present

                //ClinicalSigificane.Code == null means the dictionary value is empty

                if (dp1.ClinicalSignificance != null && dp1.ClinicalSignificance.Code == null)

                {

                    openQuery = true;

                }

//Open query use the PerformQueryAction Method

                CustomFunction.PerformQueryAction(querytext, 1, false, false, dp1, openQuery, afp.CheckID, afp.CheckHash);

            }

 return null;
