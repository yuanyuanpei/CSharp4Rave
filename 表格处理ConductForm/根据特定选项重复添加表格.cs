根据特定选项重复添加表格

//Get data point from edit check

             DataPoint dp = ((ActionFunctionParams)ThisObject).ActionDataPoint;

 

            if (!CustomFunction.DataPointIsEmpty(dp))

            {

                DataPage dpg = dp.Record.DataPage;

                Instance ins = dpg.Instance;

 

                DataPages dpgs = ins.DataPages;

 

                bool isAdd = true;

                // check whether there has the same form OID without any data entry

                for (int i = 0; i < dpgs.Count; i++)

                {

                    if (!dpgs[i].Active)

                        continue;

                    //if the current form is empty,don't add form

                    if (dpgs[i].Form.OID == dpg.Form.OID)

                    {

                        if (dpgs[i].EntryStatus == EntryStatusEnum.NoData)

                        {

                            isAdd = false;

                            break;

                        }

                    }

                }

                //if the current form is not empty, add form

                if (isAdd)

                    ins.AddCRF(dpg.Form, dp.Record.SubjectMatrixID);

            }

  return null;
