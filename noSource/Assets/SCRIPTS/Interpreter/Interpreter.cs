using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    public class ASTree
    {
        public int Symbol { get; set; }
        public int Linenr { get; set; }
        public int Offset { get; set; }
        public int Blocknr { get; set; }
        public string LexInfo { get; set; }
        public List<ASTree> Children { get; set; }
        
        public ASTree(int symbol, int linenr, int offset, string lexinfo)
        {
            Symbol = symbol;
            Linenr = linenr;
            Offset = offset;
            LexInfo = lexinfo;
            Children = new List<ASTree>();
        }

        public ASTree Adopt(params ASTree[] childrens)
        {
            for(int i = 0; i < childrens.Length; i++)
            {
                Children.Add(childrens[i]);
            }
            return this;
        }

        public override string ToString()
        {
            return ToStringRec(0);
        }

        private string ToStringRec(int depth)
        {
            string result = "";
            for(int i = 0; i < depth; i++)
            {
                result += "|   ";
            }
            result += LexInfo + " " + Linenr + ":" + Offset + "\n";
            for(int i = 0; i < Children.Count; i++)
            {
                result += Children[i].ToStringRec(depth + 1);
            }
            return result;
        }
    }

    public partial class Scanner
    {
        public int LexerReturn(int symbol)
        {
            yylval = new ASTree(symbol, 0, 0, yytext);
            return symbol;
        }

        public void LexerNewLine()
        {

        }

        public void LexerBadToken(string lexeme)
        {
            System.Diagnostics.Debug.WriteLine("Error: " + lexeme);
        }
    }

    public partial class Parser
    {
        public Parser(Scanner s) : base(s) { }

        public ASTree ParserRoot;

        public ASTree ParserNewRoot(Tokens root)
        {
            //System.Diagnostics.Debug.WriteLine("New root created");
            return new ASTree((int)root, 0, 0, "");
        }

    }

    static class Interpreter
    {
        private static RobotBrain robot;
        public static void RunInterpreter(string input)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(input, 0);
            Parser parser = new Parser(scanner);
            parser.Parse();
            SetRobot();
            Interpret(parser.ParserRoot);
        }
        private static void SetRobot()
        {
            robot = (RobotBrain)GameObject.Find("Robot").GetComponent("RobotBrain");
        }
        private static void Interpret(ASTree root)
        {
            foreach(ASTree child in root.Children)
            {
                switch(child.Symbol)
                {
                    case (int)Tokens.VARDECL:
                        switch(child.Children[0].LexInfo)
                        {
                            case "walkSpeed":
                                robot.walkSpeed = float.Parse(child.Children[1].LexInfo);
                                break;
                        }
                        break;
                    default:
                        Interpret(child);
                        break;
                }
            }
        }
    }
}
