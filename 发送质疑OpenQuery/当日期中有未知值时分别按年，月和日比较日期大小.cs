当日期中有未知值时分别按年，月和日比较日期大小

//create a function to compare the two date, return a bool value

            private bool DATcompare(DataPoint dp1, DataPoint dp2, bool openQuery)

            {

                openQuery = false;

                int dp1_Y, dp1_M, dp1_D, dp2_Y, dp2_M, dp2_D;

                if (dp1.Data.Contains("UN") || dp2.Data.Contains("UN"))

                {

                    //Split the two data use ' ' or '/'

                    string[] dp1_YMD = dp1.Data.Split(' ');

                    string[] dp2_YMD = dp2.Data.Split(' ');

                    if (int.TryParse(dp1_YMD[0], out dp1_Y) && int.TryParse(dp2_YMD[0], out dp2_Y))

                    {

                        if (dp1_Y == dp2_Y && int.TryParse(dp1_YMD[1], out dp1_M) && int.TryParse(dp2_YMD[1], out dp2_M))

                        {

                            if (dp1_M == dp2_M && int.TryParse(dp1_YMD[2], out dp1_D) && int.TryParse(dp2_YMD[2], out dp2_D))

                            {

                                //if month is equal and date not contains UN, compare the date

                                if (dp1_D < dp2_D)

                                { openQuery = true; }

                            }

                            //if month is not equal or date contains UN, compare the month

                            else if (dp1_M < dp2_M)

                            { openQuery = true; }

                        }

                        //if year is not equal or month contains UN, compare the year

                        else if (dp1_Y < dp2_Y)

                        { openQuery = true; }

                    }

  }
