namespace MikaelElkiaer.Extensions.Configuration.Bitwarden.Model
{
    public abstract class Secret
    {
        public Secret(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
