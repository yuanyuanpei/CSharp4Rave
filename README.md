# 常见的 Rave Customized Functions
This is a conclusion for general csharp codes used in medidata rave custom function.

## Custom Function in Check Steps:
### EC适用的Folder有限制时（如仅在UNS和PFS生效），Folder的限定需要用CF
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

### 需要用字符格式变量$xx与数值比较大小时，字符格式变量的值转为数字需要用Custom Function:
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
### 更新Folder的FolderName（去掉后缀(1),(2)…..，添加自定义内容等）
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
