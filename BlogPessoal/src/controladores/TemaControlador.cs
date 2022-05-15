using BlogPessoal.src.dtos;
using BlogPessoal.src.modelos;
using BlogPessoal.src.repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogPessoal.src.controladores
{
    [ApiController]
    [Route("api/Temas")]
    [Produces("application/json")]
    public class TemaControlador : ControllerBase
    {
        #region Atributos
        
        private readonly ITema _repositorio;

        #endregion


        #region Construtores

        public TemaControlador(ITema repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion


        #region Métodos

        /// <summary>
        /// Pegar todos os Temas
        /// </summary>
        /// <param Tema="TemaModelo">string</param>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna tema</response>
        /// <response code="204">Tema não existe</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TemaModelo))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> PegarTodosTemasAsync()
        {
            var lista = await _repositorio.PegarTodosTemasAsync();

            if (lista.Count < 1) return NoContent();

            return Ok(lista);
        }

        /// <summary>
        /// Pegar Tema pelo Id
        /// </summary>
        /// <param name="idTema">int</param>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna o Tema</response>
        /// <response code="404">Tema não existente</response>
        ///     
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TemaModelo))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id/{idTema}")]
        [Authorize]
        public async Task<ActionResult> PegarTemaPeloIdAsync([FromRoute] int idTema)
        {
            var tema = await _repositorio.PegarTemaPeloIdAsync(idTema);

            if (tema == null) return NotFound();

            return Ok(tema);
        }

        /// <summary>
        /// Pegar Tema pelo Descrição
        /// </summary>
        /// <param Tema="descricaoTema">string</param>
        /// <returns>ActionResult</returns>
        /// <response code="200">Retorna tema</response>
        /// <response code="204">Tema não existe</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TemaModelo))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("pesquisa")]
        [Authorize]
        public async Task<ActionResult> PegarTemasPelaDescricaoAsync([FromQuery] string descricaoTema)
        {
            var temas = await _repositorio.PegarTemasPelaDescricaoAsync(descricaoTema);

            if (temas.Count < 1) return NoContent();

            return Ok(temas);
        }

        /// <summary>
        /// Criar novo Tema
        /// </summary>
        /// <param nome="Tema">NovoTemaDTO</param>  
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/Tema
        ///     {
        ///        "nome": "Hoje o dia está lindo"
        ///       
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna usuario atualizado</response>
        /// <response code="400">Erro na requisição</response>
        /// 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TemaModelo))]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> NovoTemaAsync([FromBody] NovoTemaDTO tema)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _repositorio.NovoTemaAsync(tema);

            return Created($"api/Temas", tema);
        }

        /// <summary>
        /// Criar novo Tema
        /// </summary>
        /// <param nome="Tema">NovoTemaDTO</param>  
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/Tema
        ///     {
        ///        "Id":"1"
        ///        "nome": "Hoje o dia está Bonito"
        ///       
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna um Tema atualizado</response>
        /// <response code="400">Erro na requisição</response>
        /// 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TemaModelo))]
        [HttpPut]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult> AtualizarTema([FromBody] AtualizarTemaDTO tema)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _repositorio.AtualizarTemaAsync(tema);

            return Ok(tema);
        }
        
        /// <summary>
        /// Deletar Tema pelo Id
        /// </summary>
        /// <param name="idTema">Ide do tema</param>
        /// <returns>ActionResult</returns>
        /// <response code="204">Tema Deletdo</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("deletar/{idTema}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult> DeletarTema([FromRoute] int idTema)
        {
            await _repositorio.DeletarTemaAsync(idTema);
            return NoContent();
        }

        #endregion
    }
}