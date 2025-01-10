using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yogiyo_MerchantList
{
    class DB_Excute
    {
        private DB_Connect myDbAgent;

        public DB_Excute()
        {
            myDbAgent = new DB_Connect(DB_Connect.defaultConnectionString, "", System.Data.CommandType.StoredProcedure);
        }

        public DB_Excute(string pConnectString)
        {
            myDbAgent = new DB_Connect(pConnectString, "", System.Data.CommandType.StoredProcedure);
        }

        //파리미터 셋팅하는 부분 분리
        private void paramSetting(string SP_name, params string[] Arrparam)
        {
            myDbAgent.CommandText = SP_name;
            myDbAgent.MyCommand.Parameters.Clear();

            int i = 0;
            int k, j;
            while (i < Arrparam.Length)
            {
                //":"의 위치를 인덱스로 반환
                k = Arrparam[i].IndexOf(":");
                //char의 갯수를 반환
                j = Arrparam[i].Length;

                if (k + 1 == j)
                    myDbAgent.MyCommand.Parameters.AddWithValue(Arrparam[i].Substring(0, k), "");
                else
                    myDbAgent.MyCommand.Parameters.AddWithValue(Arrparam[i].Substring(0, k), Arrparam[i].Substring(k + 1, j - k - 1));
                i++;
            }

            //myDbAgent.MyCommand.Parameters.Add("@message", SqlDbType.NVarChar, 2000);
            //myDbAgent.MyCommand.Parameters["@message"].Direction = ParameterDirection.Output;
        }

        //반환없을때
        public void SpExecuteNoResult(string SP_name, params string[] Arrparam)
        {

            paramSetting(SP_name, Arrparam);

            string message;

            message = myDbAgent.ExcuteNonQuery();
        }

        //반환없을때
        public void SpExecuteNoResultConn(string SP_name, params string[] Arrparam)
        {

            paramSetting(SP_name, Arrparam);

            string message;

            message = myDbAgent.ExcuteNonQueryConn();
        }

        //string 을 반환하는 SpExecuteString 함수의 공통 부분
        private string SpExecuteStringCommon(string SP_name)
        {
            string message = "";

            message = myDbAgent.ExcuteNonQuery();

            if (message.ToString().Length != 0)
            {

                return message;
            }
            else
            {
                message = myDbAgent.MyCommand.Parameters["@message"].Value.ToString();

                return myDbAgent.MyCommand.Parameters["@message"].Value.ToString();
            }
        }

        //string 을 반환할때
        public string SpExecuteString(string SP_name, params string[] Arrparam)
        {

            paramSetting(SP_name, Arrparam);

            string message;

            message = SpExecuteStringCommon(SP_name);

            return message;

        }


        //Table을 반환할때
        public System.Data.DataTable SpExecuteTable(string SP_name, params string[] Arrparam)
        {
            paramSetting(SP_name, Arrparam);

            DataTable dt;


            dt = myDbAgent.ExcuteDataSet().Tables[0];


            return dt;
        }



        //DataSet을 반환할때
        public System.Data.DataSet SpExecuteDataSet(string SP_name, params string[] Arrparam)
        {
            paramSetting(SP_name, Arrparam);

            DataSet ds;


            ds = myDbAgent.ExcuteDataSet();


            return ds;
        }
    }
}
