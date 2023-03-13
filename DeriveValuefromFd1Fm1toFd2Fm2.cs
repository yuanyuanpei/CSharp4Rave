//1.if SCR_TLBNUM or TLBSC or TLBLOC or TLBPOS is not empty, then POST_TLBYN -> CF_Derive
			//2.if POST_TLBYN is not empty, then POST_TLBYN -> CF_Derive
			//为啥要写2条EC？Shaw Zhang: 1.里面筛选期的变量时固定的，没有动态的；2.里面的POST_TLBYN在一个Logline的表里。
			//所涉及的变量全都是default表/访视中的话，可以合在一起写，有logline的，有动态的，addevent出来的表里的变量，最好和default的表的变量分开
			//A表1数据点和2数据点是不同动态触发的 或者1数据点没动态 2有动态 也分开写
			//A表1，2都是3数据点=yes触发的 这种可以写一条

			ActionFunctionParams afp = (ActionFunctionParams)ThisObject;//流入的dp是POSTTU页面的数据点
			Subject sb = afp.ActionDataPoint.Record.Subject;//找到受试者

			DataPage dpgFU = afp.ActionDataPoint.Record.DataPage; //找到POSTTU页面，用于判断active和向页面中添加记录

			Records rdsBL = sb.Instances.FindByFolderOID("SCREEN").DataPages.FindByFormOID("TUTLS").Records;//找到筛选期的所有TU记录

			DataPoint dpYN = afp.ActionDataPoint.Record.DataPoints.FindByFieldOID("TLBYN"); //用于POSTTU添加记录时的判断条件

			if (dpgFU.Active == false || (dpYN.Data != "1" && dpYN.ChangeCount < 2))
				return null;

			while (rdsBL.Count > dpgFU.Records.Count)
				dpgFU.AddLogRecord();//添加记录

			//添加记录中的每个变量的值
			for (int i = 1; i < rdsBL.Count; i++)
			{
				if (rdsBL.FindByRecordPosition(i).Active == false)
				{
					dpgFU.Records.FindByRecordPosition(i).Active = false;
				}
				else
				{
					dpgFU.Records.FindByRecordPosition(i).Active = true;

					DataPoint dpTLBLSC = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBNUM");
					DataPoint dpTLFUSC = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBNUM");

					dpTLFUSC.UnFreeze();
					if (dpYN.Data == "1")
						dpTLFUSC.Enter(dpTLBLSC.Data, null, 0);
					else
						dpTLFUSC.Enter("", null, 0);
					dpTLFUSC.Freeze();



					DataPoint dpTLBLS = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBSC");
					DataPoint dpTLFUS = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBSC");

					dpTLFUS.UnFreeze();
					if (dpYN.Data == "1")
						dpTLFUS.Enter(dpTLBLS.Data, null, 0);
					else
						dpTLFUS.Enter("", null, 0);
					dpTLFUS.Freeze();


					DataPoint dpTLBNUM = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBLOC");
					DataPoint dpTLFNUM = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBLOC");

					dpTLFNUM.UnFreeze();
					if (dpYN.Data == "1")
						dpTLFNUM.Enter(dpTLBNUM.Data, null, 0);
					else
						dpTLFNUM.Enter("", null, 0);
					dpTLFNUM.Freeze();

					DataPoint dpTLBNU = rdsBL.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBPOS");
					DataPoint dpTLFNU = dpgFU.Records.FindByRecordPosition(i).DataPoints.FindByFieldOID("TLBPOS");

					dpTLFNU.UnFreeze();
					if (dpYN.Data == "1")
						dpTLFNU.Enter(dpTLBNU.Data, null, 0);
					else
						dpTLFNU.Enter("", null, 0);
					dpTLFNU.Freeze();


				}
			}
			return null;