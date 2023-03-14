动态触发重复添加Matrix（Matrix中包含3个访视）

//Get data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dp = afp.ActionDataPoint;

            Subject subj = dp.Record.Subject;

            Matrix EVL = Matrix.FetchByOID("WC", subj.CrfVersionId);

 

            Instance current_ins = null;

            int currRepeatNo = 0;

 

            if (dp.Record.DataPage.Instance.Folder.OID == "W3C")

            {

                current_ins = dp.Record.DataPage.Instance;

                currRepeatNo = current_ins.ParentInstance.InstanceRepeatNumber;

            }

 

            Instances all_ins = subj.Instances;

            Instances Cycles_WC = new Instances();

 

            int count = 0;

 

            for (int i = 0; i < all_ins.Count; i++)

            {

                if (all_ins[i].Folder.OID == "C")

                {

                    count++;

                }

            }

            //MergeMatrix for the fixed folder

            if (dp.Record.DataPage.Instance.Folder.OID == "SCREEN" && dp.Data != String.Empty && dp.IsObjectChanged)

            {

                subj.MergeMatrix(EVL);

            }

            //AddMatrix for the repeat folder

            if (dp.Record.DataPage.Instance.Folder.OID == "W3C" && dp.Data != String.Empty && dp.IsObjectChanged && currRepeatNo >= count - 1)

            {

                subj.AddMatrix(EVL);

            }

 

            for (int i = 0; i < all_ins.Count; i++)

            {

                if (all_ins[i].Folder.OID == "C")

                {

                    Cycles_WC.Add(all_ins[i]);

                }

            }

 

 

            Instances Sorted_Cycles_WC = SortAndRename_instance(Cycles_WC);

 

            //Inactive the folders once condition is false

            for (int i = 0; i < Sorted_Cycles_WC.Count; i++)

            {

                if (dp.Record.DataPage.Instance.Folder.OID == "SCREEN")

                {

                    if (dp.Data == String.Empty)

                    {

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W3C").Active = false;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W2C").Active = false;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W1C").Active = false;

                    }

                    else

                    {

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W3C").Active = true;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W2C").Active = true;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W1C").Active = true;

                    }

                }

                if (dp.Record.DataPage.Instance.Folder.OID == "W3C")

                {

                    if (dp.Data == String.Empty && i > currRepeatNo)

                    {

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W3C").Active = false;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W2C").Active = false;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W1C").Active = false;

                    }

                    else

                    {

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W3C").Active = true;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W2C").Active = true;

                        Sorted_Cycles_WC[i].Instances.FindByFolderOID("W1C").Active = true;

                    }

                }

            }

 

            return null;

        }

        //sort all folders and set folder name

        public Instances SortAndRename_instance(Instances Cycles)

        {

            Instance[] temInst = new Instance[Cycles.Count];

            Instances sortedCycle = new Instances();

            for (int i = 0; i < Cycles.Count; i++)

            {

                Cycles[i].SetInstanceName((Cycles[i].InstanceRepeatNumber + 2).ToString());

                Cycles[i].Instances.FindByFolderOID("W3C").SetInstanceName((Cycles[i].InstanceRepeatNumber + 2).ToString());

                Cycles[i].Instances.FindByFolderOID("W2C").SetInstanceName((Cycles[i].InstanceRepeatNumber + 2).ToString());

                Cycles[i].Instances.FindByFolderOID("W1C").SetInstanceName((Cycles[i].InstanceRepeatNumber + 2).ToString());

                temInst[Cycles[i].InstanceRepeatNumber] = Cycles[i];

 

            }

            for (int i = 0; i < temInst.Length; i++)

            {

                sortedCycle.Add(temInst[i]);

            }

 return sortedCycle;
