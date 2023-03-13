///Dynamic trigger cycle(addMatrix)
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
        DataPoint actionDataPoint = afp.ActionDataPoint;
        Instance instance = actionDataPoint.Record.DataPage.Instance;
        Subject subject = actionDataPoint.Record.Subject;

        int crfVersionId = subject.CrfVersionId;
        string currFolderOid = instance.Folder.OID;
        int instanceRepeatNumber = instance.InstanceRepeatNumber;

        Matrix matrixCND1_1 = Matrix.FetchByOID("CND1_1", crfVersionId);
        Matrix matrixCND1_2 = Matrix.FetchByOID("CND1_2", crfVersionId);
        Matrix matrixCND8 = Matrix.FetchByOID("CND8", crfVersionId);
        Matrix matrixC2D8 = Matrix.FetchByOID ("C2D8", crfVersionId);
        //Judge the current FolderOID whether it belong to default matrixs and return the next matrix.
        string[] strTrigFolderOids =
        {
            "SCR2", "C1D1", "C1D8", "C1D15"
        }
        ;
        string[] strMatrixOids =
        {
            "C1D1", "C1D8", "C1D15", "C2D1"
        }
        ;
        System.Collections.Generic.List<string> listStrTrigFolderOids = new System.Collections.Generic.List<string>(strTrigFolderOids);
        int index = listStrTrigFolderOids.IndexOf(currFolderOid);
        // (x>y) ? a : b ;--if x > y ==true then a, else b;
        string strToBeMergedMatrixOid = (index >= 0 && index <= 4) ? strMatrixOids[index] : string.Empty;

        //To ensure that every cases only contain one bool with true value.
        DataPoint Group1 = subject.GetAllDataPoints().FindByFieldOID("DSSRG1");
        DataPoint Group2 = subject.GetAllDataPoints().FindByFieldOID("DSSRG2");
        bool isNextCycleCND1_1 = false, isNextCycleCND1_2 = false, isNextCycleCND8 = false, isNextCycleC2D8 = false;

        bool NeedCND8 = ((Group1.Active && (Group1.Data == "2" || Group1.Data == "4")) || (Group2.Active && Group2.Data == "1"));

        if (NeedCND8 && instanceRepeatNumber < 11)
        {

            isNextCycleC2D8 = (string.Compare(currFolderOid, "C2D1", true) == 0);
            isNextCycleCND1_1 = (string.Compare(currFolderOid, "C2D8", true) == 0 || (string.Compare(currFolderOid, "CND1", true) == 0 && instanceRepeatNumber % 4 == 3));
            isNextCycleCND1_2 = (string.Compare(currFolderOid, "CND1", true) == 0 && instanceRepeatNumber % 4 == 1);
            isNextCycleCND8 = (string.Compare(currFolderOid, "CND1", true) == 0 && instanceRepeatNumber % 2 == 0);
        }
        else if (NeedCND8 && instanceRepeatNumber >= 11)
        {

            isNextCycleCND1_1 = (string.Compare(currFolderOid, "CND1", true) == 0 && instanceRepeatNumber % 2 != 0);
            isNextCycleCND1_2 = (string.Compare(currFolderOid, "CND1", true) == 0 && instanceRepeatNumber % 2 == 0);
            isNextCycleCND8 = false;
            isNextCycleC2D8 = false;
        }
        else if (!NeedCND8)
        {
            isNextCycleCND1_1 = ( string.Compare(currFolderOid, "C2D1", true) == 0 || (string.Compare(currFolderOid, "CND1", true) == 0 && instanceRepeatNumber % 2 != 0));
            isNextCycleCND1_2 = (string.Compare(currFolderOid, "CND1", true) == 0 && instanceRepeatNumber % 2 == 0);
            isNextCycleCND8 = false;
            isNextCycleC2D8 = false;
        }


        //Add matrix, active or inactive the next matrix according to each bools.
        DataPoint dpYN = actionDataPoint;
        Instance nextIns = GetNextInstance(instance, NeedCND8);
        if (strToBeMergedMatrixOid != string.Empty)
        {
            if (IsYes(dpYN) && nextIns == null)
            {
                Matrix toBeMergedMatrix = Matrix.FetchByOID(strToBeMergedMatrixOid, crfVersionId);
                subject.AddMatrix(toBeMergedMatrix);
            }
            else if (IsYes(dpYN) && nextIns != null)
            {
                nextIns.Active = true;
            }
            else if (!IsYes(dpYN) && nextIns != null)
            {
                nextIns.Active = false;
            }
        }
        if (isNextCycleC2D8 && IsYes(dpYN))
        {
            if (nextIns == null)
            subject.AddMatrix(matrixC2D8);
            else if (nextIns != null)
            nextIns.Active = true;
        }
        else if (isNextCycleC2D8 && !IsYes(dpYN) && nextIns != null)
        {
            nextIns.Active = false;
        }
        if (isNextCycleCND1_1 && IsYes(dpYN))
        {
            if (nextIns == null)
            subject.AddMatrix(matrixCND1_1);
            else if (nextIns != null)
            nextIns.Active = true;
        }
        else if (isNextCycleCND1_1 && !IsYes(dpYN) && nextIns != null)
        {
            nextIns.Active = false;
        }
        if (isNextCycleCND1_2 && IsYes(dpYN))
        {
            if (nextIns == null)
            subject.AddMatrix(matrixCND1_2);
            else if (nextIns != null)
            nextIns.Active = true;
        }
        else if (isNextCycleCND1_2 && !IsYes(dpYN) && nextIns != null)
        {
            nextIns.Active = false;
        }
        if (isNextCycleCND8 && IsYes(dpYN))
        {
            if (nextIns == null)
            subject.AddMatrix(matrixCND8);
            else if (nextIns != null)
            nextIns.Active = true;
        }
        else if (isNextCycleCND8 && !IsYes(dpYN) && nextIns != null)
        {
            nextIns.Active = false;
        }
        Instances Ins = subject.Instances;
        ResetInstanceName(Ins);
        return null;
		//Fuctions 1-5.
        //1.Input an instance then return the next instance according to the visit or InstanceRepeatNumber.

        Instance GetNextInstance(Instance curInstance, bool CohortY)
        {
            Instances ins = curInstance.Subject.Instances;
            Instances insCND1 = curInstance.Subject.GetInstancesByFolderOid("CND1", true);
            if (curInstance.Folder.OID == "SCR2")
            {
                return ins.FindByFolderOID("C1D1");
            }
            else if (curInstance.Folder.OID == "C1D1")
            {
                return ins.FindByFolderOID("C1D8");
            }
            else if (curInstance.Folder.OID == "C1D8")
            {
                return ins.FindByFolderOID("C1D15");
            }
            else if (curInstance.Folder.OID == "C1D15")
            {
                return ins.FindByFolderOID("C2D1");
            }
            else if (curInstance.Folder.OID == "C2D1")
            {
                if (CohortY)
                {
                    return ins.FindByFolderOID("C2D8");
                }
                else
                {
                    return GetInstancePerRepeatNumber(insCND1, 0);
                }
            }
            else if (curInstance.Folder.OID == "C2D8")
            {
                return GetInstancePerRepeatNumber(insCND1, 0);
            }
            else
            {
                return GetInstancePerRepeatNumber(insCND1, curInstance.InstanceRepeatNumber + 1);
            }
        }

		//2.Input some instances with the same FolderOID and InstanceRepeatNumber then return the certain instance.
        Instance GetInstancePerRepeatNumber(Instances instances, int insRepeatNumber)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                if (instances[i].InstanceRepeatNumber == insRepeatNumber)
                {
                    return instances[i];
                }
            }
            return null;
        }

		//3.Input some instances then reset their names.
        void ResetInstanceName(Instances instances)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                double Quo = instances[i].InstanceRepeatNumber / 2;
                if (instances[i].Folder.OID == "CND1")
                {
                    if (NeedCND8 && instances[i].InstanceRepeatNumber <= 12)
                    {
                        if (instances[i].InstanceRepeatNumber % 2 == 0)
                        instances[i].SetInstanceName(((Math.Truncate(Quo) + 3).ToString() + "D1").Trim());
                        else
                        instances[i].SetInstanceName(((Math.Truncate(Quo) + 3).ToString() + "D8").Trim());
                    }
                    else if (NeedCND8 && instances[i].InstanceRepeatNumber > 12)
                    {
                        instances[i].SetInstanceName(((instances[i].InstanceRepeatNumber - 3).ToString() + "D1").Trim());
                    }
                    else
                    instances[i].SetInstanceName(((instances[i].InstanceRepeatNumber + 3).ToString() + "D1").Trim());
                }
                else if (instances[i].Folder.OID != "UNSCH")
                {
                    instances[i].SetInstanceName("");
                }
            }
        }

		//4.Return true when the data of datapoint is "1".
        bool IsYes(DataPoint dp)
        {
            bool flag = (dp != null && dp.Active && dp.Data != string.Empty && dp.EntryStatus != EntryStatusEnum.NonConformant && dp.Data == "1");
            return flag;
        }

		//5.Input certain instance, FormOID and FieldOID then return the certain datapoint.
        DataPoint GetDataPoint(Instance inst, string fmOid, string flOid)
        {
            DataPoint dp = null;
            if (inst == null)
            return dp;
            DataPage page = inst.DataPages.FindByFormOID(fmOid);
            if (page == null)
            return dp;
            return page.MasterRecord.DataPoints.FindByFieldOID(flOid);
        }