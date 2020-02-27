using System;

namespace TranslationLibrary
{
    public interface ITranslator
    {
        ulong GetInstanceCount();
        String GetName();
        String GetStudentId();
        String Translate(String source);
    }
}
