//Get data point from Check Step

        DataPoint dp = (DataPoint) ThisObject;

        double i = 0;

        if (dp.Data != null && Double.TryParse(dp.Data.ToString(), out i))

        {
            if (i < 10)

            {
                return true;
            }
        }
        return false;