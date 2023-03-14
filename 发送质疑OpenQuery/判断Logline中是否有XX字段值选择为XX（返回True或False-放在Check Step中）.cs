判断Logline中是否有XX字段值选择为XX（返回True或False-放在Check Step中）

//Get data point from check step

            DataPoint input_dp = (DataPoint)ThisObject;

 

            //Get the current form OID and folder OID

            string FormOID = input_dp.Record.DataPage.Form.OID;

            string FolderOID = input_dp.Record.DataPage.Instance.Folder.OID;

 

            //Get the current data points

            DataPoints dps = CustomFunction.FetchAllDataPointsForOIDPath(input_dp.Field.OID, FormOID, FolderOID, input_dp.Record.Subject);

 

            //Loops the data points

            foreach (DataPoint dp in dps)

            {

                if (!dp.Active) continue;

                //if data point equal to the specify value, return false,else return true

                if (dp.Data == "1")

                {

                    return false;

                    break;

                }

            }

return true;
