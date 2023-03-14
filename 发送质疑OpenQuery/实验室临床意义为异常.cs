实验室临床意义为异常

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

                //ClinicalSigificane.Code != null means the dictionary value is ticked

                if (dp1.ClinicalSignificance != null && dp1.ClinicalSignificance.Code != null)

                {

                    //ClinicalSignificance.Code.Code.ToString() == "AbCS" means clinical significance is abnormal, this coded value can be find in Lab Module

                    if (dp1.ClinicalSignificance.Code.Code.ToString() == "AbCS")

                    {

                        openQuery = true;

                    }

                }

//Open query use the PerformQueryAction Method

                CustomFunction.PerformQueryAction(querytext, 1, false, false, dp1, openQuery, afp.CheckID, afp.CheckHash);

            }

 return null;
