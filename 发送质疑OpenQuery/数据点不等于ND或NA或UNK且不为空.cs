数据点不等于ND或NA或UNK且不为空

//Get the DataPoint from Check Step

            DataPoint dp = (DataPoint)ThisObject;

 

            if (dp.Data != string.Empty && dp.Data != "ND" && dp.Data != "NA" && dp.Data != "UNK")

            {

                return true;

            }

 return false;
