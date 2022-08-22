using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using API.MethodEndPoint;
using API.Class;

namespace API
{
    public class Tests
    {
        public static string url = "https://serverest.dev/";
        public static string _ID;

        [Test]
        public void TesteApi()
        {
            Debug.WriteLine("Cadastrando usuário novo.");

            POST post = new POST();
            post.nome = "Guilherme";
            post.email = "Guilherme5@gui.com.br";
            post.password = "guilherme";
            post.administrador = "false";

            string CriandoJson = JsonConvert.SerializeObject(post);

            RestClient cliente = new RestClient();
            RestRequest request = new RestRequest()
            {
                Resource = url + "/usuarios",
                Method = Method.Post
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json");
            request.AddBody(CriandoJson);

            Retorno retorno = JsonConvert.DeserializeObject<Retorno>(cliente.Post(request).Content);

            Assert.AreEqual("Cadastro realizado com sucesso", retorno.message);

            if (retorno.message == "Cadastro realizado com sucesso")
                _ID = retorno._id;
            else
                throw new ArgumentException($"Erro ao efetuar o cadastro de usuário..");

//======================================================================================================================
            Debug.WriteLine("Validando informações cadastadras");

            cliente = new RestClient();
            request = new RestRequest()
            {
                Resource = url + "/usuarios/" + _ID,
                Method = Method.Get
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json");

            Usuarios Usuario = JsonConvert.DeserializeObject<Usuarios>(cliente.Get(request).Content);

            Assert.AreEqual("Guilherme", Usuario.nome);
            Assert.AreEqual("Guilherme5@gui.com.br", Usuario.email);
            Assert.AreEqual("guilherme", Usuario.password);
            Assert.AreEqual("false", Usuario.administrador);

//======================================================================================================================
            Debug.WriteLine($"Alterando usuário com id: {_ID}");

            PUT altera = new PUT();
            altera.id = _ID;

            post = new POST();
            post.nome = "Guilherme GET";
            post.email = "Guilherme55@gui.com.br";
            post.password = "guilherme5";
            post.administrador = "true";

            CriandoJson = JsonConvert.SerializeObject(post);

            cliente = new RestClient();
            request = new RestRequest()
            {
                Resource = url + "/usuarios/" + _ID,
                Method = Method.Put
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json");
            request.AddBody(CriandoJson);

            retorno = JsonConvert.DeserializeObject<Retorno>(cliente.Put(request).Content);

            Assert.AreEqual("Registro alterado com sucesso", retorno.message);

            if (retorno.message != "Registro alterado com sucesso")
                throw new ArgumentException($"Erro ao alterar o cadastro de usuário {_ID}.");

//======================================================================================================================
            Debug.WriteLine("Validando informações do cadastro de usuário após alteração.");

            cliente = new RestClient();
            request = new RestRequest()
            {
                Resource = url + "/usuarios/" + _ID,
                Method = Method.Get
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json");

            Usuario = JsonConvert.DeserializeObject<Usuarios>(cliente.Get(request).Content);
           
            Assert.AreEqual("Guilherme GET", Usuario.nome);
            Assert.AreEqual("Guilherme55@gui.com.br", Usuario.email);
            Assert.AreEqual("guilherme5", Usuario.password);
            Assert.AreEqual("true", Usuario.administrador);

//======================================================================================================================          
            Debug.WriteLine("Listando todos usuários cadastrados.");

            cliente = new RestClient();
            request = new RestRequest(url + "/usuarios");

            GET ListaComTodosUsuarios = JsonConvert.DeserializeObject<GET>(cliente.Get(request).Content);

            foreach (var item in ListaComTodosUsuarios.usuarios)
            {
                Debug.WriteLine($"Nome: {item.nome}");
                Debug.WriteLine($"E-Mail: {item.email}");
                Debug.WriteLine($"Senha: {item.password}");
                Debug.WriteLine($"Administrador: {item.administrador}");
                Debug.WriteLine($"ID: {item._id}");
                Debug.WriteLine("===========================================");
            }

//======================================================================================================================

            Debug.WriteLine($"Deletando usuário com id.{_ID}");

            DELETE delete = new DELETE();
            delete.id = _ID;

            CriandoJson = JsonConvert.SerializeObject(delete);

            cliente = new RestClient();
            request = new RestRequest()
            {
                Resource = url + "/usuarios/" + _ID,
                Method = Method.Delete
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json");
            request.AddBody(CriandoJson);

            retorno = JsonConvert.DeserializeObject<Retorno>(cliente.Delete(request).Content);

            Assert.AreEqual("Registro excluído com sucesso", retorno.message);

            if (retorno.message != "Registro excluído com sucesso")
                throw new ArgumentException($"Erro ao deletar o usuário com id {_ID}.");
        }
    }
}