计算Logline字段值总和

//Get data point from edit check

            DataPoint TLRES_dp = (DataPoint)ThisObject;

            Records TL_REC = TLRES_dp.Record.DataPage.Records;

            DataPoint TLRES = null;

            double SUM_TLRES = 0;

            //Loop all data points of the current record

            for (int i = 1; i < TL_REC.Count; i++)

            {

                if (TL_REC[i].Active)

                {

                    //Get the specify data point

                    TLRES = TL_REC[i].DataPoints.FindByFieldOID("TLRES");

                    //caclulate the sum of the data point

                    if (TLRES != null && TLRES.StandardValue() != null && !TLRES.StandardValue().ToString().ToUpper().Contains("UN") && !TLRES.StandardValue().ToString().ToUpper().Contains("UNK"))

                    {

                        SUM_TLRES = SUM_TLRES + Convert.ToDouble(TLRES.StandardValue());

                    }

                }

            }

            // return the sum value

 return SUM_TLRES;
