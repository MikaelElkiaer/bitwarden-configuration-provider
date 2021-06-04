using System;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden.Model
{
    internal class ExistingSecret : Secret
    {
        internal ExistingSecret(string name, string originalKey, string? fieldName = null) : base(name)
        {
            OriginalKey = originalKey;
            FieldName = fieldName;
        }

        public string OriginalKey { get; }
        public string? FieldName { get; }

        public bool HasFieldName => !string.IsNullOrWhiteSpace(FieldName);
    }
}
