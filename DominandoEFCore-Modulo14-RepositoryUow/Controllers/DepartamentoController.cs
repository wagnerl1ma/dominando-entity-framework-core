﻿using DominandoEFCore_Modulo14_RepositoryUow.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore_Modulo14_RepositoryUow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly ILogger<DepartamentoController> _logger;
        //private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IUnitOfWork _uow;

        public DepartamentoController(
            ILogger<DepartamentoController> logger,
            /*IDepartamentoRepository repository,*/
            IUnitOfWork uow)
        {
            _logger = logger;
            //_departamentoRepository = repository;
            _uow = uow;
        }

        //departamento/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id/*,[FromServices]IDepartamentoRepository repository*/) //USANDO O FROM SERVICE ELE ABRE UMA INSTANCIA DO OBJETO QUE VOCE QUER USAR
        {
            //var departamento = await _departamentoRepository.GetByIdAsync(id);
            var departamento = await _uow.DepartamentoRepository.GetByIdAsync(id);

            return Ok(departamento);
        }

        ////departamento
        //[HttpPost]
        //public IActionResult CreateDepartamento(Departamento departamento)
        //{
        //    //_departamentoRepository.Add(departamento);

        //    //var saved = _departamentoRepository.Save();
        //    _uow.DepartamentoRepository.Add(departamento);

        //    _uow.Commit();

        //    return Ok(departamento);
        //}

        //departamento/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDepartamentoAsync(int id)
        {
            var departamento = await _uow.DepartamentoRepository.GetByIdAsync(id);

            _uow.DepartamentoRepository.Remove(departamento);

            _uow.Commit();

            return Ok(departamento);
        }

        //departamento/?descricao=teste
        [HttpGet]
        public async Task<IActionResult> ConsultarDepartamentoAsync([FromQuery] string descricao)
        {
            var departamentos = await _uow.DepartamentoRepository
                .GetDataAsync(
                    p => p.Descricao.Contains(descricao),
                    p => p.Include(c => c.Colaboradores),
                    take: 2
                );

            return Ok(departamentos);
        }
    }
}
