设置动态访视窗

//Get the DataPoint from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

            Instances insts = dp.Record.Subject.Instances;

            DataPoint C1D1_Date = insts.FindByFolderOID("C1D1").DataPages.FindByFormOID("SV").MasterRecord.DataPoints.FindByFieldOID("SVDAT");

            Instance C1D1 = insts.FindByFolderOID("C1D1");

 

            //Set the visit date of C1D1 as Time Zero

            DateTime dt = DateTime.MinValue;

            if (C1D1_Date.Data == string.Empty || C1D1_Date.EntryStatus == EntryStatusEnum.NonConformant) return null;

            if (C1D1_Date.StandardValue() is DateTime)

            {

                dt = Convert.ToDateTime(C1D1_Date.StandardValue());

                C1D1.SetTimeZero(dt);

            }

 

            //use the instance property Target/StartWindow/EndWindow to set the dynamic visit window.

            Instance SCREEN = insts.FindByFolderOID("SCREEN");

            Instance C2D1 = insts.FindByFolderOID("C2D1");

 

            if (SCREEN != null)

            {

                SCREEN.Target = dt.AddDays(-21);

                SCREEN.StartWindow = dt.AddDays(-21);

                SCREEN.EndWindow = dt.AddDays(-1);

            }

            if (C2D1 != null)

            {

                C2D1.Target = dt.AddDays(14);

                C2D1.StartWindow = dt.AddDays(11);

                C2D1.EndWindow = dt.AddDays(17);

            }

 

            //accoding to specific requirement to set Dynamic window

            DataPoints DOSEXs = CustomFunction.FetchAllDataPointsForOIDPath("DOSSEQ", null, null, dp.Record.Subject);

            int i = 0, x = 0;

            foreach (DataPoint dp1 in DOSEXs)

            {

                Instance inst = dp1.Record.DataPage.Instance;

 

                if (dp1.Data != string.Empty && dp1.Data != "99")

                {

                    x = 14 * (Convert.ToInt32(dp1.Data) + 5);

                    inst.Target = dt.AddDays(x);

                    inst.StartWindow = dt.AddDays(x - 3);

                    inst.EndWindow = dt.AddDays(x + 3);

                }

                if (dp1.Data == "99")

                {

                    DataPoint DOSSEQO = dp1.Record.DataPoints.FindByFieldOID("DOSSEQO");

                    if (DOSSEQO.Data != string.Empty && int.TryParse(DOSSEQO.Data, out i))

                    {

                        x = 14 * (i - 1);

                        inst.Target = dt.AddDays(x);

                        inst.StartWindow = dt.AddDays(x - 3);

                        inst.EndWindow = dt.AddDays(x + 3);

                    }

                }

            }

 return null;
