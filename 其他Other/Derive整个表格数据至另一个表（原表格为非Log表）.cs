Derive整个表格数据至另一个表（原表格为非Log表）

//Get data point form edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

            //find the source fields 

            DataPoint BRTHDAT = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("BRTHDAT");

            DataPoint AGE = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("AGE");

            DataPoint SEX = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("SEX");

            DataPoint RACE = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("RACE");

            DataPoint RACEOTH = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("RACEOTH");

 

            //find the target fields

            DataPage CR_DM = dp.Record.Subject.DataPages.FindByFormOID("CR_DM");

            if (CR_DM == null) return null;

            DataPoint CR_BRTHDAT = CR_DM.MasterRecord.DataPoints.FindByFieldOID("BRTHDAT");

            DataPoint CR_AGE = CR_DM.MasterRecord.DataPoints.FindByFieldOID("AGE");

            DataPoint CR_SEX = CR_DM.MasterRecord.DataPoints.FindByFieldOID("SEX");

            DataPoint CR_RACE = CR_DM.MasterRecord.DataPoints.FindByFieldOID("RACE");

            DataPoint CR_RACEOTH = CR_DM.MasterRecord.DataPoints.FindByFieldOID("RACEOTH");

            CR_DM.Freeze();

            //Derive the source data to the target data page

            derive(BRTHDAT, CR_BRTHDAT);

            derive(AGE, CR_AGE);

            derive(SEX, CR_SEX);

            derive(RACE, CR_RACE);

            derive(RACEOTH, CR_RACEOTH);

 

            return null;

        }

        public void derive(DataPoint DP1, DataPoint CR_DP)

        {

            //Unfreeze the target data point ,and derive the source data to the target data point, then freeze

            if (DP1.Data != string.Empty)

            {

                CR_DP.UnFreeze();

                CR_DP.Enter(DP1.Data.ToString(), string.Empty, 0);

                CR_DP.Freeze();

            }

            //if the source data is empty,derive string.empty to the target data point

            else

            {

                CR_DP.UnFreeze();

                CR_DP.Enter(string.Empty, string.Empty, 0);

                CR_DP.Freeze();

            }
