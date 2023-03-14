基于前置条件无上限添加相同页面

            //Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint input_DP = afp.ActionDataPoint;

 

            //Get current subject and instance,datapages

            Subject subj = input_DP.Record.Subject;

            Instance ins = input_DP.Record.DataPage.Instance;

            DataPages PGs = (ins == null) ? subj.DataPages : ins.DataPages;

 

            string currForm_OID = input_DP.Record.DataPage.Form.OID;

            int currPageRptNo = input_DP.Record.DataPage.PageRepeatNumber;

            string AddPageY = "1";

 

            // add page when addPageY is Yes

            if (input_DP.Data == AddPageY)

            {

                // put all page repeat number in the list arraylist

                ArrayList list = new ArrayList();

                foreach (DataPage pg in PGs)

                    if (pg.Form.OID == currForm_OID)

                    {

                        int pgRpt = pg.PageRepeatNumber;

                        list.Add(pgRpt);

                        if (pg.PageRepeatNumber == 0)

                        {

                            pg.Name = pg.Form.Name + " 1";

                        }

                    }

                // add page only when there is no page repeat number is greater than the current one and rename the page name per repeat number

 

                if (!list.Contains(currPageRptNo + 1))

                {

                    DataPage newAdd_PG = AddForm(input_DP);

                    int FormRptNo = currPageRptNo + 2;

 

                    newAdd_PG.Name = newAdd_PG.Form.Name + " " + FormRptNo;

                }

                // activate pages when their page repeat numbers are graeter than the current one.

                else

                {

                    foreach (DataPage pg in PGs)

                    {

                        if (pg.PageRepeatNumber > currPageRptNo)

                            pg.Active = true;

                    }

                }

            }

            // when addPageY is not Yes, inactivate pages when their page repeat numbers are graeter than the current one.

            else

            {

                foreach (DataPage pg in PGs)

                {

                    if (pg.Active && pg.Form.OID == currForm_OID && pg.PageRepeatNumber > currPageRptNo)

                        pg.Active = false;

                }

            }

            return null;

        }

        DataPage AddForm(DataPoint input_DP)

        {

            string form_OID = input_DP.Record.DataPage.Form.OID;

 

            Subject subj = input_DP.Record.Subject;

            Instance ins = input_DP.Record.DataPage.Instance;

 

            // if the form is under subject level, then container will be subj, otherwise it will be ins

            IDataContainer ctnr = (ins == null) ? (IDataContainer)subj : ins;

 

            Form newForm = Form.FetchByOID(form_OID, subj.CRFVersion.ID);

            DataPage newPage = new DataPage(ctnr, newForm, input_DP.Record.SubjectMatrixID);

            ctnr.DataPages.Add(newPage);

 

            return newPage;

 
