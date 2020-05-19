using System;
using System.Collections.Generic;
using System.Linq;

namespace AIForwardChaining
{
    class Program
    {
        public class Rule
        {
            public Rule(List<Statement> statements, string then)
            {
                Statements = statements;
                Then = then;
            }

            public class Statement
            {
                public Statement(string value)
                {
                    Value = value;
                }

                public string Value { get; set; }
                public bool IsInWorkingMemory { get; set; }
            }
            public List<Statement> Statements { get; set; }
            public string Then { get; set; }
            public bool IsFired { get; set; }
        }

        public static List<Rule> Rules = new List<Rule>()
        {
            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("Device's fan works by turning the device on"),
            }, then: "Device's fan is healthy"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("Device's hdd works by turning the device on without making bad noises"),
            }, then: "Device's hdd is healthy"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("Laptop shuts down quickly"),
                new Rule.Statement("CPU temp is too high"),
            }, then: "Device is not cooled properly"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("Device is not cooled properly"),
                new Rule.Statement("Device's fan is healthy"),
            }, then: "Thermal paste needs to be changed"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("Device's hdd is healthy"),
                new Rule.Statement("OS not found error is displayed"),
            }, then: "OS is not installed"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("By connecting the charger charging alert is displayed"),
            }, then: "Charging port is healthy"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("Device's battery is not charging"),
                new Rule.Statement("Charging port is healthy"),
            }, then: "Battery is broken"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("Device's screen displays nothing"),
                new Rule.Statement("GPU is healthy"),
            }, then: "Device's screen is broken"),

            new Rule(statements: new List<Rule.Statement>()
            {
                new Rule.Statement("By connecting an external monitor image is displayed"),
            }, then: "GPU is healthy"),

        };

        public static List<string> WorkingMemory = new List<string>()
        {
            "Laptop shuts down quickly",
            "Device's fan works by turning the device on",
            "CPU temp is too high",
        };
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            PrintWorkingMemory();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            int totalCycles = Rules.Count - WorkingMemory.Where(x => !Rules.Any(y => y.Then == x)).Count();
            for (int i = 0; i < totalCycles; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Cycle {i + 1}/{totalCycles}:");
                Console.WriteLine("------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();

                int j = 0;
                foreach (Rule rule in Rules)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Rule #{j + 1}/{Rules.Count}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (!rule.IsFired)
                    {
                        Console.WriteLine("\t[Rule is not fired]");
                        Console.WriteLine();
                        int k = 0;
                        foreach (Rule.Statement statement in rule.Statements)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine($"Rule #{j + 1}, Statement #{k + 1}/{rule.Statements.Count}:\n\t {statement.Value}");
                            Console.ForegroundColor = ConsoleColor.Green;
                            if (!statement.IsInWorkingMemory)
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("Searching for statement in working memory...");
                                Console.ForegroundColor = ConsoleColor.Green;
                                if (WorkingMemory.Where(x => x == statement.Value).Count() != 0)
                                {
                                    Console.WriteLine("\t[True]");
                                    statement.IsInWorkingMemory = true;
                                }
                                else
                                {
                                    Console.WriteLine("\t[Unknown]");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Already checked rule is in working memory\t[true]");
                            }
                            Console.WriteLine();
                        }
                        bool fire = true;
                        foreach (var statment in rule.Statements)
                        {
                            if (!statment.IsInWorkingMemory)
                            {
                                fire = false;
                                break;
                            }
                        }
                        if (fire)
                        {
                            if (!WorkingMemory.Contains(rule.Then))
                            {
                                Console.WriteLine("All statements found in working memory");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Then:\n\t{rule.Then}");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Firing rule...");
                                Console.ForegroundColor = ConsoleColor.Green;
                                WorkingMemory.Add(rule.Then);
                                Console.WriteLine("\t[Working memory updated]");
                                Console.WriteLine();
                                PrintWorkingMemory(true);
                            }
                            rule.IsFired = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\t[Rule is already fired]");
                    }
                    j++;
                    Console.WriteLine();
                }
                Console.WriteLine("=================================================================");
            }

            Console.WriteLine();
            Console.WriteLine("++++++++++++ Finished +++++++++++++");
            Console.WriteLine();
            PrintWorkingMemory();
        }

        static void PrintWorkingMemory(bool updated = false)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------------- Working memory --------------");
            int i = 0;
            foreach (var item in WorkingMemory)
            {
                if (updated && i + 1 == WorkingMemory.Count)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine(item);
                i++;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
        }

    }
}


