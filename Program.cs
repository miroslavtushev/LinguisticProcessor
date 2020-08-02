using Program;
using System;
using System.IO;
using System.Reflection;

static class MainProgram
{
	public static void Main(string[] args)
	{

        //var t = new Program.ProjectLinkRetrieval(1000, ProgrammingLanguage.JavaScript);
        //t.Run();
        //Console.Read();

        //var projectDownloader = new Program.Program(@"E:\JavascriptCompressed\JavascriptLinks.txt", ProgrammingLanguage.JavaScript);
        //projectDownloader.Run();
        //Console.Read();

        //var projectCompressor = new Program.SourceCodeCompressor(ProgrammingLanguage.CSharp);
        //projectCompressor.Run();
        //Console.Read();

        //var p = new Program.ProjectStatsRetrieval();
        //p.Run();
        //Console.Read();

        var namesExtr = new Program.NamesExtractors.JavaNamesExtractor(@"Z:\Test");
        namesExtr.Run();
        HelperFunctions.WriteLine("Done!");


        //var namesExtr = new Program.NamesExtractors.PythonNamesExtractor(@"Z:\TestArchived");
        //namesExtr.Run();
        //LinguisticChangeCalculator.LinguisticChangeAllProjectsToFile(@"Z:\TestArchived");

        //var namesExtr = new Program.NamesExtractors.JavascriptNamesExtractor(@"Z:\Test");
        //namesExtr.Run();
        //LinguisticChangeCalculator.LinguisticChangeAllProjectsToFile(@"Z:\Test");


        //LinguisticChangeCalculator.WriteSingleProjectToFileByRelease(@"ReactiveX/RxJava");

        //var commits = new Program.ProjectContributorsRetrieval(@"https://github.com/ReactiveX/RxJava");
        //commits.Run();
        //Console.Read();

        //var stats = new Program.DescriptiveStatistics(ProgrammingLanguage.JavaScript);
        //stats.Run();
        //Console.Read();

        //HelperFunctions.ProjectsExtractorToSDD();
    }
}
