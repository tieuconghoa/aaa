using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml;

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Sql;

namespace SqlDataProvider.BaseClass
{
    #region Sql_DbObject���ݿ�����ײ���
    public sealed class Sql_DbObject : IDisposable
    {
        #region 1���������
        System.Data.SqlClient.SqlConnection _SqlConnection;
        System.Data.SqlClient.SqlCommand _SqlCommand;
        System.Data.SqlClient.SqlDataAdapter _SqlDataAdapter;
        #endregion

        #region 2�����캯��
        public Sql_DbObject()
        {
            _SqlConnection = new SqlConnection();
        }

        public Sql_DbObject(string Path_Source, string Conn_DB)
        {
            switch (Path_Source)
            {
                case "WebConfig":
                    _SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Conn_DB].ConnectionString);                    
                    break;
                case "File":
                    _SqlConnection = new SqlConnection(Conn_DB);
                    break;
                case "AppConfig":
                    string str = System.Configuration.ConfigurationSettings.AppSettings[Conn_DB];
                    _SqlConnection = new SqlConnection(str);
                    break;
                default:
                    _SqlConnection = new SqlConnection(Conn_DB);
                    break;
            }
        }
        #endregion

        #region 3����һ�����ݿ�����
        private static bool OpenConnection(System.Data.SqlClient.SqlConnection _SqlConnection)
        {
            bool result = false;
            try
            {
                if (_SqlConnection.State != ConnectionState.Open)
                {
                    _SqlConnection.Open();
                    result = true;
                }
                else
                {
                    result = true;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                ApplicationLog.WriteError("�����ݿ����Ӵ���:" + ex.Message.Trim());
                result = false;
            }

            return result;
        }
        #endregion

        #region 4��ִ��sql��䣬����bool��ִ�н��
        /// <summary>
        ///  ִ��sql��䣬����bool��ִ�н��
        /// </summary>
        /// <param name="Sqlcomm">sql���</param>
        /// <returns>bool�ͱ���</returns>
        public bool Exesqlcomm(string Sqlcomm)
        {
            if (!OpenConnection(this._SqlConnection)) return false;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.CommandType = CommandType.Text;
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandText = Sqlcomm;
                this._SqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ��sql���: " + Sqlcomm + "������ϢΪ: " + ex.Message.Trim());
                return false;
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return true;
        }
        #endregion

        #region 5��ִ��sql��䣬����int�͵�һ�е�һ����ֵ
        /// <summary>
        /// ִ��sql��䣬���ص�һ�е�һ��
        /// </summary>
        /// <param name="Sqlcomm">sql���</param>
        /// <returns>���صĵ�һ�е�һ�е���ֵ</returns>
        public int GetRecordCount(string Sqlcomm)
        {
            int retval = 0;

            if (!OpenConnection(this._SqlConnection))
            {
                retval = 0;
            }
            else
            {
                try
                {
                    this._SqlCommand = new SqlCommand();
                    this._SqlCommand.Connection = this._SqlConnection;
                    this._SqlCommand.CommandType = CommandType.Text;
                    this._SqlCommand.CommandText = Sqlcomm;

                    if (this._SqlCommand.ExecuteScalar() == null) { retval = 0; }
                    else { retval = (int)this._SqlCommand.ExecuteScalar(); }
                }
                catch (SqlException ex)
                {
                    ApplicationLog.WriteError("ִ��sql���: " + Sqlcomm + "������ϢΪ: " + ex.Message.Trim());
                }
                finally
                {
                    this._SqlConnection.Close();
                    this.Dispose(true);
                }
            }

            return retval;
        }
        #endregion

        #region 6��ִ��sql��䣬����DataTable�������ݼ�
        /// <summary>
        /// ִ��sql��䡣����DataTable�������ݼ�
        /// </summary>
        /// <param name="TableName">���ݱ���</param>
        /// <param name="Sqlcomm">sql���</param>
        /// <returns>����DataTable�������ݼ�</returns>
        public DataTable GetDataTableBySqlcomm(string TableName, string Sqlcomm)
        {
            System.Data.DataTable ResultTable = new DataTable(TableName);

            if (!OpenConnection(this._SqlConnection)) return ResultTable;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.Text;
                this._SqlCommand.CommandText = Sqlcomm;

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;

                this._SqlDataAdapter.Fill(ResultTable);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ��sql���: " + Sqlcomm + "������ϢΪ: " + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return ResultTable;
        }
        #endregion

        #region 7��ִ��Sql��䣬����DataSet�������ݼ�
        /// <summary>
        /// ����һ��DataSet 
        /// </summary>
        /// <param name="TableName">�������</param>
        /// <param name="Sqlcomm">ִ�в�ѯ���</param>
        /// <returns></returns>
        public DataSet GetDataSetBySqlcomm(string TableName, string Sqlcomm)
        {
            System.Data.DataSet ResultTable = new DataSet();
            if (!OpenConnection(this._SqlConnection)) return ResultTable;
            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.Text;
                this._SqlCommand.CommandText = Sqlcomm;
                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;
                this._SqlDataAdapter.Fill(ResultTable);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ��Sql��䣺" + Sqlcomm + "������ϢΪ��" + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }
            return ResultTable;
        }
        #endregion

        #region 8��ִ��һ��SQL��� ���� MySqlDataReader���ݼ�
        public bool FillSqlDataReader(ref SqlDataReader Sdr, string SqlComm)
        {
            if (!OpenConnection(this._SqlConnection))
            {
                return false;
            }
            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.Text;
                this._SqlCommand.CommandText = SqlComm;
                Sdr = this._SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                return true;
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ��Sql��䣺" + SqlComm + "������ϢΪ��" + ex.Message.Trim());

            }
            finally
            {

                this.Dispose(true);
            }
            return false;
        }
        #endregion

        #region 9��ִ��sql��䣬����DataTable�������ݼ����ṩ��ҳ
        /// <summary>
        /// ִ��sql��䣬����DataTable�������ݼ����ṩ��ҳ
        /// </summary>
        /// <param name="TableName">���ݱ���</param>
        /// <param name="ProcedureName">sql���</param>
        /// <param name="StartRecordNo">��ʼ����</param>
        /// <param name="PageSize">һҳ�Ĵ�С</param>
        /// <returns>���ط�ҳDataTable�������ݼ�</returns>
        public DataTable GetDataTableBySqlcomm(string TableName, string Sqlcomm, int StartRecordNo, int PageSize)
        {
            DataTable RetTable = new DataTable(TableName);

            if (!OpenConnection(this._SqlConnection))
            {
                RetTable.Dispose();
                this.Dispose(true);
                return RetTable;
            }

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.Text;
                this._SqlCommand.CommandText = Sqlcomm;

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;

                DataSet ds = new DataSet();
                ds.Tables.Add(RetTable);

                this._SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ��sql���: " + Sqlcomm + "������ϢΪ: " + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return RetTable;
        }
        #endregion

        #region 10��ִ�д������洢���̣�����ִ�е�bool�ͽ��
        /// <summary>
        /// ִ�д������洢���̣�����ִ�е�bool�ͽ��
        /// </summary>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="SqlParameters">��������</param>
        /// <returns>ִ�е�bool�ͽ��</returns>
        public bool RunProcedure(string ProcedureName, SqlParameter[] SqlParameters)
        {
            if (!OpenConnection(this._SqlConnection)) return false;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }

                this._SqlCommand.ExecuteNonQuery();
                //this._SqlCommand.BeginExecuteNonQuery(RunProcedureCallback, null);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
                return false;
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return true;
        }
        

        #endregion

        #region 11��ִ�в��������洢���̣�����ִ�е�bool�ͽ��
        /// <summary>
        /// ִ�в��������洢���̣�����ִ�е�bool�ͽ��
        /// </summary>
        /// <param name="ProcedureName">�洢������</param>
        /// <returns>ִ�е�bool�ͽ��</returns>
        public bool RunProcedure(string ProcedureName)
        {
            if (!OpenConnection(this._SqlConnection)) return false;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                this._SqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
                return false;
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return true;
        }
        #endregion

        #region 12��ִ���޲����洢���̣��õ�SqlDataReader�������ݼ�������bool��ִ�н��
        /// <summary>
        /// ִ���޲����洢���̣��õ�SqlDataReader�������ݼ�������bool��ִ�н��  
        /// </summary>
        /// <param name="ResultDataReader">SqlDataReader�������ݼ�����</param>
        /// <param name="ProcedureName">�洢��������</param>
        /// <returns>����bool��ִ�н��</returns>
        public bool GetReader(ref SqlDataReader ResultDataReader, string ProcedureName)
        {
            if (!OpenConnection(this._SqlConnection)) return false;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                ResultDataReader = this._SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
                return false;
            }

            return true;
        }
        #endregion

        #region 13�� ִ���в����洢���̣��õ�SqlDataReader�������ݼ�������bool��ִ�н��
        /// <summary>
        /// ִ���в����洢���̣��õ�SqlDataReader�������ݼ�������bool��ִ�н��
        /// </summary>
        /// <param name="ResultDataReader">SqlDataReader�������ݼ�����</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="SqlParameters">�洢���̲�������</param>
        /// <returns>����bool��ִ�н��</returns>
        public bool GetReader(ref SqlDataReader ResultDataReader, string ProcedureName, SqlParameter[] SqlParameters)
        {
            if (!OpenConnection(this._SqlConnection)) return false;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }


                ResultDataReader = this._SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
                return false;
            }

            return true;
        }
        #endregion

        #region 14��ִ�д������Ĵ洢����,����DataSet�������ݼ�
        /// <summary>
        /// ִ�д������洢���̣�����DataSet�������ݼ�
        /// </summary>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="MySqlParameters">�洢���̲���</param>
        /// <returns>����DataSet</returns>
        public DataSet GetDataSet(string ProcedureName, SqlParameter[] SqlParameters)
        {
            System.Data.DataSet FullDataSet = new DataSet();
            if (!OpenConnection(this._SqlConnection))
            {
                FullDataSet.Dispose();
                return FullDataSet;
            }
            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;
                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }
                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;
                this._SqlDataAdapter.Fill(FullDataSet);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢���̣�" + ProcedureName + "������ϢΪ��" + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);

            }
            return FullDataSet;
        }
        #endregion

        #region 15��ִ���в����洢���̣��õ���ҳDataSet�������ݼ�������BOOL��ִ�н��
        /// <summary>
        /// ִ���в����洢���̣��õ���ҳDataSet�������ݼ�������Bool��ִ�н��
        /// </summary>
        /// <param name="ResultDataSet">��ҳDataSet�������ݼ�</param>
        /// <param name="TableName">���ݱ���</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="StartRecordNo">��ʼ����</param>
        /// <param name="PageSize">һҳ�Ĵ�С</param>
        /// <param name="MySqlParameters">��������</param>
        /// <returns>����BOOL��ִ�н��</returns>       
        public bool GetDataSet(ref DataSet ResultDataSet, ref int row_total, String TableName, string ProcedureName, int StartRecordNo, int PageSize, SqlParameter[] SqlParameters)
        {
            if (!OpenConnection(this._SqlConnection)) return false;
            try
            {
                row_total = 0;
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;
                DataSet ds = new DataSet();
                row_total = this._SqlDataAdapter.Fill(ds);
                this._SqlDataAdapter.Fill(ResultDataSet, StartRecordNo, PageSize, TableName);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢���̣�" + ProcedureName + "������ϢΪ��" + ex.Message.Trim());
                return false;
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }
            return true;
        }
        #endregion

        #region 16��ִ�д������洢���̣�����DataSet�������ݼ�
        /// <summary>
        /// ִ�д������洢���̣�����DataSet�������ݼ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="MySqlParameters">��������</param>
        /// <returns>����DataSet�������ݼ�</returns>
        public DataSet GetDateSet(string DatesetName, string ProcedureName, SqlParameter[] SqlParameters)
        {
            System.Data.DataSet FullDataSet = new DataSet(DatesetName);
            if (!OpenConnection(this._SqlConnection))
            {
                FullDataSet.Dispose();
                return FullDataSet;
            }
            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }
                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;
                this._SqlDataAdapter.Fill(FullDataSet);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢���̣�" + ProcedureName + "������ϢΪ��" + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);

            }
            return FullDataSet;
        }
        #endregion

        #region 17��ִ���в����洢���̣�����DataTable�������ݼ�
        /// <summary>
        /// ִ���в����洢���̣�����DataTable�������ݼ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="SqlParameters">��������</param>
        /// <returns>����DataTable�������ݼ�</returns>
        public DataTable GetDataTable(string TableName, string ProcedureName, SqlParameter[] SqlParameters)
        {
            //System.Data.DataTable FullTable = new DataTable(TableName);

            //if (!OpenConnection(this._SqlConnection))
            //{
            //    FullTable.Dispose();
            //    this.Dispose(true);
            //    return FullTable;
            //}

            //try
            //{
            //    this._SqlCommand = new SqlCommand();
            //    this._SqlCommand.Connection = this._SqlConnection;
            //    this._SqlCommand.CommandType = CommandType.StoredProcedure;
            //    this._SqlCommand.CommandText = ProcedureName;

            //    foreach (SqlParameter parameter in SqlParameters)
            //    {
            //        this._SqlCommand.Parameters.Add(parameter);
            //    }

            //    this._SqlDataAdapter = new SqlDataAdapter();
            //    this._SqlDataAdapter.SelectCommand = this._SqlCommand;
            //    this._SqlDataAdapter.Fill(FullTable);
            //}
            //catch (SqlException ex)
            //{
            //    ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
            //}
            //finally
            //{
            //    this._SqlConnection.Close();
            //    this.Dispose(true);
            //}

            //return FullTable;

            return GetDataTable(TableName, ProcedureName, SqlParameters, -1);
        }

        /// <summary>
        /// ִ���в����洢���̣�����DataTable�������ݼ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="SqlParameters">��������</param>
        /// <returns>����DataTable�������ݼ�</returns>
        public DataTable GetDataTable(string TableName, string ProcedureName, SqlParameter[] SqlParameters,int commandTimeout)
        {
            System.Data.DataTable FullTable = new DataTable(TableName);

            if (!OpenConnection(this._SqlConnection))
            {
                FullTable.Dispose();
                this.Dispose(true);
                return FullTable;
            }

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;
                if (commandTimeout >= 0)
                {
                    this._SqlCommand.CommandTimeout = commandTimeout;
                }

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;
                this._SqlDataAdapter.Fill(FullTable);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return FullTable;
        }

        #endregion

        #region 18�� ִ���޲����洢���̣�����DataTable�������ݼ�
        /// <summary>
        /// ִ���޲����洢���̣�����DataTable�������ݼ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <returns>����DataTable�������ݼ�</returns>
        public DataTable GetDataTable(string TableName, string ProcedureName)
        {

            System.Data.DataTable FullTable = new DataTable(TableName);

            if (!OpenConnection(this._SqlConnection))
            {
                FullTable.Dispose();
                this.Dispose(true);
                return FullTable;
            }

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;

                this._SqlDataAdapter.Fill(FullTable);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return FullTable;
        }
        #endregion

        #region 19��ִ���޲����洢���̣�����DataTable�������ݼ����ṩ��ҳ
        /// <summary>
        /// ִ���޲����洢���̣�����DataTable�������ݼ����ṩ��ҳ
        /// </summary>
        /// <param name="TableName">���ݱ���</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="StartRecordNo">��ʼ����</param>
        /// <param name="PageSize">һҳ�Ĵ�С</param>
        /// <returns>���ط�ҳDataTable�������ݼ�</returns>
        public DataTable GetDataTable(string TableName, string ProcedureName, int StartRecordNo, int PageSize)
        {
            DataTable RetTable = new DataTable(TableName);

            if (!OpenConnection(this._SqlConnection))
            {
                RetTable.Dispose();
                this.Dispose(true);
                return RetTable;
            }

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;

                DataSet ds = new DataSet();
                ds.Tables.Add(RetTable);

                this._SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return RetTable;
        }
        #endregion

        #region 20�� ִ���в����洢���̣�����DataTable�������ݼ����ṩ��ҳ
        /// <summary>
        /// ִ���в����洢���̣�����DataTable�������ݼ����ṩ��ҳ
        /// </summary>
        /// <param name="TableName">���ݱ���</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="SqlParameters">��������</param>
        /// <param name="StartRecordNo">��ʼ����</param>
        /// <param name="PageSize">һҳ�Ĵ�С</param>
        /// <returns>���ط�ҳDataTable�������ݼ�</returns>
        public DataTable GetDataTable(string TableName, string ProcedureName, SqlParameter[] SqlParameters, int StartRecordNo, int PageSize)
        {

            DataTable RetTable = new DataTable(TableName);

            if (!OpenConnection(this._SqlConnection))
            {
                RetTable.Dispose();
                this.Dispose(true);
                return RetTable;
            }

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;

                DataSet ds = new DataSet();
                ds.Tables.Add(RetTable);

                this._SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return RetTable;
        }
        #endregion

        #region 21��ִ���޲����洢���̣��õ���ҳDataTable�������ݼ�������bool��ִ�н��
        /// <summary>
        /// ִ���޲����洢���̣��õ���ҳDataTable�������ݼ�������bool��ִ�н��
        /// </summary>
        /// <param name="ResultTable">��ҳDataTable�������ݼ�</param>
        /// <param name="TableName">���ݱ���</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="StartRecordNo">��ʼ����</param>
        /// <param name="PageSize">һҳ�Ĵ�С</param>
        /// <returns>����bool��ִ�н��</returns>
        public bool GetDataTable(ref DataTable ResultTable, string TableName, string ProcedureName, int StartRecordNo, int PageSize)
        {
            ResultTable = null;

            if (!OpenConnection(this._SqlConnection)) return false;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;

                DataSet ds = new DataSet();
                ds.Tables.Add(ResultTable);

                this._SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
                ResultTable = ds.Tables[TableName];
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
                return false;
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return true;
        }
        #endregion

        #region 22��ִ���в����洢���̣��õ���ҳDataTable�������ݼ�������bool��ִ�н��
        /// <summary>
        /// ִ���в����洢���̣��õ���ҳDataTable�������ݼ�������bool��ִ�н��
        /// </summary>
        /// <param name="ResultTable">��ҳDataTable�������ݼ�</param>
        /// <param name="TableName">���ݱ���</param>
        /// <param name="ProcedureName">�洢������</param>
        /// <param name="StartRecordNo">��ʼ����</param>
        /// <param name="PageSize">һҳ�Ĵ�С</param>
        /// <param name="SqlParameters">��������</param>
        /// <returns>����bool��ִ�н��</returns>
        public bool GetDataTable(ref DataTable ResultTable, string TableName, string ProcedureName, int StartRecordNo, int PageSize, SqlParameter[] SqlParameters)
        {
            if (!OpenConnection(this._SqlConnection)) return false;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }

                this._SqlDataAdapter = new SqlDataAdapter();
                this._SqlDataAdapter.SelectCommand = this._SqlCommand;

                DataSet ds = new DataSet();
                ds.Tables.Add(ResultTable);

                this._SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
                ResultTable = ds.Tables[TableName];
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
                return false;
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }

            return true;
        }
        #endregion

        #region 23����������
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (this._SqlDataAdapter != null)
            {
                if (this._SqlDataAdapter.SelectCommand != null)
                {
                    if (this._SqlCommand.Connection != null)
                        this._SqlDataAdapter.SelectCommand.Connection.Dispose();
                    this._SqlDataAdapter.SelectCommand.Dispose();
                }
                this._SqlDataAdapter.Dispose();
                this._SqlDataAdapter = null;
            }
        }
        #endregion

        #region 24���첽ִ�д��������������ؽ���Ĵ洢����
        /// <summary>
        /// �첽ִ�д��������������ؽ���Ĵ洢����
        /// </summary>
        /// <param name="ProcedureName">�洢������</param>
        public void BeginRunProcedure(string ProcedureName, SqlParameter[] SqlParameters)
        {
            if (!OpenConnection(this._SqlConnection)) return ;

            try
            {
                this._SqlCommand = new SqlCommand();
                this._SqlCommand.Connection = this._SqlConnection;
                this._SqlCommand.CommandType = CommandType.StoredProcedure;
                this._SqlCommand.CommandText = ProcedureName;

                foreach (SqlParameter parameter in SqlParameters)
                {
                    this._SqlCommand.Parameters.Add(parameter);
                }

                this._SqlCommand.BeginExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                ApplicationLog.WriteError("ִ�д洢����: " + ProcedureName + "������ϢΪ: " + ex.Message.Trim());
            }
            finally
            {
                this._SqlConnection.Close();
                this.Dispose(true);
            }
        }
        #endregion

      


    }
    #endregion

    
}
