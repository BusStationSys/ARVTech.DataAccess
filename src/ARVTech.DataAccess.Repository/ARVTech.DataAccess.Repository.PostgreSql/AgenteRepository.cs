namespace FlooString.Repository.PostgreSql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using FlooString.Models;
    using FlooString.Repository.Common;
    using FlooString.Repository.Interfaces;
    using FlooString.UnitOfWork.PostgreSql;
    using Npgsql;

    public class AgenteRepository : Repository, IAgenteRepository
    {
        public AgenteRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            this._connection = connection;
            this._transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Agente Get(int id)
        {
            try
            {
                Agente agente = null as Agente;

                string cmdText = @"SELECT age_id,
                                          age_integrationid,
                                          age_name,
                                          age_login,
                                          age_observation,
                                          age_country,
                                          age_state,
                                          age_city,
                                          age_neighborhood,
                                          age_street,
                                          age_streetnumber,
                                          age_streetnumbercompl,
                                          age_zipcode,
                                          age_phone,
                                          age_mobilephone,
                                          age_mail,
                                          age_imeilastsync,
                                          age_datehourlastsync,
                                          agg_id,
                                          age_active,
                                          age_mobileversion,
                                          age_lastgeoposition,
                                          med_id,
                                          age_geoposition,
                                          age_batterylevel,
                                          age_mobileplatformtype,
                                          age_mobileplatformversion,
                                          age_ignoreteamfilter,
                                          age_id_insert,
                                          age_datetimeinsert,
                                          age_moduleinsert,
                                          age_id_lastupdate,
                                          age_datetimelastupdate,
                                          age_modulelastupdate,
                                          age_datetimelastgps,
                                          age_customerportfoliofilter,
                                          wkj_id,
                                          age_totalcapacity1,
                                          age_totalcapacity2,
                                          age_timezone,
                                          age_timezonelocation
                                     FROM {0}.agent
                                    WHERE Id = {1}";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    this._connection.Database,
                    id);

                var cmd = this.CreateCommand(cmdText);
                cmd.Parameters.AddWithValue("@Id", id);

                using (DataTable dt = Helpers.GetInstance().ExecuteDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count == 1)
                        agente = this.Popular(dt).ToList().FirstOrDefault();

                    dt.Dispose();
                }

                return agente;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Agente> GetAll()
        {
            try
            {
                IEnumerable<Agente> agentes = null as IEnumerable<Agente>;

                string cmdText = @"SELECT age_id,
                                          age_integrationid,
                                          age_name,
                                          age_login,
                                          age_observation,
                                          age_country,
                                          age_state,
                                          age_city,
                                          age_neighborhood,
                                          age_street,
                                          age_streetnumber,
                                          age_streetnumbercompl,
                                          age_zipcode,
                                          age_phone,
                                          age_mobilephone,
                                          age_mail,
                                          age_imeilastsync,
                                          age_datehourlastsync,
                                          agg_id,
                                          age_active,
                                          age_mobileversion,
                                          age_lastgeoposition,
                                          med_id,
                                          age_geoposition,
                                          age_batterylevel,
                                          age_mobileplatformtype,
                                          age_mobileplatformversion,
                                          age_ignoreteamfilter,
                                          age_id_insert,
                                          age_datetimeinsert,
                                          age_moduleinsert,
                                          age_id_lastupdate,
                                          age_datetimelastupdate,
                                          age_modulelastupdate,
                                          age_datetimelastgps,
                                          age_customerportfoliofilter,
                                          wkj_id,
                                          age_totalcapacity1,
                                          age_totalcapacity2,
                                          age_timezone,
                                          age_timezonelocation
                                     FROM {0}.agent";

                cmdText = string.Format(
                    CultureInfo.InvariantCulture,
                    cmdText,
                    "u28467");

                var cmd = this.CreateCommand(cmdText);

                using (DataTable dt = Helpers.GetInstance().ExecuteDataTable(cmd))
                {
                    if (dt != null && dt.Rows.Count > 0)
                        agentes = this.Popular(dt);

                    dt.Dispose();
                }

                return agentes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Populate a <see cref="IEnumerable{T}"/> of <see cref="Agente"/> from a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="dt">DataTable with the records.</param>
        /// <returns>List with the records.</returns>
        private IEnumerable<Agente> Popular(DataTable dt)
        {
            return from DataRow row in dt.AsEnumerable()
                   select new Agente
                   {
                       Id = Convert.ToInt32(row["age_id"].ToString()),
                       Name = row["age_name"].ToString(),
                   };
        }
    }
}
