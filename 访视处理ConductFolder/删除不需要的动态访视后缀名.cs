
                        删除不需要的动态访视后缀名

//Get the data point from edit check

            ActionFunctionParams afp = (ActionFunctionParams)ThisObject;

            DataPoint dpAction = afp.ActionDataPoint;

            Subject subject = dpAction.Record.Subject;

 

            bool bIncludeSubFolders = true;

            //Change to true to clear Sub Folders as well.

            String[] ExcludeFolders =

            {

                "UNS", "FUP"

            }

            ;

            //Specify as many FolderOIDs within array, enclose within double quotes and seperate by a comma.

            ClearFolder(subject, bIncludeSubFolders, ExcludeFolders);

 

            return null;

        }

 

        private void ClearFolder(Subject currSubj, bool bIncludeSubFolders, String[] ExcludeFolders)

        {

            Instances ins = currSubj.Instances;

            if (ins.Count > 0)

            {

                for (int i = 0; i < ins.Count; i++)

                {

                    bool CleanFolder = true;

                    //Loop all ExcludeFolders in array, if match then delete the folder name suffix

                    for (int f = 0; f < ExcludeFolders.Length; f++)

                    {

                        if (ins[i].Folder.OID == ExcludeFolders[f])

                        {

                            CleanFolder = false;

                            break;

                        }

                    }

                    if (CleanFolder)

                        ins[i].SetInstanceName("");

 

                    //If there are sub folder, delete the sub folder name suffix

                    if (bIncludeSubFolders)

                    {

                        Instances insChild = ins[i].Instances;

                        if (insChild.Count > 0)

                            for (int j = 0; j < insChild.Count; j++)

                            {

                                bool CleanSubFolder = true;

                                //Loop all ExcludeFolders in array, if match then delete the folder name suffix

                                for (int g = 0; g < ExcludeFolders.Length; g++)

                                {

                                    if (insChild[j].Folder.OID == ExcludeFolders[g])

                                    {

                                        CleanSubFolder = false;

                                        break;

                                    }

                                }

                                if (CleanSubFolder)

                                    insChild[j].SetInstanceName("");

                            }

                    }

                }

            }
