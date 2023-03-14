计算QTCF

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint input_DP = afp.ActionDataPoint;

            

//Get the target data points

            DataPoint QT = input_DP.Record.DataPoints.FindByFieldOID("EGQT");

            DataPoint HR = input_DP.Record.DataPoints.FindByFieldOID("EGHR");

            DataPoint QTcF = input_DP.Record.DataPoints.FindByFieldOID("EGQTCF");

            

            if (QT.Data != string.Empty && QT.EntryStatus != EntryStatusEnum.NonConformant && HR.Data != string.Empty && HR.EntryStatus != EntryStatusEnum.NonConformant)

            {

//Convert the data to double

                double q = Convert.ToDouble(QT.Data);

                double h = Convert.ToDouble(HR.Data);

                double mid = Math.Pow(60 / h, 0.33);

                //excute caclulate

                double result = Math.Round(q / mid, 1);

                //Assign value to QTCF

                QTcF.Enter(result.ToString(), string.Empty, 0);

            }

            else

            {

                QTcF.Enter(string.Empty, string.Empty, 0);

            }

 return null;
