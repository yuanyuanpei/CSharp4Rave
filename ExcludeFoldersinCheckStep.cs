//Exclude_SCREEN_folder
DataPoint dp=(DataPoint) ThisObject;
        Instance ins=dp.Record.DataPage.Instance;
        if (ins != null)
        {
            if (ins.Folder.OID=="SCREEN")
            {
                return false;
            }
        }

        return true;