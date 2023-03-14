设置受试者信息（受试者筛选号为系统自动生成）

//Get datapoint form derivation , dp may be the subject Initial

            DataPoint dp = (DataPoint)ThisObject;

            Subject subj = dp.Record.Subject;

 

            //Get the subject create sequence

            string subjectNumber = subj.NumberInStudySite.ToString().PadLeft(3,'0');

            //Get the Study site number

            string siteNumber = subj.StudySite.StudySiteNumber;

            string subjectID = subjectNumber + "-" + dp.Data + "-" + siteNumber;

            //assign subject name

            subj.Name = subjectID;  

            return subjectID;

 
