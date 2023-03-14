动态触发重复添加Matrix（Matrix里只包含一个访视）

//Get a data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

            Subject subj = dp.Record.Subject;

 

            //Define a Matrix which need to be added

            Matrix EVL = Matrix.FetchByOID("EVL", subj.CrfVersionId);

 

            Instance current_ins = null;

            int currRepeatNo = 0;

 

            if (dp.Record.Instance.Folder.OID == "EVL")

            {

                current_ins = dp.Record.DataPage.Instance;

                currRepeatNo = current_ins.InstanceRepeatNumber;

            }

 

            Instances all_ins = subj.Instances;

 

            int count = 0;

            for (int i = 0; i < all_ins.Count; i++)

            {

                if (all_ins[i].Folder.OID == "EVL")

                {

                    count++;

                }

            }

            //use MergeMatrix for the fixed folder

            if (dp.Record.DataPage.Instance.Folder.OID == "SCREEN" && dp.Data != String.Empty && dp.IsObjectChanged)

            {

                subj.MergeMatrix(EVL);

            }

            //use AddMatrix for the repeat folder

            if (dp.Record.DataPage.Instance.Folder.OID == "EVL" && dp.Data != String.Empty && dp.IsObjectChanged && currRepeatNo >= count - 1)

            {

                subj.AddMatrix(EVL);

            }

 

            //collect all the new folders

            Instances Cycles = new Instances();

            for (int i = 0; i < all_ins.Count; i++)

            {

                if (all_ins[i].Folder.OID == "EVL")

                {

                    Cycles.Add(all_ins[i]);

                }

            }

 

            // sort the new folders and set folder name

            Instance[] temInst = new Instance[Cycles.Count];

            Instances sortedCycle = new Instances();

            for (int z = 0; z < Cycles.Count; z++)

            {

                Cycles[z].SetInstanceName((Cycles[z].InstanceRepeatNumber + 1).ToString());

                temInst[Cycles[z].InstanceRepeatNumber] = Cycles[z];

            }

            for (int k = 0; k < temInst.Length; k++)

            {

                sortedCycle.Add(temInst[k]);

            }

 

            //Inactive the folders if condition is false

            for (int j = 0; j < sortedCycle.Count; j++)

            {

                if (dp.Record.DataPage.Instance.Folder.OID == "SCREEN")

                {

                    if (dp.Data == String.Empty) sortedCycle[j].Active = false;

                    else sortedCycle[j].Active = true;

                }

                if (dp.Record.DataPage.Instance.Folder.OID == "EVL")

                {

                    if (dp.Data == String.Empty && j > currRepeatNo) sortedCycle[j].Active = false;

                    else sortedCycle[j].Active = true;

                }

            }

 

 return null;
