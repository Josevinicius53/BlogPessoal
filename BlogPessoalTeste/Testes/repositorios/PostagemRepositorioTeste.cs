using System.Linq;
using BlogPessoal.src.data;
using BlogPessoal.src.dtos;
using BlogPessoal.src.repositorios;
using BlogPessoal.src.repositorios.implementacoes;
using BlogPessoal.src.utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlogPessoalTeste.Testes.repositorios
{
    [TestClass]
    public class PostagemRepositorioTeste
    {
        private BlogPessoalContexto _contexto;
        private IUsuario _repositorioU;
        private ITema _repositorioT;
        private IPostagem _repositorioP;

        [TestMethod]
        public void CriaTresPostagemNoSistemaRetornaTres()
        {
            // Definindo o contexto
           var opt = new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal21")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorioU = new UsuarioRepositorio(_contexto);
            _repositorioT = new TemaRepositorio(_contexto);
            _repositorioP = new PostagemRepositorio(_contexto);

            // GIVEN - Dado que registro 2 usuarios
            _repositorioU.NovoUsuario(
                new NovoUsuarioDTO("José Vinicius", "JoseVinicius@email.com", "134652", "URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            _repositorioU.NovoUsuario(
                new NovoUsuarioDTO("Vinicius", "Vinicius@email.com","134652","URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            // AND - E que registro 2 temas
            _repositorioT.NovoTema(new NovoTemaDTO("Nuruto"));
            _repositorioT.NovoTema(new NovoTemaDTO("The Resident"));

            // WHEN - Quando registro 3 postagens
            _repositorioP.NovaPostagem(
                new NovaPostagemDTO(
                    "Naturuto é bacana",
                    "É um anime muito assistido no mundo",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "Naruto é bacana"
                )
            );
            _repositorioP.NovaPostagem(
                new NovaPostagemDTO(
                    "Naruto esta sendo um teste",
                    "O teste unitário é importante para o desenvolvimento",
                    "URLDAFOTO",
                    "Vinicius@email.com",
                    "Naturo"
                )
            );
            _repositorioP.NovaPostagem(
                new NovaPostagemDTO(
                    "The resident muito legal",
                    "The resident acabou de ser lançado",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "The resident"
                )
            );
            
            // WHEN - Quando eu busco todas as postagens
            // THEN - Eu tenho 3 postagens
            Assert.AreEqual(3, _repositorioP.PegarTodasPostagens().Count());
        }

        [TestMethod]
        public void AtualizarPostagemRetornaPostagemAtualizada()
        {
            // Definindo o contexto
            var opt = new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal22")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorioU = new UsuarioRepositorio(_contexto);
            _repositorioT = new TemaRepositorio(_contexto);
            _repositorioP = new PostagemRepositorio(_contexto);

            // GIVEN - Dado que registro 1 usuarios
            _repositorioU.NovoUsuario(
                new NovoUsuarioDTO("Juse", "JuseVinicius@email.com", "134652", "URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            // AND - E que registro 1 tema
            _repositorioT.NovoTema(new NovoTemaDTO("Fusca"));
            _repositorioT.NovoTema(new NovoTemaDTO("Corsa"));

            // AND - E que registro 1 postagem
            _repositorioP.NovaPostagem(
                new NovaPostagemDTO(
                    "Fusca é muito antigo",
                    "É uma Reliquia, muito utilizada pra coleções",
                    "URLDAFOTO",
                    "JuseVinicius@email.com",
                    "Fusca"
                )
            );

            // WHEN - Quando atualizo postagem de id 1
            _repositorioP.AtualizarPostagem(
                new AtualizarPostagemDTO(
                    1,
                    "Corsa é muito Bonito",
                    " O corsa é muito utilizada Hoje em dia",
                    "URLDAFOTOATUALIZADA",
                    "Corsa"
                )
            );

            // THEN - Eu tenho a postagem atualizada
            Assert.AreEqual("Corsa é muito Bonito", _repositorioP.PegarPostagemPeloId(1).Titulo);
            Assert.AreEqual("O corsa é muito utilizada Hoje em dia", _repositorioP.PegarPostagemPeloId(1).Descricao);
            Assert.AreEqual("URLDAFOTOATUALIZADA",_repositorioP.PegarPostagemPeloId(1).Foto);
            Assert.AreEqual("Corsa", _repositorioP.PegarPostagemPeloId(1).Tema.Descricao);
        }

        [TestMethod]
        public void PegarPostagensPorPesquisaRetodarCustomizada()
        {
            // Definindo o contexto
            var opt = new DbContextOptionsBuilder<BlogPessoalContexto>()
                .UseInMemoryDatabase(databaseName: "db_blogpessoal23")
                .Options;

            _contexto = new BlogPessoalContexto(opt);
            _repositorioU = new UsuarioRepositorio(_contexto);
            _repositorioT = new TemaRepositorio(_contexto);
            _repositorioP = new PostagemRepositorio(_contexto);

            // GIVEN - Dado que registro 2 usuarios
            _repositorioU.NovoUsuario(
                new NovoUsuarioDTO("JoseVinicius", "JoseVinicius@email.com", "134652", "URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            _repositorioU.NovoUsuario(
                new NovoUsuarioDTO("ViniciusJose", "Vinicius@email.com","134652","URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            // AND - E que registro 2 temas
            _repositorioT.NovoTema(new NovoTemaDTO("Livros"));
            _repositorioT.NovoTema(new NovoTemaDTO("Quadrinhos"));

            // WHEN - Quando registro 3 postagens
            _repositorioP.NovaPostagem(
                new NovaPostagemDTO(
                    "Livros é muito legal",
                    "livros é muito utilizado no mundo",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "Livros"
                )
            );
            _repositorioP.NovaPostagem(
                new NovaPostagemDTO(
                    "C# pode ser usado com Testes",
                    "O teste unitário é importante para o desenvolvimento",
                    "URLDAFOTO",
                    "Vinicius@email.com",
                    "C#"
                )
            );
            _repositorioP.NovaPostagem(
                new NovaPostagemDTO(
                    "Java é muito massa",
                    "Java também é muito utilizada no mundo",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "Java"
                )
            );

            // WHEN - Quando eu busco as postagen
            // THEN - Eu tenho as postagens que correspondem aos criterios
            Assert.AreEqual(2, _repositorioP.PegarPostagensPorPesquisa("Livros", null, null).Count);
            Assert.AreEqual(2, _repositorioP.PegarPostagensPorPesquisa(null, "Fusca", null).Count);
            Assert.AreEqual(2, _repositorioP.PegarPostagensPorPesquisa(null, null, "José Vinicius").Count);
        }
    }
}