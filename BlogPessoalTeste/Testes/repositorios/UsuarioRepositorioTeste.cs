using System.Linq;
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
        public void CriarQuatroUsuariosNoBancoRetornaQuatroUsuarios()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal1")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro 4 usuarios no banco
            _repositorio.NovoUsuario(
                new NovoUsuarioDTO("Jose Vinicius", "JoseVinicius@email.com", "134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
            _repositorio.NovoUsuario(
                new NovoUsuarioDTO("Willian Silva","Willian@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
            _repositorio.NovoUsuario(
                new NovoUsuarioDTO("Sueli Silva","Sueli@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
 
            _repositorio.NovoUsuario(
                new NovoUsuarioDTO("Jaine silva","Jaine@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
			//WHEN - Quando pesquiso lista total            
            //THEN - Então recebo 4 usuarios
            Assert.AreEqual(4, _contexto.Usuarios.Count());
        }
        
        [TestMethod]
        public void PegarUsuarioPeloEmailRetornaNaoNulo()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal2")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            _repositorio.NovoUsuario(
                new NovoUsuarioDTO("Jose Nilton","Nilton@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
            //WHEN - Quando pesquiso pelo email deste usuario
            var user = _repositorio.PegarUsuarioPeloEmail("Nilton@email.com");
			
            //THEN - Então obtenho um usuario
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void PegarUsuarioPeloIdRetornaNaoNuloENomeDoUsuario()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal3")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            _repositorio.NovoUsuario(
                new NovoUsuarioDTO("Thiago Silva","Thiago@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
			//WHEN - Quando pesquiso pelo id 1
            var user = _repositorio.PegarUsuarioPeloId(1);

            //THEN - Então, deve me retornar um elemento não nulo
            Assert.IsNotNull(user);
            //THEN - Então, o elemento deve ser Neusa Boaz
            Assert.AreEqual("Thiago Silva", user.Nome);
        }

        [TestMethod]
        public void AtualizarUsuarioRetornaUsuarioAtualizado()
        {
            // Definindo o contexto
            var opt= new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal4")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorio = new UsuarioRepositorio(_contexto);

            //GIVEN - Dado que registro um usuario no banco
            _repositorio.NovoUsuario(
                new NovoUsuarioDTO("Diogo silva","Diogo@email.com","134652","URLFOTO", TipoUsuario.NORMAL)
            );
            
            //WHEN - Quando atualizamos o usuario
            _repositorio.AtualizarUsuario(
                new AtualizarUsuarioDTO(1,"Diogo Silva","123456","URLFOTONOVA")
            );
            
            //THEN - Então, quando validamos pesquisa deve retornar nome Diogo Silva
            var antigo = _repositorio.PegarUsuarioPeloEmail("Diogo@email.com");

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