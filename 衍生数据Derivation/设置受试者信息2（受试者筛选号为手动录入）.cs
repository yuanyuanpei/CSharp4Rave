设置受试者信息2（受试者筛选号为手动录入）

//Get data point from derivation

            DataPoint dp = (DataPoint)ThisObject;

            DataPoint SITEID = dp.Record.DataPoints.FindByFieldOID("SITEID");

            Subject subj = dp.Record.Subject;

            //Get the Study Site Number

            string sitenumber = subj.StudySite.StudySiteNumber.ToString().PadLeft(2, '0');

            string subjName = sitenumber + "-" + dp.Data.ToString();

            //Assign a name to the subject

            subj.Name = subjName;

            if (sitenumber != "") SITEID.Enter(sitenumber, string.Empty, 0);

            else SITEID.Enter(string.Empty, string.Empty, 0);

            //return subject Name

  return subjName;
