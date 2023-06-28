namespace Livros.Domain.Entities.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
        public DateTime? AlteradoEm { get; set; }

        public EntityBase()
        { }

        private readonly Dictionary<string, List<string>> RegrasStatus = new()
        {
            { "Ativo", new List<string> { "Inativo", "Ativo" } },
            { "Inativo", new List<string> { "Ativo", "Inativo" } }
        };

        public bool VerificaRegraStatus(string statusAtual, string novoStatus)
        {
            if (RegrasStatus.TryGetValue(statusAtual, out var statusPermitidos))
            {
                return statusPermitidos.Contains(novoStatus);
            }

            return false;
        }
    }
}