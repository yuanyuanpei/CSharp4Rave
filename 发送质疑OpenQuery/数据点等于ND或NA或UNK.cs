数据点等于ND或NA或UNK

//Get the DataPoint from Check Step

            DataPoint dp = (DataPoint)ThisObject;

 

            if (dp.Data == "ND" || dp.Data == "NA" || dp.Data == "UNK")

            {

                return true;

            }

            return false;
