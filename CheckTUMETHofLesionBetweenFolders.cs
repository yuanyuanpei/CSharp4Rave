////////////////////////////////////////////////////////
			////Check同一病灶不同周期的检查方法是否一致
            
			//Get the target fields OID

			const string TUMETH = "TUMETH";
			const string TUTL = "TUNT";
			bool IsOpenQuery = false;

			//Define query text variable

			string queryText = "";

			//Get data point from edit check

			ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

			DataPoint dpACT = afp.ActionDataPoint;

			//Get the current subject and TUMETH data points

			Subject subj = dpACT.Record.Subject;

			DataPoints dpsTUMETH = CustomFunction.FetchAllDataPointsForOIDPath(TUMETH, TUTL, null, subj);

			if (dpsTUMETH != null)

			{

				//Loops the TUMETH data points

				for (int i = 0; i < dpsTUMETH.Count; i++)

				{

					//Get the first record and the data point in this record

					IsOpenQuery = false;

					Record iRec = dpsTUMETH[i].Record;

					DataPoint iTUMETH = dpsTUMETH[i];

					DataPoint iTUWKS = iTUMETH.Record.DataPoints.FindByFieldOID("TUWKS");

					DataPoint iTUWKSO = iTUMETH.Record.DataPoints.FindByFieldOID("TUWKSO");

					DataPoint iTUMETHO = iTUMETH.Record.DataPoints.FindByFieldOID("TUMETHO");

					if (!iRec.Active || CustomFunction.DataPointIsEmpty(iTUMETH)) continue;

					for (int j = 0; j < dpsTUMETH.Count; j++)

					{

						//Get another record and the data point in this record

						Record jRec = dpsTUMETH[j].Record;

						DataPoint jTUMETH = dpsTUMETH[j];

						DataPoint jTUWKS = jTUMETH.Record.DataPoints.FindByFieldOID("TUWKS");

						DataPoint jTUWKSO = jTUMETH.Record.DataPoints.FindByFieldOID("TUWKSO");

						DataPoint jTUMETHO = jTUMETH.Record.DataPoints.FindByFieldOID("TUMETHO");

						if (!jRec.Active || CustomFunction.DataPointIsEmpty(jTUMETH)) continue;

						//open query once the record position is not equal and the data is not equal

						if (((iTUMETH.Data != jTUMETH.Data) || (iTUMETHO.Data != String.Empty && jTUMETHO.Data != string.Empty && iTUMETHO.Data != jTUMETHO.Data)) && ((iTUWKS.Data != jTUWKS.Data) || (iTUWKSO.Data != String.Empty && jTUWKSO.Data != string.Empty && iTUWKSO.Data != jTUWKSO.Data)) && iTUMETH.Record.DataPage.PageRepeatNumber == jTUMETH.Record.DataPage.PageRepeatNumber)

						{

							IsOpenQuery = true;

						}



					}

					//Use the translation workbench to get a chinese query text ID

					queryText = Localization.GetLocalDataString(6081, "eng");

					//Open query use the PerformQueryAction Method

					CustomFunction.PerformQueryAction(queryText, 1, false, false, iTUMETH, IsOpenQuery, afp.CheckID, afp.CheckHash);

				}

			}

			return null;