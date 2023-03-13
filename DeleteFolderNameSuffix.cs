///Clear FolderName Suffix (input:)
	ActionFunctionParams afp = (ActionFunctionParams)ThisObject;
	DataPoint dpt = afp.ActionDataPoint;
	Subject subj = dpt.Record.Subject;
	Instances insts = subj.instances;

	foreach (Instance inst in insts)
	{
		if (inst.Folder.OID == "UNS" || inst.Folder.OID == "TU" || inst.Folder.OID == "CXD1") continue;
		inst.SetInstanceName("");
	}
	return null;