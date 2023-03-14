删除访视名称后缀

//Get the DataPoint from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

 

            //Get the all the instances

            Instances ints = dp.Record.Subject.Instances;

 

            //Loop all instances on this subject

            foreach (Instance inst in ints)

            {

                if (inst.Folder.OID == "SAFFU" || inst.Folder.OID == "LTSAFFU")

                {

                    //Set the folder suffix as empty

                    inst.SetInstanceName("");

                }

            }

 return null;
