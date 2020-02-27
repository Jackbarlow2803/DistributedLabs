using System;

using TranslationLibrary;



namespace TranslationServer
{
    //Class has no independent existence so can't be used by client apps
    //Must use a host app to listen on socket and wait for requests to use this
    public class Translator : MarshalByRefObject, ITranslator
    {
        private static ulong Instances = 0;
        private const String Name = "Jack Barlow";
        private const String StudentId = "556980";

        public Translator()
        {
            Console.WriteLine("Created new translator class - this is instance #" + ++Translator.Instances);
        }

        public ulong GetInstanceCount()
        {
            return Translator.Instances;
        }

        public String GetName()
        {
            return Translator.Name;
        }

        public String GetStudentId()
        {
            return Translator.StudentId;
        }

        public String Translate(String englishString)
        {
            //Could write a method that translates to French etc.
            String[] words = englishString.Split(' ');
            String result = "";

            foreach (String word in words)
            {
                result += word.Substring(1);
                result += word.Substring(0, 1) + "ay ";
            }

            return result;
        }
    }
}
