﻿using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Service;
using AkademicReport.Service.ReposteServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class reporteController : ControllerBase
    {
        private readonly IReporteService _reposteService;
        public reporteController(IReporteService reposteService)
        {
            _reposteService = reposteService;
        }
        /// <summary>
        /// --Este post trae un docente y su carga
        /// /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("docente")]
        public async Task<ActionResult>PorDocente(ReporteDto filtro)
        {
            var response =await _reposteService.PorDocenteCall(filtro);
            if(response.Status==204)
            {
                var ServiceR = new ServicesResponseMessage<string>();
                ServiceR.Status = 2024;
                ServiceR.Message = "Docente no existe";
                return Ok(ServiceR);
            }
            var ResponseDone = new ServiceResponseData<List<DocenteCargaReporteDtoPorDocente>>();
            ResponseDone.Status = 200;
            ResponseDone.Data = new List<DocenteCargaReporteDtoPorDocente>();
            var Uni = new DocenteCargaReporteDtoPorDocente();
            Uni.Docente = response.Data.Docente;
            Uni.Carga = response.Data.Carga;
            Uni.CantCreditos = response.Data.CantCreditos; ;
            Uni.MontoSemanal = response.Data.MontoSemanal;
            Uni.MontoMensual = response.Data.MontoMensual;
            Uni.MontoVinculacion = response.Data.MontoVinculacion;

            ResponseDone.Data.Add(Uni);
            return Ok(ResponseDone);

        }
        /// <summary>
        /// --Este post trae la los docentes y su carga de un recinto
        /// /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("recinto")]
        public async Task<ActionResult> PorRecinto(ReportePorRecintoDto filtro)
        {
            var response = await _reposteService.PorRecinto(filtro);
            return Ok(response);
           

        }

        /// <summary>
        /// --Este post trae la los docentes y su carga de un recinto
        /// /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("recinto/reporte-diplomado")]
        public async Task<ActionResult> ReporteDiplomado(ReportePorRecintoDto filtro)
        {
            var response = await _reposteService.ReporteDiplomado(filtro);
            return Ok(response);


        }
        /// <summary>
        /// --Este post trae el reporte consolidado de todos los recintos
        /// /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("consolidar")]
        public async Task<ActionResult> ReporteConsolidado(FiltroReporteConsolidado filtro)
        {
            var response = await _reposteService.ReporteConsolidado(filtro);
            return Ok(response);
        }
    }

}
