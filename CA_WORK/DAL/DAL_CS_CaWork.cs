using CA_WORK.DAL.DataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_WORK.DAL
{
    class DAL_CS_CaWork
    {
        DataSet dsNP;
        string AllJobNo;
        public DataTable loadSubjobDetails(string jobno, string jcat)
        {
            dsNP = new DAL_DS_CaWork1();
            DataTable dt = dsNP.Tables["tblJobDetails"];
            DBconnect.connect();
            String IWO = "SELECT      psd_sub," +
                                     "psd_spec," +
                                     "psd_extc," +
                                     "psd_description," +
                                     "to_char(psd_start_date,'RRRR-MM-DD') as psd_start_date," +
                                     "to_char(psd_end_date,'RRRR-MM-DD') as psd_end_date," +
                                     "psd_status " +
                           "FROM      pms_subjob_details " +
                           "WHERE     psd_jmain = '" + jobno + "' " +
                                     " AND psd_jcat = '" + jcat + "' " +
                           "ORDER BY  psd_sub";
            //psd_jcat = 'CA'" +
            OracleDataReader IoDr = DBconnect.readtable(IWO);
            DataRow dr = null;

            while (IoDr.Read())
            {
                dr = dt.NewRow();
                dr["sub"] = IoDr["psd_sub"].ToString();
                dr["spec"] = IoDr["psd_spec"].ToString();
                dr["extc"] = IoDr["psd_extc"].ToString();
                dr["description"] = IoDr["psd_description"].ToString();
                dr["sd"] = IoDr["psd_start_date"] != DBNull.Value
    ? Convert.ToDateTime(IoDr["psd_start_date"]).ToString("yyyy-MM-dd HH:mm")
    : (object)DBNull.Value;
                //dr["sd"] = Convert.ToDateTime(IoDr["psd_start_date"]).ToString("yyyy-MM-dd HH:mm");
                //dr["ed"] = Convert.ToDateTime(IoDr["psd_end_date"]).ToString("yyyy-MM-dd HH:mm");
                dr["ed"] = IoDr["psd_end_date"] != DBNull.Value
    ? Convert.ToDateTime(IoDr["psd_end_date"]).ToString("yyyy-MM-dd HH:mm")
    : (object)DBNull.Value;



                switch (IoDr["psd_status"].ToString())
                {
                    case "P":
                        dr["status"] = "Pending";
                        break;
                    case "C":
                        dr["status"] = "Confirmed";
                        break;
                }
                dt.Rows.Add(dr);
            }
            IoDr.Close();
            DBconnect.conn_new.Close();
            addnewJobLine(jobno);

            return dt;
        }

        public void addnewJobLine(string jobno)
        {
            DataRow dr2 = dsNP.Tables["tblJobDetails"].NewRow();
            dr2["sub"] = ""; 
            dr2["spec"] = "000";
            dr2["extc"] = "S";
            dr2["status"] = "Pending";
            dsNP.Tables["tblJobDetails"].Rows.Add(dr2);
        }


        private String getNewSubJobNo(string jmain)
        {
            String val = "";
            DBconnect.connect();

            String dloc = "SELECT nvl(max(psd_sub),0)+1 " +
                          " FROM   pms_subjob_details " +
                          " WHERE  psd_jmain = '" + jmain + "'";
            //psd_jcat = 'CA' " +
            OracleDataReader ddr = DBconnect.readtable(dloc);
            while (ddr.Read())
            {
                val = ddr[0].ToString();
            }
            ddr.Close();

            DBconnect.conn_new.Close();
            return val;
        }


        public DataTable loadAllAccounts()
        {
            DAL.DataSource.DAL_DS_CaWork1 ds = new DataSource.DAL_DS_CaWork1();
            DataTable dt = ds.tblAllAccounts;
            DBconnect.connect();
            String IWO = "SELECT   fla_code," +
                                  " fla_narration" +
                        " FROM     fms_ledger_accounts" +
                        " WHERE    fla_code not in ('1') " +
                         "ORDER BY fla_code";
            OracleDataReader IoDr = DBconnect.readtable(IWO);
            DataRow dr = null;

            while (IoDr.Read())
            {
                dr = dt.NewRow();
                dr["code"] = IoDr["fla_code"].ToString();
                dr["desc"] = IoDr["fla_narration"].ToString();
                dt.Rows.Add(dr);
            }
            IoDr.Close();
            DBconnect.conn_new.Close();
            return dt;
        }



        public DataTable loadAllMainJobs(string ca)
        {
            string AllJobCat = "";
            DAL.DataSource.DAL_DS_CaWork1 ds = new DataSource.DAL_DS_CaWork1();
            DataTable dt = ds.tblMainJob;
            DBconnect.connect();
            String IWOCA = "SELECT    pmd_jmain," +
                                     "pmd_description," +
                                     "pmd_dep_rate," +
                                     "pmd_jcat," +
                                     "TO_CHAR(pmd_start_date, 'RRRR-MM-DD') AS sdate , " +
                                     "TO_CHAR(pmd_end_date, 'RRRR-MM-DD') AS edate , " +
                                     "pmd_asset_account, " +
                                     "pmd_status " +
                           "FROM      pms_mainjob_details " +
                           "WHERE     pmd_jcat = '" + ca + "'" +
                           "ORDER BY  pmd_jmain";
            // 

            String IWOWS = "SELECT     pmd_jmain," +
                                       "pmd_description," +
                                       "pmd_status " +
                             "FROM      pms_mainjob_details" +
                             " WHERE     pmd_jcat = 'WS' " +
                             "AND       pmd_jmain in('8201','8202','8203','8204','8207','8208','8215','8210','8212','8214') " +
                             "ORDER BY  pmd_jmain";

            string selectedQuery = "";

            if (AllJobCat == "WS")
                selectedQuery = IWOWS;
            else
                selectedQuery = IWOCA;

            OracleDataReader IoDr = DBconnect.readtable(selectedQuery);
            DataRow dr = null;

            while (IoDr.Read())
            {
                dr = dt.NewRow();
                dr["Jno"] = IoDr["pmd_jmain"].ToString();
                dr["description"] = IoDr["pmd_description"].ToString();
                dr["depRate"] = IoDr["pmd_dep_rate"].ToString();
                dr["jcat"] = IoDr["pmd_jcat"].ToString();
                dr["sdate"] = IoDr["sdate"].ToString();
                dr["accNo"] = IoDr["pmd_asset_account"].ToString();
                dr["edate"] = IoDr["edate"].ToString();
                switch (IoDr["pmd_status"].ToString())
                {
                    case "A":
                        dr["status"] = "OPENED";
                        break;
                    case "I":
                        dr["status"] = "CLOSED";
                        break;
                    case "P":
                        dr["status"] = "PENDING";
                        break;
                    case "C":
                        dr["status"] = "CONFIRMED";
                        break;
                    case "D":
                        dr["status"] = "CANCELLED";
                        break;
                }

                dt.Rows.Add(dr);
            }
            IoDr.Close();
            DBconnect.conn_new.Close();
            return dt;
        }


        public DataTable loadMainJob(String cat, String jobno)
        {
            DAL.DataSource.DAL_DS_CaWork1 ds = new DataSource.DAL_DS_CaWork1();
            DataTable dt = ds.tblJobDetails;
            DBconnect.connect();
            String IWO = "SELECT pmd_jcat," +
                                "pmd_jmain," +
                                "pmd_description," +
                                "to_char(pmd_start_date,'RRRR-MM-DD') as pmd_start_date," +
                                "to_char(pmd_end_date,'RRRR-MM-DD') as pmd_end_date," +
                                "pmd_status," +
                                "pmd_dep_rate," +
                                "pmd_asset_account " +
                         "FROM   pms_mainjob_details " +
                         "WHERE  pmd_jcat = '" + cat + "'" +
                                 " and pmd_jmain = '" + jobno + "'";

            OracleDataReader IoDr = DBconnect.readtable(IWO);
            DataRow dr = null;

            while (IoDr.Read())
            {
                dr = dt.NewRow();
                dr["pmd_jcat"] = IoDr["pmd_jcat"].ToString();
                dr["pmd_jmain"] = IoDr["pmd_jmain"].ToString();
                dr["pmd_description"] = IoDr["pmd_description"].ToString();
                dr["pmd_start_date"] = IoDr["pmd_start_date"].ToString();
                dr["pmd_end_date"] = IoDr["pmd_end_date"].ToString();
                dr["pmd_asset_account"] = IoDr["pmd_asset_account"].ToString();
                dr["despRate"] = IoDr["pmd_dep_rate"].ToString();

                switch (IoDr["pmd_status"].ToString())
                {
                    case "A":
                        dr["pmd_status"] = "OPENED";
                        break;
                    case "I":
                        dr["pmd_status"] = "CLOSED";
                        break;
                    case "P":
                        dr["pmd_status"] = "PENDING";
                        break;
                    case "C":
                        dr["pmd_status"] = "CONFIRMED";
                        break;
                    case "D":
                        dr["pmd_status"] = "CANCELLED";
                        break;
                }
                dt.Rows.Add(dr);
            }
            IoDr.Close();
            DBconnect.conn_new.Close();
            return dt;
        }



        //-----------------------NIRMITHA

        //public Boolean saveCAMainJobRegister(string DspRate, string jcat, string jmain, string description, string startdate, string enddate, string status, string assetAccount)
        //{
        //    Boolean saveStatus = false;
        //    Boolean ins = false;
        //    Boolean upd = false;
        //    DBconnect.connect();

        //    switch (status)
        //    {
        //        case "OPENED":
        //            status = "A";
        //            break;
        //        case "CLOSED":
        //            status = "I";
        //            break;
        //        case "PENDING":
        //            status = "P";
        //            break;
        //        case "CONFIRMED":
        //            status = "C";
        //            break;
        //        case "CANCELLED":
        //            status = "D";
        //            break;
        //    }





        //    //mainjob
        //    DBconnect.connect();

        //    string qtextMainJobUpd = "UPDATE pms_mainjob_details SET pmd_description = '" + description + "', " +
        //                                                            "pmd_start_date = to_date('" + startdate + "','RRRR-MM-DD')," +
        //                                                            "pmd_end_date = to_date('" + enddate + "','RRRR-MM-DD')," +
        //                                                            "updated_by = user," +
        //                                                            "updated_date = sysdate," +
        //                                                            "pmd_status = '" + status + "'," +
        //                                                            "pmd_dep_rate = '" + DspRate + "'," +
        //                                                            "pmd_asset_account = '" + assetAccount + "' " +
        //                                                      "WHERE pmd_jcat='" + jcat + "' " +
        //                                                        "AND pmd_jmain = '" + jmain + "'";

        //    upd = DBconnect.AddEditDel(qtextMainJobUpd);

        //    if (!upd)
        //    {


        //        jmain = getNewMainJobNo(jcat);
        //        ////////
        //        DBconnect.connect();
        //        string qtextMainJobIns = "INSERT into pms_mainjob_details( pmd_jcat," +
        //                                                             "pmd_jmain," +
        //                                                             "pmd_description," +
        //                                                             "pmd_start_date," +
        //                                                             "pmd_end_date," +
        //                                                             "created_by," +
        //                                                             "created_date," +
        //                                                             "pmd_status," +
        //                                                             "pmd_dep_rate," +
        //                                                             "pmd_asset_account) " +
        //                                                     "VALUES('" + jcat + "'," +
        //                                                            "'" + jmain + "'," +
        //                                                            "'" + description + "'," +
        //                                                            "to_date('" + startdate + "','RRRR-MM-DD')," +
        //                                                            "to_date('" + enddate + "','RRRR-MM-DD')," +
        //                                                            "user," +
        //                                                            "sysdate," +
        //                                                            "'" + status + "'," +
        //                                                            "'" + DspRate + "'," +
        //                                                            "'" + assetAccount + "')";
        //        ins = DBconnect.AddEditDel(qtextMainJobIns);
        //    }

        //    if (upd || ins)
        //    {
        //        saveStatus = true;
        //        //AllJobNo = jmain;
        //        Main.val = jmain;
        //    }

        //    //  if (ins == true)
        //    //   {
        //    if (jcat == "CA")
        //    {

        //        DBconnect.connect();

        //        string qteAsset = "INSERT into rms_asset_categories ( rac_category_code," +
        //                                                             "rac_description," +
        //                                                             "rac_status," +
        //                                                             "created_by," +
        //                                                             "created_date) " +
        //                                                     "VALUES('" + jmain + "'," +
        //                                                            "'" + description + "'," +
        //                                                            "'A'," +
        //                                                            "user," +
        //                                                            "sysdate ) ";


        //        DBconnect.AddEditDel(qteAsset);

        //        string qteAsset1 = " UPDATE fms_cdl_assets " +
        //                                    " SET   fca_account_code = '" + assetAccount + "', " +
        //                                    "       fca_dep_rate = '" + DspRate + "', " +
        //                                    "       fca_exp_life = ' trunc(100 / nvl(" + DspRate + ", 0.00), 1) ' " +
        //                                    " WHERE fca_main = '" + jmain + "'";

        //        DBconnect.AddEditDel(qteAsset1);


        //        //PROCESS_DATA_ANALYSIS1 Procedure

        //        OracleCommand cmd = new OracleCommand("process_data_analysis1", DBconnect.connect());
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("pmd_jmain", OracleType.VarChar).Value = jmain;
        //        // result = cmd.ExecuteNonQuery();

        //    }
        //    // }


        //    return saveStatus;
        //}





        //------------------------------------------
        public bool IsMainJobExists(string jcat, string jmain)
        {
            DBconnect.connect();
            string query = "SELECT COUNT(*) FROM pms_mainjob_details " +
                         $"WHERE pmd_jcat = '{jcat}' AND pmd_jmain = '{jmain}'";

            OracleDataReader reader = DBconnect.readtable(query);
            int count = 0;

            if (reader.Read())
            {
                count = Convert.ToInt32(reader[0]);
            }

            reader.Close();
            DBconnect.conn_new.Close();

            return count > 0;
        }


        //-------------------------------NIRMITHA ----------------------------------------------------//

        //public Boolean saveCAMainJobRegister(string DspRate, string jcat, string jmain,
        //                           string description, string startdate,
        //                           string enddate, string status, string assetAccount)
        //{

        //    if (IsMainJobExists(jcat, jmain))
        //    {
        //        return false;
        //    }


        //    switch (status)
        //    {
        //        case "OPENED": status = "A"; break;
        //        case "CLOSED": status = "I"; break;
        //        case "PENDING": status = "P"; break;
        //        case "CONFIRMED": status = "C"; break;
        //        case "CANCELLED": status = "D"; break;
        //    }


        //    if (string.IsNullOrEmpty(jmain))
        //    {
        //        jmain = getNewMainJobNo(jcat);
        //    }


        //    DBconnect.connect();
        //    string qtextMainJobIns = "INSERT into pms_mainjob_details( pmd_jcat," +
        //                                                         "pmd_jmain," +
        //                                                         "pmd_description," +
        //                                                         "pmd_start_date," +
        //                                                         "pmd_end_date," +
        //                                                         "created_by," +
        //                                                         "created_date," +
        //                                                         "pmd_status," +
        //                                                         "pmd_dep_rate," +
        //                                                         "pmd_asset_account) " +
        //                                                 "VALUES('" + jcat + "'," +
        //                                                        "'" + jmain + "'," +
        //                                                        "'" + description + "'," +
        //                                                        "to_date('" + startdate + "','RRRR-MM-DD')," +
        //                                                        "to_date('" + enddate + "','RRRR-MM-DD')," +
        //                                                        "user," +
        //                                                        "sysdate," +
        //                                                        "'" + status + "'," +
        //                                                        "'" + DspRate + "'," +
        //                                                        "'" + assetAccount + "')";
        //    bool ins = DBconnect.AddEditDel(qtextMainJobIns);

        //    if (ins)
        //    {
        //        Main.val = jmain;

        //        if (jcat == "CA")
        //        {

        //            string qteAsset = "INSERT into rms_asset_categories ( rac_category_code," +
        //                                                             "rac_description," +
        //                                                             "rac_status," +
        //                                                             "created_by," +
        //                                                             "created_date) " +
        //                                                     "VALUES('" + jmain + "'," +
        //                                                            "'" + description + "'," +
        //                                                            "'A'," +
        //                                                            "user," +
        //                                                            "sysdate ) ";
        //            DBconnect.AddEditDel(qteAsset);


        //            string qteAsset1 = " UPDATE fms_cdl_assets " +
        //                                    " SET   fca_account_code = '" + assetAccount + "', " +
        //                                    "       fca_dep_rate = '" + DspRate + "', " +
        //                                    "       fca_exp_life = ' trunc(100 / nvl(" + DspRate + ", 0.00), 1) ' " +
        //                                    " WHERE fca_main = '" + jmain + "'";
        //            DBconnect.AddEditDel(qteAsset1);


        //            OracleCommand cmd = new OracleCommand("process_data_analysis1", DBconnect.connect());
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("pmd_jmain", OracleType.VarChar).Value = jmain;
        //            cmd.ExecuteNonQuery();
        //        }
        //    }

        //    return ins;
        //}


        public Boolean saveCAMainJobRegister(string DspRate, string jcat, string jmain,
                               string description, string startdate,
                               string enddate, string status, string assetAccount)
        {
            if (IsMainJobExists(jcat, jmain))
            {
                
                DBconnect.connect();

                string qUpdate = "UPDATE pms_mainjob_details " +
                                 "SET pmd_end_date = TO_DATE('" + enddate + "', 'RRRR-MM-DD'), " +
                                 "    updated_by   = user, " +
                                 "    updated_date = sysdate " +
                                 "WHERE pmd_jcat = '" + jcat + "' " +
                                 "  AND pmd_jmain = '" + jmain + "'";

                return DBconnect.AddEditDel(qUpdate);
            }

           
            switch (status)
            {
                case "OPENED": status = "A"; break;
                case "CLOSED": status = "I"; break;
                case "PENDING": status = "P"; break;
                case "CONFIRMED": status = "C"; break;
                case "CANCELLED": status = "D"; break;
            }

            if (string.IsNullOrEmpty(jmain))
            {
                jmain = getNewMainJobNo(jcat);
            }

            DBconnect.connect();
            string qtextMainJobIns = "INSERT into pms_mainjob_details( pmd_jcat," +
                                                                     "pmd_jmain," +
                                                                     "pmd_description," +
                                                                     "pmd_start_date," +
                                                                     "pmd_end_date," +
                                                                     "created_by," +
                                                                     "created_date," +
                                                                     "pmd_status," +
                                                                     "pmd_dep_rate," +
                                                                     "pmd_asset_account) " +
                                                             "VALUES('" + jcat + "'," +
                                                                    "'" + jmain + "'," +
                                                                    "'" + description + "'," +
                                                                    "to_date('" + startdate + "','RRRR-MM-DD')," +
                                                                    "to_date('" + enddate + "','RRRR-MM-DD')," +
                                                                    "user," +
                                                                    "sysdate," +
                                                                    "'" + status + "'," +
                                                                    "'" + DspRate + "'," +
                                                                    "'" + assetAccount + "')";
            bool ins = DBconnect.AddEditDel(qtextMainJobIns);

            if (ins)
            {
                Main.val = jmain;

                if (jcat == "CA")
                {
                    string qteAsset = "INSERT into rms_asset_categories ( rac_category_code," +
                                                                         "rac_description," +
                                                                         "rac_status," +
                                                                         "created_by," +
                                                                         "created_date) " +
                                                                 "VALUES('" + jmain + "'," +
                                                                        "'" + description + "'," +
                                                                        "'A'," +
                                                                        "user," +
                                                                        "sysdate ) ";
                    DBconnect.AddEditDel(qteAsset);

                    string qteAsset1 = " UPDATE fms_cdl_assets " +
                                            " SET   fca_account_code = '" + assetAccount + "', " +
                                            "       fca_dep_rate = '" + DspRate + "', " +
                                            "       fca_exp_life = trunc(100 / nvl(" + DspRate + ", 0.00), 1) " +
                                            " WHERE fca_main = '" + jmain + "'";
                    DBconnect.AddEditDel(qteAsset1);

                    OracleCommand cmd = new OracleCommand("process_data_analysis1", DBconnect.connect());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("pmd_jmain", OracleType.VarChar).Value = jmain;
                    cmd.ExecuteNonQuery();
                }
            }

            return ins;
        }


        public string getNewMainJobNo(string jcat)
        {
            string val = "";
            DBconnect.connect();

            string dloc = " SELECT nvl(max(pmd_jmain),0)+1 " +
                          " FROM   pms_mainjob_details " +
                          " WHERE  pmd_jcat = '" + jcat + "' " +
                          " AND    pmd_jmain not in ('9009')";

            OracleDataReader ddr = DBconnect.readtable(dloc);
            while (ddr.Read())
            {
                val = Convert.ToDouble(ddr[0].ToString()).ToString("0000");
            }
            ddr.Close();

            DBconnect.conn_new.Close();
            return val;
        }




        public Boolean saveCASubJobRegister(string jcat, string jmain, string sub, string spec, string extc, string desc, string start, string end, string status)
        {
            Boolean saveStatus = false;
            Boolean ins = false;
            Boolean upd = false;
            DBconnect.connect();

            switch (status)
            {
                case "Confirmed":
                    status = "C";
                    break;
                case "Pending":
                    status = "P";
                    break;
                case "Cancelled":
                    status = "D";
                    break;
            }

            
            DBconnect.connect();
            string rsno = sub + spec + extc;
            string qtextMainJobUpd = "UPDATE pms_subjob_details SET   psd_rsno = '" + rsno + "', " +
                                                                     "psd_start_date = to_date('" + start + "','RRRR-MM-DD'), " +
                                                                     "psd_end_date = to_date('" + end + "','RRRR-MM-DD'), " +
                                                                     "updated_by = user, " +
                                                                     "updated_date = sysdate, " +
                                                                     "psd_description = '" + desc.Replace("'", "'||CHR(39)||'") + "'," +
                                                                     "psd_status = '" + status + "' " +
                                                              "WHERE  psd_jcat='" + jcat + "' " +
                                                              "AND    psd_jmain = '" + jmain + "' " +
                                                              "AND    psd_sub = '" + sub + "' ";

            upd = DBconnect.AddEditDel(qtextMainJobUpd);

            if (!upd)
            {
                DBconnect.connect();

                
                string sjNo = string.IsNullOrEmpty(sub) ? getsJobNo(jcat, jmain) : sub;

                if (string.IsNullOrEmpty(sub))
                {
                    sub = Convert.ToDouble(getNewSubJobNo(sub)).ToString("000");
                }

                rsno = sjNo + spec + extc;
                DBconnect.connect();
                string qtextMainJobIns = "INSERT INTO pms_subjob_details (psd_jcat, " +
                                                                         "psd_jmain, " +
                                                                         "psd_sub, " +
                                                                         "psd_spec, " +
                                                                         "psd_extc, " +
                                                                         "psd_rsno, " +
                                                                         "psd_description, " +
                                                                         "psd_status, " +
                                                                         "psd_start_date, " +
                                                                         "psd_end_date, " +
                                                                         "created_by, " +
                                                                         "created_date) " +
                                                                  "values('" + jcat + "', " +
                                                                         "'" + jmain + "', " +
                                                                         "'" + sjNo + "', " +
                                                                         "'" + spec + "', " +
                                                                         "'" + extc + "', " +
                                                                         "'" + rsno + "', " +
                                                                         "'" + desc.Replace("'", "'||CHR(39)||'") + "'," +
                                                                         "'" + status + "', " +
                                                                         "to_date('" + start + "','RRRR-MM-DD'), " +
                                                                         "to_date('" + end + "','RRRR-MM-DD'), " +
                                                                         "user, " +
                                                                         "sysdate)";
                ins = DBconnect.AddEditDel(qtextMainJobIns);
            }

            if (upd || ins)
            {
                saveStatus = true;
            }

            return saveStatus;
        }






        public string getsJobNo(string jcat, string jmain)
        {
            string val = "";
            DBconnect.connect();

            string dloc =
                " SELECT LPAD(COALESCE(MAX(TO_NUMBER(psd_sub)) + 1, 1), 3, '0') AS NextSub " +
                " FROM pms_subjob_details " +
                "WHERE psd_jcat = '" + jcat + "' " +
                "AND    psd_jmain = '" + jmain + "' ";

            OracleDataReader ddr = DBconnect.readtable(dloc);
            while (ddr.Read())
            {
                val = ddr[0].ToString();
            }
            ddr.Close();

            DBconnect.conn_new.Close();
            return val;
        }




        //public Boolean saveCASubJobRegister(string jcat, string jmain, string sub, string spec, string extc, string desc, string start, string end, string status)
        //{
        //    Boolean saveStatus = false;
        //    Boolean ins = false;
        //    Boolean upd = false;
        //    DBconnect.connect();


        //    switch (status)
        //    {
        //        case "Confirmed":
        //            status = "C";
        //            break;
        //        case "Pending":
        //            status = "P";
        //            break;
        //        case "Cancelled":
        //            status = "D";
        //            break;
        //    }


        //    DBconnect.connect();
        //    string rsno = sub + spec + extc;
        //    string qtextMainJobUpd = "UPDATE pms_subjob_details SET " +
        //                             "psd_rsno = '" + rsno + "', " +
        //                             "psd_start_date = to_date('" + start + "','RRRR-MM-DD'), " +
        //                             "psd_end_date = to_date('" + end + "','RRRR-MM-DD'), " +
        //                             "updated_by = user, " +
        //                             "updated_date = sysdate, " +
        //                             "psd_description = '" + desc.Replace("'", "'||CHR(39)||'") + "', " +
        //                             "psd_status = '" + status + "' " +
        //                             "WHERE psd_jcat = '" + jcat + "' " +
        //                             "AND psd_jmain = '" + jmain + "' " +
        //                             "AND psd_sub = '" + sub + "'";

        //    upd = DBconnect.AddEditDel(qtextMainJobUpd);

        //    if (!upd)
        //    {

        //        sub = Convert.ToDouble(getNewSubJobNo(sub)).ToString("000");
        //        rsno = sub + spec + extc;
        //        DBconnect.connect();

        //        string qtextMainJobIns = "INSERT INTO pms_subjob_details (psd_jcat, " +
        //                                 "psd_jmain, " +
        //                                 "psd_sub, " +
        //                                 "psd_spec, " +
        //                                 "psd_extc, " +
        //                                 "psd_rsno, " +
        //                                 "psd_description, " +
        //                                 "psd_status, " +
        //                                 "psd_start_date, " +
        //                                 "psd_end_date, " +
        //                                 "created_by, " +
        //                                 "created_date) " +
        //                                 "VALUES('" + jcat + "', " +
        //                                 "'" + jmain + "', " +
        //                                 "'" + sub + "', " +
        //                                 "'" + spec + "', " +
        //                                 "'" + extc + "', " +
        //                                 "'" + rsno + "', " +
        //                                 "'" + desc.Replace("'", "'||CHR(39)||'") + "', " +
        //                                 "'" + status + "', " +
        //                                 "to_date('" + start + "','RRRR-MM-DD'), " +
        //                                 "to_date('" + end + "','RRRR-MM-DD'), " +
        //                                 "user, " +
        //                                 "sysdate)";

        //        ins = DBconnect.AddEditDel(qtextMainJobIns);
        //    }

        //    if (upd || ins)
        //    {
        //        saveStatus = true;
        //    }

        //    return saveStatus;
        //}



        public DataTable loadJCat()
        {
            DAL.DataSource.DAL_DS_CaWork1 ds = new DataSource.DAL_DS_CaWork1();
            DataTable dt = ds.tblJobCat;
            DBconnect.connect();
            String IWO = " SELECT  cpc_job_category ," +
                                  "cpc_description" +
                         " FROM    cdl_project_category " +
                         " WHERE   cpc_job_type = 'N'";

            OracleDataReader IoDr = DBconnect.readtable(IWO);
            DataRow dr = null;

            while (IoDr.Read())
            {
                dr = dt.NewRow();
                dr["jCode"] = IoDr["cpc_job_category"].ToString();
                dr["Description"] = IoDr["cpc_description"].ToString();
                dt.Rows.Add(dr);
            }
            IoDr.Close();
            DBconnect.conn_new.Close();
            return dt;
        }


    }
}
