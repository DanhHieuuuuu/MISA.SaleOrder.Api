using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Core.Dtos;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.MISAAttributes;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repositories
{
    public class BaseRepo<T, FilterBase, PageResult> : IBaseRepo<T, FilterBase, PageResult>  
        where T : class
        where FilterBase : FilterBaseDto
        where PageResult : PageResultDto<T>, new()
    {
        protected readonly string connectionString;

        public BaseRepo(IConfiguration config)
        {
            connectionString = config.GetConnectionString("Default");
        }

        public int Delete(Guid id)
        {
            var tableName = GetTableName();
            var keyProp = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttribute<MISAKeyAttribute>() != null);

            if (keyProp == null)
                throw new Exception($"Không tìm thấy thuộc tính khóa chính (MISAKey) trong {typeof(T).Name}");

            var keyColAttr = keyProp.GetCustomAttribute<MISAColumnNameAttribute>();
            var keyColumn = keyColAttr?.ColumnName ?? keyProp.Name;

            var sql = $"DELETE FROM {tableName} WHERE {keyColumn} = @Id";

            using (var connection = new MySqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);
                return connection.Execute(sql, param);
            }
        }

        public T? FindById(Guid id)
        {
            var tableName = GetTableName();
            var keyProp = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttribute<MISAKeyAttribute>() != null);

            if (keyProp == null)
                throw new Exception($"Không tìm thấy thuộc tính khóa chính (MISAKey) trong {typeof(T).Name}");
            var keyColAttr = keyProp.GetCustomAttribute<MISAColumnNameAttribute>();
            var keyColumn = keyColAttr?.ColumnName ?? keyProp.Name;

            var sql = $"SELECT * FROM {tableName} WHERE {keyColumn} = @Id";

            using (var connection = new MySqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);
                return connection.QueryFirstOrDefault<T>(sql, param);
            }
        }

        public List<T> GetAll()
        {
            var tableName = GetTableName();
            var sql = $"Select * from {tableName}";
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                var data = mySqlConnection.Query<T>(sql).ToList();

                return data;
            }
        }

        public T Insert(T entity)
        {
            var tableName = GetTableName();

            var properties = typeof(T).GetProperties()
                                      .Where(p => p.CanRead)
                                      .ToList();

            var columns = new List<string>();
            var parameters = new DynamicParameters();

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<MISAColumnNameAttribute>();
                var columnName = columnAttr?.ColumnName ?? prop.Name;
                columns.Add(columnName);
                parameters.Add($"@{prop.Name}", prop.GetValue(entity));
            }

            var columnNames = string.Join(", ", columns);
            var paramNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames});";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sql, parameters);
                return entity;
            }
        }

        public int Update(T entity)
        {
            var tableName = GetTableName();
            var props = typeof(T).GetProperties().Where(p => p.CanRead).ToList();

            // Tìm property có [MISAKey]
            var keyProp = props.FirstOrDefault(p => p.GetCustomAttribute<MISAKeyAttribute>() != null);
            if (keyProp == null)
                throw new Exception($"Không tìm thấy thuộc tính khóa chính (MISAKey) trong {typeof(T).Name}");

            var keyColAttr = keyProp.GetCustomAttribute<MISAColumnNameAttribute>();
            var keyColumn = keyColAttr?.ColumnName ?? keyProp.Name;
            var keyValue = keyProp.GetValue(entity);

            var setClauses = new List<string>();
            var parameters = new DynamicParameters();

            foreach (var prop in props)
            {
                if (prop == keyProp) continue; // bỏ qua cột khóa chính

                var colAttr = prop.GetCustomAttribute<MISAColumnNameAttribute>();
                var colName = colAttr?.ColumnName ?? prop.Name;
                setClauses.Add($"{colName} = @{prop.Name}");
                parameters.Add("@" + prop.Name, prop.GetValue(entity));
            }

            parameters.Add("@Id", keyValue);

            var sql = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {keyColumn} = @Id";

            using (var connection = new MySqlConnection(connectionString))
            {
                return connection.Execute(sql, parameters);
            }
        }

        public PageResult Paging(FilterBase filter)
        {
            string tableName = GetTableName();

            //Tạo danh sách điều kiện WHERE động
           var offset = (filter.PageIndex - 1) * filter.PageSize;

            // Tạo danh sách điều kiện WHERE động
            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            // Keyword search (dựa trên attribute [MISAKeyword])
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var keywordCols = typeof(T).GetProperties()
                    .Where(p => p.GetCustomAttribute<MISAKeywordAttribute>() != null)
                    .Select(p => p.GetCustomAttribute<MISAColumnNameAttribute>()?.ColumnName ?? p.Name)
                    .ToList();

                if (keywordCols.Any())
                {
                    conditions.Add($"CONCAT_WS(' ', {string.Join(", ", keywordCols)}) LIKE @Keyword");
                    parameters.Add("@Keyword", $"%{filter.Keyword}%");
                }
            }

            // Gộp điều kiện WHERE
            var whereClause = conditions.Any()
                ? "WHERE " + string.Join(" AND ", conditions)
                : "";

            string sqlData = $@"
            SELECT * FROM {tableName}
            {whereClause}
            LIMIT @PageSize OFFSET @Offset";

            string sqlCount = $@"
            SELECT COUNT(*) FROM {tableName}
            {whereClause}";

            parameters.Add("@PageSize", filter.PageSize);
            parameters.Add("@Offset", offset);


            using (var connection = new MySqlConnection(connectionString))
            {
                var data = connection.Query<T>(sqlData, parameters).ToList();
                var total = connection.ExecuteScalar<int>(sqlCount, parameters);
                
                return new PageResult
                {
                    Results = data,
                    TotalItem = total
                };
            }

        }

        private string GetTableName()
        {
            var tableName = typeof(T).Name;

            var tableAttribute = (MISATableNameAttribute?)Attribute.GetCustomAttribute(typeof(T), typeof(MISATableNameAttribute));
            if (tableAttribute != null)
            {
                tableName = tableAttribute.TableName;
            }

            return tableName;
        }
    }
}
