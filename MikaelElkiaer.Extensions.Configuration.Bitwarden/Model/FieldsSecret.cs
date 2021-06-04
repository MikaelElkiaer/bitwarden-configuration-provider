using System;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden.Model
{
    public class FieldsSecret : Secret
    {
        public FieldsSecret(string name, bool includeNotes = false, string notesFieldName = "notes", string nameToFieldSeperator = "__") : base(name)
        {
            IncludeNotes = includeNotes;
            NotesFieldName = notesFieldName;
            NameToFieldSeperator = nameToFieldSeperator;
        }

        public bool IncludeNotes { get; }
        public string NotesFieldName { get; }
        public string NameToFieldSeperator { get; }
    }
}
