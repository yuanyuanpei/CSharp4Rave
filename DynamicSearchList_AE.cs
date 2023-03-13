//Linked edit checks:
	//1.If AETERM is not empty or AESTDAT is not empty, then CMINDAE1->set DSL_AE,CMINDCAE2->set DSL_AE,CMINDCAE3->set DSL_AE
	//2.If CMINDCAE1 is present, then CMINDCAE1->set DSL_AE
	//3.If CMINDCAE2 is present, then CMINDCAE2->set DSL_AE
	//4.If CMINDCAE3 is present, then CMINDCAE3->set DSL_AE

	DynamicSearchParams dsl = (DynamicSearchParams)ThisObject;//dsl参数
	DataPoint dp = dsl.DataPoint;//根据dsl参数找到当前数据点
	Subject sb = dp.Record.Subject;//找到当前受试者
	KeyValueCollection AE = new KeyValueCollection();//新建AE用于收集keyvalue
	Records rdsAE = sb.Instances.FindByFolderOID("LOG").DataPages.FindByFormOID("AE").Records;//找到受试者所有AE记录rdsAE
	for (int i = 1; i < rdsAE.Count; i++)  //每条AE记录来说（如AE#1,AE#2,AE#3...)
	{
		Record rdAE = rdsAE.FindByRecordPosition(i); //AE#i记录赋给rdAE
		if (rdAE.Active) //如果rdAE未失活
		{
			DataPoints dptsAE = rdAE.DataPoints; //找到该条AE记录的所有数据点dptsAE
					
			bool isEntry = false; //预设isEntry为F
			foreach (DataPoint dptAE in dptsAE) //该条AE记录的所有数据点中，对每个数据点来说
			{
				if (dptAE.EntryStatus == EntryStatusEnum.EnteredComplete) //如果数据点的entry状态为已录入
				{
					isEntry = true; //isentry为T
					break;//跳出整个foreach循环,continue跳出当前循环
				}
			}

			if (isEntry) //如果数据点状态为is entry
			{
				DataPoint dpAEterm = rdsAE.FindByRecordPosition(i).DataPoints.FindByFieldOID("AETERM"); //通过AE#i找到数据点AETERM
				DataPoint dpAEstdat = rdsAE.FindByRecordPosition(i).DataPoints.FindByFieldOID("AESTDAT"); //通过AE#i找到数据点AESTDAT
				AE.Add(new KeyValue(i.ToString(), i + "-" + dpAEterm.Data + "-" + dpAEstdat.Data)); //变量AE加一条value，key为i，值为i-TERM-DAT
			}
		}
	}
	return AE;