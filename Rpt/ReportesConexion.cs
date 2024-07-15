using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoGimnasio.Rpt
{
    public class ReportesConexion
    {
        public static CrystalDecisions.Shared.ConnectionInfo getConexion()
        {
            CrystalDecisions.Shared.ConnectionInfo infocon = new
                CrystalDecisions.Shared.ConnectionInfo();
            infocon.ServerName = "DESKTOP-PP002E5"; // conexion bd
            infocon.DatabaseName = "GimnasioBD"; // nombre bd
            infocon.IntegratedSecurity = true;
            return infocon;
        }

    }
}