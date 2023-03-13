///Calculate sum of dp in logline form (TLBPPD in log, TLBSUM not in log)
///1.SCR_TLBPPD is present, then TLBPPD ->CF_SUM
			///2.SCR_TLBPPD is present or TLBSUM is not ampty, then TLBSUM -> CF_SUM
			///
			ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
			DataPoint dpTL = afp.ActionDataPoint;

			Subject subj = dpTL.Record.Subject;
			string insOID = dpTL.Record.DataPage.Instance.Folder.OID;//找当前的Ins
			//SUM在整个肿评page中不在logline里，仅有一个数据点
			DataPoint dpSUM = dpTL.Record.DataPage.MasterRecord.DataPoints.FindByFieldOID("TLBSUM");
			//找当前的datapage:TUTR
			DataPage dpgTL = dpTL.Record.DataPage;

			double i_DP = 0;

			for (int i = 0; i < dpgTL.Records.Count; i++)//SCR的肿评是logline，所以会有n个records
			{
				Record recTL = dpgTL.Records.FindByRecordPosition(i);//找到每个record
				DataPoint dose_temp = recTL.DataPoints.FindByFieldOID("TLBPPD");//找出每个record中的TLBPPD
				//*:匹配前面的子表达式任意次,+:匹配前面的子表达式一次或多次(大于等于1次）
				//?:当该字符紧跟在任何一个其他限制符（*,+,?，{n}，{n,}，{n,m}）后面时，匹配模式是非贪婪的。
				//非贪婪模式尽可能少地匹配所搜索的字符串，而默认的贪婪模式则尽可能多地匹配所搜索的字符串。
				//例如，对于字符串“oooo”，“o+”将尽可能多地匹配“o”，得到结果[“oooo”]，而“o+?”将尽可能少地匹配“o”，得到结果 ['o', 'o', 'o', 'o']
				//Regex: Regular Expression，^:行首，$：行尾，[+-]：正负号，[.]：小数点，\d：匹配一个数字字符，*：匹配\d任意次，?：尽可能少地匹配所搜索的字符串
				//@：表示其后的字符串是个逐字字符串verbatim string，@只能对字符串常量作用，@会取消字符串中的转义符。类似于SAS中的%Qsysfunc,不加@的话\n翻译为换行。加@\n翻译为\和n
				if (dose_temp.Active && !CustomFunction.DataPointIsEmpty(dose_temp) && Regex.IsMatch(dose_temp.Data, @"^[+-]?\d*[.]?\d*$"))
				{
					i_DP = i_DP + double.Parse(dose_temp.Data.ToString());//求和，double.parse指将dp的值存为string后转为double数字
				}
			}

			string totals = i_DP.ToString();//将和转为string格式

			dpSUM.Enter(totals, null, 0);//数值，单位，changecode

			return null;