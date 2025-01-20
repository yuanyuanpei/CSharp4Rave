# 常见的 Rave Customized Functions
This is a conclusion for general csharp codes used in medidata rave custom function.

## Custom Function in Check Steps:
### A.EC适用的Folder有限制时（如仅在UNS和PFS生效），Folder的限定需要用CF
1. 在 EC处的配置为：
![1737362358167](https://github.com/user-attachments/assets/73c73ac2-e612-4a56-a848-2179d31614e1)

2. CF 的code (return true/false): CFID: Include_Folder_UNS/PFS
```c#
        DataPoint dp=(DataPoint) ThisObject;
        Instance ins=dp.Record.DataPage.Instance;
        if (ins != null)
        {
            if (ins.Folder.OID=="UNS"||ins.Folder.OID=="PFS")
            {
                return true;
            }
        }
        return false;

```
3. 最终呈现效果：
返回的true/false作为EC的Check steps是否成立的判断条件

### B.需要用字符格式变量$xx与数值比较大小时，字符格式变量的值转为数字需要用Custom Function:
1.Form的配置：
![1737362496949](https://github.com/user-attachments/assets/7b65a7ce-8dea-46e9-b073-129b72711cc2)

2.EC的配置：
![image](https://github.com/user-attachments/assets/1806d8a9-128d-4895-adf7-0130be93dcd9)

3.CF的code (return true/false): CFID: EC_OpenQuery_TUTL2_13_NA
```C#
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

```

## Custom Function in Check Actions:
### A.更新Folder的FolderName（去掉后缀(1),(2)…..，添加自定义内容等）
Step 1: 删除FolderName的后缀(1),(2)…，适用于Add Event添加和MergeMatrix添加的Folder：

1.Folder配置：
![1737362753518](https://github.com/user-attachments/assets/9f35931d-513a-43e6-b8f7-841ae66ad2f3)

2.Matrices配置：
![1737362799180](https://github.com/user-attachments/assets/0a5b3580-58f9-4bc9-8fa7-6e833e3542b2)

3.Form配置：
![1737362836926](https://github.com/user-attachments/assets/9ce6ec80-efd1-4b07-a98f-23da5312bce6)

4.EC配置：
![1737362868854](https://github.com/user-attachments/assets/cb19300c-adcf-49bc-a1bc-59e3efd68a41)

5.CF Code(return null), CFID:CF_Delete_FolderName_Suffix
```C#
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
        DataPoint actiondatapoint = afp.ActionDataPoint;
        Subject subject = actiondatapoint.Record.Subject;
        Instances allInstances = subject.Instances;

        string[] strIncludeFolderOids =
        {
            "SCREEN", "C0D3", "C1D1", "C1D8", "C1D15", "C1D22", "C2D1", "C2D15", "PC", "EXPPC", "LOG", "PANTU", "DTH" , "EOT", "EOTV", "EOS" , "TUMOR", "SAFFU", "OS"
        }
        ;
        System.Collections.Generic.List<string> listIncludeFolderOids = new System.Collections.Generic.List<string>(strIncludeFolderOids);

        for(int i = 0; i < allInstances.Count; i++)
        {
            Instance instance = allInstances[i];
            if (instance != null && instance.Active && listIncludeFolderOids.Contains(instance.Folder.OID))
            {
                instance.SetInstanceName("");
            }
        }
        return null;
```

6.最终界面呈现效果：
![1737362937655](https://github.com/user-attachments/assets/db5bf190-a419-4fec-a525-b9dbca1ed811)

Step 2:定义FolderName，如Add Event出的Cycle folder，将FolderName由”Cycle(8)”改为”Cycle 8 Day 1”:
1.Folder配置：
![1737363015416](https://github.com/user-attachments/assets/46f10fed-d338-409e-8ea0-09f51bfa9afa)

2.Matrices配置：
![1737363047764](https://github.com/user-attachments/assets/0bfc3010-ead6-497b-9963-d13b6c8439df)

3.Form配置：
![1737363075757](https://github.com/user-attachments/assets/b75b680c-1efd-4b47-8439-fed63e9bcdbf)

4.字典配置：
![1737363096761](https://github.com/user-attachments/assets/28f36dad-5fea-4f0a-a981-8a22615ff5f1)

5.EC的配置：
![1737363129989](https://github.com/user-attachments/assets/8a2db295-feb8-4df6-9ead-01dd6befc815)

6.CF Code(return null). CFID:UpdateFolderName_Cycle
```C#
        DataPoint TDP = ((ActionFunctionParams) ThisObject).ActionDataPoint;

        Instance TIST = TDP.Record.DataPage.Instance;

        if(!CustomFunction.DataPointIsEmpty(TDP))

        {
            if (TDP.Data !="99")

            TIST.SetInstanceName(TDP.Data + " Day 1");

            else

            TIST.SetInstanceName(TDP.UserValue().ToString() + " Day 1");

        }
        return null;

```
7.最终页面呈现效果：
当Cycles选择非Other, please sepcify时：
![image](https://github.com/user-attachments/assets/dec9cc52-89ce-42fb-b4b7-f9263c2e1335)

当Cycles选择Other, please sepcify时：
![image](https://github.com/user-attachments/assets/1e8e23b1-7d56-4496-9eb5-83818af021d7)

### B.与AE/MH有关的Form(如EX, EG等)中的Field设置动态下拉框，关联到已录入的AE/MH数据，且关联的AE/MH记录有更新时，在Field处出提示
Part 1: 在EX,EG等form的Field设置动态下拉框，关联已录入的AE/MH记录：
1.Form设置：
![1737363431538](https://github.com/user-attachments/assets/1e3e3720-12d3-43e7-af21-bbf45a19b214)

2.EC设置：
![image](https://github.com/user-attachments/assets/a8762ea8-06aa-427a-b031-26dc96bdde6e)
需要加两条EC，EC1:当AE表中的AETERM和AESTDAT有变动，则ECG中三个Field刷新DSL_AE的结果。EC2:当ECG表中的Field本身有变动，刷新DSL_AE的结果。

![1737363493028](https://github.com/user-attachments/assets/87d2a231-955e-4c7d-a7be-2a41ae2716d5)

![1737363519229](https://github.com/user-attachments/assets/7f49b0d1-a49d-4cc2-8b0c-c23de4cc60c0)

3.CF的code(return 值)， CFID:DSL_AE
```c#
DynamicSearchParams dsl = (DynamicSearchParams) ThisObject;
        DataPoint dp = dsl.DataPoint;
        Subject sb = dp.Record.Subject;
        KeyValueCollection AE = new KeyValueCollection();
        Records rdsAE = sb.Instances.FindByFolderOID("LOG").DataPages.FindByFormOID("AE").Records;
        for(int i=1; i<rdsAE.Count; i++)
        {
            Record rdAE = rdsAE.FindByRecordPosition(i);
            if (rdAE.Active)
            {
                DataPoints dptsAE = rdAE.DataPoints;
                bool isEntry = false;
                foreach(DataPoint dptAE in dptsAE)
                {
                    if (dptAE.EntryStatus == EntryStatusEnum.EnteredComplete)
                    {
                        isEntry = true;
                        break;
                    }
                }
                if( isEntry )
                {
                    DataPoint dpAEterm = rdsAE.FindByRecordPosition(i).DataPoints.FindByFieldOID("AETERM");
                    DataPoint dpAEstdat = rdsAE.FindByRecordPosition(i).DataPoints.FindByFieldOID("AESTDAT");
                    AE.Add(new KeyValue(i.ToString(), i+"-"+dpAEterm.Data+"-"+dpAEstdat.Data.ToUpper()));
                }
            }
        }
        return AE;
```
4.最终界面呈现效果：
![image](https://github.com/user-attachments/assets/3df31a9a-1e94-4d85-b0af-3fee6614cfeb)

Part 2: 关联的AE/MH记录有更新时，在Field处出提示：
1.Form设置：
![1737363694146](https://github.com/user-attachments/assets/206ca2ca-083a-412f-b4d0-1f0ea8edf57d)

2.EC配置：
![1737363748828](https://github.com/user-attachments/assets/ba8e0930-5f54-47ef-bc35-467c6f584f6f)

3.CF的code(return null).CFID:CF_DynamicSearchList_AE_Check_pyyfor2.0
```C#
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
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
            string recPos = dpTerm.Record.RecordPosition.ToString();
            DataPoint dpDate = dpTerm.Record.DataPoints.FindByFieldOID(Date_OID);

            val = recPos+"-"+dpTerm.Data+"-"+ dpDate.Data.ToUpper();

            arlValues.Add(val);
        }

        //Find corresponding log dps on all corresponding forms using Dynamic SearchList
        string[] DSLFields =
        {
            "EXAE", "EXAE1", "EXAE2" , "EXOAE", "EXOAE1", "EXOAE2", "CMINDAE1", "CMINDAE2", "CMINDAE3", "CMINDAE4", "CMINDAE5", "PRINDAE1", "PRINDAE2", "PRINDAE3", "ECGINDAE1", "ECGINDAE2", "ECGINDAE3", "ECHOAE1", "ECHOAE2", "ECHOAE3"
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

```
4.最终界面呈现效果：
![image](https://github.com/user-attachments/assets/3a21d26d-78bd-45fb-963d-af98863d6230)

### C.同一个Form中，将可添加行logline中的Field的值算总和和，赋值给同form中另一个非logline中的变量
1.Form的配置：
![1737363875889](https://github.com/user-attachments/assets/cb0dd258-ef36-4dd4-9e47-aa89b0dc1257)

2.EC配置：
![1737363903222](https://github.com/user-attachments/assets/057b8c8a-15c2-452c-bd5b-da62da43d2ec)

![1737363927798](https://github.com/user-attachments/assets/1d4dd326-2484-41aa-a6f0-1b395bd0efbe)

3.CF的code(return null): CFID:TLsum_Scr
```C#
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
        DataPoint dpTL = afp.ActionDataPoint;

        Subject subj = dpTL.Record.Subject;
        string insOID = dpTL.Record.DataPage.Instance.Folder.OID;

        DataPoint dpSUM = dpTL.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("TLBSUM");

        DataPage dpgTL = dpTL.Record.DataPage;

        //sum的初始值设为0        
        double i_DP = 0; 

        for(int i = 0; i < dpgTL.Records.Count; i++)
        {
            Record recTL = dpgTL.Records.FindByRecordPosition(i);
            DataPoint dose_temp = recTL.DataPoints.FindByFieldOID("TLBPPD");

            if (dose_temp.Active && !CustomFunction.DataPointIsEmpty(dose_temp) && Regex.IsMatch(dose_temp.Data, @"^[+-]?\d*[.]?\d*$"))
            {
                i_DP = Math.Round(i_DP + double.Parse(dose_temp.Data.ToString()), 2);
            }
        }

        string totals = i_DP.ToString();

        dpSUM.Enter(totals, null, 0);

        return null;

```
4.最终呈现的效果：
Leading question = Yes, logline中的PPD值未填，save，则SUM为0：
![image](https://github.com/user-attachments/assets/e609ea7b-ac27-4208-b90a-acd12ea509e0)

Logline填了一条，sum值=PPD值+0：
![image](https://github.com/user-attachments/assets/4f2c01b6-58e8-45f9-992a-e21f34f643fa)

Logline中填了不止一条，SUM值=所有已填的PPD值之和：
![image](https://github.com/user-attachments/assets/4407b8fc-d7c6-4efc-a72e-abb0a50860bd)

### D.将筛选期\靶病灶Form\A,B,C Field的值，赋给肿评访视\靶病灶Form\D,E,F Field
1.筛选期\靶病灶Form的设置：
Leading question+logline：
![image](https://github.com/user-attachments/assets/13fd1f1b-da3c-4171-b47c-fb6ef2056e07)

![image](https://github.com/user-attachments/assets/f34cc14f-6e40-4b18-9fba-2557964d2216)

![image](https://github.com/user-attachments/assets/522d5707-df8d-4c2c-b86a-f32578044274)

2.TA Folder的设置：
![image](https://github.com/user-attachments/assets/d7440adf-eec9-4cb6-b0e7-6dcf44860a57)

3.Matrices配置：
![1737364132372](https://github.com/user-attachments/assets/32bc50da-7cd7-4d3c-88f8-b85a76622a06)

![1737364152327](https://github.com/user-attachments/assets/285e6a67-9eec-48ac-9ad9-8e732a1af461)

4.子访视TA Folder中靶病灶/非靶病灶/新病灶等form的添加：
![1737364191530](https://github.com/user-attachments/assets/a70aed6c-5602-4437-af21-5e166416c259)

5.TA Folder\靶病灶Form：
![image](https://github.com/user-attachments/assets/d464d9d3-b989-4ac5-95cc-113a6f54c1ef)

6.EC设置：
在筛选期\靶病灶\lesion number变量上添加EC：
![1737364252609](https://github.com/user-attachments/assets/7cf27f95-ffac-438b-b1fc-a4ae78baa495)

7.CF的code(return null), CFID:TA_TL_Derive
```c#
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
        Subject sb = afp.ActionDataPoint.Record.Subject;

        DataPage dpgFU = afp.ActionDataPoint.Record.DataPage;
        Records rdsBL = sb.Instances.FindByFolderOID("SCREEN").DataPages.FindByFormOID("TUTLS").Records;

        DataPoint dpYN = afp.ActionDataPoint.Record.DataPoints.FindByFieldOID("TLBYN");
        if(dpgFU.Active==false || (dpYN.Data!="1" && dpYN.ChangeCount<2))
        return null;

        while(rdsBL.Count>dpgFU.Records.Count)
        dpgFU.AddLogRecord();

        for(int i=1; i<rdsBL.Count; i++)
        {
            if(rdsBL.FindByRecordPosition(i).Active == false)
            {
                dpgFU.Records.FindByRecordPosition(i).Active = false;
            }
            else
            {
                dpgFU.Records.FindByRecordPosition(i).Active = true;
                DataPoint dpTLBLSC = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBNUM");
                DataPoint dpTLFUSC = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBNUM");

                dpTLFUSC.UnFreeze();
                if(dpYN.Data=="1")
                dpTLFUSC.Enter(dpTLBLSC.Data, null, 0);
                else
                dpTLFUSC.Enter("", null, 0);
                dpTLFUSC.Freeze();



                DataPoint dpTLBLS = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBSC");
                DataPoint dpTLFUS = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBSC");

                dpTLFUS.UnFreeze();
                if(dpYN.Data=="1")
                dpTLFUS.Enter(dpTLBLS.Data, null, 0);
                else
                dpTLFUS.Enter("", null, 0);
                dpTLFUS.Freeze();


                DataPoint dpTLBNUM = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBLOC");
                DataPoint dpTLFNUM = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBLOC");

                dpTLFNUM.UnFreeze();
                if(dpYN.Data=="1")
                dpTLFNUM.Enter(dpTLBNUM.Data, null, 0);
                else
                dpTLFNUM.Enter("", null, 0);


                DataPoint dpTLBNU = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBPOS");
                DataPoint dpTLFNU = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBPOS");

                dpTLFNU.UnFreeze();
                if(dpYN.Data=="1")
                dpTLFNU.Enter(dpTLBNU.Data, null, 0);
                else
                dpTLFNU.Enter("", null, 0);



            }
        }
        return null;

```
8.最终界面呈现效果：
![image](https://github.com/user-attachments/assets/e747e5e6-ee5a-42c6-995a-446b2d51f145)

![image](https://github.com/user-attachments/assets/44c60163-b844-4552-b10f-5d8e9883c2b7)

### E.实验室检查项：检查值超出范围但临床意义为空；临床意义为CS但备注为空
1.Form的配置：
![1737364370338](https://github.com/user-attachments/assets/20c6c85b-e3cb-4c77-a329-324a3ac22f26)

2.EC的配置：
![1737364414493](https://github.com/user-attachments/assets/4d7c7e76-ee50-4590-abf6-2fcb0fa09774)

3.CF的code(return null). CFID:CF_LB_Significance_pyyfor3.0
```C#
        ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
        DataPoint ThisDp = afp.ActionDataPoint;
        DataPoints DPs = ThisDp.Record.DataPoints;

        string OverRange_Text = Localization.GetLocalDataString(20507);
        string CSCO_Text = Localization.GetLocalDataString(20508);

        foreach (DataPoint DP in DPs)
        {
            bool OverRange = false;
            bool CSCO = false;
            if (DP.AnalyteRange != null && DP.AnalyteRange.Active)
            {
                if (DP.AnalyteRangeStatus.ToString() == "OutOfRangeLow" || DP.AnalyteRangeStatus.ToString() == "OutOfRangeHigh")
                {
                    //如果ClinicalSignificance没有触发出来，则出质疑：超范围但临床意义为空
                    if (DP.ClinicalSignificance == null)
                    {
                        OverRange = true;
                    }
                                                     //如果ClinicalSignificance有触发出来但为空，则出质疑：超范围但临床意义为空
                    else if (DP.ClinicalSignificance.Code == null)
                    {
                        OverRange = true;
                    }
                                                     //如果ClinicalSignificance有触发出来且选CS，但备注为空，则出质疑：临床意义为CS但备注为空
                    else if (DP.ClinicalSignificance.Code.IsCommentRequired)
                    {
                        if (DP.ClinicalSignificance.Comment == string.Empty)
                        {
                            CSCO = true;
                        }
                    }
                }
            }
            CustomFunction.PerformQueryAction(OverRange_Text, 1, false, false, DP, OverRange);
            CustomFunction.PerformQueryAction(CSCO_Text, 1, false, false, DP, CSCO);
        }

        return null;

```
4.最终界面呈现效果：
![image](https://github.com/user-attachments/assets/37eec377-2bf3-4fa0-b8a3-64f696022049)

![image](https://github.com/user-attachments/assets/78c70278-aa1f-484f-a859-8e593fd378e4)

![image](https://github.com/user-attachments/assets/293b8e58-5c39-4d01-9742-1ddf7366b894)

### F.不同条件控制不同的Next Visit和Merge Matrix
1.Folder的配置：
![1737364530860](https://github.com/user-attachments/assets/a52ce1c4-2254-425f-b4c0-3739e93c4955)

2.Matrices的配置：
![1737364560828](https://github.com/user-attachments/assets/0676d918-0095-47d9-872e-cac67a2774cb)

3.Form的配置：
![image](https://github.com/user-attachments/assets/5cb3bd17-d420-4711-b931-ca69449f18ad)

![image](https://github.com/user-attachments/assets/38567702-5333-4996-919d-ebddcd385d96)

4.EC的配置：
![1737364611104](https://github.com/user-attachments/assets/3f41ee34-1d1b-4868-8c0d-ad579275ceb0)

5.CF的code, CFID:CF_AddMatrix_CN_&C2D8
```C#
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
                        instances[i].SetInstanceName(("0" + (Math.Truncate(Quo) + 3).ToString() + "D1").Trim());
                        else
                        instances[i].SetInstanceName(("0" + (Math.Truncate(Quo) + 3).ToString() + "D8").Trim());//1.用CF控制的cycle都是靠NEXTVIS = Yes逐个添加出来的; 2. rave后台在对subject的CRFLocation排序时，从C3D1起，均是按照text逐位排序，即：C10,C11,C12....C19,C20,C3,C4，会引起比较日期的EC出错。所以需要将InstanceName set为C03D1, C04D1而不是C3D1, C4D1.
                    }
                    else if (NeedCND8 && instances[i].InstanceRepeatNumber > 12)
                    {
                        instances[i].SetInstanceName(((instances[i].InstanceRepeatNumber - 3).ToString() + "D1").Trim());
                    }
                    else
                     {
                if (instances[i].InstanceRepeatNumber < 7)
                        {
                            instances[i].SetInstanceName(("0" +(instances[i].InstanceRepeatNumber + 3).ToString() + "D1").Trim());
                        }
                        else
                        {
                            instances[i].SetInstanceName(((instances[i].InstanceRepeatNumber + 3).ToString() + "D1").Trim());
            }
                        }                 }
                else if (instances[i].Folder.OID != "UNSCH")
                {
                    instances[i].SetInstanceName("");
                }
            }
        }
        bool IsYes(DataPoint dp)
        {
            bool flag = (dp != null && dp.Active && dp.Data != string.Empty && dp.EntryStatus != EntryStatusEnum.NonConformant && dp.Data == "1");
            return flag;
        }
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

```
注：本条CF可能会影响到其他EC的执行，例如：
![1737364839367](https://github.com/user-attachments/assets/bcc8b1f2-fdcd-4f72-9957-93640d2209ac)

原因：本条EC中涉及的“>Next>Subject>CRFLocation”会受CF_AddMatrix_CN_&C2D8中void ResetInstanceName (Instances instances)的影响：当InstanceName设定为C3D1,C4D1.....时，由于实际cycle是通过NEXTVIS = Yes逐个添加的，那么实际系统后台对于subject 的CRFLocation排序将如下图蓝色字体所示，而非红色字体所示（即：后台将对InstanceName按照Text型排序而不是按照Numeric型排序），故实际用到ECID:EG_003中，将出现C20D1访视下的EGDAT与C3D1的SVDAT相比较，而非与C21D1或者EOT访视的SVDAT相比较，故会错跳query。
解决办法是：在void ResetInstanceName函数中，对于C XD1的InstanceName:当X <10时，以**0X**填补，当X>=10时，以X填补。debug过程中可以在EG页面新增一个“下一个访视日期”变量，添加derivation（>Next>Subject>CRFLocation）以显示系统后台实际取得的值。![image](https://github.com/user-attachments/assets/06e7f375-236d-4e31-a68b-21f95707d184)

### G.计算QTCF并与CRC手动录入的QTCF比较
1.Form的配置：
![image](https://github.com/user-attachments/assets/304819de-3766-4470-8e4d-5dde5a5f30d5)

2.EC的配置：
![image](https://github.com/user-attachments/assets/4938a848-926c-4630-900e-3129728a2424)

3.CF的code(return null), CFID:CF_OpenQuery_EG_018_pyyfor2.0
```C#
ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
        DataPoint datapoint = afp.ActionDataPoint;
        Record record = datapoint.Record;

        const string flEGQT = "EGQT";
        const string flEGRR = "EGRR";
        const string flEGQTC = "EGQTC";

        string queryText = Localization.GetLocalDataString(20953, "eng");
        bool fire = false;
        //string TMP = "";
        //string TMP0 = "";
        //string TMP1 = "";
        DataPoint dpEGQT = record.DataPoints.FindByFieldOID(flEGQT);
        DataPoint dpEGRR = record.DataPoints.FindByFieldOID(flEGRR);
        DataPoint dpEGQTC = record.DataPoints.FindByFieldOID(flEGQTC);

        if (IsEligible(dpEGQT) && IsEligible(dpEGRR) && IsEligible(dpEGQTC) && Convert.ToDouble(dpEGRR.Data) > 0)
        {
            double EGQT = Convert.ToDouble(dpEGQT.Data);
            double EGQTm=10*EGQT;
            double EGRR = Convert.ToDouble(dpEGRR.Data);
            double EGQTC = Convert.ToDouble(dpEGQTC.Data);
            //TMP0 = Convert.ToString(EGRR);
            //TMP1 = Convert.ToString(Math.Pow(EGRR, 1.0/3.0));
            //TMP = Convert.ToString(Math.Floor(EGQTm/Math.Pow(EGRR, 1.0/3.0)));
            //if (EGQTC < Convert.ToDouble(string.Format("{0:#.#}", EGQTm / Convert.ToDouble(string.Format("{0:#.#}", Math.Pow(EGRR,1.0/3.0)))))-1.0 || EGQTC > Convert.ToDouble(string.Format("{0:#.#}", EGQTm / Convert.ToDouble(string.Format("{0:#.#}", Math.Pow(EGRR, 1.0/3.0)))))+1.0 )
//1.在计算过程中不要有format的限制，仅针对计算结果取format。2.因录入的QTCF为整数，计算其范围时，范围的format最好限制在取小数点后一位。
            if (EGQTC < Math.Floor(EGQTm/Math.Pow(EGRR, 1.0/3.0)) - 1.0 || EGQTC > Math.Floor(EGQTm/Math.Pow(EGRR, 1.0/3.0)) + 1.0)
            {
                fire = true;

            }
        }
        //CustomFunction.PerformQueryAction(TMP0, 1, false, false, dpEGQTC, fire, afp.CheckID, afp.CheckHash);
        //CustomFunction.PerformQueryAction(TMP1, 1, false, false, dpEGQTC, fire, afp.CheckID, afp.CheckHash);
        //CustomFunction.PerformQueryAction(TMP, 1, false, false, dpEGQTC, fire, afp.CheckID, afp.CheckHash);
        CustomFunction.PerformQueryAction(queryText, 1, false, false, dpEGQTC, fire, afp.CheckID, afp.CheckHash);
        return null;
    }
    double GetDataAsDouble(DataPoint dp)
    {
        double data = double.NaN;
        double outdata;
        if (double.TryParse(dp.Data, out outdata)) data = outdata;
        return data;
    }
    bool IsEligible(DataPoint dp)
    {
        bool flag = false;
        if (dp != null && dp.Active && dp.Data != string.Empty && dp.EntryStatus != EntryStatusEnum.NonConformant && dp.MissingCode == null) flag = true;
        return flag;
}
```
