检查MH编号（如已选择的MH编号在原MH表格上被更新则报不规则数据的标记）

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint input_DP = afp.ActionDataPoint;

 

//Get the current subject

            Subject subj = input_DP.Record.Subject;

            string str = string.Empty;

 

//Get the MHTERM datapoints

            DataPoints MHTERMs = CustomFunction.FetchAllDataPointsForOIDPath("MHTERM", "MH", null, subj);

            //Define a arrary list to collect all the MH

            ArrayList arlValues = new ArrayList();

            arlValues.Add(string.Empty);

 

            //Loops the MHTERM Data point

            for (int i = 0; i < MHTERMs.Count; i++)

            {

                DataPoint dpTerm = MHTERMs[i];

                if (!dpTerm.Record.Active) continue;

 

//Get the MHSTDAT data point

                DataPoint MHSTDAT = dpTerm.Record.DataPoints.FindByFieldOID("MHSTDAT");

 

                //Get the MH Number

                string recPos = dpTerm.Record.RecordPosition.ToString();

 

                //collect all the MH into the arlValues

                str = recPos + " - " + dpTerm.Data.ToString() + " - " + MHSTDAT.Data.ToString();

                arlValues.Add(str);

 

            }

 

            //list all the indication fields

            string[] DSLFields =

            {

                "PRINDC", "CMINDC"

            }

            ;

 

            //Loops all fields which use the MH dynamic searchlist

            for (int i = 0; i < DSLFields.Length; i++)

            {

                DataPoints dpsDSL = CustomFunction.FetchAllDataPointsForOIDPath(DSLFields[i], null, null, subj);

//Loops all the datapoints for the specific field

                for (int j = 0; j < dpsDSL.Count; j++)

                {

                    DataPoint dpDSL = dpsDSL[j];

                    if (!dpDSL.Record.Active) continue;

                    //Once the result is MH, check the MHNO

                    if (dpDSL.Data == "2")

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
