///Update FolderName (add suffix or rearrange) (input e.g.:CYCLE)
	//CodedValue: 99-Other, please specify
	ActionFunctionParams afp = (ActionFunctionParams)ThisObject;
	DataPoint dpt = afp.ActionDataPoint;
	Subject subj = dpt.Record.Subject;

	string updFolderOID = "CYCLE";
	Instance inst = subj.instances.FindByFolderOID(updFolderOID);

	if (!CustomFunction.DataPointIsEmpty(dpt))
	{
		if (dpt.Data !== "99")
			inst.SetInstanceName(dpt.Data + " Day 01");
		else
			inst.SetInstanceName(dpt.UserValue().ToString() + " Day 01");
	}
	return null;
	///Update PageName (肿瘤评估非靶病灶_1_肝左叶，16进制unicode与中文互转）
	ActionFunctionParams afp = (ActionFunctionParams)ThisObject;
	DataPoint dpt = afp.ActionDataPoint;

	string Name_ID = "TUSPID";
	string Name_LOC = "TUSPLOC";
	DataPoint dpt_ID = dpt.Record.DataPoints.FindByFieldOID(Name_ID);
	DataPoint dpt_LOC = dpt.Record.DataPoints.FindByFieldOID(Name_LOC);

	string name = "";
	try
	{
		name = "_" + dpt_ID.Data + "_" + dpt_LOC.Data;
		//肿瘤评估非靶病灶的十六进制：
		dpt.Record.DataPage.Name = HexToCN(@"\u80bf\u7624\u8bc4\u4ef7\u005f\u9776\u75c5\u7076") + name;
	}
	catch
	{
	}
	return null;
}
private string HexToCN(string Hex_code)
{
	string outStr = "";
	if (!string.IsNullOrEmpty(Hex_code))
       {
		string[] strList = Hex_code.Replace("\\", "").Split('u');
		for (int i=1;i<strList.Length;i++)
         {
			outStr +=(char) int.Parse(strList[i]),System.Globalization.NumberStyles.HexNumber;
        }
    }
	return outStr;
