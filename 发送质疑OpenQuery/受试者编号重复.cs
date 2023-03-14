受试者编号重复

//Get the DataPoint from Check Step

            DataPoint thisDP = (DataPoint)ThisObject;

 

            int ID = thisDP.Record.Subject.ID;

 

            bool isDuplicate = false;

 

            if (!CustomFunction.DataPointIsEmpty(thisDP))

            {

                //Loop all datapoints except the current one

                string[] allSUBNUM = thisDP.Record.Subject.StudySite.GetAllValuesForOIDPath(null, "SC", "SUBJNUM", ID);

                for (int i = 0; i < allSUBNUM.Length; i++)

                {

                    //if match, return true

                    if (allSUBNUM[i] == thisDP.Data)

                    {

                        isDuplicate = true;

                    }

                }

            }

  return isDuplicate;
