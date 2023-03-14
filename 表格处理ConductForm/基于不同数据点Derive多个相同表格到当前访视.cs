                基于不同数据点Derive多个相同表格到当前访视

 

            const string PAGE_FORMOID = "EG";

            const string PTP_FIELDOID = "EGTPT";

            const string PAGE_FOLDEROID = "TREAT_P1";

            const string NO_FIELDOID = "EGSPID1";

            const string PAGE_SUBFOLDEROID = "P1_D0";

            string[,] LIST_NO =

            {

            {

 

                "1st ECG of triplet", "1"

            }

 

            ,

 

            {

 

                "2nd ECG of triplet", "2"

 

            }

 

            ,

 

            {

 

                "3rd ECG of triplet", "3"

 

            }

 

            }

 

            ;

 

            //Planned time point

 

            string[,] LIST_PTP =

 

            {

            {

 

                "predose", ""

 

            }

            ,

 

            {

 

                "1 HOUR POST", "100"

 

            }

            ,

 

            {

 

                "2 HOUR POST", "200"

 

            }

 

            ,

 

            {

 

                "3 HOUR POST", "300"

 

            }

            ,

 

            {

                "4 HOUR POST", "400"

 

            }

            ,

            {

                "6 HOUR POST", "600"

 

            }

            ,

            {

                "12 HOUR POST", "1200"

 

            }

            ,

        }

            ;

            // ---- OTHER CONSTANTS ----------------------------------- //

 

            int NO_OF_MEASUREMENTS_PER_PTP = LIST_NO.GetLength(0);

 

            int NO_OF_PAGES = NO_OF_MEASUREMENTS_PER_PTP * LIST_PTP.GetLength(0);

 

            // -------------------------------------------------------- //

            //Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

            //Get current subject

            Subject sub = dp.Record.Subject;

            Instance instRepeatPage = sub.Instances.FindByFolderOID(PAGE_FOLDEROID);

 

            if (PAGE_SUBFOLDEROID != string.Empty)

                instRepeatPage = instRepeatPage.Instances.FindByFolderOID(PAGE_SUBFOLDEROID);

            DataPages dpgsRepeat = instRepeatPage.DataPages.FilterDataPagesByFormOID(PAGE_FORMOID);

 

            if (dpgsRepeat.Count > 1)  return null;

 

            Form frm = dp.Record.Subject.CRFVersion.Forms.FindByOID(PAGE_FORMOID);

            //add pages, expecting that the first one exists already!

            for (int i = 1; i < NO_OF_PAGES; i++)

            {

                instRepeatPage.AddCRF(frm, dp.Record.SubjectMatrixID);

            }

            dpgsRepeat = instRepeatPage.DataPages.FilterDataPagesByFormOID(PAGE_FORMOID);

 

            foreach (DataPage dpgRepeat in dpgsRepeat)

            {

                //if (dpgRepeat.Form.OID != PAGE_FORMOID)

                // continue;

                DataPoint dpPlannedTimepoint = dpgRepeat.MasterRecord.DataPoints.FindByFieldOID(PTP_FIELDOID);

                DataPoint dpNo = dpPlannedTimepoint.Record.DataPoints.FindByFieldOID(NO_FIELDOID);

 

                //get list index

                int iPTP = dpgRepeat.PageRepeatNumber / NO_OF_MEASUREMENTS_PER_PTP;

                int iNO = dpgRepeat.PageRepeatNumber % NO_OF_MEASUREMENTS_PER_PTP;

 

                //update form name

                dpgRepeat.Name = dpgRepeat.Form.Name + " " + LIST_PTP[iPTP, 0] + " " + LIST_NO[iNO, 0];

 

                //set planned time point and ECG no.

                dpPlannedTimepoint.Enter(LIST_PTP[iPTP, 1], "", 0);

                dpPlannedTimepoint.IsVisible = true;

                dpNo.Enter(LIST_NO[iNO, 1], "", 0);

            }

 

            return null;
