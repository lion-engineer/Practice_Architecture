using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Architecture.API.Service._DataAccess
{
    public class BaseRepository
    {
        private readonly string _conStr;
        public BaseRepository()
        {
            _conStr = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public int ExecuteSqlCommand(string sql, params SqlParameter[] parameters)
        {
            try
            {
                var res = 0;
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        conn.Open();
                        res = cmd.ExecuteNonQuery();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
            
        }
        public IList<T> ExecuteSqlQuery<T>(string sql, params SqlParameter[] parameters) 
        {
            return DoSqlQuery<T>(sql, (reader) => 
            {
                var res = new List<T>();
                while (reader.Read())
                {
                    var value = reader.GetFieldValue<object>(0);
                    if (value.GetType() == typeof(System.DBNull))
                    {
                        value = null;
                    }
                    else
                    {
                        if (typeof(T) != value.GetType())
                        {
                            var uPropType = Nullable.GetUnderlyingType(typeof(T));
                            if (uPropType == null || uPropType != value.GetType())
                            {
                                if (uPropType == null)
                                {
                                    uPropType = typeof(T);
                                }
                                value = ConvertValue(value, uPropType);
                            }
                        }
                    }
                    res.Add((T)value);
                }

                return res;
            }, parameters);
        }
        
        private IList<T> DoSqlQuery<T>(string sql, Func<DbDataReader, List<T>> setResultValues, params SqlParameter[] parameters)
        {
            try
            {
                DbDataReader reader = null;
                using (var conn = new SqlConnection(_conStr)) 
                {
                    using (var cmd = conn.CreateCommand()) 
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null) 
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        conn.Open();
                        reader = cmd.ExecuteReader();
                        return setResultValues(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private object ConvertValue(object value, Type targetType) 
        {
            if (targetType == typeof(decimal))
            {
                return Convert.ToDecimal(value);
            }
            else if (targetType == typeof(double))
            {
                return Convert.ToDouble(value);
            }
            else if (targetType == typeof(float))
            {
                return Convert.ToSingle(value);
            }
            else if (targetType == typeof(int))
            {
                return Convert.ToInt32(value);
            }
            else if (targetType == typeof(short))
            {
                return Convert.ToInt16(value);
            }
            else if (targetType == typeof(Int64))
            {
                return Convert.ToInt64(value);
            }
            else if (targetType == typeof(byte))
            {
                return Convert.ToByte(value);
            }
            return value;
        }
    }
}
