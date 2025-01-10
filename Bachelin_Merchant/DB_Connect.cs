using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yogiyo_MerchantList
{
    class DB_Connect
    {
        #region Memeber Variables
        private SqlConnection _myConnection;
        private SqlCommand _myCommand;
        private SqlTransaction _myTransaction;


        //서버 연결 셋팅
        static public string defaultConnectionString = "data source=127.0.0.1;initial catalog=TEST;password=sa123;persist security info=true;user id=sa;packet size=4096;Connect Timeout=120";

        #endregion

        #region Property 부분(DBType, ConnectionString, SqlCommand)

        //현재 ConnectionString값에 대한 속성  
        public string ConnectionString
        {
            get
            {
                return _myConnection.ConnectionString;
            }

            set
            {
                _myConnection.ConnectionString = value;
            }

        }

        //현재 가지고 있는 (System.Data.SqlClient.SqlConnection)myConnection 속성
        public SqlConnection MyConnection
        {
            get
            {
                return _myConnection;
            }

            set
            {
                _myConnection = value;
            }
        }

        //현재 가지고 있는 (System.Data.SqlClient.SqlCommand)MyCommand 속성 
        public SqlCommand MyCommand
        {
            get
            {
                return _myCommand;
            }

            set
            {
                _myCommand = value;
            }
        }


        // 현재 가지고 있는 (System.Data.SqlClient.SqlCommand)MyCommand.CommandType.
        //		(System.Data.CommandType)CommandType 속성으로 System.Data.CommandType중 하나
        public CommandType CommandType
        {
            set
            {
                _myCommand.CommandType = value;
            }
            get
            {
                return _myCommand.CommandType;
            }
        }

        // 현재 가지고 있는 (System.Data.SqlClient.SqlCommand)MyCommand.CommandText.
        //			(string)CommandText 속성으로 실행할 Stored procedure명 or T-SQL 문장 
        public string CommandText
        {
            get
            {
                return _myCommand.CommandText;
            }
            set
            {
                //CommandText가 바뀌었으므로, Parameters 정보를 초기화시킨다.
                _myCommand.Parameters.Clear();
                _myCommand.CommandText = value;
            }
        }

        public SqlTransaction MyTransaction
        {
            get
            {
                return _myTransaction;
            }

            set
            {
                _myTransaction = value;
            }
        }
        #endregion

        #region 생성자 DB_Connect(string connectionString, string cmdText, CommandType cmdType)

        /// <summary>
        /// DbAgent의 생성자로 3개의 override method로 구성되어, ConnectionString, CommandText, CommandType 속성 등을
        /// 설정해준다.
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="cmdType">the CommandType(System.Data.CommandType)</param>
        /// <param name="cmdText">실행할 Stored procedure명 or T-SQL 문장 (string)</param>
        public DB_Connect(string connectionString, string cmdText, CommandType cmdType)
        {
            _myConnection = new SqlConnection();

            _myCommand = new SqlCommand(cmdText, (SqlConnection)_myConnection);

            //#. memeber variable의 속성 설정 
            _myConnection.ConnectionString = (connectionString == "") ? defaultConnectionString : connectionString;
            _myCommand.CommandType = (((int)cmdType) == -1) ? CommandType.Text : cmdType;
        }  // end of public constructor


        public DB_Connect(string cmdText, CommandType cmdType)
            : this("", cmdText, cmdType) { }

        public DB_Connect(string cmdText)
            : this("", cmdText, (CommandType)(-1)) { }

        #endregion

        #region  트랜잭션 관리

        public bool BeginTransaction()
        {
            OpenConnection();
            _myTransaction = _myConnection.BeginTransaction();
            return true;
        }

        public bool CommitTran()
        {
            _myTransaction.Commit();
            CloseConnection();

            return true;
        }

        public bool RollbackTran()
        {
            _myTransaction.Rollback();
            CloseConnection();

            return true;
        }

        #endregion

        #region ExcuteDataReader
        //IDataReader 반환(db 특성이 반영되지 않은 상태에서 넘긴다.)
        public SqlDataReader ExcuteDataReader()
        {
            SqlDataReader rd = null;

            try
            {
                OpenConnection();
                rd = _myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                return null;
            }
            return rd;
        }
        #endregion

        #region ExcuteNonQuery
        public string ExcuteNonQuery()
        {
            int result;
            string message;

            try
            {
                OpenConnection();

                result = _myCommand.ExecuteNonQuery();

                message = "";
                return message;

            }
            catch (SqlException e)
            {
                message = "Error : " + e.Message.ToString();

                return message;
            }
            finally
            {
                CloseConnection();

            }
        }

        public string ExcuteNonQueryConn()
        {
            int result;
            string message;

            try
            {
                OpenConnection();

                result = _myCommand.ExecuteNonQuery();

                message = "";
                return message;

            }
            catch (SqlException e)
            {
                message = "Error : " + e.Message.ToString();

                return message;
            }
            finally
            {
                //CloseConnection();

            }
        }

        //트랜잭션 처리용
        public string ExcuteNonQuery(int TransFlowType)
        {
            string message;

            try
            {
                if (TransFlowType == 0 || TransFlowType == 3)
                {
                    OpenConnection();
                    _myTransaction = _myConnection.BeginTransaction();
                }

                _myCommand.Transaction = _myTransaction;

                _myCommand.ExecuteNonQuery();

                if (TransFlowType == 2 || TransFlowType == 3)
                {
                    _myTransaction.Commit();
                    CloseConnection();
                }

                message = "";

                return message;

            }
            catch (SqlException e)
            {
                //sql server에서 raiserror를 이용하여 에러를 발생시키면서 에러 메시지를 text로 입력한 경우 : 업무 로직에 의한 프로세스 중단
                if (e.Number == 50000)
                    message = "경고!! : " + e.Message.ToString();
                else
                    message = "Error : " + e.Message.ToString();

                _myTransaction.Rollback();
                CloseConnection();

                return message;
            }
        }

        public void ExcuteNonQuery(ref int rValue, ref string Msg, bool TransactionType)
        {
            try
            {
                OpenConnection();

                _myCommand.ExecuteNonQuery();
                rValue = System.Convert.ToInt16(_myCommand.Parameters["ReturnValue"].Value);
                //Msg = _myCommand.Parameters["@message"].Value.ToString();

            }
            catch (SqlException e)
            {
                rValue = -1;//성공시 SP에서 return @@error 값인 0을 보낸다.
                Msg = "Error : " + e.Message.ToString();

            }
            finally
            {
                if (TransactionType == false)
                    CloseConnection();

            }
        }

        public void ExcuteNonQuery(ref int rValue, ref string Msg, ref int recordsAffected, bool TranscationType)
        {

            try
            {
                OpenConnection();

                recordsAffected = _myCommand.ExecuteNonQuery();
                rValue = System.Convert.ToInt16(_myCommand.Parameters["ReturnValue"].Value);
                //Msg = _myCommand.Parameters["@message"].Value.ToString();

            }
            catch (SqlException e)
            {
                recordsAffected = -1;//
                rValue = -1;//성공시 SP에서 return @@error 값인 0을 보낸다.
                Msg = "Error : " + e.Message.ToString();

            }
            finally
            {
                if (TranscationType == false)
                    CloseConnection();

            }
        }
        #endregion

        #region ExcuteDataSet

        /// <summary>
        /// DbAgent의 생성자로 3개의 override method로 구성되어, ConnectionString, CommandText, CommandType 속성 등을
        /// 설정해준다.
        /// </summary>
        /// <returns> 생성자이므로 Return값없음ted by the command</returns>
        public DataSet ExcuteDataSet()
        {
            DataSet myDataSet = new DataSet();

            try
            {
                OpenConnection();

                SqlDataAdapter myDataAdapter = new SqlDataAdapter(_myCommand);
                myDataAdapter.Fill(myDataSet);
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
            return myDataSet;
        }
        #endregion

        #region Private Fuction
        private void OpenConnection()
        {
            if (_myConnection.State == ConnectionState.Closed)
                _myConnection.Open();

        }

        private void CloseConnection()
        {
            if (_myConnection.State == ConnectionState.Open)
                _myConnection.Close();
        }

        #endregion
    }
}
