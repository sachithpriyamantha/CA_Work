using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_WORK.BLL
{
    class BLL_CS_CaWork
    {

        DAL.DAL_CS_CaWork d1 = new DAL.DAL_CS_CaWork();

        internal DataTable loadSubjobDetails(string jobno, string jcat )
        {
            return d1.loadSubjobDetails(jobno,jcat);
        }

        internal DataTable loadJCat()
        {
            return d1.loadJCat();
        }

        internal DataTable loadAllAccounts()
        {
            return d1.loadAllAccounts();
        }

        internal DataTable loadAllMainJobs(string ca)
        {
            return d1.loadAllMainJobs(ca);
        }

        internal DataTable loadMainJob(String cat, String jobno)
        {
            return d1.loadMainJob( cat,  jobno);
        }

        public bool saveCAMainJobRegister(string DspRate, string jcat, string jmain, string description, string startdate, string enddate, string status, string assetAccount)
        {

            return d1.saveCAMainJobRegister(DspRate, jcat, jmain, description, startdate, enddate, status, assetAccount);
        }

        public bool saveCASubJobRegister (string jcat, string jmain, string sub, string spec, string extc, string desc, string start, string end, string status)
        {

            return d1.saveCASubJobRegister ( jcat,  jmain,  sub,  spec,  extc,  desc,  start,  end,  status);
        }


        public string getNewMainJobNo(string jcat)
        {
            return d1.getNewMainJobNo(jcat);
        }

        public void addnewJobLine(string jobno)
        {
             d1.addnewJobLine(jobno);
        }

        internal bool IsMainJobExists(string jcat, string jmain)
        {
            return d1.IsMainJobExists(jcat, jmain);
        }



        //internal addnewJLine(string jobno)
        //{
        //    d1.addnewJLine(jobno);
        //}


    }
}
