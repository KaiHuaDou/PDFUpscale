﻿using System;
using System.IO;
using System.Linq;
using CommandLine;
using PDFUpscale.Handler;
using PDFUpscale.Properties;

namespace PDFUpscale;

public static class Program
{
    public static Options Option { get; set; } = new Options( );

    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
    }

    public static void Run(Options option)
    {
        Option = option;
        if (!Options.Models.Contains(Option.Model))
            Option.Model = Options.Models[2];
        if (File.Exists(Option.Input))
        {
            UpscalePDF.Exec(new FileInfo(Option.Input), Option.Output);
        }
        else if (Directory.Exists(Option.Input))
        {
            DirectoryInfo directory = new(Option.Input);
            foreach (FileInfo file in directory.GetFiles("*.pdf", SearchOption.TopDirectoryOnly))
                UpscalePDF.Exec(file, $"{Option.Output}/Upscale_{file.Name}");
        }
        else
        {
            throw new FileNotFoundException($"{Text.CannotFound} {Option.Input}", Option.Input);
        }
    }

    public static void RunImages(Options option)
    {
        Option = option;
        if (!Options.Models.Contains(Option.Model))
            Option.Model = Options.Models[2];
        DirectoryInfo directory = new(Option.Input);
        UpscaleImage.Batch(
            directory.GetFiles("*.png", SearchOption.AllDirectories).ToList( ),
            (image) => Console.WriteLine($"{Text.UpscaleImage} {image.Name}")
        );
    }
}
