using System.Linq;
using System.Threading.Tasks;
using BlogPessoal.src.data;
using BlogPessoal.src.dtos;
using BlogPessoal.src.repositorios;
using BlogPessoal.src.repositorios.implementacoes;
using BlogPessoal.src.utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlogPessoalTest.Testes.repositorios
{
    [TestClass]
    public class UsuarioRepositorioTeste
    {
        private BlogPessoalContexto _contexto;
        private IUsuario _repositorio;

        [TestMethod]
        public async Task  CriarQuatroUsuariosNoBancoRetornaQuatroUsuarios()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal1")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro 4 usuarios no banco
            await _repositorio.NovoUsuarioAsync(
                new NovoUsuarioDTO("Jose Vinicius", "JoseVinicius@email.com", "134652","URLFOTO", TipoUsuario.NORMAL)
            );

            await _repositorio.NovoUsuarioAsync(
                new NovoUsuarioDTO("Willian Silva","Willian@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );

            await _repositorio.NovoUsuarioAsync(
                new NovoUsuarioDTO("Sueli Silva","Sueli@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );

            await _repositorio.NovoUsuarioAsync(
                new NovoUsuarioDTO("Jaine silva","Jaine@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
			//WHEN - Quando pesquiso lista total            
            //THEN - Então recebo 4 usuarios
            Assert.AreEqual(4, _contexto.Usuarios.Count());
        }
        
        [TestMethod]
        public async Task PegarUsuarioPeloEmailRetornaNaoNulo()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal2")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            await _repositorio.NovoUsuarioAsync(
                new NovoUsuarioDTO("Jose Nilton","Nilton@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
            //WHEN - Quando pesquiso pelo email deste usuario
            var user = await _repositorio.PegarUsuarioPeloEmailAsync("Nilton@email.com");
			
            //THEN - Então obtenho um usuario
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task PegarUsuarioPeloIdRetornaNaoNuloENomeDoUsuario()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal3")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            await _repositorio.NovoUsuarioAsync(
                new NovoUsuarioDTO("Thiago Silva","Thiago@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
			//WHEN - Quando pesquiso pelo id 1
            var user = await _repositorio.PegarUsuarioPeloIdAsync(1);

            //Então, deve me retornar um elemento não nulo
            Assert.IsNotNull(user);
            //THEN - Então, o elemento deve ser Thiago silva
            Assert.AreEqual("Thiago Silva", user.Nome);
        }

        [TestMethod]
        public async Task AtualizarUsuarioRetornaUsuarioAtualizado()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal4")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            await _repositorio.NovoUsuarioAsync(
                new NovoUsuarioDTO("Diogo silva","Diogo@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
            //WHEN - Quando atualizamos o usuario
            await _repositorio.AtualizarUsuarioAsync(
                new AtualizarUsuarioDTO(1,"Diogo Silva","123456","URLFOTONOVA")
            );
            
            //THEN - Então, quando validamos pesquisa deve retornar nome Diogo Silva
            var antigo = await _repositorio.PegarUsuarioPeloEmailAsync("Diogo@email.com");

            Assert.AreEqual(
                "Diogo Silva",
                _contexto.Usuarios.FirstOrDefault(u => u.Id == antigo.Id).Nome
            );
            
            //THEN - Então, quando validamos pesquisa deve retornar senha 123456
            Assert.AreEqual(
                "123456",
                _contexto.Usuarios.FirstOrDefault(u => u.Id == antigo.Id).Senha
            );
        }

    }
}