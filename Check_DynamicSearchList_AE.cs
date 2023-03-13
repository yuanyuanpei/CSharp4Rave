///DSL_AE_Check: AE record is modified, then check whether the linked variable is updated.
//Linked edit check:
			//If AETERM is Present or AESTDAT is Present or SUBJNUM is Present, then AETERM->CF DSL_AE_Check.

			ActionFunctionParams afp = (ActionFunctionParams)ThisObject;
			DataPoint input_DP = afp.ActionDataPoint;

			Subject subj = input_DP.Record.Subject;
			const string Term_OID = "AETERM";
			const string Date_OID = "AESTDAT";
			const string queryText = "The selected Adverse Event value has been modified on the Adverse Event form. Please re-select the correct event.";

			//Build DSL options
			DataPoints Term_DPs = CustomFunction.FetchAllDataPointsForOIDPath(Term_OID, null, null, subj, false);
			ArrayList arlValues = new ArrayList();
			arlValues.Add(string.Empty);

			for (int i = 0; i < Term_DPs.Count; i++)
			{
				DataPoint dpTerm = Term_DPs[i];
				string val;
				if (!dpTerm.Record.Active) continue;
				string recPos = dpTerm.Record.RecordPosition.ToString().PadLeft(3, '0');
				DataPoint dpDate = dpTerm.Record.DataPoints.FindByFieldOID(Date_OID);

				if (dpTerm.Data.Length > 160)
					val = String.Format("{0} > {1} > {2}", recPos, ((dpTerm.Data).Trim()).Substring(0, 160), dpDate.Data);
				else
					val = String.Format("{0} > {1} > {2}", recPos, (dpTerm.Data).Trim(), dpDate.Data);
				arlValues.Add(val);
			}

			//Find corresponding log dps on all corresponding forms using Dynamic SearchList
			string[] DSLFields =
			{
			"EXAE", "EXAE1", "EXAE2" , "EXOAE", "EXOAE1", "EXOAE2", "CMINDAE1", "CMINDAE2", "CMINDAE3", "PRINDAE1", "PRINDAE2", "PRINDAE3", "ECGINDAE1", "ECGINDAE2", "ECGINDAE3", "ECHOAE1", "ECHOAE2", "ECHOAE3"
		}
			;
			for (int i = 0; i < DSLFields.Length; i++)
			{
				DataPoints dpsDSL = CustomFunction.FetchAllDataPointsForOIDPath(DSLFields[i], null, null, subj);

				//Check currently entered value in DSL
				for (int j = 0; j < dpsDSL.Count; j++)
				{
					DataPoint dpDSL = dpsDSL[j];
					if (!dpDSL.Record.Active || dpDSL.Data.Length == 0) continue;
					bool IsNonConformant = !arlValues.Contains((dpDSL.Data).ToString());

					//dpDSL.SetNonConformant(IsNonConformant);
					CustomFunction.PerformQueryAction(queryText, 1, true, true, dpDSL, IsNonConformant);
				}
			}
			return null;