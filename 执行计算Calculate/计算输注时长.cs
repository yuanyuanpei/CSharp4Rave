计算输注时长

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            //Get target data points

            DataPoint EXSTDTM = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("EXSTDTM");

            DataPoint EXENDTM = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("EXENDTM");

            DataPoint EXFLDUR = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("EXFLDUR");

            DataPoint EXITRPD = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("EXITRPD");

            DataPoint EXITRPYN = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("EXITRPYN");

            if (EXSTDTM.StandardValue() is DateTime && EXSTDTM.EntryStatus != EntryStatusEnum.NoData && EXENDTM.StandardValue() is DateTime && EXENDTM.EntryStatus != EntryStatusEnum.NoData)

            {

                //Convert the data to DateTime value

                DateTime date1 = (DateTime)EXSTDTM.StandardValue();

                DateTime date2 = (DateTime)EXENDTM.StandardValue();

                TimeSpan date3 = date2 - date1;

                //According to EXITRPYN's value to excute caclulate

                if (EXITRPYN.Data == "1" && EXITRPD.Data != string.Empty && EXITRPD.EntryStatus != EntryStatusEnum.NonConformant)

                {

                    int result = Convert.ToInt16(date3.TotalMinutes) - Convert.ToInt16(EXITRPD.Data);

                    //Assign value to EXFLDUR

                    EXFLDUR.Enter(result.ToString(), string.Empty, 0);

                }

                else if (EXITRPYN.Data == "2")

                {

                    EXFLDUR.Enter(date3.TotalMinutes.ToString(), string.Empty, 0);

                }

            }

            else

            {

                EXFLDUR.Enter(string.Empty, string.Empty, 0);

            }

 return null;
