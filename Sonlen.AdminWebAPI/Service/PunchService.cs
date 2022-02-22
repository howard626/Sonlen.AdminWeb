using Dapper;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Data;
using System.Data.SqlClient;

namespace Sonlen.AdminWebAPI.Service
{
    public class PunchService : IPunchService
    {
        private readonly string connectString;

        public PunchService(IConfiguration configuration)
        {
            connectString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection Connection
        {
            get { return new SqlConnection(connectString); }
        }

        public int PunchIn(Employee employee)
        {
            int result;

            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employee.EmployeeID, DbType.String);
                parameters.Add("@PunchDate", DateTime.Today, DbType.Date);

                Punch punch = conn.QueryFirstOrDefault<Punch>("GetPunchRecord", parameters, commandType: CommandType.StoredProcedure);
                if (punch == null)
                {
                    DateTime now = DateTime.Now;
                    punch = new Punch()
                    {
                        EmployeeID = employee.EmployeeID,
                        PunchDate = DateTime.Today,
                        PunchInTime = $"{now.Hour}:{now.Minute}",
                        PunchOutTime = null,
                        WorkHour = null
                    };

                    parameters = punch.ToDynamicParameters();
                    result = conn.Execute("PunchIn", parameters, commandType: CommandType.StoredProcedure);
                }
                else 
                {
                    result = -1;
                }
            }

            return result;
        }

        public int PunchOut(Employee employee)
        {
            int result;

            using (var conn = Connection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employee.EmployeeID, DbType.String);
                parameters.Add("@PunchDate", DateTime.Today, DbType.Date);

                Punch punch = conn.QueryFirstOrDefault<Punch>("GetPunchRecord", parameters, commandType: CommandType.StoredProcedure);
                if (punch != null)
                {
                    if (string.IsNullOrEmpty(punch.PunchOutTime))
                    {

                        DateTime now = DateTime.Now;
                        int hour = now.Hour - int.Parse(punch.PunchInTime.Split(':')[0]);
                        int min = now.Minute - int.Parse(punch.PunchInTime.Split(':')[1]);
                        if (now.Hour > 14 && hour > 4)
                        {
                            hour -= 1;
                        }
                        if (min < 0)
                        {
                            min += 60;
                            hour -= 1;
                        }
                        decimal workHour = hour + (decimal)min / 60;
                        punch.PunchOutTime = $"{now.Hour}:{now.Minute}";
                        punch.WorkHour = workHour;

                        parameters = punch.ToDynamicParameters();
                        result = conn.Execute("PunchOut", parameters, commandType: CommandType.StoredProcedure);
                    }
                    else 
                    {
                        result = -2;
                    }
                }
                else 
                {
                    result = -1;
                }
            }
            return result;
        }
    }
}
