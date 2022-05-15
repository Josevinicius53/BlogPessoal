using System.Linq;
using System.Threading.Tasks;
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
        public async Task CriaTresPostagemNoSistemaRetornaTres()
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
            await _repositorioU.NovoUsuarioAsync(
                new NovoUsuarioDTO("José Vinicius", "JoseVinicius@email.com", "134652", "URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            await _repositorioU.NovoUsuarioAsync(
                new NovoUsuarioDTO("Vinicius", "Vinicius@email.com","134652","URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            // AND - E que registro 2 temas
            await _repositorioT.NovoTemaAsync(new NovoTemaDTO("Nuruto"));
            await _repositorioT.NovoTemaAsync(new NovoTemaDTO("The Resident"));

            // WHEN - Quando registro 3 postagens
            await _repositorioP.NovaPostagemAsync(
                new NovaPostagemDTO(
                    "Naturuto é bacana",
                    "É um anime muito assistido no mundo",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "Naruto é bacana"
                )
            );
            await _repositorioP.NovaPostagemAsync(
                new NovaPostagemDTO(
                    "Naruto esta sendo um teste",
                    "O teste unitário é importante para o desenvolvimento",
                    "URLDAFOTO",
                    "Vinicius@email.com",
                    "Naturo"
                )
            );
            await _repositorioP.NovaPostagemAsync(
                new NovaPostagemDTO(
                    "The resident muito legal",
                    "The resident acabou de ser lançado",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "The resident"
                )
            );
            
            // WHEN - Quando eu busco todas as postagens
            var lista = await _repositorioP.PegarTodasPostagensAsync();

            // THEN - Eu tenho 3 postagens
            Assert.AreEqual(3, lista.Count());
        }

        [TestMethod]
        public async Task AtualizarPostagemRetornaPostagemAtualizada()
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
            await _repositorioU.NovoUsuarioAsync(
                new NovoUsuarioDTO("Juse", "JuseVinicius@email.com", "134652", "URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            // AND - E que registro 1 tema
            await _repositorioT.NovoTemaAsync(new NovoTemaDTO("Fusca"));
            await _repositorioT.NovoTemaAsync(new NovoTemaDTO("Corsa"));

            // AND - E que registro 1 postagem
            await _repositorioP.NovaPostagemAsync(
                new NovaPostagemDTO(
                    "Fusca é muito antigo",
                    "É uma Reliquia, muito utilizada pra coleções",
                    "URLDAFOTO",
                    "JuseVinicius@email.com",
                    "Fusca"
                )
            );

            // WHEN - Quando atualizo postagem de id 1
            await _repositorioP.AtualizarPostagemAsync(
                new AtualizarPostagemDTO(
                    1,
                    "Corsa é muito Bonito",
                    "O corsa é muito utilizada Hoje em dia",
                    "URLDAFOTOATUALIZADA",
                    "Corsa"
                )
            );

            var postagem = await _repositorioP.PegarPostagemPeloIdAsync(1);

            // THEN - Eu tenho a postagem atualizada
            Assert.AreEqual("Corsa é muito Bonito", postagem.Titulo);
            Assert.AreEqual("O corsa é muito utilizada Hoje em dia", postagem.Descricao);
            Assert.AreEqual("URLDAFOTOATUALIZADA", postagem.Foto);
            Assert.AreEqual("Corsa", postagem.Tema.Descricao);
        }

        [TestMethod]
        public async Task PegarPostagensPorPesquisaRetodarCustomizada()
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
            await _repositorioU.NovoUsuarioAsync(
                new NovoUsuarioDTO("JoseVinicius", "JoseVinicius@email.com", "134652", "URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            await _repositorioU.NovoUsuarioAsync(
                new NovoUsuarioDTO("ViniciusJose", "Vinicius@email.com","134652","URLDAFOTO", TipoUsuario.NORMAL)
            );
            
            // AND - E que registro 2 temas
            await _repositorioT.NovoTemaAsync(new NovoTemaDTO("Livros"));
            await _repositorioT.NovoTemaAsync(new NovoTemaDTO("Quadrinhos"));

            // WHEN - Quando registro 3 postagens
            await _repositorioP.NovaPostagemAsync(
                new NovaPostagemDTO(
                    "Livros é muito legal",
                    "livros é muito utilizado no mundo",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "Livros"
                )
            );
            await _repositorioP.NovaPostagemAsync(
                new NovaPostagemDTO(
                    "C# pode ser usado com Testes",
                    "O teste unitário é importante para o desenvolvimento",
                    "URLDAFOTO",
                    "Vinicius@email.com",
                    "C#"
                )
            );
            await _repositorioP.NovaPostagemAsync(
                new NovaPostagemDTO(
                    "Java é muito massa",
                    "Java também é muito utilizada no mundo",
                    "URLDAFOTO",
                    "JoseVinicius@email.com",
                    "Java"
                )
            );

            var postagensTeste1 = await _repositorioP.PegarPostagensPorPesquisaAsync("Livros", null, null);
            var postagensTeste2 = await _repositorioP.PegarPostagensPorPesquisaAsync(null, "Fusca", null);
            var postagensTeste3 = await _repositorioP.PegarPostagensPorPesquisaAsync(null, null, "JoseVinicius@email.com");

            // WHEN - Quando eu busco as postagen
            // THEN - Eu tenho as postagens que correspondem aos criterios
            Assert.AreEqual(1, postagensTeste1.Count);
            Assert.AreEqual(0, postagensTeste2.Count);
            Assert.AreEqual(2, postagensTeste3.Count);
        }
    }
}