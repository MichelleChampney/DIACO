﻿using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiRest.Repository;

namespace WebApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuejaTiposController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public QuejaTiposController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los tipos de quejas registradas
        /// </summary>
        /// <returns>Listado de los tipos de quejas</returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<eQuejaTipo>>> GetAll()
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaTipo>("sp_GetAllQuejaTipos");

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de un tipo de queja
        /// </summary>
        /// <param name="id">Id del tipo de queja</param>
        /// <returns>Objeto con datos del tipo de queja</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<eCatalogo>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eQuejaTipo>("sp_GetQuejaTipos", new { @Id = id });

                if (obj == null)
                    return NotFound();
                else
                    return obj;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Guarda un tipo de queja
        /// </summary>
        /// <param name="obj">Objeto con los datos del tipo de queja</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] eQuejaTipo obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveQuejaTipos", new { @Id = 0, @Abreviatura = obj.Abreviatura, @Nombre = obj.Nombre });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de un tipo de queja en especifico
        /// </summary>
        /// <param name="id">Id del tipo de queja</param>
        /// <param name="obj">Objeto con los datos del tipo de queja</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] eQuejaTipo obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveQuejaTipos", new { @Id = obj.Id, @Abreviatura = obj.Abreviatura, @Nombre = obj.Nombre });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina un tipo de queja en especifico
        /// </summary>
        /// <param name="id">Id del tipo de queja</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eQuejaTipo>("sp_GetQuejaTipos", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteQuejaTipos", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de los tipos de queja registrados para su seleccion
        /// </summary>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de tipos de queja para su seleccion</returns>
        [HttpGet("GetAllValueList/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaTipo>("sp_GetAllQuejaTipos");
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione una tipo de queja", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
