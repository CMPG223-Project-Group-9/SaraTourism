using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    /// <summary>Small helpers for pulling typed values out of a DataRow, DBNull-safe.</summary>
    public static class RowExtensions
    {
        public static string GetStr(this DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? row[col].ToString() : null;
        }

        public static int GetInt(this DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? Convert.ToInt32(row[col]) : 0;
        }

        public static int? GetIntOrNull(this DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? (int?)Convert.ToInt32(row[col]) : null;
        }

        public static decimal GetDec(this DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? Convert.ToDecimal(row[col]) : 0m;
        }

        public static bool GetBool(this DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value && Convert.ToInt32(row[col]) == 1;
        }

        public static DateTime GetDate(this DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? Convert.ToDateTime(row[col]) : DateTime.MinValue;
        }

        public static TimeSpan GetTime(this DataRow row, string col)
        {
            if (!row.Table.Columns.Contains(col) || row[col] == DBNull.Value) return TimeSpan.Zero;
            var val = row[col];
            return val is TimeSpan ? (TimeSpan)val : TimeSpan.Parse(val.ToString());
        }

        public static long GetLong(this DataRow row, string col)
        {
            return row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? Convert.ToInt64(row[col]) : 0L;
        }
    }
}