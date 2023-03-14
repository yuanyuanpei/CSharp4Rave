检查AE编号（如已选择的AE在原AE表格上被更新则报不规则数据的标记）

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint input_DP = afp.ActionDataPoint;

 

            //Get the current subject

            Subject subj = input_DP.Record.Subject;

            string str = string.Empty;

 

            //Get the AETERM datapoints

            DataPoints AETERMs = CustomFunction.FetchAllDataPointsForOIDPath("AETERM", "AE", null, subj);

 

            //Define a arrary list to collect all the AE

            ArrayList arlValues = new ArrayList();

            arlValues.Add(string.Empty);

 

            //Loops the AETERM Data point

            for (int i = 0; i < AETERMs.Count; i++)

            {

                DataPoint dpTerm = AETERMs[i];

                if (!dpTerm.Record.Active) continue;

 

                //Get the AESTDAT data point

                DataPoint AESTDAT = dpTerm.Record.DataPoints.FindByFieldOID("AESTDAT");

 

                //Get the AE Number,this code need to be changed if the AE page is a logline form

                string recPos = dpTerm.Record.RecordPosition.ToString();

                

                //collect all the AE into the arlValues

                str = recPos + " - " + dpTerm.Data.ToString() + " - " + AESTDAT.Data.ToString();

                arlValues.Add(str);

 

            }

 

            //list all the indication fields

            string[] DSLFields =

            {

                 "PRINDC", "CMINDC"

            }

            ;

 

            //Loops all fields which use the AE dynamic searchlist

            for (int i = 0; i < DSLFields.Length; i++)

            {

                DataPoints dpsDSL = CustomFunction.FetchAllDataPointsForOIDPath(DSLFields[i], null, null, subj);

                //Loops all the datapoints for the specific field

                for (int j = 0; j < dpsDSL.Count; j++)

                {

                    DataPoint dpDSL = dpsDSL[j];

                    if (!dpDSL.Record.Active) continue;

                    //Once the result is AE, check the AENO

                    if (dpDSL.Data == "1")

                    {

                        //loops all the AEMHNO fields and set Nonconformant

                        foreach (DataPoint dpAE in dpDSL.Record.DataPoints)

                        {

                            if (dpAE.Field.OID == "AEMHNO1" || dpAE.Field.OID == "AEMHNO2" || dpAE.Field.OID == "AEMHNO3" || dpAE.Field.OID == "AEMHNO4" || dpAE.Field.OID == "AEMHNO5" || dpAE.Field.OID == "AEMHNO6")

                            {

                                //if the value is not contained in the arlValues, set the data point Nonconformant

                                bool IsNonConformant = !arlValues.Contains((dpAE.Data).ToString());

                                dpAE.SetNonConformant(IsNonConformant);

                            }

                        }

                    }

 

                }

            }

            return null;
