排除筛选期

//Get the Data Point from edit check

            DataPoint dp = (DataPoint)ThisObject;

            Instance instance = dp.Record.DataPage.Instance;

            if (instance != null)

            {

//return false if the folder OID is SCREEN

                if (instance.Folder.OID == "SCREEN")

                {

                    return false;

                }

            }

 return true;
