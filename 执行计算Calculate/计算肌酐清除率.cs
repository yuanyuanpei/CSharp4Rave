计算肌酐清除率

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            //Get target data points

            DataPoint SEX = dp.Record.Subject.Instances.FindByFolderOID("SCREEN").DataPages.FindByFormOID("DM").MasterRecord.DataPoints.FindByFieldOID("SEX");

            DataPoint AGE = dp.Record.Subject.Instances.FindByFolderOID("SCREEN").DataPages.FindByFormOID("DM").MasterRecord.DataPoints.FindByFieldOID("AGE");

            DataPoint WT = null;

            DataPoint CCR = dp.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("CCR");

            string weight = "";

 

            //Get the WEIGHT data point

            string folderOID = dp.Record.Instance.Folder.OID;

            if (folderOID == "SCREEN")

            {

                WT = dp.Record.Instance.DataPages.FindByFormOID("VS1").MasterRecord.DataPoints.FindByFieldOID("WEIGHT");

            }

            double i = 0, x = 0, y = 0, z = 0;

            //Get the data point Analye Range

            Medidata.Core.Objects.Labs.AnalyteRange AR = dp.AnalyteRange;

 

            //excute calculate

            if (SEX.Data != String.Empty && AGE.Data != String.Empty && AGE.EntryStatus != EntryStatusEnum.NonConformant && WT.Data != String.Empty && WT.EntryStatus != EntryStatusEnum.NonConformant && dp.Data != String.Empty && double.TryParse(dp.Data, out i))

            {

                //Convert the data to double

                x = Convert.ToDouble(AGE.Data.ToString());

                y = Convert.ToDouble(WT.Data.ToString());

                if (SEX.Data == "1" && AR != null)

                {

                    if (AR.EnteredLabUnit.Name.ToString().Contains("mg")) z = ((140 - x) * y) / (72 * i);

                    if (AR.EnteredLabUnit.Name.ToString().Contains("mol")) z = ((140 - x) * y) / (0.818 * i);

                }

                if (SEX.Data == "2" && AR != null)

                {

                    if (AR.EnteredLabUnit.Name.ToString().Contains("mg")) z = ((140 - x) * y * 0.85) / (72 * i);

                    if (AR.EnteredLabUnit.Name.ToString().Contains("mol")) z = ((140 - x) * y * 0.85) / (0.818 * i);

                }

                //assign value to CCR

                CCR.Enter(z.ToString("f2"), string.Empty, 0);

            }

            else CCR.Enter(string.Empty, string.Empty, 0);

return null;
