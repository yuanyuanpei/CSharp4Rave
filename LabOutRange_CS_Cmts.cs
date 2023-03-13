/* Calling Check/s: Significance
        * Description:
        * MODIFICATION HISTORY
        * Person Date Comments
        * --------- ------ ---------
        * JW02 20211124
        */
        ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
        DataPoint input_DP = afp.ActionDataPoint;
        Subject subj = input_DP.Record.Subject;
        DataPoints dps = input_DP.Record.DataPage.GetAllDataPoints();
        string queryText = Localization.GetLocalDataString(20507);
        string queryText2 = Localization.GetLocalDataString(20508);

        bool fire = false;
        bool open = false;
        foreach (DataPoint dp in dps)
        {
            fire = false;
            open = false;
            if (dp.Active && dp.Field.IsClinicalSignificance && dp.Record.RecordPosition == 0)
            {
                DataPoint.Significance dpclinc = dp.ClinicalSignificance;
                if (dpclinc != null && dpclinc.Code == null)
                {
                    fire = true;
                }
                if (dpclinc != null && dpclinc.Code != null && dpclinc.Code.Code.ToString() == "CS" && dpclinc.Comment == string.Empty)
                {
                    open = true;
                }
            }
            CustomFunction.PerformQueryAction(queryText, 1, false, false, dp, fire, afp.CheckID, afp.CheckHash);
            CustomFunction.PerformQueryAction(queryText2, 1, false, false, dp, open, afp.CheckID, afp.CheckHash);
        }
        return null;