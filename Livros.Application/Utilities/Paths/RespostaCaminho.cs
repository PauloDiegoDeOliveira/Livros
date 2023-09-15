//namespace Livros.Application.Utilities.Paths
//{
//    public class RespostaCaminho
//    {
//        public bool Sucesso => Erros.Count == 0;
//        public string Caminho { get; set; } = string.Empty;
//        public List<string> Erros { get; private set; } = new List<string>();

//        public RespostaCaminho()
//        { }

//        public RespostaCaminho(string caminho)
//        {
//            this.Caminho = caminho;
//        }

//        public void AdicionarErro(string erro) => Erros.Add(erro);

//        public void AdicionarErros(IEnumerable<string> erros) => Erros.AddRange(erros);
//    }
//}