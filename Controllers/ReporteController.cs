using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ProyectoGimnasio.Rpt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoGimnasio.Controllers
{
    public class ReporteController : Controller
    {
       
        public ActionResult ReporteSimple()
        {
            return View();
        }

        // metodo ver reporte Ingreso
        public ActionResult VerReporteIngreso()
        {
            var reporte = new ReportClass();

            reporte.FileName = Server.MapPath("/Rpt/reporteUno.rpt");  // Ruta del reporte de maquina 

            // conexion para el reporte 
            var coninfo = ReportesConexion.getConexion(); // llamar a la clase conexion 
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            Tables tables;
            tables = reporte.Database.Tables;  // llamado a crear reportes de tablas

            foreach (Table item in tables)
            {
                // meter la info de la conexion a cada tabla ( si mi reporte trae varias tablas ) 
                logoninfo = item.LogOnInfo;
                logoninfo.ConnectionInfo = coninfo;
                item.ApplyLogOnInfo(logoninfo);
            }

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat);  // exportar documento
            return new FileStreamResult(stream, "application/pdf");
        }



        // metodo para el reporte Maquina
        public ActionResult VerReporteMaquina()
        {
            var reporte = new ReportClass();

            reporte.FileName = Server.MapPath("/Rpt/reporteDos.rpt");  // Ruta del reporte de maquina 

            // conexion para el reporte 
            var coninfo = ReportesConexion.getConexion(); // llamar a la clase conexion 
            TableLogOnInfo logoninfo = new TableLogOnInfo();
            Tables tables;
            tables = reporte.Database.Tables;  // llamado a crear reportes de tablas

            foreach (Table item in tables)
            {
                // meter la info de la conexion a cada tabla ( si mi reporte trae varias tablas ) 
                logoninfo = item.LogOnInfo;
                logoninfo.ConnectionInfo = coninfo;
                item.ApplyLogOnInfo(logoninfo);
            }

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat);  // exportar documento
            return new FileStreamResult(stream, "application/pdf");
        }








    }

}