namespace Livros.Application.Utilities.Paths
{
    public class PathResponse
    {
        public bool Sucesso => Erros.Count == 0;

        public string Path = "";

        public List<string> Erros { get; private set; }

        public PathResponse() => Erros = new List<string>();

        public PathResponse(bool sucesso = true, string path = null) : this()
        {
            this.Path = path;
        }

        public void AdicionarErro(string erro) => Erros.Add(erro);

        public void AdicionarErros(IEnumerable<string> erros) => Erros.AddRange(erros);
    }
}