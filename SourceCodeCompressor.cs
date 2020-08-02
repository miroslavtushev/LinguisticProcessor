using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using SEEL.LinguisticProcessor;

namespace Program
{
    /// <summary>
    /// Combines all source code files into a single source.dat file
    /// </summary>
    class SourceCodeCompressor
    {
        private string m_compressed_dir = Constants.PROJECTS_PATH;

        private string m_projects_dir;

        private string m_pathToProject = null;

        public SourceCodeCompressor(ProgrammingLanguage progLang)
        {
            if (progLang == ProgrammingLanguage.Java)
            {
                m_projects_dir = Constants.JAVA_PROJECTS_PATH;
            }
            else if (progLang == ProgrammingLanguage.CSharp)
            {
                m_projects_dir = Constants.CSHARP_PROJECTS_PATH;
            }
            else if (progLang == ProgrammingLanguage.Python)
            {
                m_projects_dir = Constants.PYTHON_PROJECTS_PATH;
            }
            else if (progLang == ProgrammingLanguage.JavaScript)
            {
                m_projects_dir = Constants.JAVASCRIPT_PROJECTS_PATH;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public SourceCodeCompressor(ProgrammingLanguage progLang, string pathToProject)
        {
            m_pathToProject = pathToProject;
            if (progLang == ProgrammingLanguage.Java)
            {
                m_projects_dir = Constants.JAVA_PROJECTS_PATH;
            }
            else if (progLang == ProgrammingLanguage.CSharp)
            {
                m_projects_dir = Constants.CSHARP_PROJECTS_PATH;
            }
            else if (progLang == ProgrammingLanguage.Python)
            {
                m_projects_dir = Constants.PYTHON_PROJECTS_PATH;
            }
            else if (progLang == ProgrammingLanguage.JavaScript)
            {
                m_projects_dir = Constants.JAVASCRIPT_PROJECTS_PATH;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void CombineSourceCodeFilesForSingleProject(string path)
        {
            Console.WriteLine($@"Processing {path}...");
            string saveFile = m_compressed_dir + new DirectoryInfo(path).Name + ".zip";
            ZipFile.CreateFromDirectory(path, saveFile);
            Console.WriteLine($@"File {saveFile} is saved!"); 
        }

        //private void CombineSourceCodeFilesAll()
        //{
        //    var projectDirs = Directory.GetDirectories(m_projects_dir);
        //    Console.WriteLine($@"Found {projectDirs.Length} projects");
        //    foreach (var dir in projectDirs)
        //    {
        //        Console.WriteLine($@"Processing {dir}...");
        //        CombineSourceCodeFilesForSingleProject(dir);
        //    }
        //}

        public void Run()
        {
            if (m_pathToProject != null)
            {
                CombineSourceCodeFilesForSingleProject(m_pathToProject);
            }
            else
            {
                //CombineSourceCodeFilesAll();
            }
        }
    }
}
